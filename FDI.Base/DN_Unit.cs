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
    
    public partial class DN_Unit
    {
        public DN_Unit()
        {
            this.Cars = new HashSet<Car>();
            this.Cate_Value = new HashSet<Cate_Value>();
            this.Categories = new HashSet<Category>();
            this.Product_Value = new HashSet<Product_Value>();
            this.Shop_Product_Detail = new HashSet<Shop_Product_Detail>();
            this.Shop_Product_Value = new HashSet<Shop_Product_Value>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<Car> Cars { get; set; }
        public virtual ICollection<Cate_Value> Cate_Value { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Product_Value> Product_Value { get; set; }
        public virtual ICollection<Shop_Product_Detail> Shop_Product_Detail { get; set; }
        public virtual ICollection<Shop_Product_Value> Shop_Product_Value { get; set; }
    }
}
