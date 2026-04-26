using DuckyEQ.Contracts.Models;
using FluentValidation;

namespace DuckyEQ.Api.Validators;

public class UpdateDuckCharacterRequestValidator : AbstractValidator<UpdateDuckCharacterRequest>
{
    public UpdateDuckCharacterRequestValidator()
    {
        RuleFor(x => x.DuckCharacter)
            .IsInEnum().WithMessage("Duck character must be Ducky or Daisy.");
    }
}
