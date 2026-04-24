using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DuckyEQ.Contracts.Models;

using DuckyEQ.Domain.Enums;

public record RegisterRequest(
    string Email,
    string Password,
    DuckCharacter DuckCharacter,
    string KnownAs
);