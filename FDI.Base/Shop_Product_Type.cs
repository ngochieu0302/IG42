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
    
    public partial class Shop_Product_Type
    {
        public Shop_Product_Type()
        {
            this.Shop_Product = new HashSet<Shop_Product>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string NameAscii { get; set; }
        public Nullable<int> PictureID { get; set; }
        public Nullable<bool> IsActived { get; set; }
        public Nullable<bool> IsHasSize { get; set; }
        public Nullable<bool> IsHasWeight { get; set; }
        public Nullable<bool> IsHasColor { get; set; }
        public Nullable<bool> IsHasBrand { get; set; }
        public string Description { get; set; }
        public Nullable<bool> Delete { get; set; }
        public Nullable<int> DisplayOrder { get; set; }
        public string LanguageId { get; set; }
    
        public virtual Gallery_Picture Gallery_Picture { get; set; }
        public virtual ICollection<Shop_Product> Shop_Product { get; set; }
    }
}