using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleX.Data.Entities.Common
{
    public class Attribute
    {
        public long Id { get; set; }
        public string AttributeCode { get; set; }
        public string UnitOfMeasure { get; set; }
        public long? Sequence { get; set; }

        public List<AttributeTranslation> Translations { get; set; }
    }
}
