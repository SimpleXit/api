using System.Collections.Generic;
using SimpleX.Data.Entities.Base;
using SimpleX.Data.Entities.Interfaces;

namespace SimpleX.Data.Entities
{
    public class AppUser : TrackEntity, IEntity
    {
        #region Public Properties

        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string LanguageCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mail { get; set; }
        public byte StoreID { get; set; }
        public long? EmployeeID { get; set; }
        public List<AppUserRole> Roles { get; set; }

        #endregion
    }

    public class AppUserRole : TrackEntity, IEntity
    {
        public long Id { get; set; }
        public long UserID { get; set; }
        public string Role { get; set; }
    }
}
