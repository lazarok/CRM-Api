using FluentValidation;

namespace CRM.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);
        
        RuleFor(x => x.Description)
            .MaximumLength(256);
        
        RuleFor(x => x.ProductStock)
            .GreaterThan(0);
        
        RuleFor(x => x.Price)
            .GreaterThan(0);
    }
}