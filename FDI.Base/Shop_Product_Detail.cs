//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FDI.Base
{
    using System;
    using System.Collections.Generic;
    
    public partial class Shop_Product_Detail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Shop_Product_Detail()
        {
            this.AttributeOptions = new HashSet<AttributeOption>();
            this.Category_Product_Recipe = new HashSet<Category_Product_Recipe>();
            this.FreightWarehouses = new HashSet<FreightWarehouse>();
            this.Mapping_ProductDetail_Recipe = new HashSet<Mapping_ProductDetail_Recipe>();
            this.ProduceCatogories = new HashSet<ProduceCatogory>();
            this.ProduceProductDetails = new HashSet<ProduceProductDetail>();
            this.Product_Promotion = new HashSet<Product_Promotion>();
            this.Product_Value = new HashSet<Product_Value>();
            this.ProductDetail_Recipe = new HashSet<ProductDetail_Recipe>();
            this.Shop_ContactOrder_Details = new HashSet<Shop_ContactOrder_Details>();
            this.Shop_Product = new HashSet<Shop_Product>();
            this.Categories = new HashSet<Category>();
            this.DN_Sale = new HashSet<DN_Sale>();
            this.Gallery_Picture2 = new HashSet<Gallery_Picture>();
            this.System_Tag = new HashSet<System_Tag>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Nullable<int> QuantityDay { get; set; }
        public Nullable<decimal> StartDate { get; set; }
        public Nullable<int> Minutes { get; set; }
        public Nullable<decimal> EndDate { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> PriceCost { get; set; }
        public Nullable<decimal> PriceOld { get; set; }
        public Nullable<int> Sale { get; set; }
        public string NameAscii { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public Nullable<int> UnitID { get; set; }
        public Nullable<bool> IsHot { get; set; }
        public Nullable<bool> IsShow { get; set; }
        public Nullable<int> PictureID { get; set; }
        public Nullable<int> Sort { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string SEOTitle { get; set; }
        public string SEODescription { get; set; }
        public string SEOKeyword { get; set; }
        public Nullable<int> CateID { get; set; }
        public Nullable<decimal> DateCreate { get; set; }
        public Nullable<int> IDPictureMap { get; set; }
        public Nullable<decimal> Percent { get; set; }
        public Nullable<decimal> Value { get; set; }
        public Nullable<decimal> Incurred { get; set; }
        public Nullable<System.Guid> UserID { get; set; }
        public Nullable<decimal> DateUpdate { get; set; }
        public Nullable<bool> IsShow24hApp { get; set; }
        public string Knowledge { get; set; }
        public string Proccess { get; set; }
        public Nullable<int> IsUpcoming { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<bool> IsAll { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AttributeOption> AttributeOptions { get; set; }
        public virtual Category Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Category_Product_Recipe> Category_Product_Recipe { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual DN_Unit DN_Unit { get; set; }
        public virtual DN_Users DN_Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FreightWarehouse> FreightWarehouses { get; set; }
        public virtual Gallery_Picture Gallery_Picture { get; set; }
        public virtual Gallery_Picture Gallery_Picture1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mapping_ProductDetail_Recipe> Mapping_ProductDetail_Recipe { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProduceCatogory> ProduceCatogories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProduceProductDetail> ProduceProductDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product_Promotion> Product_Promotion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product_Value> Product_Value { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductDetail_Recipe> ProductDetail_Recipe { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Shop_ContactOrder_Details> Shop_ContactOrder_Details { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Shop_Product> Shop_Product { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Category> Categories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DN_Sale> DN_Sale { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Gallery_Picture> Gallery_Picture2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<System_Tag> System_Tag { get; set; }
    }
}
