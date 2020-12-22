using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;

namespace FDI.Simple
{
    [Serializable]
    public class ContactOderDetailItem
    {
        public Guid GId { get; set; }
        public int? ContactOrderId { get; set; }
        public int? ProductId { get; set; }
        public int? ComboId { get; set; }
        public int? QuantityOld { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? DateCreated { get; set; }
        public int? Status { get; set; }

        public virtual DN_Combo DN_Combo { get; set; }
        public virtual Shop_ContactOrder Shop_ContactOrder { get; set; }
        public virtual Shop_Product Shop_Product { get; set; }
    }
    public class ContactOderDetailAppItem
    {
        public int? ContactOrderId { get; set; }
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        public int? Status { get; set; }
    }
}
