using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class PromotionItem : BaseSimple
    {
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Quantity { get; set; }
        public string UserCreated { get; set; }
        public string Content { get; set; }
        public string ShopId { get; set; }
        public int? Type { get; set; }
        public decimal? FromPrice { get; set; }
        public decimal? ToPrice { get; set; }
        public string Mode { get; set; }
        public bool Status { get; set; }
        public decimal? Sale { get; set; }
        public List<DiscountCodeItem> DiscountCodeItems { get; set; }
    }

    public class ModelPromotionItem : BaseModelSimple
    {
        public List<PromotionItem> ListItem { get; set; }
    }

    public class DiscountCodeItem : BaseSimple
    {
        public int? PromotionId { get; set; }
        public string Code { get; set; }
        public decimal? Sale { get; set; }
    }

}
