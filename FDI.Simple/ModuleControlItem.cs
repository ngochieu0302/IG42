using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class ModuleControlItem : BaseSimple
    {
        public int? PageID { get; set; }
        public string PageName { get; set; }
        public string Module { get; set; }
        public string Action { get; set; }
        public string Section { get; set; }
        public string Layout { get; set; }
        public int? Sort { get; set; }
        public DateTime? CreateDate { get; set; }
        public string LanguageId { get; set; }
        
    }
    public class ModelModuleControlItem : BaseModelSimple
    {
        public IEnumerable<ModuleControlItem> ListItem { get; set; }
        public ModuleControlItem ModuleControlItem { get; set; }
        public List<string> LstAction { get; set; }
        public List<string> LstSection { get; set; }
        public List<string> LstModules { get; set; }
        public List<SysPageItem> SysPageItems { get; set; }

    }
}
