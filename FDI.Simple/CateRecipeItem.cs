using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
    public class CateRecipeItem:BaseSimple
    {
        public string Code { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? TotalPriceFinal { get; set; }
        public decimal? TotalIncurred { get; set; }
        public decimal? Totalkg { get; set; }
        public decimal? TotalPercent { get; set; }
        public decimal? Weight { get; set; }
        public decimal? DateCreate { get; set; }
        public decimal? DateUpdate { get; set; }
        public int? CategoryID { get; set; }
        public string CateName { get; set; }
        public string Username { get; set; }
        public bool? IsDeleted { get; set; }
        public decimal? Loss { get; set; }
        public bool? IsUse { get; set; }
        public decimal? Price { get; set; }
        public decimal? PriceFinal { get; set; }
        public IEnumerable<CategoryRecipeItem> LstCategoryRecipeItems { get; set; }
        public IEnumerable<MappingCategoryRecipeItem> LstMappingCategoryRecipeItems { get; set; }
    }

    public class ModelCateRecipeItem:BaseModelSimple
    {
        public IEnumerable<CateRecipeItem> ListItems { get; set; }
    }

}
