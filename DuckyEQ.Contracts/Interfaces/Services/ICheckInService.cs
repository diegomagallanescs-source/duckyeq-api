using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuckyEQ.Contracts.DTOs;
using DuckyEQ.Contracts.Models;

namespace DuckyEQ.Contracts.Interfaces.Services
{
    public interface ICheckInService
    {
        // Returns null if user has not checked in today (controller → 204)
        Task<CheckInDto?> GetTodayAsync(Guid userId);

        // Creates check-in. Throws AlreadyCheckedInException if today's row exists.
        Task<CheckInDto> CheckInAsync(Guid userId, CheckInRequest request);

    }
}

