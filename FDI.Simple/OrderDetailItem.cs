using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class OrderDetailItem : BaseSimple
    {
        public Guid GID { get; set; }
        public long ID { get; set; }
        public int? OrderID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ComboName { get; set; }
        public string CateName { get; set; }
        public string UrlImg { get; set; }
        public string UnitName { get; set; }
        public int? ProductID { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Weight { get; set; }
        public int? QuantityOld { get; set; }
        public decimal? Price { get; set; }
        public decimal? StartDate { get; set; }
        public decimal? EndDate { get; set; }
        public int? Status { get; set; }
        public bool? IsBoas { get; set; }
        public decimal? Discount { get; set; }
        public decimal? TotalPrice { get; set; }
        public string ContentPromotion { get; set; }
        public decimal TodayCode { get; set; }
        public int OrderType { get; set; }
        public  decimal ReceiveDate { get; set; }
        public IEnumerable<RecipeItem> LstRecipeItems { get; set; }
        public IEnumerable<SetupProductionItem> SetupProductionItems { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
    }

    public class OrderDetailNewItem
    {
        public Guid GID { get; set; }
        public int? ProductID { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public string Key { get; set; }
        public decimal? Discount { get; set; }
        public string ContentPromotion { get; set; }
        public bool? IsPromotion { get; set; }
    }

    public class ModelOrderDetailNewItem 
    {
        public string Key { get; set; }
        public string json { get; set; }
    }

    public class ModelOrderDetailItem : BaseModelSimple
    {
        public List<OrderDetailItem> ListItems { get; set; }
    }
}
