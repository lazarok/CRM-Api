using FluentValidation;

namespace CRM.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50)
            .EmailAddress();
        
        RuleFor(x => x.Description)
            .MaximumLength(256);
        
        RuleFor(x => x.Description)
            .MaximumLength(256);
        
        RuleFor(x => x.ProductStock)
            .GreaterThan(0);
        
        RuleFor(x => x.Price)
            .GreaterThan(0);
    }
}