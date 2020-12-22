using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;

namespace FDI.Simple
{
    public class WorkShopItem:BaseSimple
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int? CompanyID { get; set; }
        public bool? IsActive { get; set; }
        public decimal? DateActive { get; set; }
        public decimal? DateCreated { get; set; }
        public string Latitute { get; set; }
        public string Longitude { get; set; }
        public Guid? UserID { get; set; }
        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public CompanyItem Company { get; set; }
        public IEnumerable<CateRecipeItem> CateRecipeItems { get; set; }
        public virtual IEnumerable<ProductDetailRecipeItem> ProductDetailRecipeItems { get; set; }
    }

    public class ModelWorkShopItem:BaseModelSimple
    {
        public IEnumerable<WorkShopItem> ListItems { get; set; }
        public WorkShopItem WorkShopItem { get; set; }
        public IEnumerable<ProductDetailRecipeItem> LstRecipeItems { get; set; }
        public IEnumerable<CateRecipeItem> LstCateRecipeItems { get; set; }
        public IEnumerable<CompanyItem> LstCompanyItems { get; set; }
    }
}
