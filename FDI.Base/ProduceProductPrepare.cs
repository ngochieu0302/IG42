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
    
    public partial class ProduceProductPrepare
    {
        public int ID { get; set; }
        public int ProduceId { get; set; }
        public int ProductId { get; set; }
        public string Code { get; set; }
        public System.Guid UserId { get; set; }
        public decimal DateCreate { get; set; }
        public Nullable<System.Guid> UserUpdate { get; set; }
        public Nullable<decimal> DateUpdate { get; set; }
        public bool Isdelete { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual DN_Users DN_Users { get; set; }
        public virtual DN_Users DN_Users1 { get; set; }
        public virtual Produce Produce { get; set; }
    }
}
