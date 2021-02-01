using SimpleX.Common.Enums;
using SimpleX.Data.Entities.Base;
using SimpleX.Data.Entities.Interfaces;
using System;

namespace SimpleX.Data.Entities
{
    public abstract class Communication : TrackEntity, IEntity
    {
        #region Public Properties

        public long Id { get; set; }
        public long? EmployeeID { get; set; }

        public DateTime CommunicationDate { get; set; }
        public CommunicationType CommunicationType { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public string Correspondents { get; set; }

        #endregion

        #region Navigation Properties

        public Employee Employee { get; set; }

        #endregion
    }

    public class CustomerCommunication : Communication
    {
        public long CustomerID { get; set; }
    }

    public class SupplierCommunication : Communication
    {
        public long SupplierID { get; set; }
    }
}
