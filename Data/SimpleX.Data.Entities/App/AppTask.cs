using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleX.Data.Entities.App
{
    public class AppTask
    {
        public string FullName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool IsEnabled { get; set; }

        public bool RunOnMonday { get; set; }
        public bool RunOnTuesday { get; set; }
        public bool RunOnWednesday { get; set; }
        public bool RunOnThursday { get; set; }
        public bool RunOnFriday { get; set; }
        public bool RunOnSaturday { get; set; }
        public bool RunOnSunday { get; set; }

        public bool RunOnHour00 { get; set; }
        public bool RunOnHour01 { get; set; }
        public bool RunOnHour02 { get; set; }
        public bool RunOnHour03 { get; set; }
        public bool RunOnHour04 { get; set; }
        public bool RunOnHour05 { get; set; }
        public bool RunOnHour06 { get; set; }
        public bool RunOnHour07 { get; set; }
        public bool RunOnHour08 { get; set; }
        public bool RunOnHour09 { get; set; }
        public bool RunOnHour10 { get; set; }
        public bool RunOnHour11 { get; set; }
        public bool RunOnHour12 { get; set; }
        public bool RunOnHour13 { get; set; }
        public bool RunOnHour14 { get; set; }
        public bool RunOnHour15 { get; set; }
        public bool RunOnHour16 { get; set; }
        public bool RunOnHour17 { get; set; }
        public bool RunOnHour18 { get; set; }
        public bool RunOnHour19 { get; set; }
        public bool RunOnHour20 { get; set; }
        public bool RunOnHour21 { get; set; }
        public bool RunOnHour22 { get; set; }
        public bool RunOnHour23 { get; set; }

        public DateTime? ExecutedAt { get; set; }
        public DateTime? SucceededAt { get; set; }
    }

    public class AppTaskLog
    {
        public string FullName { get; set; }
        public DateTime TimeStamp { get; set; }
        public int LogLevel { get; set; }
        public string Log { get; set; }
    }
}
