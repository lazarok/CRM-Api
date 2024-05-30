using FluentValidation;

namespace CRM.Application.Fetures.Auth.RegisterAdmin;

public class RegisterAdminCommandValidator : AbstractValidator<RegisterAdminCommand>
{
    public RegisterAdminCommandValidator()
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
            .MaximumLength(16);
    }
}