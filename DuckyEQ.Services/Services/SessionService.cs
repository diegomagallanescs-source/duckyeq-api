using DuckyEQ.Contracts.Interfaces.Services;
using DuckyEQ.Contracts.Models;
using Microsoft.Extensions.Caching.Memory;

namespace DuckyEQ.Services.Services;

public class SessionService : ISessionService
{
    private readonly IMemoryCache _cache;

    public SessionService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public string CreateSession(Guid userId, Guid lessonId)
    {
        var activeKey = ActiveKey(userId, lessonId);
        if (_cache.TryGetValue(activeKey, out _))
            throw new InvalidOperationException($"An active session already exists for lesson {lessonId}.");

        var token = Guid.NewGuid().ToString();
        var session = new LessonSession(userId, lessonId, DateTime.UtcNow);
        var expiry = TimeSpan.FromMinutes(30);

        _cache.Set(token, session, expiry);
        _cache.Set(activeKey, token, expiry);

        return token;
    }

    public LessonSession? GetSession(string token) =>
        _cache.TryGetValue(token, out LessonSession? session) ? session : null;

    public void RemoveSession(string token)
    {
        if (_cache.TryGetValue(token, out LessonSession? session) && session is not null)
            _cache.Remove(ActiveKey(session.UserId, session.LessonId));
        _cache.Remove(token);
    }

    private static string ActiveKey(Guid userId, Guid lessonId) =>
        $"active:{userId}:{lessonId}";
}
