using System;
using System.Collections.Generic;
using System.ServiceModel.PeerResolvers;

namespace FDI.Simple
{
    public class CustomerAppIG4Item : BaseSimple
    {
        public string Fullname { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool? Gender { get; set; }
        public bool? IsPrestige { get; set; }
        public string TaxCode { get; set; }
        public DateTime? Birthday { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public decimal? DateCreated { get; set; }
        public string PeoplesIdentity { get; set; }
        public string PasswordSalt { get; set; }
        public string Password { get; set; }
        public int? DistrictId { get; set; }
        public int? CityId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsAdmin { get; set; }
        public int? TypeId { get; set; }
        public bool? IsLockedOut { get; set; }
        public decimal? Wallets { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string AvartaUrl { get; set; }
        public string ImageTimeline { get; set; }
        public string Description { get; set; }
        public Nullable<int> Ratings { get; set; }
        public Nullable<double> AvgRating { get; set; }
        public Nullable<int> LikeTotal { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public CategoryAppIG4Item CategoryItem { get; set; }
        public double? Km { get; set; }
        public decimal? PriceReward { get; set; }
        public decimal? PriceReceive { get; set; }
        public decimal? PriceReceiver { get; set; }
        public decimal? CashOutWallet { get; set; }
        public decimal? WalletCus { get; set; }
        public decimal? WalletOr { get; set; }
        public decimal? CashOut { get; set; }
        public decimal? TotalCP10 { get; set; }
        public decimal? TotalWallets { get; set; }
        public int? TotalCP { get; set; }
        public int? ParentID { get; set; }
        public string ListID { get; set; }
        public decimal? LevelAdd { get; set; }
        public int? Type { get; set; }
        public string tokenDevice { get; set; }
        public int? TypePoint { get; set; }
        public List<int> ListCateId { get; set; }
        public List<int> ListCateId1 { get; set; }
        public IEnumerable<CustomerAddressAppIG4Item> ListCustomerAddressItems { get; set; }
        public CustomerAddressAppIG4Item CustomerAddressItem { get; set; }
    }

    public class Customer1Item : BaseSimple
    {
        public string Fullname { get; set; }
        public string Mobile { get; set; }
        public byte? Type { get; set; }
        public bool? IsPrestige { get; set; }
        public int? TypePoint { get; set; }
        public CustomerAddressAppIG4Item CustomerAddressItem { get; set; }
    }
    public class Customer2Item : BaseSimple
    {
        public string Fullname { get; set; }
        public string Mobile { get; set; }
        public byte? Type { get; set; }
        public bool? IsPrestige { get; set; }
        public CustomerAddressAppIG4Item CustomerAddressItem { get; set; }
    }
    public class ModelCustomerAppIG4Item : BaseModelSimple
    {
        public IEnumerable<CustomerItem> ListItem { get; set; }
        public CustomerItem CustomerItem { get; set; }
        public string page { get; set; }
        public int? pageId { get; set; }
    }
    public class ModelCustomerAppIG4Checkout
    {
        public CustomerItem CustomerItem { get; set; }
        public List<ProductCartItem> ListcCartItems { get; set; }
        public decimal? PriceShip { get; set; }
    }

    public class ModelCustomerAppIG4History : BaseModelSimple
    {
        public CustomerItem CustomerItem { get; set; }
        public IEnumerable<OrderDetailItem> LstOrderDetailItems { get; set; }
    }

    public class ModelPurchasedbooksAppIG4:BaseModelSimple
    {
        public CustomerItem CustomerItem { get; set; }
        public int? Page { get; set; }
        public string sort { get; set; }
        public IEnumerable<OrderDetailItem> LstProductPurchaseItems { get; set; }
    }

    public class ProductReadingAppIG4Item:BaseSimple
    {
        public string ProductName { get; set; }
        public string Author { get; set; }
        public int? ProductId { get; set; }
        public int? NumberPage { get; set; }
        public string PictureUrl { get; set; }
        public decimal? Price { get; set; }
        public int? Page { get; set; }
        public int? Year { get; set; }
        public bool? IsEbook { get; set; }
        public DateTime? DateCreate { get; set; }
        public string ProductNameAscii { get; set; }
        public CategoryItem CategoryItem { get; set; }
}
    public class ModelBookReading : BaseModelSimple
    {
        public CustomerItem CustomerItem { get; set; }
        public int? Page { get; set; }
        public string sort { get; set; }
        public IEnumerable<ProductReadingAppIG4Item> LsstProductReadingItems { get; set; }
    }

    public class CookieLoginAppIG4Item:BaseSimple
    {
        public int? CustomerID { get; set; }
        public string key { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateCreate { get; set; }

    }

    public class ModelListChartAppIG4Item
    {
        public IEnumerable<ListChartAppIG4Item> ListChartItems { get; set; }
        public IEnumerable<ListChartPacketAppIG4Item> ListChartPacketItems { get; set; }

    }
    public class ListChartAppIG4Item
    {
        public int? I { get; set; }
        public int? TotalCus { get; set; }
        public decimal? TotalReward { get; set; }
    }
    public class ListChartPacketAppIG4Item
    {
        public int? I { get; set; }
        public decimal? Total { get; set; }
    }
    public class ListOrderShopChartAppIG4Item
    {
        public int? I { get; set; }
        public int? Total { get; set; }
    }
    public class jsonChartAppIG4
    {
        public string I { get; set; }
        public List<int> TotalCus { get; set; }
        public List<decimal> TotalReward { get; set; }

    }

    public class OrderShopAppIG4Item:BaseSimple
    {
        public string Customername { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int? AddressType { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? FeeShip { get; set; }
        public string UrlPicture { get; set; }
        public int? Level { get; set; }
        public double? Lat { get; set; }
        public double? Longt { get; set; }
        public int? Check { get; set; }
        public string Note { get; set; }
        public IEnumerable<OrderDetailAppIG4Item> ListItems { get; set; }
    }

    public class PostOtpLoginAppIG4
    {
        public string msisdn { get; set; }
        public string brandname { get; set; }
        public string msgbody { get; set; }
        public string telco { get; set; }
        public string user { get; set; }
        public string pass { get; set; }
        public string charset { get; set; }
    }
    public class ResultotpAppIG4 
    {
        public MessageAppIG4 Result { get; set; }
    }

    public class MessageAppIG4
    {
        public string code { get; set; }
        public string message { get; set; }
    }

    public class ConfigItemAppIG4
    {
        public decimal? Discount { get; set; }
        public decimal? FeeShip { get; set; }
    }

    public class TopCustomerStaticAppIG4
    {
        public string Fullname { get; set; }
        public string Email { get; set; }
        public decimal? Total { get; set; }
    }

}
