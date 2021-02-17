//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FDI.Base
{
    using System;
    using System.Collections.Generic;
    
    public partial class ReceiptVoucher
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public Nullable<int> AgencyId { get; set; }
        public Nullable<int> CostTypeId { get; set; }
        public Nullable<int> PaymentMethodId { get; set; }
        public Nullable<int> OrderId { get; set; }
        public string Name { get; set; }
        public Nullable<System.Guid> UserCashier { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<decimal> DateCreated { get; set; }
        public Nullable<decimal> DateReturn { get; set; }
        public Nullable<decimal> DateActive { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Note { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    
        public virtual CostType CostType { get; set; }
        public virtual DN_Users DN_Users { get; set; }
    }
}
