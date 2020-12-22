using System.Collections.Generic;

namespace FDI.Simple
{

    public class ModelPageItem
    {
        public List<string> LstModules { get; set; } 
        public List<string> LstSection { get; set; }
        public List<string> LstAction { get; set; }
        public List<SysPageItem> LstSysPage { get; set; }
        public string DoAction { get; set; }
        public int PageId { get; set; }
        public int CtrId { get; set; }
        public ModeItem ModeItem { get; set; }
    }
}
