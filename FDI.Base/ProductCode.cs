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
    
    public partial class ProductCode
    {
        public ProductCode()
        {
            this.ProductCode_CostUser = new HashSet<ProductCode_CostUser>();
        }
    
        public int ID { get; set; }
        public string Code { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<decimal> DateCreated { get; set; }
        public Nullable<int> BiasProduceID { get; set; }
        public Nullable<decimal> StartDate { get; set; }
        public Nullable<decimal> EndDate { get; set; }
        public string Note { get; set; }
    
        public virtual BiasProduce BiasProduce { get; set; }
        public virtual ICollection<ProductCode_CostUser> ProductCode_CostUser { get; set; }
    }
}
