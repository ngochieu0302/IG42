using FDI.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
    public class LogItem : BaseSimple
    {
        public string User { get; set; }
        public string ChucNang { get; set; }
        public DateTime? NgayTao { get; set; }
        public string CuaHang { get; set; }
        public string NoiDung { get; set; }

    }
    public class ModelLogItem : BaseModelSimple
    {
        public IEnumerable<LogItem> ListItem { get; set; }
    }

}
