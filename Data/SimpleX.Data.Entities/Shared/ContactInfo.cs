using SimpleX.Data.Entities.Base;
using SimpleX.Data.Entities.Interfaces;

namespace SimpleX.Data.Entities
{
    public class ContactInfo : TrackEntity, IEntity
    {
        public long Id { get; set; }
        public string Mail { get; set; }
        public string Tel { get; set; }
        public string Mob { get; set; }
        public string Fax { get; set; }
        public string Web { get; set; }
    }

    public class CustomerContactInfo : ContactInfo
    {
        public long? CustomerID { get; set; }
    }

    public class EmployeeContactInfo : ContactInfo
    {
        public long? EmployeeID { get; set; }
    }

    public class SupplierContactInfo : ContactInfo
    {
        public long? SupplierID { get; set; }
    }

    public class PersonContactInfo : ContactInfo
    {
        public long? PersonID { get; set; }
    }

    public class DocumentAssociateContactInfo : ContactInfo
    {
        public long? DocumentAssociateID { get; set; }
    }
}
