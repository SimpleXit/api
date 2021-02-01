using SimpleX.Domain.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleX.Domain.Models.Common
{
    public class AttributeDto : Dto<AttributeDto>
    {
        public long Id { get; set; }
        public long AttributeID { get; set; }
        public string AttributeCode { get; set; }
        public long? AttributeSequence { get; set; }
        public string AttributeName { get; set; }
        public string AttributeDescription { get; set; }
        public string AttributeUnitOfMeasure { get; set; }

        public string Value { get; set; }
    }
}
