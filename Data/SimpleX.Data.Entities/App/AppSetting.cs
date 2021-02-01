using SimpleX.Data.Entities.Interfaces;

namespace SimpleX.Data.Entities
{
    public class AppSetting : IEntity
    {

        #region Public Properties

        public long Id { get; set; }
        public string Category { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public string CurrentValue { get; set; }
        public string DefaultValue { get; set; }
        public string TypeOfValue { get; set; }
        public string ItemSource { get; set; }

        #endregion

    }
}
