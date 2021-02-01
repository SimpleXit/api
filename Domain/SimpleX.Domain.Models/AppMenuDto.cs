using System.Collections.Generic;

namespace SimpleX.Domain.Models
{
    public class AppMenuDto
    {
        public long Id { get; set; }
        public long? ParentID { get; set; }
        public string Name { get; set; }
        public string Caption { get; set; }
        public string View { get; set; }
        public string Args { get; set; }
        public bool IsActive { get; set; }
        public bool IsFavorite { get; set; }
    }
}
