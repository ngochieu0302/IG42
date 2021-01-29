using System;
using System.Collections.Generic;
using System.Linq;
using FDI.Base;
using FDI.Simple;
using System.Web;
using FDI.Utils;
using System.Threading.Tasks;

namespace FDI.DA
{
    public class CustomerAddressAppIG4DA : BaseDA
    {
        #region Constructer
        public CustomerAddressAppIG4DA()
        {
        }

        public CustomerAddressAppIG4DA(string pathPaging)
        {
            PathPaging = pathPaging;

        }

        public CustomerAddressAppIG4DA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion
        public List<CustomerAddressAppIG4Item> GetListSimpleByRequest(HttpRequestBase httpRequest,int id)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.CustomerAddresses
                        where c.IsDelete == false && c.CustomerId == id
                        orderby c.ID descending
                        select new CustomerAddressAppIG4Item()
                        {
                            ID = c.ID,
                            CustomerName = c.CustomerName,
                            Latitude = c.Latitude,
                            Longitude = c.Longitude,
                            Phone = c.Phone,
                            Address = c.Address,
                            AddressType = c.AddressType,
                            IsDefault = c.IsDefault,
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public void Add(CustomerAddress data)
        {
            FDIDB.CustomerAddresses.Add(data);
        }
        

        public bool CheckExit(int customerId, double latitude, double longitude)
        {
            return FDIDB.CustomerAddresses.Any(m => m.CustomerId == customerId && m.Longitude == longitude && m.Latitude == latitude && !m.IsDelete);
        }
        public bool CheckExit(int id, int customerId, double latitude, double longitude)
        {
            return FDIDB.CustomerAddresses.Any(m => m.ID != id && m.CustomerId == customerId && m.Longitude == longitude && m.Latitude == latitude && !m.IsDelete);
        }
        public void ResetDefault(int customerId)
        {
            var lst = FDIDB.CustomerAddresses.Where(m => m.CustomerId == customerId && !m.IsDelete).ToList();
            foreach (var item in lst)
            {
                item.IsDefault = false;
            }
        }
        public List<CustomerAddressAppIG4Item> GetAll(int customerId)
        {
            var query = from c in FDIDB.CustomerAddresses
                        where c.CustomerId == customerId
                        select new CustomerAddressAppIG4Item
                        {
                            ID = c.ID,
                            Phone = c.Customer.Mobile,
                            CustomerName = c.Customer.FullName,
                            Address = c.Address,
                            City = c.City,
                            District = c.District,
                            Commune = c.Commune,
                            AddressType = c.AddressType,
                            IsDefault = c.IsDefault,
                            Latitude = c.Latitude,
                            Longitude = c.Longitude
                        };
            return query.ToList();
        }
        public CustomerAddress GetById(int? id, int customerId)
        {
            return FDIDB.CustomerAddresses.FirstOrDefault(m => m.ID == id && m.CustomerId == customerId);
        }
        public CustomerAddress GetById(int id)
        {
            return FDIDB.CustomerAddresses.FirstOrDefault(m => m.ID == id);
        }
        public List<CustomerAddress> GetListArrById(List<int> lst)
        {
            return FDIDB.CustomerAddresses.Where(m => lst.Contains(m.ID)).ToList();
        }
        public CustomerAddressAppIG4Item GetItemById(int id)
        {
            var query = from c in FDIDB.CustomerAddresses
                        where c.ID == id
                        select new CustomerAddressAppIG4Item()
                        {
                            CustomerName = c.CustomerName,
                            Address = c.Address,
                            IsDefault = c.IsDefault,
                            Latitude = c.Latitude,
                            Longitude = c.Longitude,
                            Phone = c.Phone,
                            ID = c.ID,
                            AddressType = c.AddressType
                        };
            return query.FirstOrDefault();
        }
        public void save()
        {
            FDIDB.SaveChanges();

        }
       
    }
}
