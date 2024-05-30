using FluentValidation;

namespace CRM.Application.Features.Register.Commands.RegisterAdmin;

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
            .MinimumLength(4)
            .MaximumLength(16);
        
        RuleFor(x => x.OrganizationName)
            .NotEmpty()
            .MaximumLength(50);
    }
}