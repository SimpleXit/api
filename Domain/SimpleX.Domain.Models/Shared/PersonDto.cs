using SimpleX.Common.Extensions;
using SimpleX.Domain.Models.Base;

namespace SimpleX.Domain.Models.Shared
{
    public class PersonDto : Dto<PersonDto>
    {
        private string _addressCountryCode;
        private string _addressCity;
        private string _addressZipCode;
        private string _addressStreetAndNumber;
        private string _contactInfoMail;
        private string _contactInfoMob;
        private string _contactInfoFax;
        private string _contactInfoTel;
        private string _languageCode;
        private string _prefix;
        private string _suffix;
        private string _function;
        private long _id;
        private string _firstName;
        private string _lastName;

        public long Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }
        public string FullName => $"{Prefix} {FirstName} {LastName} {Suffix}".RemoveDoubleSpaces();
        public string Prefix
        {
            get => _prefix;
            set => SetProperty(ref _prefix, value);
        }
        public string Suffix
        {
            get => _suffix;
            set => SetProperty(ref _suffix, value);
        }
        public string Function
        {
            get => _function;
            set => SetProperty(ref _function, value);
        }
        public string LanguageCode
        {
            get => _languageCode;
            set => SetProperty(ref _languageCode, value);
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

        public long AddressId { get; set; }
        public string AddressStreetAndNumber
        {
            get => _addressStreetAndNumber;
            set => SetProperty(ref _addressStreetAndNumber, value);
        }
        public string AddressZipCode
        {
            get => _addressZipCode;
            set => SetProperty(ref _addressZipCode, value);
        }
        public string AddressCity
        {
            get => _addressCity;
            set => SetProperty(ref _addressCity, value);
        }
        public string AddressCountryCode
        {
            get => _addressCountryCode;
            set => SetProperty(ref _addressCountryCode, value);
        }
        public double? AddressLatitude { get; set; }
        public double? AddressLongitude { get; set; }

        public override string ToString()
        {
            return FullName;
        }
    }

   
}
