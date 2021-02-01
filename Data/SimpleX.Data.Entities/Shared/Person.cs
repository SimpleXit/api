using SimpleX.Data.Entities.Base;
using SimpleX.Data.Entities.Interfaces;

namespace SimpleX.Data.Entities
{
    public abstract class Person : TrackEntity, IEntity
    {
        #region Public Properties

        public long Id { get; set; }

        //Personal info
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public string Function { get; set; }

        public string LanguageCode { get; set; }

        public PersonAddress Address { get; set; }
        public PersonContactInfo ContactInfo { get; set; }

        #endregion
    }

    public class CustomerPerson : Person
    {
        public long CustomerID { get; set; }
    }

    public class EmployeePerson : Person
    {
        public long EmployeeID { get; set; }
    }

    public class SupplierPerson : Person
    {
        public long SupplierID { get; set; }
    }
}
