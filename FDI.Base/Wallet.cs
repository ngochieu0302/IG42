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
    
    public partial class Wallet
    {
        public int ID { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<decimal> WalletOrder { get; set; }
        public Nullable<decimal> WalletCus { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<int> AgencyId { get; set; }
        public Nullable<decimal> CashOutWallet { get; set; }
    
        public virtual Customer Customer { get; set; }
    }
}
