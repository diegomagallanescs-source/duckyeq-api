using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuckyEQ.Domain.Enums;

namespace DuckyEQ.Contracts.Models;

public record AddGratitudeRequest(
    string Text,
    GratitudeCategory Category
);
