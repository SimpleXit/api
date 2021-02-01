using SimpleX.Data.Entities.Interfaces;
using System.Collections.Generic;
using SimpleX.Data.Entities.Base;

namespace SimpleX.Data.Entities.App
{
    public class AppMenu : IEntity
    {
        #region Public Properties

        public long Id { get; set; }
        public long? ParentID { get; set; }
        public string Name { get; set; }
        public string View { get; set; }
        public string Args { get; set; }
        public bool IsActive { get; set; }

        #endregion

        #region Navigation Properties

        public List<AppMenu> SubMenus { get; set; } = new List<AppMenu>();
        public List<AppMenuTranslation> Translations { get; set; } = new List<AppMenuTranslation>();

        #endregion

    }

    public class AppPermission : TrackEntity, IEntity
    {
        public long Id { get; set; }
        public string Role { get; set; }
        public string View { get; set; }
        public string Action { get; set; }
        public bool Allow { get; set; }
        public bool Deny { get; set; }
    }
}
