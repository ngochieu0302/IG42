using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
    public class TotalSalaryMonthItem : BaseSimple
    {
        public Guid? UserId { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public int? TotalDateCC { get; set; }
        public int? TotalSom { get; set; }
        public int? TotalMuon { get; set; }
        public decimal? DateUpdate { get; set; }
        public Guid? UserUpdate { get; set; }
        public int? FixedSalary { get; set; }
		public int? SalaryAward { get; set; }
		public int? TotalSchedule { get; set; }
        public string Note { get; set; }
        public virtual DNUserItem DN_Users { get; set; }
        public IEnumerable<DNCriteriaTotalitem> CriteriaList { get; set; }
	}

	public class ModelSalaryMonthDetailItem
	{
		public List<SalaryMonthDetailItem> ListItems { get; set; }
		public DateTime DateStart { get; set; }
	}

	public class ModelUserSalaryMonthDetailItem
	{
		public SalaryMonthDetailItem UserSalaryMonthItem { get; set; }
		public DateTime DateStart { get; set; }
	}


	public class SalaryMonthDetailItem : BaseSimple
	{
		public Guid? UserId { get; set; }
		public int? Month { get; set; }
		public int? Year { get; set; }
		public int? TotalDateCC { get; set; }
		public int? TotalSom { get; set; }
		public int? TotalMuon { get; set; }
		public int? FixedSalary { get; set; }
		public int? SalaryAward { get; set; }
		public string UserName { get; set; }
		public string Note { get; set; }
		public string LoweredUserName { get; set; }
		public IEnumerable<DNUserInRolesUserItem> RolesUserItems;
		public IEnumerable<DNCriteriaTotalitem> CriteriaList { get; set; }
		public IEnumerable<CDNTimeJobItem> DN_Time_Job { get; set; }
		public IEnumerable<CalendarItem> CalendarItems { get; set; }
		public IEnumerable<RCalendarItem> RCalendarItems { get; set; }
	}


}
