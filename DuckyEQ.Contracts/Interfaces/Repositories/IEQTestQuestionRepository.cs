using DuckyEQ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.Interfaces.Repositories
{
    public interface IEQTestQuestionRepository
    {
        Task<IReadOnlyList<EQTestQuestion>> GetRandomSetAsync(int count);
        Task<IReadOnlyList<EQTestQuestion>> GetAllAsync();

    }
}

