using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuckyEQ.Contracts.DTOs;
using DuckyEQ.Contracts.Models;

namespace DuckyEQ.Contracts.Interfaces.Services
{
    public interface ISessionService
    {
        // Creates a GUID token stored in IMemoryCache with 30-minute TTL
        // Throws if userId already has an active session for this lesson
        string CreateSession(Guid userId, Guid lessonId);

        // Returns null if token doesn't exist or has expired
        LessonSession? GetSession(string token);

        // Called after CompleteLessonAsync to clean up the token
        void RemoveSession(string token);

    }
}

