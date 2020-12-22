using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class WeeklyItem : BaseSimple
    {
        public string Name { get; set; }
        public int? Sort { get; set; }
        public int? AgencyID { get; set; }
        public bool? IsShow { get; set; }
        public string ScheduleID { get; set; }
        public IEnumerable<ScheduleItem> ListScheduleItem { get; set; }
    }
    public class ModelWeeklyItem : BaseModelSimple
    {
        public WeeklyItem Item { get; set; }
        public IEnumerable<WeeklyItem> ListItem { get; set; }
    }
}
