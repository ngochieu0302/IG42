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
    
    public partial class Shop_Product
    {
        public Shop_Product()
        {
            this.Author_Product = new HashSet<Author_Product>();
            this.BiasProduces = new HashSet<BiasProduce>();
            this.Cost_Product = new HashSet<Cost_Product>();
            this.Cost_Product_User = new HashSet<Cost_Product_User>();
            this.DN_Product_Packet = new HashSet<DN_Product_Packet>();
            this.DN_RequestWareDetail = new HashSet<DN_RequestWareDetail>();
            this.Export_Product_Detail = new HashSet<Export_Product_Detail>();
            this.FreightWareHouse_Active = new HashSet<FreightWareHouse_Active>();
            this.ProductRatings = new HashSet<ProductRating>();
            this.Shop_Order_Details = new HashSet<Shop_Order_Details>();
            this.Shop_Product_Picture = new HashSet<Shop_Product_Picture>();
            this.TM_Module_Rating = new HashSet<TM_Module_Rating>();
            this.TM_Products_Comment = new HashSet<TM_Products_Comment>();
            this.DN_Combo = new HashSet<DN_Combo>();
            this.Categories = new HashSet<Category>();
        }
    
        public int ID { get; set; }
        public Nullable<int> ProductionCostID { get; set; }
        public Nullable<int> ProductDetailID { get; set; }
        public Nullable<int> SizeID { get; set; }
        public Nullable<int> ColorID { get; set; }
        public string CodeSku { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<int> QuantityOrder { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public string LanguageId { get; set; }
        public Nullable<decimal> EndDate { get; set; }
        public Nullable<decimal> CreateDate { get; set; }
        public Nullable<bool> IsShow { get; set; }
        public Nullable<bool> IsHot { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<int> QuantityDay { get; set; }
        public Nullable<int> TypeID { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<int> CategoyId { get; set; }
        public string Name { get; set; }
        public Nullable<int> CustomerID1 { get; set; }
        public Nullable<decimal> QuantityOut { get; set; }
        public Nullable<int> PictureID { get; set; }
        public int CategoryId { get; set; }
        public string NameAscii { get; set; }
        public string Code { get; set; }
        public string Author { get; set; }
        public Nullable<decimal> PriceNew { get; set; }
        public string Format { get; set; }
        public Nullable<decimal> Weight { get; set; }
        public Nullable<int> YearOfManufacture { get; set; }
        public Nullable<int> FileBookId { get; set; }
        public Nullable<int> FIleReadtryId { get; set; }
        public Nullable<decimal> DateCreated { get; set; }
        public Nullable<int> Sort { get; set; }
        public Nullable<bool> BookOld { get; set; }
        public Nullable<bool> HasTransfer { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public Nullable<int> Ratings { get; set; }
        public Nullable<double> AvgRating { get; set; }
        public Nullable<int> AddressId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public Nullable<bool> IsUpcoming { get; set; }
        public Nullable<int> freeShipFor { get; set; }
        public Nullable<int> Buyed { get; set; }
    
        public virtual ICollection<Author_Product> Author_Product { get; set; }
        public virtual ICollection<BiasProduce> BiasProduces { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Cost_Product> Cost_Product { get; set; }
        public virtual ICollection<Cost_Product_User> Cost_Product_User { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual CustomerAddress CustomerAddress { get; set; }
        public virtual ICollection<DN_Product_Packet> DN_Product_Packet { get; set; }
        public virtual ICollection<DN_RequestWareDetail> DN_RequestWareDetail { get; set; }
        public virtual ICollection<Export_Product_Detail> Export_Product_Detail { get; set; }
        public virtual ICollection<FreightWareHouse_Active> FreightWareHouse_Active { get; set; }
        public virtual Gallery_Picture Gallery_Picture { get; set; }
        public virtual Product_Size Product_Size { get; set; }
        public virtual ICollection<ProductRating> ProductRatings { get; set; }
        public virtual ICollection<Shop_Order_Details> Shop_Order_Details { get; set; }
        public virtual ICollection<Shop_Product_Picture> Shop_Product_Picture { get; set; }
        public virtual Shop_Product_Detail Shop_Product_Detail { get; set; }
        public virtual Shop_Product_Type Shop_Product_Type { get; set; }
        public virtual System_Color System_Color { get; set; }
        public virtual ICollection<TM_Module_Rating> TM_Module_Rating { get; set; }
        public virtual ICollection<TM_Products_Comment> TM_Products_Comment { get; set; }
        public virtual ICollection<DN_Combo> DN_Combo { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}
