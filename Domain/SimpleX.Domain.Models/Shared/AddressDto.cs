using SimpleX.Common.Enums;
using SimpleX.Common.Extensions;
using SimpleX.Domain.Models.Base;

namespace SimpleX.Domain.Models.Shared
{
    public class AddressDto : Dto<AddressDto>
    {
        private long _id;
        private string _name;
        private string _streetAndNumber;
        private string _zipCode;
        private string _city;
        private string _countryCode;
        private double? _latitude;
        private double? _longitude;
        private AddressType _addressType;

        public long Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        public string StreetAndNumber
        {
            get => _streetAndNumber;
            set => SetProperty(ref _streetAndNumber, value);
        }
        public string ZipCode
        {
            get => _zipCode;
            set => SetProperty(ref _zipCode, value);
        }
        public string City
        {
            get => _city;
            set => SetProperty(ref _city, value);
        }
        public string CountryCode
        {
            get => _countryCode;
            set => SetProperty(ref _countryCode, value);
        }
        public double? Latitude
        {
            get => _latitude;
            set => SetProperty(ref _latitude, value);
        }
        public double? Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }
        public AddressType AddressType
        {
            get => _addressType;
            set => SetProperty(ref _addressType, value);
        }

        public override string ToString()
        {
            return $"{StreetAndNumber}, {CountryCode}{ZipCode} {City}".RemoveDoubleSpaces();
        }
    }


}
