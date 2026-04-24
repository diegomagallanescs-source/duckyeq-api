using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.Models;

public record PurchaseRequest(
    Guid ShopItemId
);