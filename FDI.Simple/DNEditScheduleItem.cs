using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class EditScheduleItem : BaseSimple
    {
        public Guid? UserId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public decimal? Date { get; set; }
        public int? ScheduleID { get; set; }
        public string ScheduleName { get; set; }
        public decimal? Datecreated { get; set; }
        public Guid? UserChangeId { get; set; }
        public string UserNameChange { get; set; }
        public int? AgencyID { get; set; }
        public int? Type { get; set; }
        public int? DayOffId { get; set; }
        public string FullName { get; set; }       
        //public int countorder { get; set; }

        public decimal? DateCreated { get; set; }

        public virtual ScheduleItem ScheduleItem { get; set; }
        //public virtual DN_Users DN_Users1 { get; set; }
    }
    public class CkeckEditScheduleItem
    {
        public decimal? Date { get; set; }
        public bool? Check { get; set; }
    }
    public class CEditScheduleItem : BaseSimple
    {
        public Guid? UserId { get; set; }
        public decimal? Date { get; set; }
        public string Name { get; set; }
        public int? ScheduleID { get; set; }
        public Guid? UserChangeId { get; set; }
    }
    public class ModelEditScheduleItem : BaseModelSimple
    {
        public EditScheduleItem Item { get; set; }
        public IEnumerable<EditScheduleItem> ListItem { get; set; }
    }
}
