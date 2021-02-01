using FluentValidation;
using SimpleX.Domain.Models.App;

namespace SimpleX.Domain.Validation
{
    public class AppUserValidator : AbstractValidator<AppUserDto>
    {
        public AppUserValidator()
        {
            RuleFor(c => c.Id).NotNull();
            RuleFor(c => c.Username).NotNull().NotEmpty().MaximumLength(50);
            RuleFor(c => c.FirstName).MaximumLength(100);
            RuleFor(c => c.LastName).MaximumLength(100);
            RuleFor(c => c.Mail).EmailAddress().MaximumLength(250);
            RuleFor(c => c.LanguageCode).MaximumLength(2);
        }
    }

    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(c => c.Username).NotEmpty().MaximumLength(50);
            RuleFor(c => c.Password).NotEmpty().MaximumLength(50);
        }
    }
}
