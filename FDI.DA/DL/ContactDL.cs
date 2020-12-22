using System.Collections.Generic;
using System.Linq;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public class ContactDL : BaseDA
    {
        public List<SystemConfigItem> SysConfig( )
        {
            var model = from c in FDIDB.System_Config
                where c.LanguageId == LanguageId && c.IsShow == true
                select new SystemConfigItem
                {
                    Email = c.Email,
                    Phone = c.Phone,
                    PhoneMobile = c.PhoneMobile,
                    PhoneAdvice = c.PhoneAdvice,
                    PhoneAdvice1 = c.PhoneAdvice1,
                    PhoneAdvice2 = c.PhoneAdvice2,
                    Name = c.Name,
                    Address = c.Address,
                    Fax = c.Fax,
                    Embedmaps = c.Embedmaps,
                    BusinessLicence = c.BusinessLicence,
                    Latitude = c.Latitude,
                    Website = c.Website,
                    Longitude = c.Longitude,
                    EmailSend = c.EmailSend,
                    EmailSendPwd = c.EmailSendPwd,
                    EmailReceive = c.EmailReceive
                };
            return model.ToList();
        }
        public SystemConfigItem SysConfigItems( )
        {
            var model = from c in FDIDB.System_Config
                where c.LanguageId == LanguageId && c.IsShow == true
                select new SystemConfigItem
                {
                    Email = c.Email,
                    Phone = c.Phone,
                    PhoneMobile = c.PhoneMobile,
                    PhoneAdvice = c.PhoneAdvice,
                    PhoneAdvice1 = c.PhoneAdvice1,
                    PhoneAdvice2 = c.PhoneAdvice2,
                    Name = c.Name,
                    Embedmaps = c.Embedmaps,
                    Address = c.Address,
                    Fax = c.Fax,
                    BusinessLicence = c.BusinessLicence,
                    Latitude = c.Latitude,
                    Website = c.Website,
                    Longitude = c.Longitude,
                    EmailSend = c.EmailSend,
                    EmailSendPwd = c.EmailSendPwd,
                    EmailReceive = c.EmailReceive
                };
            return model.FirstOrDefault();
        }
        public List<CityItem> GetCity()
        {
            var query = (from c in FDIDB.System_City
                         where c.LanguageID == "vi"
                         select new CityItem
                         {
                             ID = c.ID,
                             Name = c.Name
                         }).ToList();
            return query;
        }

        public List<GoogleMapItem> GetGoogleMap()
        {
            var query = (from c in FDIDB.GoogleMaps
                         where c.IsShow == true
                         select new GoogleMapItem
                         {
                             ID = c.ID,
                             Name = c.Name,
                             Address = c.Address,
                             Fax = c.Fax,
                             Tel = c.Tel,
                             Time = c.Time
                         }).ToList();
            return query;
        }

        public bool CheckEmail(string txt)
        {
            var query = from c in FDIDB.Customers
                        where c.Email.Equals(txt) && c.IsDelete == false
                        select c;
            return query.Any();

        }

        public void Add(CustomerContact customer)
        {
            FDIDB.CustomerContacts.Add(customer);
        }
        
        public void Add(Customer item)
        {
            FDIDB.Customers.Add(item);
        }
        
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}