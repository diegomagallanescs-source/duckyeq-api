using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.Models;

public record AuthResult(
    string Token,
    string Username,
    string KnownAs,
    Guid UserId
);
