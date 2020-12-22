using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple.QLCN
{
    public class MenCNItem:BaseSimple
    {
        public string Name { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsShow { get; set; }
        public string Unitname { get; set; }
        public int? UnitID { get; set; }
        public IEnumerable<MenNguyenlieuCNItem> LstNguyenlieuCnItems { get; set; }
    }

    public class ModelMenCNItem : BaseModelSimple
    {
        public IEnumerable<MenCNItem> ListItems { get; set; }
    }

    public class MenNguyenlieuCNItem : BaseSimple
    {
        public int IdNguyenlieu { get; set; }
        public int IdMen { get; set; }
        public decimal? Quantity { get; set; }
        public bool? IsDeleted { get; set; }
        public string Nguyenlieu { get; set; }
        public string Key { get; set; }

    }
}
