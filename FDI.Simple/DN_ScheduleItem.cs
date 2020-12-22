using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class ScheduleItem : BaseSimple
    {
        public string Name { get; set; }
        public int? HoursStart { get; set; }
        public int? MinuteStart { get; set; }
        public int? HoursEnd { get; set; }
        public int? Soft { get; set; }
        public int? MinuteEnd { get; set; }
        public int? AgencyID { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsDelete { get; set; }
		public string WeeklyId { get; set; }
        public IEnumerable<WeeklyItem> ListWeeklyItem { get; set; }
        public IEnumerable<WeeklyScheduleItem> ListWeeklyScheduleItem { get; set; }
        
    }
    public class ModelScheduleItem : BaseModelSimple
    {
        public ScheduleItem Item { get; set; }
        public IEnumerable<ScheduleItem> ListItem { get; set; }
    }
}
