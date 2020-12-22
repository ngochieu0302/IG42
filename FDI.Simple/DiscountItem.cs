using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
    public class DiscountItem : BaseSimple
    {
        public decimal? Percent { get; set; }
        public decimal? PriceS { get; set; }
        public decimal? PriceE { get; set; }
        public decimal? DateStart { get; set; }
        public decimal? DateEnd { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsAll { get; set; }
        public bool? IsCard { get; set; }
        public int? Type { get; set; }
        public IEnumerable<int> ListInt { get; set; }
    }

    public class ModelDiscountItem : BaseModelSimple
    {
        public List<SaleItem> SaleItems { get; set; }
        public CusSaleItem CusSaleItem { get; set; }
        public Guid Key { get; set; }
        public bool Check { get; set; }
        public List<Guid> LstKey { get; set; }
        public decimal? Total { get; set; }
        public decimal? TotalPrice { get; set; }
        public IEnumerable<DiscountItem> ListItem { get; set; }
        public DiscountItem DiscountItem { get; set; }
    }
    public class CusSaleItem
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string UrlImg { get; set; }
        public string NoteCate { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Bonus { get; set; }
        public decimal? Birthday { get; set; }
    }
    public class AgentSaleItem
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string UrlImg { get; set; }
        public string NoteCate { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Bonus { get; set; }
        public decimal? Birthday { get; set; }
        public string TaxCode { get; set; }
        public string CompanyName { get; set; }
        public string AgentGroup { get; set; }

    }
    public class SaleItem
    {
        public int ProductID { get; set; }
        public int? Quantity { get; set; }
        public decimal? Value { get; set; }
        public decimal? Price { get; set; }
        public string UrlImg { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public string Barcode { get; set; }
        public string Key { get; set; }
        public int ProductdetailID { get; set; }
        public decimal? PriceSale { get; set; }
        public decimal? PercentSale { get; set; }
        public bool? IsAllSale { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? Discount { get; set; }
        public Guid? Idimport { get; set; }
        public IEnumerable<DNPromotionPItem> PromotionPs { get; set; }
    }
    public class WholeSaleItem
    {
        public int? ProductID { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Value { get; set; }
        public decimal? Price { get; set; }
        public string UrlImg { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public string Barcode { get; set; }
        public string Key { get; set; }
        public int? ProductdetailID { get; set; }
        public decimal? PriceSale { get; set; }
        public decimal? PercentSale { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? Discount { get; set; }
        public Guid? Idimport { get; set; }
        public IEnumerable<DNPromotionPItem> PromotionPs { get; set; }
    }
    public class WholeSaleOItem
    {
        public int? ProductID { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        public string UrlImg { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public string Barcode { get; set; }
        public string Key { get; set; }
        public decimal? PriceSale { get; set; }
        public decimal? PercentSale { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? Total { get; set; }
        public decimal? Discount { get; set; }
        public int? ProductValueID { get; set; }
        public int? CateValueID { get; set; }
        public int? Idimport { get; set; }
        public decimal? Value { get; set; }

        public IEnumerable<ListDNImportItem> ListDnImportItems { get; set; }
    }

    public class ListDNImportItem
    {
        public int? ProductID { get; set; }
        public decimal? Quantity { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public string Barcode { get; set; }
        public decimal? Value { get; set; }
        public decimal? Price { get; set; }
        public decimal? DateS { get; set; }
        public decimal? DateE { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? PriceSale { get; set; }
        public decimal? PercentSale { get; set; }
        public decimal? Discount { get; set; }
        public string UrlImg { get; set; }
        public Guid? Idimport { get; set; }
        public int? ProductValueID { get; set; }
        public int? CateValueID { get; set; }
    }
    public class DNPromotionPItem:BaseSimple
    {
        public string Title { get; set; }
        public int Quantity { get; set; }
        public IEnumerable<PromotionSPItem> PromotionSPItems { get; set; }
    }
    public class PromotionSPItem : BaseSimple
    {
        public int? ProductID { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? PriceSp { get; set; }
        public decimal? Percent { get; set; }
        public string UrlImg { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public string Key { get; set; }
        public decimal? TotalPrice { get; set; }
        public bool? IsOnly { get; set; }
    }
    public class Sale
    {
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public decimal? PercentSale { get; set; }
        public bool? IsAll { get; set; }
        public int? QuantityCode { get; set; }
    }
    //public class PromotionP
    //{
    //    public bool? IsOnly { get; set; }
    //    public IEnumerable<SuggestionsProduct> SuggestionsProducts { get; set; }
    //}
    public class ModelSaleItem
    {
        public List<SaleItem> SaleItems { get; set; }
        public CusSaleItem CusSaleItem { get; set; }
        public IEnumerable<DNPromotionPItem> PromotionOrder { get; set; }
        public IEnumerable<Sale> SaleOrder { get; set; }
        public Guid Key { get; set; }
        public bool Check { get; set; }
        public List<Guid> LstKey { get; set; }
        public decimal? Total { get; set; }
        public decimal? Discount { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? TotalSaleSP { get; set; }
        public decimal? VoucherPer { get; set; }
        public decimal? VoucherPrice { get; set; }
        public string SaleCode { get; set; }
        public decimal? SalePercent { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? ContentPromotion { get; set; }
        public IEnumerable<PaymentMethodItem> PaymentMethodItems { get; set; }
    }
    public class ModelWholeSaleItem
    {
        public List<WholeSaleItem> WholeSaleItems { get; set; }
        public AgentSaleItem AgentSaleItem { get; set; }
        public IEnumerable<DNPromotionPItem> PromotionOrder { get; set; }
        public IEnumerable<Sale> SaleOrder { get; set; }
        public Guid Key { get; set; }
        public bool Check { get; set; }
        public List<Guid> LstKey { get; set; }
        public decimal? Total { get; set; }
        public decimal? DiscountSale { get; set; }
        public decimal? Discount { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? TotalSaleSP { get; set; }
        public decimal? VoucherPer { get; set; }
        public decimal? VoucherPrice { get; set; }
        public string SaleCode { get; set; }
        public decimal? SalePercent { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? ContentPromotion { get; set; }
        public IEnumerable<PaymentMethodItem> PaymentMethodItems { get; set; }
    }
    public class ModelPSaleItem
    {
        public IEnumerable<Sale> Sale { get; set; }
        //public IEnumerable<PromotionP> Promotion { get; set; }
    }
    public class ModelWholeSaleOItem
    {
        public List<WholeSaleOItem> WholeSaleItems { get; set; }
        public AgentSaleItem AgentSaleItem { get; set; }
        public IEnumerable<DNPromotionPItem> PromotionOrder { get; set; }
        public IEnumerable<Sale> SaleOrder { get; set; }
        public Guid Key { get; set; }
        public bool Check { get; set; }
        public List<Guid> LstKey { get; set; }
        public decimal? Total { get; set; }
        public decimal? DiscountSale { get; set; }
        public decimal? Discount { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? TotalSaleSP { get; set; }
        public decimal? VoucherPer { get; set; }
        public decimal? VoucherPrice { get; set; }
        public string SaleCode { get; set; }
        public decimal? SalePercent { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? ContentPromotion { get; set; }
        public IEnumerable<PaymentMethodItem> PaymentMethodItems { get; set; }
    }

}
