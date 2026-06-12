using FluentValidation;

namespace StockNexusAPI.Application.Features.Request.Queries.GetRequestById
{
    public class GetProductRequestByIdQueryValidator : AbstractValidator<GetProductRequestByIdQuery>
    {
        public GetProductRequestByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Request ID must be greater than 0.");
        }
    }
}
