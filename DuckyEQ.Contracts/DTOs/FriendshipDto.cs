using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuckyEQ.Domain.Enums;

namespace DuckyEQ.Contracts.DTOs;

public record FriendshipDto(
    Guid Id,
    Guid RequesterId,
    Guid AddresseeId,
    FriendshipStatus Status,
    DateTime CreatedAt
);