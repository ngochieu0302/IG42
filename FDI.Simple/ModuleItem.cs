using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class ModuleItem : BaseSimple
    {
        public string NameModule { get; set; }
        public string Tag { get; set; }
        public string ClassCss { get; set; }
        public string LstUserIds { get; set; }
        public string LstRoleIds { get; set; }
        public int? Ord { get; set; }
        public int? Level { get; set; }
        public int? PrarentID { get; set; }
        public string Content { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsShowAgency { get; set; }
        public virtual ModuleadminItem DN_Module { get; set; }

        public virtual IEnumerable<ActionActiveItem> listActionActiveuser { get; set; }
        public virtual IEnumerable<ActionActiveItem> listActionActiverole { get; set; }
        public virtual IEnumerable<Role_ModuleActiveItem> Role_ModuleActive { get; set; }
        public virtual IEnumerable<UserModuleActiveItem> User_ModuleActive { get; set; }

        public virtual IEnumerable<AspnetRolesItem> AspnetRoles { get; set; }
        public virtual IEnumerable<AspnetUsersItem> AspnetUsers { get; set; }
    }

    public class ModulePrarentItem : BaseSimple
    {
        public string NameModule { get; set; }
        public string Tag { get; set; }
        public string Content { get; set; }
        public string ClassCss { get; set; }
        public int? Ord { get; set; }
        public int? PrarentID { get; set; }
        public bool? IsShowAgency { get; set; }
    }
    public class RouterItem
    {
        public string Controller { get; set; }       
        public int? ID { get; set; }
        public int? ParentID { get; set; }
    }
    public class ModuleUpdateItem : BaseSimple
    {
        public string NameModule { get; set; }
        public string LstUserIds { get; set; }
        public string LstRoleIds { get; set; }
    }

    public class ModelModuleItem : BaseModelSimple
    {
        public string Container { get; set; }
        public bool SelectMutil { get; set; }
        public int? PrarentID { get; set; }
        public IEnumerable<ModuleItem> ListItem { get; set; }
    }
}
