using SimpleX.Common.Enums;
using SimpleX.Data.Entities.Interfaces;
using System.Collections.Generic;

namespace SimpleX.Data.Entities
{
    public class Category : IEntity
    {
        #region Public Properties

        public long Id { get; set; }
        public long? ParentID { get; set; }
        public EntityType OwnerType { get; set; }
        public long CategoryNumber { get; set; }
        public string CategoryCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        #endregion

        #region Navigation Properties

        public List<CategoryTranslation> Translations { get; set; } = new List<CategoryTranslation>();

        #endregion
    }
}