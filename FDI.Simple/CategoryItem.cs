using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class CategoryItem : BaseSimple
    {
        public string Name { get; set; }
        public string NameRoot { get; set; }
        public string Slug { get; set; }
        public int? ParentId { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public int? IsLevel { get; set; }
        public int? Type { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsHome { get; set; }
        public bool? IsMenu { get; set; }
        public bool? IsDeleted { get; set; }
        public int? Sort { get; set; }
        public int? AgencyId { get; set; }
        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }
        public string SeoKeyword { get; set; }
        public string LanguageId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UserCreate { get; set; }
        public string UrlPicture { get; set; }
        public int? PictureID { get; set; }
        public decimal? Price { get; set; }
        public int? Count { get; set; }
        public int? CountNews { get; set; }
        public decimal? Percent { get; set; }
        public decimal? PercentLoss { get; set; }
        public decimal? PriceCost { get; set; }
        public decimal? Incurred { get; set; }
        public decimal? WeightDefault { get; set; }
        public int? UnitID { get; set; }
        public  string UnitName { get; set; }
        public int? TotalRecipe { get; set; }
        public decimal? PriceRecipe { get; set; }
        public decimal? PriceOld { get; set; }
        public decimal? Profit { get; set; }
        public decimal? PriceFinal { get; set; }
        public decimal? PriceRecipeFinal { get; set; }
        public decimal? TotalIncurredFinal { get; set; }
        public decimal? TotalPriceChild { get; set; }
        public decimal? CostPrice { get; set; }
         public IEnumerable<NewsItem> ListNewsItem { get; set; }
        public IEnumerable<ShopProductDetailItem> ListProductItem { get; set; }
        public IEnumerable<CategoryRecipeItem> LstRecipeCategory { get; set; }
    }
    public class CategoryAppItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
    }
    public class ModelCategoryItem : BaseModelSimple
    {
        public CategoryItem CategoryItem { get; set; }
        public IEnumerable<CategoryItem> ListItem { get; set; }
        public string Slug { get; set; }
        public string Listid { get; set; }
        public int? ParentId { get; set; }
        public Guid UserId { get; set; }
        public bool SelectMutil { get; set; }
    }

    public class CategoryRecipeItem : BaseSimple
    {
        public int? ProductId { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        public bool? IsDeleted { get; set; }
        public decimal? DateCreate { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public decimal? TotalPrice { get; set; }
        public bool? IsCheck { get; set; }
        public decimal? Percent { get; set; }
        public decimal? Incurred { get; set; }
        public decimal? PercentProduct { get; set; }
        public decimal? PriceProduct { get; set; }
        public decimal? TotalPriceProduct { get; set; }
    }
    public class MappingCategoryRecipeItem : BaseSimple
    {
        public int? CategoryID { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        public bool? IsDeleted { get; set; }
        public decimal? DateCreate { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public decimal? TotalPrice { get; set; }
        public bool? IsCheck { get; set; }
        public decimal? Percent { get; set; }
        public decimal? Incurred { get; set; }
        public decimal? PercentProduct { get; set; }
        public decimal? PriceProduct { get; set; }
        public decimal? TotalPriceProduct { get; set; }
        public int? Sl { get; set; }
    }
    public class RecipeCateItem
    {
        public int? ProductId { get; set; }
        public int? CategoryID { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? Percent { get; set; }
        public decimal? PercentProduct { get; set; }
        public decimal? Incurred { get; set; }
        public decimal? PriceProduct { get; set; }
        public int? IsCheck { get; set; }
        public int? Sl { get; set; }
        public string Key { get; set; }
    }
}

