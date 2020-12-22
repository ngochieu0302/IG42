using System;
using System.Collections.Generic;

namespace FDI.Simple
{
	public class DNTimeJobItem : BaseSimple
	{
		public decimal? DateCreated { get; set; }
		public Guid? UserId { get; set; }
		public int? AgencyID { get; set; }
		public DateTime? DateJob { get; set; }
		public int? DayInDateJob { get; set; }
		public int? MonthInDateJob { get; set; }
		public int? YearInDateJob { get; set; }
		public decimal? DateEnd { get; set; }
		public int? MinutesLater { get; set; }
		public int? MinutesEarly { get; set; }
		public int? ScheduleID { get; set; }
		public int? ScheduleEndID { get; set; }
		public virtual DNUserItem DNUserItem { get; set; }
	}

	public class DNJobTimeDayGroup
	{
		public Guid? UserId { get; set; }
        public int? DayInMonth { get; set; }
		public int? DaysInMonth { get; set; }
		public int? MinutesLater { get; set; }
		public int? MinutesEarly { get; set; }
	}

	public class CDNTimeJobItem : BaseSimple
	{
		public decimal? DateCreated { get; set; }
        public decimal? DateEnd { get; set; }
        public int? MinutesLater { get; set; }
        public int? MinutesEarly { get; set; }
	}
	public class ModelDNTimeJobItem : BaseModelSimple
	{
		public DNTimeJobItem Item { get; set; }
		public IEnumerable<DNTimeJobItem> ListItem { get; set; }
	}
}
