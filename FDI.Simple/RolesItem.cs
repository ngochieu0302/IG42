using System;
using System.Collections.Generic;
using FDI.Base;

namespace FDI.Simple
{
    public class RolesItem :BaseSimple   
    {
        public Guid ApplicationId { get; set; }
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public string LoweredRoleName { get; set; }
        public string Description { get; set; }

        public virtual IEnumerable<Role_ModuleActiveItem> RoleModuleActive { get; set; }
        public virtual ICollection<AspnetUsersItem> AspnetUsers { get; set; }
        public virtual IEnumerable<ActiveRoleItem> ActiveRoles { get; set; }
        public virtual IEnumerable<ModuleItem> Modules { get; set; }
    }
    public class ModelRolesItem : BaseModelSimple
    {
        public IEnumerable<RolesItem> ListItem { get; set; }
        public DN_Roles DnRoles { get; set; }
        public IEnumerable<ActiveRoleItem> ActiveRoles { get; set; }
    }
}
