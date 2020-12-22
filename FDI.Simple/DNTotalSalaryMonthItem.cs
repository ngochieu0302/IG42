using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
	public class DNTotalSalaryMonthItem : BaseSimple
	{
		public Guid? UserId { get; set; }
		public string Username { get; set; }
		public string LowerUsername { get; set; }
		public int? Month { get; set; }
		public int? Year { get; set; }
		public int? TotalDateCC { get; set; }
		public int? TotalDateNLV { get; set; }
		public int? TotalSom { get; set; }
		public int? TotalMuon { get; set; }
		public decimal? DateUpdate { get; set; }
		public Guid? UserUpdate { get; set; }
		public int? FixedSalary { get; set; }
		public int? SalaryAward { get; set; }
		public int? TotalSchedule { get; set; }
		public string Note { get; set; }
		public int? AgencyID { get; set; }
		public bool? Status { get; set; }
	}
}
