using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuckyEQ.Contracts.DTOs;

namespace DuckyEQ.Contracts.Models;

public record PurchaseResponse(
    bool Success,
    int NewBalance,
    EquippedItems EquippedItems
);