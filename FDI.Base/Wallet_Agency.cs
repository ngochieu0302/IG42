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
    
    public partial class Wallet_Agency
    {
        public int ID { get; set; }
        public Nullable<int> AgencyID { get; set; }
        public Nullable<int> DocumentID { get; set; }
        public Nullable<decimal> Value { get; set; }
    
        public virtual DN_Agency DN_Agency { get; set; }
        public virtual Document Document { get; set; }
    }
}
