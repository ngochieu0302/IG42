using System;
using System.Collections;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class WeeklyScheduleItem : BaseSimple
    {
        public int? WeeklyID { get; set; }
        public int? ScheduleID { get; set; }
        public int? AgencyID { get; set; }
        public decimal? DateCreated { get; set; }
        public string WeeklyName { get; set; }
        public string ScheduleName { get; set; }
        public decimal? ScheduleTimeStart { get; set; }
        public decimal? ScheduleTimeEnd { get; set; }
        public string CalendarName { get; set; }
        public bool IsCalender { get; set; }
        public string NameDayOff { get; set; }
        public bool IsDayOff { get; set; }
        public virtual DNUserBedDeskItem DNUserBedDeskItem { get; set; }
        //public virtual ICollection<DN_Calendar_Weekly_Schedule> DN_Calendar_Weekly_Schedule { get; set; }
        public virtual ScheduleItem ScheduleItem { get; set; }
        public virtual DNRolesItem DnRolesItem { get; set; }
        public IEnumerable<DNUserItem> ListDNUserItem { get; set; }
    }
    public class ModeWeeklyScheduleItem : BaseModelSimple
    {
        public Guid UserID { get; set; }
        public WeeklyItem Item { get; set; }
        public DateTime DateStart { get; set; }
        public IEnumerable<WeeklyScheduleItem> ListItem { get; set; }
    }
}
