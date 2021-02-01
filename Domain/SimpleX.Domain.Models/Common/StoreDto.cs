using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleX.Domain.Models.Common
{
    public class StoreDto
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public string TaxNumber { get; set; }
        public string IBAN { get; set; }
        public string BIC { get; set; }

        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string CountryCode { get; set; }

        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Mob { get; set; }
        public string Mail { get; set; }
        public string Web { get; set; }
    }
}
