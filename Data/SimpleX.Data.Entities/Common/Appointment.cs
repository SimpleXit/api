using SimpleX.Data.Entities.Base;
using SimpleX.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;

namespace SimpleX.Data.Entities
{
    public class Appointment : TrackEntity, IEntity
    {
        #region Public Properties

        public long Id { get; set; }
        public byte StoreID { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public bool AllDay { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public long? CategoryID { get; set; }
        public int StatusID { get; set; }
        public long? CustomerID { get; set; }
        public long? EmployeeID { get; set; }
        public long? ProjectID { get; set; }

        #endregion

        #region Navigation Properties

        public Customer Customer { get; set; }
        public Employee Employee { get; set; }
        public Project Project { get; set; }
        public List<AppointmentNote> Notes { get; set; }

        #endregion

    }
}
