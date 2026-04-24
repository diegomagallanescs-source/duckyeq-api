using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.DTOs
{
    public record EQTestQuestionDto(
        Guid Id,
        string QuestionText,
        string OptionA,
        string OptionB,
        string OptionC,
        string OptionD
    // CorrectOption intentionally excluded from DTO
    );

}
