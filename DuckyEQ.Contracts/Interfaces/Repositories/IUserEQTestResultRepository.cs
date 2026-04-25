using DuckyEQ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.Interfaces.Repositories
{
    public interface IUserEQTestResultRepository
    {
        Task<UserEQTestResult> CreateAsync(UserEQTestResult result);
        Task<UserEQTestResult?> GetBestByUserAsync(Guid userId);

    }
}

