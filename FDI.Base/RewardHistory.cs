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
    
    public partial class RewardHistory
    {
        public int ID { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<int> OrderID { get; set; }
        public Nullable<decimal> Date { get; set; }
        public Nullable<int> BonusTypeId { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<decimal> Percent { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.Guid> ImportID { get; set; }
        public Nullable<int> AgencyId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<decimal> TotalCp { get; set; }
        public Nullable<int> WalletCusId { get; set; }
        public Nullable<int> OrderPacketID { get; set; }
        public Nullable<decimal> DateCreate { get; set; }
    
        public virtual BonusType BonusType { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual DN_Agency DN_Agency { get; set; }
        public virtual DN_ImportProduct DN_ImportProduct { get; set; }
        public virtual Order_Package Order_Package { get; set; }
        public virtual Shop_Orders Shop_Orders { get; set; }
        public virtual WalletCustomer WalletCustomer { get; set; }
    }
}
