using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SimpleX.Domain.Tasks.Helpers
{
    public static class ServiceFinder
    {
        public static IEnumerable<Type> GetAllServices()
        {
            var services = Assembly.GetExecutingAssembly()
                                   .GetTypes()
                                   .Where(t => t.IsClass
                                           && !t.IsAbstract)
                                   .ToList();
            return services;
        }

        public static Assembly GetServiceAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }
    }
}
