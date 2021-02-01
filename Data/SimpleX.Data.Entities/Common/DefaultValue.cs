using SimpleX.Common.Enums;
using SimpleX.Data.Entities.Base;
using SimpleX.Data.Entities.Interfaces;

namespace SimpleX.Data.Entities
{
    public class DefaultValue : TrackEntity, IEntity
    {
        public long Id { get; set; }
        public EntityType EntityType { get; set; }
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
        public string PropertyType { get; set; }
    }
}
