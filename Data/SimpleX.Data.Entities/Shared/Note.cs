using SimpleX.Data.Entities.Base;
using SimpleX.Data.Entities.Interfaces;

namespace SimpleX.Data.Entities
{
    public abstract class Note : TrackEntity, IEntity
    {
        #region Public Properties

        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        #endregion
    }

    public class AppointmentNote : Note
    {
        public long AppointmentID { get; set; }
    }

    public class CustomerNote : Note
    {
        public long CustomerID { get; set; }
    }

    public class DocumentNote : Note
    {
        public long DocumentID { get; set; }
    }

    public class EmployeeNote : Note
    {
        public long EmployeeID { get; set; }
    }

    public class ProductNote : Note
    {
        public long ProductID { get; set; }
    }

    public class ProjectNote : Note
    {
        public long ProjectID { get; set; }
    }

    public class SupplierNote : Note
    {
        public long SupplierID { get; set; }
    }
}
