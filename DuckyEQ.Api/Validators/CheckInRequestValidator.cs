using DuckyEQ.Contracts.Models;
using DuckyEQ.Domain.Enums;
using FluentValidation;

namespace DuckyEQ.Api.Validators;

public class CheckInRequestValidator : AbstractValidator<CheckInRequest>
{
    public CheckInRequestValidator()
    {
        RuleFor(x => x.EmotionIds)
            .NotNull().WithMessage("emotionIds is required.")
            .NotEmpty().WithMessage("At least one emotion is required.")
            .Must(ids => ids.All(id => Enum.TryParse<Emotion>(id, ignoreCase: true, out _)))
            .WithMessage("One or more emotion values are invalid.");

        RuleFor(x => x.Phrase)
            .MaximumLength(120).WithMessage("Phrase must be 120 characters or fewer.")
            .When(x => x.Phrase is not null);
    }
}
