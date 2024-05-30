using FluentValidation;

namespace CRM.Application.Features.Auth.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(50)
            .EmailAddress();
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .MaximumLength(64);
    }
}