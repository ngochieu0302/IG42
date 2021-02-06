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
    
    public partial class Gallery_Picture
    {
        public Gallery_Picture()
        {
            this.Advertisings = new HashSet<Advertising>();
            this.Categories = new HashSet<Category>();
            this.Customers = new HashSet<Customer>();
            this.Customer_Type = new HashSet<Customer_Type>();
            this.DN_Combo = new HashSet<DN_Combo>();
            this.Gallery_Video = new HashSet<Gallery_Video>();
            this.Languages = new HashSet<Language>();
            this.News_News = new HashSet<News_News>();
            this.Partners = new HashSet<Partner>();
            this.Shop_Brands = new HashSet<Shop_Brands>();
            this.Shop_Product_Detail = new HashSet<Shop_Product_Detail>();
            this.Shop_Product_Detail1 = new HashSet<Shop_Product_Detail>();
            this.Shop_Product = new HashSet<Shop_Product>();
            this.Shop_Product_Picture = new HashSet<Shop_Product_Picture>();
            this.Shop_Product_Type = new HashSet<Shop_Product_Type>();
            this.Sources = new HashSet<Source>();
            this.System_FileType = new HashSet<System_FileType>();
            this.CustomerRatings = new HashSet<CustomerRating>();
            this.DN_Agency = new HashSet<DN_Agency>();
            this.ProductRatings = new HashSet<ProductRating>();
            this.Shop_Product_Detail2 = new HashSet<Shop_Product_Detail>();
        }
    
        public int ID { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public Nullable<int> Type { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsShow { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> SourceID { get; set; }
        public string Folder { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public string LanguageId { get; set; }
        public Nullable<decimal> DateCreated { get; set; }
    
        public virtual ICollection<Advertising> Advertisings { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Customer_Type> Customer_Type { get; set; }
        public virtual ICollection<DN_Combo> DN_Combo { get; set; }
        public virtual ICollection<Gallery_Video> Gallery_Video { get; set; }
        public virtual ICollection<Language> Languages { get; set; }
        public virtual ICollection<News_News> News_News { get; set; }
        public virtual ICollection<Partner> Partners { get; set; }
        public virtual ICollection<Shop_Brands> Shop_Brands { get; set; }
        public virtual ICollection<Shop_Product_Detail> Shop_Product_Detail { get; set; }
        public virtual ICollection<Shop_Product_Detail> Shop_Product_Detail1 { get; set; }
        public virtual ICollection<Shop_Product> Shop_Product { get; set; }
        public virtual ICollection<Shop_Product_Picture> Shop_Product_Picture { get; set; }
        public virtual ICollection<Shop_Product_Type> Shop_Product_Type { get; set; }
        public virtual ICollection<Source> Sources { get; set; }
        public virtual ICollection<System_FileType> System_FileType { get; set; }
        public virtual ICollection<CustomerRating> CustomerRatings { get; set; }
        public virtual ICollection<DN_Agency> DN_Agency { get; set; }
        public virtual ICollection<ProductRating> ProductRatings { get; set; }
        public virtual ICollection<Shop_Product_Detail> Shop_Product_Detail2 { get; set; }
    }
}
