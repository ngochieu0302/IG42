using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class OrderDetailAppIG4Item : BaseSimple
    {
        public long ID { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public decimal? Quantity { get; set; }
        public string ProductName { get; set; }
        public string UrlPicture { get; set; }
        public string Address { get; set; }
        public decimal? Price { get; set; }
        public decimal? TotalPrice { get; set; }
        public int? Type { get; set; }
        public decimal? DateEnd { get; set; }
        public decimal? DateCreate { get; set; }
        public int? Status { get; set; }
        public bool? IsTime { get; set; }
        public byte? StatusPayment { get; set; }
        public int CustomerId { get; set; }
        public string Customername { get; set; }
        public int? Check { get; set; }
        public virtual CustomerItem Customer { get; set; }
        public virtual OrderItem Order { get; set; }
        public virtual ProductItem Shop_Product { get; set; }
        public bool? IsPrestige { get; set; }
    }

    public class ModelOrderDetailAppIG4Item : BaseModelSimple
    {
        public List<OrderDetailItem> ListOrderDetailItem { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}
