using Microsoft.EntityFrameworkCore;
using System;

namespace SimpleX.Data
{
    public class DataHistory : AutoHistory
    {
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
