using FluentValidation;

namespace CRM.Application.Features.Register.Commands.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(50)
            .EmailAddress();
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(4)
            .MaximumLength(16);
    }
}