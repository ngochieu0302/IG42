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
    
    public partial class DN_RequestWareDetail
    {
        public int Id { get; set; }
        public Nullable<System.Guid> RequestWareId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public Nullable<decimal> Weight { get; set; }
    
        public virtual DN_RequestWare DN_RequestWare { get; set; }
        public virtual Shop_Product Shop_Product { get; set; }
    }
}
