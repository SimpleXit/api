using SimpleX.Common.Enums;
using System;
using System.Resources;

namespace SimpleX.Common.Extensions
{
    public static class EnumExtensions
    {
        private static ResourceManager rm;

        private static ResourceManager GetResourceManager()
        {
            if (rm == null)
                rm = new ResourceManager(typeof(EnumResource));

            return rm;
        }

        public static string GetDisplayName(this Enum e)
        {
            var resourceDisplayName = GetResourceManager().GetString(e.GetType().Name + "." + e);

            return string.IsNullOrWhiteSpace(resourceDisplayName) ? string.Format("[[{0}]]", e) : resourceDisplayName;
        }
    }
}
