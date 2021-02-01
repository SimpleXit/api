using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using SimpleX.Domain.Tasks.Base;

namespace SimpleX.Domain.Tasks.Helpers
{
    public static class TaskFinder
    {
        public static IEnumerable<Type> GetAllTasks()
        {
            var tasks = Assembly.GetExecutingAssembly()
                                .GetTypes()
                                .Where(t => t.IsClass
                                        && !t.IsAbstract
                                        && t.IsSubclassOf(typeof(SimpleTask)))
                                .ToList();
            return tasks;
        }
    }
}
