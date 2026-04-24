using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckyEQ.Contracts.DTOs;
using DuckyEQ.Domain.Enums;
using System.Threading.Tasks;
using DuckyEQ.Contracts.Models;

namespace DuckyEQ.Contracts.Interfaces.Services
{
    internal interface ICooldownService
    {
        Task<CooldownStatus> GetPillarStatusAsync(Guid userId, Pillar pillar);
        Task<bool> CanStartNewLessonAsync(Guid userId, Pillar pillar);

    }
}
