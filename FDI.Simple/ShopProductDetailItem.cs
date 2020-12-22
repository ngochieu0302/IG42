using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class ShopProductDetailItem : BaseSimple
    {
        public decimal? QuantityDay { get; set; }
        public int? Quantity { get; set; }       
        public decimal? StartDate { get; set; }
        public int? Minutes { get; set; }
        public decimal? Price { get; set; }
        public decimal? PriceOld { get; set; }
        public decimal? PriceCost { get; set; }
        public decimal? Incurred { get; set; }
        public decimal? Percent { get; set; }
        public string Name { get; set; }
        public string NamePicture { get; set; }
        public string NameAscii { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public int? UnitID { get; set; }
        public bool? IsHot { get; set; }
        public bool? IsShow { get; set; }
        public int? PictureID { get; set; }
        public int? Sort { get; set; }
        public string UrlPictureMap { get; set; }
        public string UrlPicture { get; set; }
        public string Code { get; set; }
        public string ListCateId { get; set; }
        public string UnitName { get; set; }
        public int? Sale { get; set; }
        public int? CategoryID { get; set; }
        public decimal? Value { get; set; }
        public string Catename { get; set; }
        public decimal? DateCreate { get; set; } 
        public virtual IEnumerable<ProductItem> ListProductItem { get; set; }
        public virtual IEnumerable<GalleryPictureItem> ListGalleryPictureItems { get; set; }
        public virtual IEnumerable<ColorItem> ListColorProductItem { get; set; }
        public virtual IEnumerable<ProductSizeItem> ListSizeProductItem { get; set; }
        public virtual CategoryItem CategoryItem { get; set; }
        public virtual IEnumerable<CategoryItem> ListCategoryItem { get; set; }
        public IEnumerable<ProductDetailRecipeItem> DetailRecipeItem { get; set; }

    }
    public class ModelShopProductDetailItem : BaseModelSimple
    {
        public IEnumerable<ShopProductDetailItem> ListItem { get; set; }
        public ShopProductDetailItem Items { get; set; }
    }
    public class BreadcrumbItem
    {
        public string Breadcrumb { get; set; }
    }

    public class ShopProductDetailCartItem
    {
        public int? Quantity { get; set; }
        public int? ProductId { get; set; }
        public int? SizeID { get; set; }
        public int? ShopID { get; set; }
    }
    public class ShopProductCartItem : BaseSimple
    {
        public string Name { get; set; }
        public string NameAscii { get; set; }
        public string UrlPicture { get; set; }
        public decimal? Price { get; set; }
        public string ColorCode { get; set; }
        public string ColorName { get; set; }
        public string Size { get; set; }
        public decimal? Total { get; set; }
        public int? Quantity { get; set; }
        public int? ProductId { get; set; }
        public int? SizeId { get; set; }
        public int? ShopID { get; set; }
    }

    public class ProductDetailRecipeItem:BaseSimple
    {
        
        public decimal? ProductPrice { get; set; }
        public decimal? Quantity { get; set; }
        public string UnitName { get; set; }
        public decimal? Price { get; set; }
        public bool? IsDelete { get; set; }
        public string Key { get; set; }
        public string ProductName { get; set; }
        public int? ProductDetailId { get; set; }
        public decimal? DateCreate { get; set; }
        public bool? IsDeleted { get; set; }
        public decimal? ValuePrice { get; set; }
        public Guid? UserID { get; set; }
        public decimal? DateUpdate { get; set; }
        public bool? IsUse { get; set; }
        public int? TimeM { get; set; } // thời gian thực hiện sản phảm từ công thức
        public decimal? Incurred { get; set; } // chi phí phát sinh làm ra sản phẩm
        public string Code { get; set; }
        public string Note { get; set; }
        public string Username { get; set; }
        public IEnumerable<RecipeProductDetail> LstRecipeProductDetails { get; set; }
        public IEnumerable<RecipeProductValue> LstRecipeProductValues { get; set; }
    }

    public class ModelProductDetailRecipeItem:BaseModelSimple
    {
        public IEnumerable<ProductDetailRecipeItem> ListItems { get; set; }
    }
    public class ProductRecipeItem
    {
        public int? ValueId { get; set; }
        public int? ProductDetailID { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        public string Key { get; set; }
    }
    public class RecipeProductDetail:BaseSimple
    {
        public int? RecipeID { get; set; }
        public int? DetailID { get; set; }
        public decimal? Quantity { get; set; }
        public bool? IsDeleted { get; set; }
        public decimal? DateCreate { get; set; }
        public string ProductName { get; set; }
        public decimal? ProductPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public string UnitName { get; set; }
        public decimal UnitValue { get; set; }
    }
    public class RecipeProductValue : BaseSimple
    {
        public int? RecipeID { get; set; }
        public int? ProductValueId { get; set; }
        public decimal? Quantity { get; set; }
        public bool? IsDeleted { get; set; }
        public decimal? DateCreate { get; set; }
        public string ValueName { get; set; }
        public string UnitName { get; set; }
        public decimal? ValuePrice { get; set; }
        public decimal? TotalPrice { get; set; }
    }



}
