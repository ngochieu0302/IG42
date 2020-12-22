using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public partial class CustomerDA : BaseDA
    {
        #region Constructer
        public CustomerDA()
        {
            //_systemCountryDA = new System_CountryDA("#");
            //_systemCityDA = new System_CityDA("#");
            //_systemDistrictDA = new System_DistrictDA("#");

        }

        public CustomerDA(string pathPaging)
        {
            PathPaging = pathPaging;
            //_systemCountryDA = new System_CountryDA("#");
            //_systemCityDA = new System_CityDA("#");
            //_systemDistrictDA = new System_DistrictDA("#");

        }

        public CustomerDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
            //_systemCountryDA = new System_CountryDA("#");
            //_systemCityDA = new System_CityDA("#");
            //_systemDistrictDA = new System_DistrictDA("#");

        }
        #endregion

        public List<CustomerItem> GetListAdminByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.Customers
                        where c.IsDelete == false
                        orderby c.ID
                        select new CustomerItem
                        {
                            ID = c.ID,
                            FullName = c.FullName,
                            UserName = c.UserName,
                            CardSerial = c.DN_Card.Serial,
                            PeoplesIdentity = c.PeoplesIdentity,
                            Birthday = c.Birthday,
                            Address = c.Address,
                            DateCreated = c.DateCreated,
                            IsActive = c.IsActive,
                            Phone = c.Phone,
                            Email = c.Email,
                            Gender = c.Gender,
                        };
            var isActive = httpRequest["IsActive"];
            if (isActive == "true")
                query = query.Where(c => c.IsActive == true);
            else if (isActive == "false")
                query = query.Where(c => c.IsActive == false);
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<CustomerItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.Customers
                        where c.IsDelete == false
                        orderby c.ID descending
                        select new CustomerItem
                        {
                            ID = c.ID,
                            UserName = c.UserName,
                            FullName = c.FullName,
                            CardSerial = c.DN_Card.Serial,
                            PeoplesIdentity = c.PeoplesIdentity,
                            Birthday = c.Birthday,
                            Address = c.Address,
                            DateCreated = c.DateCreated,
                            IsActive = c.IsActive,
                            Phone = c.Phone,
                            Email = c.Email,
                            Gender = c.Gender,
                            AgencyName = c.DN_Agency.Name
                        };
            var isActive = httpRequest["IsActive"];
            if (isActive == "true")
                query = query.Where(c => c.IsActive == true);
            else if (isActive == "false")
                query = query.Where(c => c.IsActive == false);
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public List<CustomerItem> GetDiscountRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.Customers
                        where c.IsDelete == false && c.GroupID != null
                        orderby c.ID descending
                        select new CustomerItem
                        {
                            ID = c.ID,
                            UserName = c.UserName,
                            FullName = c.FullName,
                            GroupID = c.GroupID,
                            GroupName = c.Customer_Groups.Name,
                            CardSerial = c.DN_Card.Serial,
                            PeoplesIdentity = c.PeoplesIdentity,
                            Birthday = c.Birthday,
                            Address = c.Address,
                            DateCreated = c.DateCreated,
                            IsActive = c.IsActive,
                            Phone = c.Phone,
                            Email = c.Email,
                            Gender = c.Gender
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<CustomerItem> GetList()
        {
            var query = from c in FDIDB.Customers
                        where c.IsDelete == false
                        orderby c.ID descending
                        select new CustomerItem
                        {
                            ID = c.ID,
                            FullName = c.FullName,
                            UserName = c.UserName,
                            CardSerial = c.DN_Card.Serial,
                            PeoplesIdentity = c.PeoplesIdentity,
                            Birthday = c.Birthday,
                            Address = c.Address,
                            DateCreated = c.DateCreated,
                            Phone = c.Phone,
                            Email = c.Email,
                            Gender = c.Gender,
                            IsActive = c.IsActive,
                            AgencyName = c.DN_Agency.Name
                        };
            return query.ToList();
        }

        public List<CustomerItem> GetAll()
        {
            var query = from c in FDIDB.Customers
                        where c.IsDelete == false
                        orderby c.ID descending
                        select new CustomerItem
                        {
                            ID = c.ID,
                            FullName = c.FullName,
                            UserName = c.UserName,
                            CardSerial = c.DN_Card.Serial,
                            PeoplesIdentity = c.PeoplesIdentity,
                            Birthday = c.Birthday,
                            Address = c.Address,
                            DateCreated = c.DateCreated,
                            Phone = c.Phone,
                            Email = c.Email,
                            Gender = c.Gender,
                            IsActive = c.IsActive,
                        };
            return query.ToList();
        }

        public List<CustomerItem> GetListByParent(int parentId)
        {
            var query = from c in FDIDB.Customers
                        where c.IsDelete == false
                        orderby c.ID descending
                        select new CustomerItem
                        {
                            ID = c.ID,
                            FullName = c.FullName,
                            PeoplesIdentity = c.PeoplesIdentity,
                            Birthday = c.Birthday,
                            Address = c.Address,
                            Phone = c.Phone,
                            Email = c.Email,
                            Gender = c.Gender
                        };
            return query.ToList();
        }
        public CustomerItem GetCustomerItem(int id, int agencyId)
        {
            var query = from c in FDIDB.Customers
                        where c.IsDelete == false && c.ID == id
                        orderby c.ID descending
                        select new CustomerItem
                        {
                            ID = c.ID,
                            FullName = c.FullName,
                            UserName = c.UserName,
                            CardID = c.CardID,
                            GroupID = c.GroupID,
                            CardSerial = c.DN_Card.Serial,
                            PeoplesIdentity = c.PeoplesIdentity,
                            Birthday = c.Birthday,
                            Address = c.Address,
                            DateCreated = c.DateCreated,
                            IsActive = c.IsActive,
                            Phone = c.Phone,
                            Email = c.Email,
                            Gender = c.Gender,
                            CityID = c.CityID,
                            DistrictID = c.DistrictID,
                            DistrictName = c.System_District.Name,
                            CityName = c.System_District.System_City.Name,
                            NoteCate = c.Customer_Care.Where(v => v.AgencyId == agencyId).Select(v => v.Note).FirstOrDefault()
                        };

            return query.FirstOrDefault();
        }
        public CustomerItem GetCustomerAwad(int id)
        {
            var query = from c in FDIDB.Customers
                        where c.IsDelete == false && c.ID == id
                        orderby c.ID descending
                        select new CustomerItem
                        {
                            ID = c.ID,
                            GroupID = c.GroupID,
                            PrizeMoney = c.Customer_Groups.Discount
                        };

            return query.FirstOrDefault();
        }
        public CustomerItem GetCustomerItem(int id)
        {
            var query = from c in FDIDB.Customers
                        where c.IsDelete == false && c.ID == id
                        orderby c.ID descending
                        select new CustomerItem
                        {
                            ID = c.ID,
                            FullName = c.FullName,
                            UserName = c.UserName,
                            CardID = c.CardID,
                            CardSerial = c.DN_Card.Serial,
                            PeoplesIdentity = c.PeoplesIdentity,
                            Birthday = c.Birthday,
                            Address = c.Address,
                            DateCreated = c.DateCreated,
                            IsActive = c.IsActive,
                            Phone = c.Phone,
                            Email = c.Email,
                            Gender = c.Gender,
                            CityID = c.CityID,
                            DistrictID = c.DistrictID,
                            DistrictName = c.System_District.Name,
                            CityName = c.System_District.System_City.Name,
                            AgencyId = c.AgencyID
                        };

            return query.FirstOrDefault();
        }
        public CustomerItem GetCustomerItem(string parent)
        {
            var query = from c in FDIDB.Customers
                        where c.IsDelete == false && (c.DN_Card.Serial == parent || c.PeoplesIdentity == parent)
                        select new CustomerItem
                        {
                            ID = c.ID,
                        };

            return query.FirstOrDefault();
        }

        public CustomerItem GetCustomerBySerial(string serial)
        {
            var query = from c in FDIDB.Send_Card
                        where c.Customer.DN_Card.Serial != serial
                        select new CustomerItem
                        {
                            CardID = c.CardID,
                            PeoplesIdentity = c.Customer.PeoplesIdentity
                        };
            return query.FirstOrDefault();
        }

        public CustomerItem GetCustomerItemBySerial(string serial)
        {
            var query = from c in FDIDB.Customers
                        where c.DN_Card.Serial == serial
                        select new CustomerItem
                        {
                            ID = c.ID,
                            FullName = c.FullName,
                            CardSerial = c.DN_Card.Serial
                        };
            return query.FirstOrDefault();
        }
        public CustomerItem GetPass(string userName)
        {
            var query = from c in FDIDB.Customers
                        where (c.UserName == userName || (c.CardID.HasValue && c.DN_Card.Serial == userName) || c.Phone == userName) && c.IsActive == true && c.IsDelete == false
                        select new CustomerItem
                        {
                            ID = c.ID,
                            UserName = c.UserName,
                            Password = c.PassWord,
                            PasswordSalt = c.PasswordSalt
                        };
            return query.FirstOrDefault();
        }
        public CustomerItem GetCustomerItemByUserName(string userName)
        {
            var query = from c in FDIDB.Customers
                        where (c.UserName == userName || (c.CardID.HasValue && c.DN_Card.Serial == userName) || c.Phone == userName) && c.IsActive == true && c.IsDelete == false
                        select new CustomerItem
                        {
                            ID = c.ID,
                            UserName = c.UserName,
                            FullName = c.FullName
                        };
            return query.FirstOrDefault();
        }
        public List<SuggestionsProduct> GetListAuto(string keword, int agencyId)
        {
            const int stt = (int)Card.Released;
            var query = from c in FDIDB.Customers
                        where c.IsDelete == false && c.DN_Card.Code == keword && c.DN_Card.Status == stt
                        select new SuggestionsProduct
                        {
                            value = "%" + c.DN_Card.Code + "?",
                            TotalPrice = c.Customer_Reward.Where(u => u.AgencyID == agencyId).Select(v => v.PriceReward - v.PriceReceive).FirstOrDefault(),
                            TotalWallet = c.Wallets.Sum(v => v.WalletCus - v.WalletOrder),
                        };
            return query.ToList();
        }
        public List<SuggestionsProduct> GetListAuto(string keword, int showLimit, int agencyId)
        {
            //var price = FDIDB.DN_Agency.Where(c => c.ID == agencyId).Select(c => c.DN_Enterprises.PercentOrder ?? 0).FirstOrDefault();
            const int stt = (int)Card.Released;
            var query = from c in FDIDB.Customers
                        where c.IsDelete == false && ((c.DN_Card.Code == keword && c.DN_Card.Status == stt) || c.Phone.Contains(keword) || c.FullName.Contains(keword))
                        select new SuggestionsProduct
                        {
                            ID = c.ID,
                            value = c.CardID.HasValue ? "%" + c.DN_Card.Code + "?" : "",
                            title = c.FullName,
                            UrlImg = "/Content/Admin/images/auto-custommer.jpg",
                            data = c.Phone,
                            name = c.Address,
                            keword = "%" + keword + "?",
                            Serial = c.CardID.HasValue ? c.DN_Card.Code : "",
                            phone = c.Phone,
                            code = c.Address,
                            Birthday = c.Birthday,
                            pricenew = c.GroupID.HasValue ? c.Customer_Groups.Discount : 0,
                            Unit = c.Customer_Care.Where(v => v.AgencyId == agencyId).Select(v => v.Note).FirstOrDefault(),
                            TotalPrice = c.Customer_Reward.Where(u => u.AgencyID == agencyId).Select(v => v.PriceReward - v.PriceReceive).FirstOrDefault(),
                            TotalWallet = c.Wallets.Sum(v => v.WalletCus - v.WalletOrder),
                            Type = 4
                        };
            return query.Take(showLimit).ToList();
        }

        public List<Customer> GetListByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.Customers where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }
        public List<TreeViewItem> GetListTree(int id, int lv, int agencyId)
        {
            var strId = id.ToString();
            var query = from c in FDIDB.Customers
                        where
                        c.ID == id
                        orderby c.ID
                        select new TreeViewItem
                        {
                            ID = c.ID,
                            UserName = c.UserName,
                            Name = c.FullName,
                        };
            return query.ToList();
        }
        public Customer GetById(int customerId)
        {
            var query = from c in FDIDB.Customers where c.ID == customerId select c;
            return query.FirstOrDefault();
        }

        public Customer GetByQrCode(string qrCode)
        {
            return FDIDB.Customers
                .FirstOrDefault(m => m.QRCode.Equals(qrCode, StringComparison.OrdinalIgnoreCase));
        }
        public DNCardItem GetCardCustomer(string card)
        {
            var query = from c in FDIDB.Send_Card
                        where c.DN_Card.Serial == card
                        select new DNCardItem
                        {
                            Code = c.DN_Card.Code,
                            CustomerID = c.CustomerID,
                            Serial = c.DN_Card.Serial,
                        };
            return query.FirstOrDefault();
        }
        public BonusTypeItem GetBonusTypeItem()
        {
            var query = from c in FDIDB.BonusTypes
                        orderby c.ID descending
                        select new BonusTypeItem
                        {
                            ID = c.ID,
                            RootID = c.RootID
                        };
            return query.FirstOrDefault();
        }
        public DNCardItem GetCardItem(string card, string pin)
        {
            var stt = (int)Card.Released;
            var query = from c in FDIDB.DN_Card
                        where c.Serial == card && c.PinCard == pin && c.Status == stt
                        select new DNCardItem
                        {
                            ID = c.ID,
                            CustomerID = c.Send_Card.Select(u => u.CustomerID).FirstOrDefault()
                        };
            return query.FirstOrDefault();
        }
        public bool CheckParent(string txt)
        {
            var query = FDIDB.Customers.Any(c => (c.DN_Card.Serial == txt || c.Phone == txt) && c.IsDelete == false);
            return query;
        }
        public bool CheckUserName(string txt)
        {
            var query = FDIDB.Customers.Any(c => c.UserName == txt && c.IsDelete == false);
            return query;
        }
        public bool CheckUserName(string txt, int id)
        {
            var query = FDIDB.Customers.Any(c => c.UserName == txt && c.IsDelete == false && c.ID != id);
            return query;
        }
        public bool CheckPhone(string txt, int id)
        {
            var query = FDIDB.Customers.Any(c => c.Phone.Equals(txt) && c.IsDelete == false && c.ID != id);
            return query;
        }
        public bool CheckEmail(string txt, int id)
        {
            var query = FDIDB.Customers.Any(c => c.Email == txt && c.IsDelete == false && c.ID != id);
            return query;
        }
        public void Add(Customer customer)
        {
            FDIDB.Customers.Add(customer);
        }
        public void Delete(Customer customer)
        {
            customer.IsDelete = true;
            FDIDB.Customers.Attach(customer);
            var entry = FDIDB.Entry(customer);
            entry.Property(e => e.IsDelete).IsModified = true;
            // DB.Customers.Remove(customer);
        }

        public List<CustomerItem> GetAllByAgencyId(int agencyId, int rowPerPage, int page, ref int total)
        {
          
            var query = from c in FDIDB.Customer_LastOrder
                        where c.AgencyID == agencyId
                        orderby c.DateCreated descending
                        select new CustomerItem
                        {
                            ID = c.ID,
                            FullName = c.FullName,
                            DateCreated = c.DateCreated,
                            Phone = c.Phone,

                        };
            query = query.Paging(page, rowPerPage, ref total);
            return query.ToList();
        }
    }
}
