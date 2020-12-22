using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;

namespace FDI.Simple
{
    public class DNSalesItem:BaseSimple
    {
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public decimal? Percent { get; set; }
        public decimal? DateStart { get; set; }
        public decimal? DateEnd { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsAll { get; set; }
        public int? QuantityCode { get; set; }
        public int? AgencyId { get; set; }
        public bool? IsAgency { get; set; }
        public Guid? UserCreate { get; set; }
        public string Username { get; set; }
        public bool? IsDeleted { get; set; }
        public Guid? UserUpdate { get; set; }
        public bool? IsMonth { get; set; }
        public bool? IsDay { get; set; }
        public decimal? PriceLimit { get; set; }
        public decimal? TotalOrder { get; set; }
        public decimal? TotalUse { get; set; }
        public int? Type { get; set; }
        public virtual IEnumerable<SaleCode> SaleCodes { get; set; }
        public virtual IEnumerable<ShopProductDetailItem> ListProductDetailItems { get; set; }
        public IEnumerable<CategoryItem> LstCategoryItems { get; set; }
    }
    public class ModelDNSalesItem : BaseModelSimple
    {
        public IEnumerable<DNSalesItem> ListItems { get; set; }
    }

    public class SaleCodeItem:BaseSimple
    {
        public string Code { get; set; }
        public bool? IsUser { get; set; }
        public int? SaleId { get; set; }
        public decimal? DateUser { get; set; }
        public decimal? Percent { get; set; }
        public decimal? PriceSale { get; set; }
    }
    public class ModelSaleCodeItem:BaseModelSimple
    {
        public IEnumerable<SaleCodeItem> ListItems { get; set; }
    }
}
