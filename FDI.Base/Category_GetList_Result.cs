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
    
    public partial class Category_GetList_Result
    {
        public int Id { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string Name { get; set; }
        public string NameRoot { get; set; }
        public string Slug { get; set; }
        public Nullable<int> Type { get; set; }
        public string Icon { get; set; }
        public Nullable<int> IsLevel { get; set; }
        public string LanguageId { get; set; }
        public string Symbol { get; set; }
        public Nullable<decimal> Percent { get; set; }
        public Nullable<decimal> WeightDefault { get; set; }
        public Nullable<decimal> PercentLoss { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> PriceFinal { get; set; }
        public Nullable<decimal> PriceRecipeFinal { get; set; }
    }
}
