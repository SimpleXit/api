using SimpleX.Common.Enums;
using SimpleX.Domain.Models.Base;
using System.Collections.Generic;

namespace SimpleX.Domain.Models.Shared
{
    public class CategoryDto : Dto<CategoryDto>
    {

        private EntityType _ownerType;
        private long _id;
        private long? _parentID;
        private string _name;
        private long _number;
        private string _code;
        private string _description;
        private List<TranslationDto> _translations;

        public CategoryDto()
        {

        }

        public CategoryDto(EntityType ownerType)
        {
            _ownerType = ownerType;
        }

        public EntityType OwnerType
        {
            get => _ownerType;
            set => SetProperty(ref _ownerType, value);
        }

        public long Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        public long? ParentID
        {
            get => _parentID;
            set => SetProperty(ref _parentID, value);
        }
        public long CategoryNumber
        {
            get => _number;
            set => SetProperty(ref _number, value);
        }
        public string CategoryCode
        {
            get => _code;
            set => SetProperty(ref _code, value);
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
        public List<TranslationDto> Translations
        {
            get => _translations ?? new List<TranslationDto>();
            set => SetProperty(ref _translations, value);
        }

        public override string ToString()
        {
            return string.Join(" ", this.CategoryCode, this.Name).Trim();
        }
    }
}
