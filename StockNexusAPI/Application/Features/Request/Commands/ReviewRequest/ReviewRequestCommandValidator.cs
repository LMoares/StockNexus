using FluentValidation;

namespace StockNexusAPI.Application.Features.Request.Commands.ReviewRequest
{
    public class ReviewRequestCommandValidator : AbstractValidator<ReviewRequestCommand>
    {
        public ReviewRequestCommandValidator()
        {
            RuleFor(x => x.RequestId)
                .GreaterThan(0).WithMessage("RequestId must be greater than 0.");
            RuleFor(x => x.Remarks)
                .MaximumLength(500).WithMessage("Remarks cannot exceed 500 characters in length.");
        }
    }
}
