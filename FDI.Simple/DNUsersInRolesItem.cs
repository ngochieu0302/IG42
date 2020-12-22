using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class DNUsersInRolesItem
    {
        public int ID { get; set; }
        public Guid UserId { get; set; }
        public bool IsOnline { get; set; }
        public Guid RoleId { get; set; }
        public int? DepartmentID { get; set; }
        public int? AgencyID { get; set; }
        public Decimal? DateCreated { get; set; }

        public  DNRolesItem DNRolesItem { get; set; }
        public DNUserItem DNUserItem { get; set; }
        public virtual IEnumerable<DNTimeJobItem> DN_Time_Job { get; set; }
        public DepartmentItem DepartmentItem { get; set; }
    }

	public class DNUserInRolesUserItem
	{
		public int ID { get; set; }
		public Guid? RoleId { get; set; }
		public string RoleName { get; set; }
		public string DepartmentName { get; set; }
	}

    public class ModelDNUsersInRolesItem : BaseModelSimple
    {
        public IEnumerable<DNUsersInRolesItem> ListItem { get; set; }
    }
}
