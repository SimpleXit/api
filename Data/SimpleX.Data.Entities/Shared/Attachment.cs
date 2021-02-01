using SimpleX.Data.Entities.Base;
using SimpleX.Data.Entities.Interfaces;

namespace SimpleX.Data.Entities
{
    public abstract class Attachment : TrackEntity, IEntity
    {
        #region Public Properties

        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }

        #endregion
    }

    public class CustomerAttachment : Attachment
    {
        public long CustomerID { get; set; }
    }

    public class DocumentAttachment : Attachment
    {
        public long DocumentID { get; set; }
    }

    public class EmployeeAttachment : Attachment
    {
        public long EmployeeID { get; set; }
    }

    public class ProductAttachment : Attachment
    {
        public long ProductID { get; set; }
    }

    public class ProjectAttachment : Attachment
    {
        public long ProjectID { get; set; }
    }

    public class SupplierAttachment : Attachment
    {
        public long SupplierID { get; set; }
    }
}
