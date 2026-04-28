using FluentValidation;
using StockNexusAPI.Domain.Entities;

namespace StockNexusAPI.Application.Common.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(256);
        }
    }
}
