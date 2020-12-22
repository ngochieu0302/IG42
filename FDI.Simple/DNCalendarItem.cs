using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class DNCalendarItem : BaseSimple
    {
        public string Name { get; set; }
        public int? AgencyId { get; set; }
        public decimal? DateCreated { get; set; }
        public int? Sort { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsShow { get; set; }
        public string WeeklySchedule { get; set; }
        public string ListProductId { get; set; }
        public decimal? DateStart { get; set; }
        public decimal? DateEnd { get; set; }
        public string LstUserIds { get; set; }
        public string LstRoleIds { get; set; }
        public IEnumerable<DNCalendarWeeklyScheduleItem> DNCalendarWeeklySchedule { get; set; }
        public IEnumerable<WeeklyScheduleItem> WeeklyScheduleItems { get; set; }
        
        public IEnumerable<DNRolesItem> ListDnRolesItem { get; set; }
        public IEnumerable<DNUserItem> ListDnUserItem { get; set; }
        public virtual IEnumerable<BedDeskItem> ListDnBedDeskItem { get; set; }
     
    }

    public class CalendarItem : BaseSimple
    {
        public string Name { get; set; }
        public int? AgencyId { get; set; }
        public decimal? DateCreated { get; set; }
        public int? Sort { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsShow { get; set; }
        public string WeeklySchedule { get; set; }
        public decimal? DateStart { get; set; }
        public decimal? DateEnd { get; set; }

        public IEnumerable<CalendarWeeklyScheduleItem> DNCalendarWeeklySchedule { get; set; }
    }

    public class CheckInItem:BaseSimple
    {
        public int? Hms { get; set; }
        public int? Hme { get; set; }
    }

    public class RCalendarItem : BaseSimple
    {
        public string Name { get; set; }
        public int? AgencyId { get; set; }
        public decimal? DateCreated { get; set; }
        public int? Sort { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsShow { get; set; }
        public string WeeklySchedule { get; set; }
        public decimal? DateStart { get; set; }
        public decimal? DateEnd { get; set; }

        public IEnumerable<RCalendarWeeklyScheduleItem> DNCalendarWeeklySchedule { get; set; }
    }
    public class ModelDNCalendarItem : BaseModelSimple
    {
        public IEnumerable<DNCalendarItem> ListItem { get; set; }
        public IEnumerable<WeeklyScheduleItem> ListWeeklyScheduleItem { get; set; }
    }

    public class ModelDnUserCalendarItem 
    {
        public string DnUserId { get; set; }
        public string DnRolesId { get; set; }
    }
}
