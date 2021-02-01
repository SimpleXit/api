using SimpleX.Common.Enums;

namespace SimpleX.Data.Entities
{
    public abstract class Address
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string StreetAndNumber { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string CountryCode { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public AddressType AddressType { get; set; }
    }

    public class CustomerAddress : Address
    {
        public long? CustomerID { get; set; }
    }

    public class EmployeeAddress : Address
    {
        public long? EmployeeID { get; set; }
    }

    public class PersonAddress : Address
    {
        public long? PersonID { get; set; }
    }

    public class SupplierAddress : Address
    {
        public long? SupplierID { get; set; }
    }

    public class DocumentAssociateAddress : Address
    {
        public long? DocumentAssociateID { get; set; }
    }
}
