using FluentValidation;
using SimpleX.Domain.Models.Base;

namespace SimpleX.Domain.Models.Shared
{
    public class TranslationDto : Dto<TranslationDto>
    {
        private long _id;
        private string _languageCode;
        private string _name;
        private string _description;

        public long Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        public string LanguageCode
        {
            get => _languageCode;
            set => SetProperty(ref _languageCode, value);
        }
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public override string ToString()
        {
            return $"{LanguageCode}: {Name} | {Description}".Trim();
        }
    }


}
