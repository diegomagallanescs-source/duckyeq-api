using DuckyEQ.Contracts.Models;
using FluentValidation;

namespace DuckyEQ.Api.Validators;

public class UpdateKnownAsRequestValidator : AbstractValidator<UpdateKnownAsRequest>
{
    public UpdateKnownAsRequestValidator()
    {
        RuleFor(x => x.KnownAs)
            .NotEmpty().WithMessage("Display name is required.")
            .MinimumLength(2).WithMessage("Display name must be at least 2 characters.")
            .MaximumLength(10).WithMessage("Display name must be 10 characters or fewer.")
            .Matches(@"^[a-zA-Z0-9 ]+$").WithMessage("Display name may only contain letters, numbers, and spaces.");
    }
}
