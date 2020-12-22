using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class ModuleadminItem : BaseSimple
    {
        public string NameModule { get; set; }
        public string Tag { get; set; }
        public string ClassCss { get; set; }
        public int? Ord { get; set; }
        public int? PrarentID { get; set; }
        public string Content { get; set; }
        public bool IsShow { get; set; }
        public int Active { get; set; }

        public virtual IEnumerable<ModuleItem> Module1 { get; set; }
        public virtual ModulePrarentItem Module2 { get; set; }

        public virtual IEnumerable<ActionActiveItem> listActionActiveuser { get; set; }

        public virtual IEnumerable<ActionActiveItem> listActionActiverole { get; set; }
    }
    public class ModelModuleadminItem : BaseModelSimple
    {
        public IEnumerable<ModuleadminItem> ListItem { get; set; }
        public bool check { get; set; }
        public int TotalMailInbox { get; set; }
        public Guid UserId { get; set; }
        public int TotalMailDrafts { get; set; }
        public int? PrarentID { get; set; }
    }

    public class CountAdminItem : BaseSimple
    {
        public int CountE { get; set; }
        public int CountA { get; set; }
        public int CountC { get; set; }
        public int CountCard { get; set; }
    }
}
