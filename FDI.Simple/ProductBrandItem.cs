using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class ProductBrandItem : BaseSimple
    {
        public string Code{ get; set; }
        public string Name{ get; set; }
        public string Address{ get; set; }
        public string Email{ get; set; }
        public int? PictureID { get; set; }
        public string PictureUrl { get; set; }
        public string Phone{ get; set; }
        public string TaxCode{ get; set; }
        public string Note{ get; set; }
        public bool? IsShow{ get; set; }
        public string LanguageId{ get; set; }
        public bool? IsDeleted { get; set; }
      
    }
    
    [Serializable]
    public class ProductGroupItem : BaseSimple
    {
        public string Name { get; set; }
        public bool? IsDelete { get; set; }
        public string LanguageId { get; set; }
      
    }
    public class ModelProductBrandItem : BaseModelSimple
    {
        public IEnumerable<ProductBrandItem> ListItem { get; set; }
        public string SelectMutil { get; set; }
    }
}
