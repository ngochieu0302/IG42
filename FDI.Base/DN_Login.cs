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
    
    public partial class DN_Login
    {
        public int ID { get; set; }
        public Nullable<decimal> DateCreated { get; set; }
        public Nullable<decimal> DateEnd { get; set; }
        public Nullable<System.Guid> UserId { get; set; }
        public Nullable<bool> IsOut { get; set; }
        public string Code { get; set; }
        public Nullable<int> CustomerID { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual DN_Users DN_Users { get; set; }
    }
}
