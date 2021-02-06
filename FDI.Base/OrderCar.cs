//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FDI.Base
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderCar
    {
        public OrderCar()
        {
            this.OrderCarProductDetails = new HashSet<OrderCarProductDetail>();
            this.PurchaseOrders = new HashSet<PurchaseOrder>();
        }
    
        public int ID { get; set; }
        public int SupplierId { get; set; }
        public int ProductId { get; set; }
        public int CarId { get; set; }
        public decimal DepartureDate { get; set; }
        public decimal ReceiveDate { get; set; }
        public decimal ReturnDate { get; set; }
        public decimal Quantity { get; set; }
        public string Code { get; set; }
        public int Status { get; set; }
        public Nullable<decimal> DateUpdate { get; set; }
        public decimal Price { get; set; }
        public decimal PriceNow { get; set; }
        public bool IsDelete { get; set; }
        public System.Guid UserCreateId { get; set; }
        public Nullable<System.Guid> UserUpdate { get; set; }
        public decimal DateCreate { get; set; }
        public decimal TodayCode { get; set; }
        public int WorkshopID { get; set; }
    
        public virtual Car Car { get; set; }
        public virtual Category Category { get; set; }
        public virtual DN_Supplier DN_Supplier { get; set; }
        public virtual DN_Users DN_Users { get; set; }
        public virtual DN_Users DN_Users1 { get; set; }
        public virtual P_Workshop P_Workshop { get; set; }
        public virtual ICollection<OrderCarProductDetail> OrderCarProductDetails { get; set; }
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
    }
}
