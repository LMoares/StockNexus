using FluentValidation;

namespace StockNexusAPI.Application.Features.Product.Queries.GetProducts
{
    public class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
    {
        public GetProductsQueryValidator() 
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("Page number must be greater than 0.");
            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Page size cannot exceed 100.");
            RuleFor(x => x.SearchTerm)
                .MaximumLength(100).WithMessage("Search term cannot exceed 100 characters in length.");
        }
    }
}
