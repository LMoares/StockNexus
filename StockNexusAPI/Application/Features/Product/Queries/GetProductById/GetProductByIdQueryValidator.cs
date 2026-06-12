using FluentValidation;

namespace StockNexusAPI.Application.Features.Product.Queries.GetProductById
{
    public class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
    {
        public GetProductByIdQueryValidator() 
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Product ID must be greater than 0.");
        }
    }
}
