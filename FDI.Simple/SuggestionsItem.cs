using System;

namespace FDI.Simple
{
    public class SuggestionsItem
    {
        public int? ID { get; set; }
        public int? ID1 { get; set; }
        public string value { get; set; }
        public string title { get; set; }
        public string data { get; set; }
        public string name { get; set; }
        public string User { get; set; }
        public int? Type { get; set; }
        public decimal? Total { get; set; }
    }

    public class SuggestionsTMCustomer
    {
        public int? ID { get; set; }
        public string value { get; set; }
        public string data { get; set; }
        public string title { get; set; }
        public string name { get; set; }
    }


    public class BaseSuggestions
    {
        public int? ID { get; set; }
        public string value { get; set; }
        public string title { get; set; }
    }

    public class SuggestionsTMNews
    {
        public int? ID { get; set; }
        public string value { get; set; }
        public string data { get; set; }
        public string title { get; set; }
        public string name { get; set; }
    }

    /// <summary>
    /// 1. title
    /// 2. Data
    /// 3. Name
    /// </summary>
    public class SuggestionsProduct
    {
        public Guid? GuiID { get; set; }
        public int? ID { get; set; }
        public int IsCombo { get; set; }
        public int? QuantityDay { get; set; }
        public int Type { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? QuantityOut { get; set; }
        public decimal? Date { get; set; }
        public decimal? DateEnd { get; set; }
        public string DateS { get; set; }
        public string DateE { get; set; }
        public decimal? pricenew { get; set; }
        public decimal? PriceCost { get; set; }
        public decimal? TotalPrice { get; set; }
        public string Imei { get; set; }
        public string value { get; set; }
        public string UrlImg { get; set; }
        public string title { get; set; }
        public string data { get; set; }
        public string BarCode { get; set; }
        public string name { get; set; }
        public string keword { get; set; }
        public string Serial { get; set; }
        public string phone { get; set; }
        public string code { get; set; }
        public string Unit { get; set; }
        public string StrDate { get; set; }
        public string StrDateEnd { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public decimal? TotalWallet { get; set; }
        public Guid? IdDetail { get; set; }
        public string ContentPromotion { get; set; }
        public decimal? Birthday { get; set; }
        public int? QuantityActive { get; set; }
        public decimal? Valueweight { get; set; }
        public Guid? Idimport { get; set; }
        public decimal? Incurred { get; set; }
        public decimal? PercentProduct { get; set; }
    }
}
