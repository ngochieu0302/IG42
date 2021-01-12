using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class CustomerItem : BaseSimple
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string CardSerial { get; set; }
        public int? CardID { get; set; }
        public int? GroupID { get; set; }
        public string GroupName { get; set; }
        public string PeoplesIdentity { get; set; }
        public decimal? Birthday { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string NoteCate { get; set; }        
        public bool? Gender { get; set; }
        public int? PictureID { get; set; }
        public decimal? DateCreated { get; set; }
        public int? DistrictID { get; set; }
        public int? CityID { get; set; }
        public string DistrictName { get; set; }
        public string CityName { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsQa { get; set; }
        public bool? IsDelete { get; set; }
        public string ParentName { get; set; }
        public decimal? PriceOrder { get; set; }
        public decimal? PrizeMoney { get; set; }    
        public string Token { get; set; }
        public int? AgencyId { get; set; }
        public string AgencyName { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? LatitudeBuyRecently { get; set; }
        public double? LongitudeBuyRecently { get; set; }
        public string AddressBuyRecently { get; set; }
        public string PhoneAgency { get; set; }
    }
    public class CustomerAwardItem : BaseSimple
    {
        public IEnumerable<LineItem> RewardHistoryItems { get; set; }
        public IEnumerable<LineItem> ListItem { get; set; }
    }
    public class ModelCustomerItem : BaseModelSimple
    {
        public IEnumerable<CustomerItem> ListItem { get; set; }
    }
}
