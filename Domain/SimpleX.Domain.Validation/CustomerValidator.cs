using FluentValidation;
using SimpleX.Domain.Models.Relations;
using SimpleX.Domain.Validation.Helpers;

namespace SimpleX.Domain.Validation
{
    public class CustomerValidator : AbstractValidator<CustomerDto>
    {
        public CustomerValidator()
        {
            RuleFor(c => c.Number).NotNull().NotEmpty().NotEqual(0);
            RuleFor(c => c.FirstName).MaximumLength(100);
            RuleFor(c => c.LastName).NotNull().NotEmpty().MaximumLength(100);
            RuleFor(c => c.ShortName).MaximumLength(50);
            RuleFor(c => c.Prefix).MaximumLength(25);
            RuleFor(c => c.Suffix).MaximumLength(25);
            RuleFor(c => c.LanguageCode).NotNull().NotEmpty().MaximumLength(3);
            RuleFor(c => c.TaxNumber).IsValidTaxNumber(c => c.TaxCode).MaximumLength(25);
            RuleFor(c => c.Currency).NotNull().NotEmpty().MaximumLength(3);
            RuleFor(c => c.IBAN).IsValidIban().MaximumLength(50);
            RuleFor(c => c.BIC).MaximumLength(25);

            RuleFor(c => c.ContactInfoMail).MaximumLength(250).EmailAddress();
            RuleFor(c => c.ContactInfoTel).MaximumLength(100);
            RuleFor(c => c.ContactInfoFax).MaximumLength(100);
            RuleFor(c => c.ContactInfoMob).MaximumLength(100);
            RuleFor(c => c.ContactInfoWeb).MaximumLength(100).IsValidUri();

            RuleForEach(c => c.Addresses).SetValidator(new AddressValidator());
            RuleForEach(c => c.Communications).SetValidator(new CommunicationValidator());
            RuleForEach(c => c.Attachments).SetValidator(new AttachmentValidator());
            RuleForEach(c => c.Notes).SetValidator(new NoteValidator());
            RuleForEach(c => c.Persons).SetValidator(new PersonValidator());
        }
    }
}
