using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class DNCalendarWeeklyScheduleItem : BaseSimple
    {
        public int? CalenderID { get; set; }
        public int? MWSID { get; set; }
        public int? AgencyID { get; set; }
        public int? BedID { get; set; }
        public string NameBed { get; set; }

        public virtual DNCalendarItem DN_Calendar { get; set; }
        public virtual DNUserBedDeskItem DNUserBedDeskItem { get; set; }
        public virtual WeeklyScheduleItem DN_Weekly_Schedule { get; set; }
    }
    public class RCalendarWeeklyScheduleItem : BaseSimple
    {
        public int? MWSID { get; set; }
        public int? Weekid { get; set; }
    }
    public class CalendarWeeklyScheduleItem : BaseSimple
    {
        public int? MWSID { get; set; }
        public int? Weekid { get; set; }
    }

    public class ModelDNCalendarWeeklyScheduleItem : BaseModelSimple
    {
        public IEnumerable<DNCalendarWeeklyScheduleItem> ListItem { get; set; }
    }
}
