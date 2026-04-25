using DuckyEQ.Contracts.Interfaces.Repositories;
using DuckyEQ.Contracts.Interfaces.Services;

namespace DuckyEQ.Services.Utilities;

public class UsernameGenerator : IUsernameGenerator
{
    private static readonly string[] Adjectives =
    [
        "Sunny", "Bold", "Calm", "Swift", "Brave", "Plucky", "Fuzzy", "Pudgy", "Cozy", "Zippy",
        "Sleek", "Fluffy", "Cheeky", "Jolly", "Perky", "Snappy", "Dandy", "Merry", "Bouncy", "Floaty",
        "Dreamy", "Bubbly", "Gentle", "Lucky", "Gleamy", "Breezy", "Misty", "Dapper", "Peppy", "Waddly"
    ];

    private static readonly string[] Nouns =
    [
        "Duck", "Quack", "Waddle", "Paddle", "Splash", "Feather", "Beak", "Diver", "Glider", "Floater",
        "Pebble", "Ripple", "Dabbler", "Swimmer", "Drifter", "Raindrop", "Napper", "Hopper", "Skipper", "Nestling",
        "Duckling", "Flapper", "Crest", "Brook", "Wader", "Tufter", "Puddle", "Flier", "Plover", "Dabbler"
    ];

    private readonly IUserRepository _userRepo;
    private readonly Random _random = new();

    public UsernameGenerator(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task<string> GenerateUniqueAsync()
    {
        for (int i = 0; i < 15; i++)
        {
            var adj = Adjectives[_random.Next(Adjectives.Length)];
            var noun = Nouns[_random.Next(Nouns.Length)];
            var digits = _random.Next(0, 10000).ToString("D4");
            var username = $"{adj}{noun}{digits}";

            if (!await _userRepo.IsUsernameTakenAsync(username))
                return username;
        }

        throw new InvalidOperationException("Failed to generate a unique username after 15 attempts.");
    }
}
