using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.Interfaces.Services
{
    public interface IUsernameGenerator
    {
        // Generates a unique PascalCase username, e.g. SunnyDuck4832
        // Format: AdjectiveNounXXXX where XXXX is a 4-digit random number
        // Retries until a non-taken username is found (max 10 attempts)
        Task<string> GenerateUniqueAsync();

    }
}

