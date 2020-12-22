using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple.QLCN
{
    public class CamCNItem:BaseSimple
    {
        public string Name { get; set; }
        public string Catename { get; set; }
        public int? CateId { get; set; }
        public decimal? Price { get; set; }
        public int? HSD { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsShow { get; set; }
        public IEnumerable<CamNguyenlieuCNItem> LstCamNguyenlieuCnItem { get; set; }
    }

    public class ModelCamCNItem : BaseModelSimple
    {
        public IEnumerable<CamCNItem> ListItems { get; set; }
    }

    public class CamNguyenlieuCNItem : BaseSimple
    {
        public int IdCam { get; set; }
        public int IdNguyenlieu { get; set; }
        public string Nguyenlieu { get; set; }
        public decimal? Quantity { get; set; }
        public bool? IsDeleted { get; set; }
        public string Key { get; set; }
    }
}
