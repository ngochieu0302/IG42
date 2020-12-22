using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple.QLCN
{
    public class NguyenlieuCNItem:BaseSimple
    {
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsShow { get; set; }
        public string Unitname { get; set; }
        public int? UnitID { get; set; }
    }

    public class ModelNguyenlieuCNItem:BaseModelSimple
    {
        public IEnumerable<NguyenlieuCNItem> ListItems { get; set; }
    }
}
