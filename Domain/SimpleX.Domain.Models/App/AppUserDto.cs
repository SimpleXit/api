using SimpleX.Domain.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimpleX.Domain.Models.App
{
    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AppUserDto : Dto<AppUserDto>
    {
        private long _id;
        private string _username;
        private string _firstName;
        private string _lastName;
        private string _mail;
        private string _languageCode;
        private byte _storeID;

        public long Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
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
        public string Mail
        {
            get => _mail;
            set => SetProperty(ref _mail, value);
        }
        public string LanguageCode
        {
            get => _languageCode;
            set => SetProperty(ref _languageCode, value);
        }
        public byte StoreID
        {
            get => _storeID;
            set => SetProperty(ref _storeID, value);
        }

        public List<string> Roles { get; set; } = new List<string>();

        public override string ToString()
        {
            return $"{Username} ({Id})";
        }
    }
}
