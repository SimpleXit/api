using SimpleX.Data.Entities.Common;

namespace SimpleX.Data.Entities.Assets
{
    public class ProductAttribute
    {
        public long Id { get; set; }
        public long ProductID { get; set; }
        public long AttributeID { get; set; }
        public string Value { get; set; }

        public Attribute Attribute { get; set; }
    }
}
