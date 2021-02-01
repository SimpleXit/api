using FluentValidation;
using SimpleX.Domain.Models.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleX.Domain.Validation
{
    public class PersonValidator : AbstractValidator<PersonDto>
    {
        public PersonValidator()
        {
            RuleFor(c => c.FirstName).MaximumLength(100);
            RuleFor(c => c.LastName).NotNull().NotEmpty().MaximumLength(100);
            RuleFor(c => c.Prefix).MaximumLength(25);
            RuleFor(c => c.Suffix).MaximumLength(25);
            RuleFor(c => c.Function).MaximumLength(50);
            RuleFor(c => c.LanguageCode).NotNull().NotEmpty().MaximumLength(3);

            RuleFor(c => c.ContactInfoMail).MaximumLength(250).EmailAddress();
            RuleFor(c => c.ContactInfoTel).MaximumLength(100);
            RuleFor(c => c.ContactInfoFax).MaximumLength(100);
            RuleFor(c => c.ContactInfoMob).MaximumLength(100);

            RuleFor(c => c.AddressStreetAndNumber).MaximumLength(250);
            RuleFor(c => c.AddressZipCode).MaximumLength(25);
            RuleFor(c => c.AddressCity).MaximumLength(100);
            RuleFor(c => c.AddressCountryCode).MaximumLength(3);
        }
    }
}
