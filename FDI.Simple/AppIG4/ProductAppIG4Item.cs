using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;

namespace FDI.Simple
{
    [Serializable]
    public class ProductAppIG4Item : BaseSimple
    {
        public int? LabelId { get; set; }
        public int? CateId { get; set; }
        public int? BrandId { get; set; }
        public int? PictureId { get; set; }
        public int? ProductTypeId { get; set; }
        public int? ColorId { get; set; }
        public int? Sale { get; set; }
        public int? SizeID { get; set; }
        public int? Sort { get; set; }
        public decimal? Quantity { get; set; }
        public int? freeShipFor { get; set; }
        public string Name { get; set; }
        public string NameCustomer { get; set; }
        public string CodeCustomer { get; set; }
        public string SubName { get; set; }
        public int? AuthorId { get; set; }
        public int? IssuingID { get; set; }
        public string AuthorName { get; set; }
        public string Language { get; set; }
        public bool? IsEbook { get; set; }
        public bool? IsBook { get; set; }
        public string Color { get; set; }
        public string NameAscii { get; set; }
        public string Url { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal? QuantityOut { get; set; }
        public string Translater { get; set; }
        public string Country { get; set; }
        public string Details { get; set; }
        public string IssuingName { get; set; }
        public string IssuingDescription { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsPrestige { get; set; }
        public bool? IsPrestige1 { get; set; }
        public bool? IsSlide { get; set; }
        public bool? IsHot { get; set; }
        public decimal? DateCreated { get; set; }
        public string CreateBy { get; set; }
        public string UrlPicture { get; set; }
        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }
        public string SeoKeyword { get; set; }
        public decimal? PriceOld { get; set; }
        public decimal? PriceNew { get; set; }
        public decimal? TotalPrice { get; set; }
        public string BrandName { get; set; }
        public string Material { get; set; }
        public int? Buyed { get; set; }
        public int? Viewer { get; set; }
        public string Guarantee { get; set; }
        public string Status { get; set; }
        public string TransportFee { get; set; }
        public string Author { get; set; }
        public string AboutAuthor { get; set; }
        public string Format { get; set; }
        public int? NumberPage { get; set; }
        public int? YearOfManufacture { get; set; }
        public int? ChapterNumber { get; set; }
        public string UrlPdf { get; set; }
        public bool? IsReadFree { get; set; }
        public string DescriptionReview { get; set; }
        public bool? BookOld { get; set; }
        public bool? hasShip{ get; set; }
        public int? TypePacket { get; set; }
        public int? TypePacketPoint { get; set; }
        public string PictureUrlIssUing { get; set; }
        public IEnumerable<int> LstInt { get; set; }
        public IEnumerable<PictureAppIG4Item> LstPictures { get; set; }
        public PictureAppIG4Item PictureItem { get; set; }
        public IEnumerable<TagItem> Tags { get; set; }
        public List<CategoryAppIG4Item> CategoryItems { get; set; }
        public CategoryItem CategoryItem { get; set; }
        public string FolderFile { get; set; }
        public string Filename { get; set; }
        public string FolderReadtry { get; set; }
        public string FileReadtry { get; set; }
        public int? FileReadId { get; set; }
        public int? FileReadTryId { get; set; }
        public string ISBNE { get; set; }
        public decimal? Weight { get; set; }
        public IEnumerable<ShopproductDetailsItem> LstShopproductDetailsItems { get; set; }
        public bool? HasTransfer { get; set; }
        public string Type { get; set; }
        public int? AddressId { get; set; }
        public int[] PictureDeleteIds { get; set; }
        public int Ratings { get; set; }
        public double AvgRating { get; set; }
        public double? Km { get; set; }
        public double? kc { get; set; }
        public double? kc1 { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CategoryName { get; set; }
        public string Address { get; set; }
        public string Address1 { get; set; }
        public int TopRankShop { get; set; }
        public string Phone { get; set; }
        public virtual IEnumerable<OrderDetailAppIG4Item> OrderDetails { get; set; }
        public virtual Customer1Item CustomerItem { get; set; }
        public virtual Customer2Item CustomerItem1 { get; set; }
        public virtual ProductSizeItem SizeItem { get; set; }
        public int? CustomerId1 { get; set; }
        public bool? IsShop { get; set; }
    }
    public class ModelProductAppIG4Item : BaseModelSimple
    {
        public ProductAppIG4Item ProductItem { get; set; }
        public IEnumerable<ProductAppIG4Item> ListItem { get; set; }
        public CategoryItem CategoryItem { get; set; }
        public int? TotalResult { get; set; }
        public string Author { get; set; }
        public IEnumerable<OrderDetailItem> ListProductBoughtItems { get; set; }
        public string type { get; set; }
        public string sort { get; set; }
        public decimal? Total { get; set; }
        public int? Quantity { get; set; }
    }
    public class ModelProductSearchItem : BaseModelSimple
    {
        public ProductAppIG4Item ProductAppIG4Item { get; set; }
        public IEnumerable<ProductAppIG4Item> ListItem { get; set; }
        public CategoryItem CategoryItem { get; set; }
        public int? TotalResult { get; set; }
        public IEnumerable<OrderDetailItem> ListProductBoughtItems { get; set; }
        public string type { get; set; }
        public string sort { get; set; }
        public string keyword { get; set; }
        public int? Cate { get; set; }
        public int? categoryFilter { get; set; }
        public string author { get; set; }
        public string lang { get; set; }
        public string price { get; set; }
        public IEnumerable<ItemSearch> lstCate { get; set; }
        public IEnumerable<ItemSearch> lstAuthor { get; set; }
        public IEnumerable<ItemSearch> lstPartners { get; set; }
        public IssuingUnitItem IssuingUnitItem { get; set; }
    }

    public class ItemSearch
    { 
        public int? Id { get; set; }
        public string Name { get; set; }
        public int? Count { get; set; }
        public string UrlPicture { get; set; }
    }
    public class ProductRssItem
    {
        public TextSyndicationContent Name { get; set; }
        public TextSyndicationContent Price { get; set; }
        public TextSyndicationContent Author { get; set; }
        public TextSyndicationContent Code { get; set; }
        public TextSyndicationContent Numberpage { get; set; }
        public TextSyndicationContent Booksize { get; set; }
        public TextSyndicationContent PublishYear { get; set; }
        public TextSyndicationContent Type { get; set; }
        public TextSyndicationContent Description { get; set; }
        public TextSyndicationContent Details { get; set; }
        public TextSyndicationContent AboutAuthor { get; set; }
        public List<SyndicationLink> Links { get; set; }
    }
    public class ProductApiItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal? PriceNew { get; set; }
        public string Author { get; set; }
        public string Code { get; set; }
        public int? NumberPage { get; set; }
        public string Format { get; set; }
        public int? YearOfManufacture { get; set; }
        public string LanguageId { get; set; }
        public int? AgencyId { get; set; }

    }

    public class ShopproductDetailsItem
    {
        public int ID { get; set; }
        public int? ProductId { get; set; }
        public int? ColorID { get; set; }
        public int? SizeID { get; set; }

        public virtual ProductAppIG4Item Shop_Product { get; set; }
        public virtual SizeItem Size { get; set; }
        public virtual ColorItem System_Color { get; set; }
    }

    public class ProductCartItem
    {
        public int ID { get; set; }
        public int? Quantity { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public string UrlPicture { get; set; }
        public int? Type { get; set; }
        public int? isThang { get; set; }
        public decimal? Total { get; set; }
        public string NameAscii { get; set; }
        public int? CateId { get; set; }
        public decimal? QuantityStorage { get; set; }
        public decimal? QuantityOut { get; set; }
    }

    public class KeySearch
    {
        public string Name { get; set; }
    }

    public class SuggestionsProductAppIG4
    {
        public int? ID { get; set; }
        public string UrlImg { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string data { get; set; }
        public string title { get; set; }
        public decimal? pricenew { get; set; }
        public int? Type { get; set; }
        public string value { get; set; }
    }
}
