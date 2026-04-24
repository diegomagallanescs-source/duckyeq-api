using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.DTOs;
using DuckyEQ.Domain.Enums;

public record LessonContentDto(
    Guid Id,
    Pillar Pillar,
    int Level,
    string Title,
    string ContentJson
);
