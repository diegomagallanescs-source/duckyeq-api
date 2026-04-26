using DuckyEQ.Contracts.Models;
using DuckyEQ.Domain.Enums;
using FluentValidation;

namespace DuckyEQ.Api.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.");

        RuleFor(x => x.KnownAs)
            .NotEmpty().WithMessage("Display name is required.")
            .MinimumLength(2).WithMessage("Display name must be at least 2 characters.")
            .MaximumLength(10).WithMessage("Display name must be 10 characters or fewer.")
            .Matches(@"^[a-zA-Z0-9 ]+$").WithMessage("Display name may only contain letters, numbers, and spaces.");

        RuleFor(x => x.DuckCharacter)
            .IsInEnum().WithMessage("Duck character must be Ducky or Daisy.");
    }
}
