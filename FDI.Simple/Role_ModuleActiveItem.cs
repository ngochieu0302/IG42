using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class Role_ModuleActiveItem :BaseSimple
    {
        
        public Guid? RoleId { get; set; }
        public int? ActiveRoleId { get; set; }
        public int? ModuleId { get; set; }
        public string ActiveName { get; set; }
        public bool? Active { get; set; }
        public bool? Check { get; set; }

        public virtual AspnetRolesItem AspnetRoles { get; set; }
    }
    public class ModelRoleModuleActiveItem : BaseModelSimple
    {
        public IEnumerable<Role_ModuleActiveItem> ListItem { get; set; }
    }
}
