using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class DNTreeItem : BaseSimple
    {
        public int? Level { get; set; }
        public int? ParentID { get; set; }
        public string ListID { get; set; }
        public int? UserInRoleID { get; set; }
        public Guid? RoleId { get; set; }
        public int? DepartmentId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public Guid? UserID { get; set; }
        public bool IsSelect { get; set; }
    }

    public class ModelDNTreeItem
    {
        public List<DNTreeItem> ListItem { get; set; }      
        public Guid? UserID { get; set; }
    }
}
