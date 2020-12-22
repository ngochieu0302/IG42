using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class DNSalaryItem : BaseSimple
    {
        public Guid? UserId { get; set; }
        public string UserName { get; set; }
        public int OrderId { get; set; }
        public decimal? DateCreated { get; set; }
        public int? CriteriaId { get; set; }
        public int? Salary { get; set; }
        public int? AgencyID { get; set; }
	    public int? Month { get; set; }
	    public int? Year { get; set; }

		public virtual DNUserItem DnUserItem { get; set; }
        public virtual DNCriteriaItem DNCriteriaItem { get; set; }
    }
    public class ModelDNSalaryItem : BaseModelSimple
    {
        public DNSalaryItem DnSalaryItem { get; set; }
        public IEnumerable<DNSalaryItem> ListItem { get; set; }
    }
}
