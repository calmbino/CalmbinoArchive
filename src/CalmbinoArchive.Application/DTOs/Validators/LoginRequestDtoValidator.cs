using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CalmbinoArchive.Application.DTOs.Validators;

public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestDtoValidator(IOptions<IdentityOptions> identityOptions, ILogger<LoginRequestDtoValidator> logger)
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email format");

        var passwordOptions = identityOptions.Value.Password;
        logger.LogInformation("Password valid coditions >> {@PasswordOptions}", passwordOptions);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(passwordOptions.RequiredLength)
            .WithMessage($"Password must be at least {passwordOptions.RequiredLength} characters long.");

        if (passwordOptions.RequireDigit)
        {
            RuleFor(x => x.Password)
                .Matches("[0-9]")
                .WithMessage("Password must contain at least one digit.");
        }

        if (passwordOptions.RequireUppercase)
        {
            RuleFor(x => x.Password)
                .Matches("[A-Z]")
                .WithMessage("Password must contain at least one uppercase letter.");
        }

        if (passwordOptions.RequireLowercase)
        {
            RuleFor(x => x.Password)
                .Matches("[a-z]")
                .WithMessage("Password must contain at least one lowercase letter.");
        }

        if (passwordOptions.RequireNonAlphanumeric)
        {
            RuleFor(x => x.Password)
                .Matches("[^a-zA-Z0-9]")
                .WithMessage("Password must contain at least one special character.");
        }

        RuleFor(x => x.Password)
            .Must(password => password.Distinct()
                                      .Count() >= passwordOptions.RequiredUniqueChars)
            .WithMessage($"Password must have at least {passwordOptions.RequiredUniqueChars} unique characters.");
    }
}