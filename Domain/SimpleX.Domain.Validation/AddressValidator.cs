using FluentValidation;
using SimpleX.Domain.Models.Shared;

namespace SimpleX.Domain.Validation
{
    public class AddressValidator : AbstractValidator<AddressDto>
    {
        public AddressValidator()
        {
            RuleFor(c => c.Name).MaximumLength(250);
            RuleFor(c => c.StreetAndNumber).NotNull().NotEmpty().MaximumLength(250);
            RuleFor(c => c.ZipCode).MaximumLength(25);
            RuleFor(c => c.City).MaximumLength(100);
            RuleFor(c => c.CountryCode).MaximumLength(3);
        }
    }
}
