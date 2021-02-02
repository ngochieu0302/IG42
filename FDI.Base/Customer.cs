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
    
    public partial class Customer
    {
        public Customer()
        {
            this.CashOutWallets = new HashSet<CashOutWallet>();
            this.CookieLogins = new HashSet<CookieLogin>();
            this.StorageWarehousings = new HashSet<StorageWarehousing>();
            this.Customer_Care = new HashSet<Customer_Care>();
            this.Customer_Reward = new HashSet<Customer_Reward>();
            this.CustomerAddresses = new HashSet<CustomerAddress>();
            this.CustomerRatings = new HashSet<CustomerRating>();
            this.DN_Login = new HashSet<DN_Login>();
            this.DN_Mail_SSC = new HashSet<DN_Mail_SSC>();
            this.DN_Mail_SSC1 = new HashSet<DN_Mail_SSC>();
            this.DN_StatusEmail = new HashSet<DN_StatusEmail>();
            this.DN_Users = new HashSet<DN_Users>();
            this.ReceiveHistories = new HashSet<ReceiveHistory>();
            this.Order_Package = new HashSet<Order_Package>();
            this.Product_Reading = new HashSet<Product_Reading>();
            this.ProductRatings = new HashSet<ProductRating>();
            this.ReceiptPayments = new HashSet<ReceiptPayment>();
            this.RewardHistories = new HashSet<RewardHistory>();
            this.Send_Card = new HashSet<Send_Card>();
            this.Shop_ContactOrder = new HashSet<Shop_ContactOrder>();
            this.Shop_Order_Details = new HashSet<Shop_Order_Details>();
            this.Shop_Orders = new HashSet<Shop_Orders>();
            this.Shop_Product = new HashSet<Shop_Product>();
            this.Shop_Product_Detail = new HashSet<Shop_Product_Detail>();
            this.SMS = new HashSet<SM>();
            this.Therapy_History = new HashSet<Therapy_History>();
            this.WalletCustomers = new HashSet<WalletCustomer>();
            this.WalletOrder_History = new HashSet<WalletOrder_History>();
            this.Wallets = new HashSet<Wallet>();
        }
    
        public int ID { get; set; }
        public Nullable<int> DistrictID { get; set; }
        public Nullable<int> CityID { get; set; }
        public Nullable<int> GroupID { get; set; }
        public Nullable<int> PictureID { get; set; }
        public string PassWord { get; set; }
        public string PasswordSalt { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public Nullable<bool> Gender { get; set; }
        public Nullable<decimal> Birthday { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PeoplesIdentity { get; set; }
        public Nullable<decimal> DateCreated { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<int> CardID { get; set; }
        public Nullable<decimal> Reward { get; set; }
        public Nullable<int> AgencyID { get; set; }
        public string QRCode { get; set; }
        public Nullable<double> Latitude { get; set; }
        public Nullable<double> Longitude { get; set; }
        public Nullable<double> LatitudeBuyRecently { get; set; }
        public Nullable<double> LongitudeBuyRecently { get; set; }
        public string AddressBuyRecently { get; set; }
        public string Mobile { get; set; }
        public Nullable<int> Type { get; set; }
        public string idUserFacebook { get; set; }
        public string idUserGoogle { get; set; }
        public Nullable<int> ParentID { get; set; }
        public string ListID { get; set; }
        public string TokenDevice { get; set; }
        public Nullable<int> CustomerPolicyID { get; set; }
        public Nullable<int> Ratings { get; set; }
        public Nullable<double> AvgRating { get; set; }
        public Nullable<int> LikeTotal { get; set; }
        public string ImageTimeline { get; set; }
        public bool IsPrestige { get; set; }
        public string AvatarUrl { get; set; }
        public string Description { get; set; }
        public string idUserZalo { get; set; }
    
        public virtual ICollection<CashOutWallet> CashOutWallets { get; set; }
        public virtual ICollection<CookieLogin> CookieLogins { get; set; }
        public virtual ICollection<StorageWarehousing> StorageWarehousings { get; set; }
        public virtual ICollection<Customer_Care> Customer_Care { get; set; }
        public virtual Customer_Groups Customer_Groups { get; set; }
        public virtual Customer_Policy Customer_Policy { get; set; }
        public virtual DN_Agency DN_Agency { get; set; }
        public virtual DN_Card DN_Card { get; set; }
        public virtual Gallery_Picture Gallery_Picture { get; set; }
        public virtual ICollection<Customer_Reward> Customer_Reward { get; set; }
        public virtual System_District System_District { get; set; }
        public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; }
        public virtual ICollection<CustomerRating> CustomerRatings { get; set; }
        public virtual ICollection<DN_Login> DN_Login { get; set; }
        public virtual ICollection<DN_Mail_SSC> DN_Mail_SSC { get; set; }
        public virtual ICollection<DN_Mail_SSC> DN_Mail_SSC1 { get; set; }
        public virtual ICollection<DN_StatusEmail> DN_StatusEmail { get; set; }
        public virtual ICollection<DN_Users> DN_Users { get; set; }
        public virtual ICollection<ReceiveHistory> ReceiveHistories { get; set; }
        public virtual ICollection<Order_Package> Order_Package { get; set; }
        public virtual ICollection<Product_Reading> Product_Reading { get; set; }
        public virtual ICollection<ProductRating> ProductRatings { get; set; }
        public virtual ICollection<ReceiptPayment> ReceiptPayments { get; set; }
        public virtual ICollection<RewardHistory> RewardHistories { get; set; }
        public virtual ICollection<Send_Card> Send_Card { get; set; }
        public virtual ICollection<Shop_ContactOrder> Shop_ContactOrder { get; set; }
        public virtual ICollection<Shop_Order_Details> Shop_Order_Details { get; set; }
        public virtual ICollection<Shop_Orders> Shop_Orders { get; set; }
        public virtual ICollection<Shop_Product> Shop_Product { get; set; }
        public virtual ICollection<Shop_Product_Detail> Shop_Product_Detail { get; set; }
        public virtual ICollection<SM> SMS { get; set; }
        public virtual ICollection<Therapy_History> Therapy_History { get; set; }
        public virtual ICollection<WalletCustomer> WalletCustomers { get; set; }
        public virtual ICollection<WalletOrder_History> WalletOrder_History { get; set; }
        public virtual ICollection<Wallet> Wallets { get; set; }
    }
}
