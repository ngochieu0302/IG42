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
    
    public partial class Category_Product_Recipe
    {
        public int ID { get; set; }
        public Nullable<int> ProductId { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<decimal> DateCreate { get; set; }
        public Nullable<decimal> Percent { get; set; }
        public Nullable<bool> IsCheck { get; set; }
        public Nullable<int> RecipeID { get; set; }
        public Nullable<decimal> Incurred { get; set; }
        public Nullable<decimal> PriceProduct { get; set; }
    
        public virtual Category_Recipe Category_Recipe { get; set; }
        public virtual Shop_Product_Detail Shop_Product_Detail { get; set; }
    }
}
