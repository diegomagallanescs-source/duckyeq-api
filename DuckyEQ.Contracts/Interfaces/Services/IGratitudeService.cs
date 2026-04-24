using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuckyEQ.Contracts.DTOs;
using DuckyEQ.Contracts.Models;

namespace DuckyEQ.Contracts.Interfaces.Services
{
    internal interface IGratitudeService
    {
        // Awards 10 coins on first entry of the day only
        Task<GratitudeResponse> AddEntryAsync(Guid userId, AddGratitudeRequest request);

        Task<IReadOnlyList<GratitudeEntryDto>> GetAllPagedAsync(
            Guid userId, int page, int pageSize);

        // Returns null if < 3 entries exist (Pick Me Up guard)
        Task<GratitudeEntryDto?> GetRandomAsync(Guid userId);

        Task<GratitudeStreakDto> GetStreakAsync(Guid userId);

    }
}
