using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;
using FDI.CORE;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA.DA.AppCustomer
{
    public class CusLoginAppDA : BaseDA
    {
        #region Contruction

        public CusLoginAppDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public CusLoginAppDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }

        #endregion
        public CustomerAppItem GetUserItemByCode(string code)
        {
            var date = DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.Customers
                        where (!c.IsDelete.HasValue || !c.IsDelete.Value) && c.DN_Login.Any(v => v.Code == code && v.DateCreated < date && v.DateEnd > date && v.IsOut == false)
                        orderby c.ID descending
                        select new CustomerAppItem
                        {
                            ID = c.ID,
                            UserName = c.UserName,
                            Password = c.PassWord,
                            PasswordSalt = c.PasswordSalt,
                            Gender = c.Gender == true ? 1 : 0,
                            Address = c.Address,
                            Birthday = c.Birthday,
                            FullName = c.FullName,
                            Reward = c.Reward,
                            GroupID = c.GroupID,
                            Phone = c.Phone,
                            CodeLogin = code,
                            AgencyId = c.AgencyID,
                            PhoneAgency = c.DN_Agency.Phone
                        };
            return query.FirstOrDefault();
        }
        
        public CustomerAppItem GetPassByUserName(string userName)
        {
            //var s = Utility.S;
            var date = DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.Customers
                        where c.UserName == userName && (!c.IsDelete.HasValue || !c.IsDelete.Value)
                        select new CustomerAppItem
                        {
                            ID = c.ID,
                            UserName = c.UserName,
                            Password = c.PassWord,
                            PasswordSalt = c.PasswordSalt,
                            Gender = c.Gender == true ? 1 : 0,
                            Address = c.Address,
                            GroupID = c.GroupID,
                            Birthday = c.Birthday,
                            FullName = c.FullName,
                            Reward = c.Reward,
                            Phone = c.Phone,
                            AgencyId = c.AgencyID,
                            PhoneAgency = c.DN_Agency.Phone
                        };
            return query.FirstOrDefault();
        }
        public Customer GetByUserName(string userName)
        {
            //var s = Utility.S;
            var date = DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.Customers
                        where c.UserName == userName && (!c.IsDelete.HasValue || !c.IsDelete.Value)
                        select c;
            return query.FirstOrDefault();
        }
        public Customer GetByUserID(int cusId)
        {
            //var s = Utility.S;
            var date = DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.Customers
                        where c.ID == cusId && (!c.IsDelete.HasValue || !c.IsDelete.Value)
                        select c;
            return query.FirstOrDefault();
        }
        public List<CustomerGroupItem> GetAllGroup()
        {
            //var date = DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.Customer_Groups
                        where (!c.IsDelete.HasValue || !c.IsDelete.Value)
                        select new CustomerGroupItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                        };
            return query.ToList();
        }
        public bool Logout(string code)
        {
            var query = from c in FDIDB.DN_Login
                        where c.Code.ToLower() == code.ToLower()
                        select c;
            var obj = query.FirstOrDefault();
            var date = DateTime.Now;
            var time = date.TotalSeconds();
            if (obj != null)
            {
                obj.DateEnd = time;
                obj.IsOut = true;
                FDIDB.SaveChanges();
            }
            return true;
        }

        public CustomerItem GetCustomerItem(int cusId)
        {
            //var s = Utility.S;
            var date = DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.Customers
                where c.ID == cusId && (!c.IsDelete.HasValue || !c.IsDelete.Value)
                select new CustomerItem()
                {
                    FullName = c.FullName,
                    Address = c.Address,
                    Phone = c.Phone,
                    Email = c.Email,
                    Gender = c.Gender,
                    AddressBuyRecently = c.AddressBuyRecently,
                    LatitudeBuyRecently = c.LatitudeBuyRecently,
                    Latitude = c.Latitude,
                    LongitudeBuyRecently = c.LongitudeBuyRecently,
                    Longitude = c.Longitude,
                };
            return query.FirstOrDefault();
        }

        public void Add(DN_Login dnLogin)
        {
            FDIDB.DN_Login.Add(dnLogin);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
