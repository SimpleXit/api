using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleX.Domain.Tasks.Helpers
{
    public static class TaskExtensions
    {
        public static string GetDisplayName(this Type taskType)
        {
            return taskType.GetTaskAttribute()?.Name;
        }

        public static string GetDescription(this Type taskType)
        {
            return taskType.GetTaskAttribute()?.Description;
        }

        private static TaskAttribute GetTaskAttribute(this Type taskType)
        {
            try
            {
                return (TaskAttribute)Attribute.GetCustomAttribute(taskType, typeof(TaskAttribute));
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
