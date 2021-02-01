using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleX.Domain.Tasks.Helpers
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class TaskAttribute : Attribute
    {
        public TaskAttribute(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}
