using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class DNRolesItem :BaseSimple   
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public string LoweredRoleName { get; set; }
        public string Description { get; set; }
        public int? LevelRoomId { get; set; }
        public bool IsBed { get; set; }
        public IEnumerable<int> ListModuleID { get; set; }
        public virtual IEnumerable<DNUserSimpleItem> UserItems { get; set; }
        public virtual IEnumerable<DNUsersInRolesItem> DN_UsersInRoles { get; set; }
        public virtual IEnumerable<ActiveRoleItem> ActiveRoles { get; set; }
        public virtual IEnumerable<ModuleItem> ModuleItems { get; set; }
        public virtual IEnumerable<Role_ModuleActiveItem> RoleModuleActiveItems { get; set; }
    }

    public class DNRolesJsonItem
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public string LoweredRoleName { get; set; }
        public string Description { get; set; }
        public int? LevelRoomId { get; set; }
        public string Code { get; set; }
    }
    public class ModelDNRolesItem : BaseModelSimple
    {
        public Guid RoleId { get; set; }
        public IEnumerable<DNRolesItem> ListItem { get; set; }
        public DNRolesItem Item { get; set; }
        public IEnumerable<WeekItem> ListWeekItems { get; set; }
        public IEnumerable<ActiveRoleItem> ActiveRoleItems { get; set; }
    }

	public class ModelDNRolesMonthth : BaseModelSimple
	{
		public Guid RoleId { get; set; }
		public string RoleName { get; set; }
		public virtual IEnumerable<DNUsersInRolesItem> DN_UsersInRoles { get; set; }

	}
}
