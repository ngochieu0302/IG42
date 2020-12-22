using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
    public class DNPromotionItem:BaseSimple
    {
        public string Name { get; set; }
        public decimal? DateStart { get; set; }
        public decimal? DateEnd { get; set; }
        public bool? IsShow { get; set; }
        public string Note { get; set; }
        public bool? IsAll { get; set; }
        public bool? IsEnd { get; set; }
        public int? Quantity { get; set; }
        public int? AgencyId { get; set; }
        public bool? IsAgency { get; set; }
        public Guid? UserCreate { get; set; }
        public bool? IsDeleted { get; set; }
        public string Username { get; set; }
        public decimal? TotalOrder { get; set; }
        public decimal? QuantityUse { get; set; }
        public bool? IsOnly { get; set; }
        public int? Type { get; set; }
        public IEnumerable<ShopProductDetailItem> ListProductDetailItems { get; set; }
        public IEnumerable<DNPromotionProductItem> ListPromotionDetailItems { get; set; }
        public IEnumerable<CategoryItem> LstCategoryItems { get; set; }
    }
    public class ModelDNPromotionItem : BaseModelSimple
    {
        public IEnumerable<DNPromotionItem> ListItems { get; set; }

    }
    public class DNImportProductPromotionItem
    {
        public int? ProductID { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public int? ValueId { get; set; }
        public int? QuantityDay { get; set; }
        public string Key { get; set; }
    }

    public class DNPromotionProductItem:BaseSimple
    {
        public int? ProductID { get; set; }
        public int? Quantity { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public decimal? PriceProduct { get; set; }
        public string Key { get; set; }
        public string UrlPicture { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }
        public bool? IsEnd { get; set; }
        public decimal? Percent { get; set; }
    }
}
