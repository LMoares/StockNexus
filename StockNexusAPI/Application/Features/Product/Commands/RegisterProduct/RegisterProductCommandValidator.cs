using FluentValidation;

namespace StockNexusAPI.Application.Features.Product.Commands.RegisterProduct
{
    public class RegisterProductCommandValidator : AbstractValidator<RegisterProductCommand>
    {
        public RegisterProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is Required.")
                .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters in length");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Product description is Required.")
                .MaximumLength(500).WithMessage("Product description cannot exceed 500 characters in length");
            RuleFor(x => x.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than zero.");
        }
    }
}
