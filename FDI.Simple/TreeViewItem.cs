using System;

namespace FDI.Simple
{
    public class TreeViewItem : BaseSimple
    {
        public Guid GuiId { get; set; }
        public string RolesName { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public bool? IsShow { get; set; }
        public int? Count { get; set; }
        public int? Sort { get; set; }
        public int? GroupId { get; set; }
        public int? Level { get; set; }
    }
}
