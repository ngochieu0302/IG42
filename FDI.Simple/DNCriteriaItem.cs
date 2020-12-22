using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class DNCriteriaItem : BaseSimple
    {
        public string Name { get; set; }
        public decimal? Value { get; set; }
        public int? Type { get; set; }
        public decimal? Price { get; set; }
        public int? AgencyID { get; set; }
        public string LstRoleIds { get; set; }
        public string LstUserIds { get; set; }
        public int? TypeID { get; set; }
        public bool? IsAll { get; set; }
        public bool? IsSchedule { get; set; }
        public string NameType { get; set; }
        public IEnumerable<DNRolesItem> DNRolesItem { get; set; }
        public IEnumerable<DNUserItem> DNUserItem { get; set; }
    }

	public class DNCriteriaTotalitem
	{
		public int? Id { get; set; }
		public string Name { get; set; }
		public decimal? Value { get; set; }
	}

	public class ModelDNCriteriaItem : BaseModelSimple
    {
        public DNCriteriaItem Item { get; set; }
        public IEnumerable<DNCriteriaItem> ListItem { get; set; }
    }
}
