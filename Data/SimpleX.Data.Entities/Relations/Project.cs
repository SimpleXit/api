using SimpleX.Data.Entities.Base;
using System.Collections.Generic;

namespace SimpleX.Data.Entities
{
    public class Project : TrackEntity
    {
        #region Public Properties

        public long Id { get; set; }
        public byte StoreID { get; set; }
        public long? CustomerID { get; set; }

        #endregion

        #region Navigation Properties

        public Customer Customer { get; set; }
        public List<Appointment> Appointments { get; set; }
        public List<ProjectNote> Notes { get; set; }
        public List<ProjectTranslation> Translations { get; set; }

        #endregion
    }
}
