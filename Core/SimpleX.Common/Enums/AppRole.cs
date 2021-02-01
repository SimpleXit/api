using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleX.Common.Enums
{
    public static class AppRole
    {
        public static readonly string Admin = "Admin";
        public static readonly string SecurityAdmin = "Admin";
        public static readonly string Manager = "Manager";
        public static readonly string SuperUser = "SuperUser";
        public static readonly string User = "User";

        public static readonly string[] All = { "Admin", "SecurityAdmin", "Manager", "SuperUser", "User" };
    }
}
