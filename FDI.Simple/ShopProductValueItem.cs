using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
    public class ShopProductValueItem : BaseSimple
    {
        public string Name { get; set; }
        public bool IsShow { get; set; }
        public bool IsDeleted { get; set; }
        public int? UnitId { get; set; }
        public string UnitName { get; set; }
        public int? AgencyId { get; set; }
        public decimal? Price { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? QuantityOut { get; set; }
        public decimal? Date { get; set; }
        public decimal? DateEnd { get; set; }
        public int? QuantityDay { get; set; }
        public decimal? QuantityInv { get; set; }
    }

    public class ModuleShopProductValueItem : BaseModelSimple
    {
        public List<ShopProductValueItem> ListItems { get; set; }
        public IEnumerable<DNImportItem> LstImportItem { get; set; }
        public IEnumerable<ExportProductValueItem> LstExportValueItem { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? Quantity { get; set; }
        public int? AgencyID { get; set; }
        public Guid? UserID { get; set; }
        public string UserName { get; set; }
        public string DateTime { get; set; }
    }    
}
