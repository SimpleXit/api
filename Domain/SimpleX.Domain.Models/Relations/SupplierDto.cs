using SimpleX.Common.Enums;
using SimpleX.Common.Extensions;
using SimpleX.Domain.Models.Base;
using SimpleX.Domain.Models.Shared;
using System;
using System.ComponentModel;
using System.Linq;

namespace SimpleX.Domain.Models.Relations
{
    public class SupplierDto : Dto<SupplierDto>
    {
        private long _id;
        private long _number;
        private long? _categoryID;
        private string _name;
        private string _taxNumber;
        private TaxCode _taxCode;
        private string _iban;
        private string _bic;
        private TermOfPayment _termOfPayment;
        private string _currency;
        private string _languageCode;
        private string _shortName;

        private DateTime? _createdOn;
        private string _createdBy;
        private DateTime? _updatedOn;
        private string _updatedBy;
        private byte[] _rowVersion;
        private string _contactInfoMail;
        private string _contactInfoTel;
        private string _contactInfoFax;
        private string _contactInfoMob;
        private string _contactInfoWeb;

        public long Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        public long Number
        {
            get => _number;
            set => SetProperty(ref _number, value);
        }
        public long? CategoryID
        {
            get => _categoryID ;
            set => SetProperty(ref _categoryID, value);
        }
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        public string LanguageCode
        {
            get => _languageCode;
            set => SetProperty(ref _languageCode, value);
        }
        public string ShortName
        {
            get => _shortName;
            set => SetProperty(ref _shortName, value);
        }
        public string FullName => $"{Name}".RemoveDoubleSpaces();
        public string TaxNumber
        {
            get => _taxNumber;
            set => SetProperty(ref _taxNumber, value);
        }
        public TaxCode TaxCode
        {
            get => _taxCode;
            set => SetProperty(ref _taxCode, value);
        }
        public string IBAN
        {
            get => _iban;
            set => SetProperty(ref _iban, value?.Trim());
        }
        public string BIC
        {
            get => _bic;
            set => SetProperty(ref _bic, value?.Trim());
        }
        public TermOfPayment TermOfPayment
        {
            get => _termOfPayment;
            set => SetProperty(ref _termOfPayment, value);
        }
        public string Currency
        {
            get => _currency;
            set => SetProperty(ref _currency, value);
        }

        public long ContactInfoId { get; set; }
        public string ContactInfoTel
        {
            get => _contactInfoTel;
            set => SetProperty(ref _contactInfoTel, value);
        }
        public string ContactInfoFax
        {
            get => _contactInfoFax;
            set => SetProperty(ref _contactInfoFax, value);
        }
        public string ContactInfoMob
        {
            get => _contactInfoMob;
            set => SetProperty(ref _contactInfoMob, value);
        }
        public string ContactInfoMail
        {
            get => _contactInfoMail;
            set => SetProperty(ref _contactInfoMail, value);
        }
        public string ContactInfoWeb
        {
            get => _contactInfoWeb;
            set => SetProperty(ref _contactInfoWeb, value);
        }
        public byte[] ContactInfoRowVersion { get; set; }

        public DateTime? CreatedOn
        {
            get => _createdOn;
            set => SetProperty(ref _createdOn, value);
        }
        public string CreatedBy
        {
            get => _createdBy;
            set => SetProperty(ref _createdBy, value);
        }
        public DateTime? UpdatedOn
        {
            get => _updatedOn;
            set => SetProperty(ref _updatedOn, value);
        }
        public string UpdatedBy
        {
            get => _updatedBy;
            set => SetProperty(ref _updatedBy, value);
        }
        public byte[] RowVersion
        {
            get => _rowVersion;
            set => SetProperty(ref _rowVersion, value);
        }

        public BindingList<AddressDto> Addresses { get; set; } = new BindingList<AddressDto>();
        public BindingList<AppointmentDto> Appointments { get; set; } = new BindingList<AppointmentDto>();
        public BindingList<AttachmentDto> Attachments { get; set; } = new BindingList<AttachmentDto>();
        public BindingList<NoteDto> Notes { get; set; } = new BindingList<NoteDto>();
        public BindingList<PersonDto> Persons { get; set; } = new BindingList<PersonDto>();
        public BindingList<CommunicationDto> Communications { get; set; } = new BindingList<CommunicationDto>();

        public override string ToString()
        {
            return FullName;
        }
    }



    public class SupplierSearchResultDto
    {
        public long Id { get; set; }
        public string Number { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TaxNumber { get; set; }
        public byte StoreID { get; set; }
        public string FullName => $"{FirstName} {LastName}".Trim();
    }

    public class SupplierFilterDto
    {
        public string Number { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string TaxNumber { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
    }
}
