﻿using FDI.Base;
using FDI.Simple;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FDI.Utils;
using System.Data.Entity;
using FDI.CORE;

namespace FDI.DA
{
    public partial class CustomerAppIG4DA : BaseDA
    {
        #region Constructer
        public CustomerAppIG4DA()
        {
        }

        public CustomerAppIG4DA(string pathPaging)
        {
            PathPaging = pathPaging;

        }

        public CustomerAppIG4DA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<CustomerAppIG4Item> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.Customers
                        where c.IsDelete == false
                        orderby c.ID descending
                        select new CustomerAppIG4Item
                        {
                            ID = c.ID,
                            Fullname = c.FullName,
                            Mobile = c.Mobile,
                            IsActive = c.IsActive,
                            Email = c.Email,
                            DateCreated = c.DateCreated,
                            Type = c.Type,
                            PriceReward = c.Customer_Reward.Select(m => m.PriceReward).FirstOrDefault(),
                            TotalCP10 = c.Customer_Reward.Select(m => m.TotalCP).FirstOrDefault(),
                            PriceReceive = c.Customer_Reward.Select(m => m.PriceReceive).FirstOrDefault(),
                            PriceReceiver = c.Customer_Reward.Select(m => m.PriceReceiver).FirstOrDefault(),
                            CashOutWallet = c.Customer_Reward.Select(m => m.CashOutWallet).FirstOrDefault(),
                            TotalWallets = c.Customer_Reward.Select(m => m.PriceReward - m.PriceReceive).FirstOrDefault(),
                            Wallets = c.Wallets.Sum(a => (a.WalletCus ?? 0) - (a.CashOutWallet ?? 0))
                        };
            try
            {
                if (!string.IsNullOrEmpty(httpRequest["IsActive"]))
                {
                    if (Convert.ToInt32(httpRequest["IsActive"]) != -1)
                    {
                        var status = (httpRequest["IsActive"] == "1");
                        query = query.Where(o => o.IsActive == status);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogError(GetType(), ex);
            }

            var typ = httpRequest["IsType"];
            if (!string.IsNullOrEmpty(typ))
            {
                var type = int.Parse(typ);
                query = query.Where(o => o.Type == type);
            }
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public List<TopCustomerStaticAppIG4> Top100CustomerBuy(int page, int take)
        {
            var query = from c in FDIDB.Top100CustomerBuy()
                        select new TopCustomerStaticAppIG4
                        {
                            Fullname = c.FullName,
                            Email = c.Email,
                            Total = c.total,
                        };
            return query.Skip((page - 1) * take).Take(take).ToList();
        }
        public List<TopCustomerStaticAppIG4> Top100CustomerSell(int page, int take)
        {
            var query = from c in FDIDB.Top100CustomerSell()
                select new TopCustomerStaticAppIG4
                {
                    Fullname = c.FullName,
                    Email = c.Email,
                    Total = c.total,
                };
            return query.Skip((page - 1) * take).Take(take).ToList();
        }
        public List<SuggestionsProductAppIG4> GetListAuto(string keword, int showLimit)
        {
            var query = from c in FDIDB.Customers
                where c.IsDelete == false && (c.Mobile.Contains(keword) || c.FullName.Contains(keword))
                select new SuggestionsProductAppIG4
                {
                    ID = c.ID,
                    value = c.FullName,
                    title = c.FullName,
                    UrlImg = "/Content/Admin/images/auto-custommer.jpg",
                    data = c.Mobile,
                    name = c.Address,
                };
            return query.Take(showLimit).ToList();
        }
        public List<Select2Model> GetAllListSelect2(string key, int page, ref int total)
        {
            
            var query = from c in FDIDB.Customers
                where (c.IsDelete == false || !c.IsDelete.HasValue) && c.FullName.ToLower().Contains(key) 
                orderby c.ID descending
                select new Select2Model()
                {
                    text = c.FullName,
                    id = c.FullName,
                };
            total = query.Count();
            query = query.Skip((page - 1) * 10).Take(10);
            return query.ToList();
        }
        public List<Customer> GetListByArrId(List<int> ltsArrID)
        {
            var query = from c in FDIDB.Customers where ltsArrID.Contains(c.ID) select c;
            return query.ToList();
        }
        public List<CustomerAppIG4Item> GetListCustomerListID(List<int> ltsArrId)
        {

            var query = from c in FDIDB.Customers
                        where (!c.IsDelete.HasValue || !c.IsDelete.Value) && ltsArrId.Contains(c.ID)
                        orderby c.ID descending
                        select new CustomerAppIG4Item
                        {
                            ID = c.ID,
                            ParentID = c.ParentID,
                            //GroupID = c.GroupID,
                            ListID = c.ListID,
                            Email = c.Email,
                            LevelAdd = c.Customer_Policy.Number ?? 0,
                            tokenDevice = c.TokenDevice,
                            //PrizeMoney = c.Customer_Groups.Discount,
                            //IsActive = c.WalletCustomers.Where(m => m.DateCreate > date && m.DateCreate < dateend && m.IsDelete == false).Sum(m => m.TotalPrice) >= 500000,
                            //WalletCus = c.WalletCustomers.Where(m => m.DateCreate > date && m.DateCreate < dateend && m.IsDelete == false).Sum(m => m.TotalPrice),
                            //WalletOr = c.WalletOrder_History.Where(m => m.DateCreate > date && m.DateCreate < dateend && m.IsDelete == false).Sum(m => m.TotalPrice),
                        };
            return query.ToList();
        }
        public List<ListChartAppIG4Item> GetStaticBarcharCustomerInMonth(int month)
        {
            var query = from c in FDIDB.StaticChartsCustomer(month)
                        select new ListChartItem
                        {
                            I = c.I,
                            TotalCus = c.Total
                        };
            return query.ToList();
        }
        public List<ListChartPacketAppIG4Item> GetStaticCustomerbuyPacketInMonth(int month)
        {
            var query = from c in FDIDB.StaticChartsCustomerBuyPacket(month)
                select new ListChartPacketItem
                {
                    I = c.I,
                    Total = c.Total ?? 0
                };
            return query.ToList();
        }
        public List<ListOrderShopChartAppIG4Item> GetStaticChartsShop(int year,int month,int week,DateTime date,int shopId,int cateId)
        {
            var query = from c in FDIDB.StaticChartsShop(year,month,week,date,shopId, cateId)
                select new ListOrderShopChartItem
                {
                    I = c.I,
                    Total = c.Total
                };
            return query.ToList();
        }
        public OrderDetail CheckBook(int id, int cus)
        {
            var com = (int)StatusOrder.Complete;
            var date = DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.OrderDetails
                        where c.Order.CustomerID == cus && c.ProductID == id &&
                              ((c.IsTime == true && c.DateEnd > date) || ((!c.IsTime.HasValue || c.IsTime == false))) &&
                               c.Order.StatusPayment == com && c.Shop_Product.IsDelete == false
                        orderby c.ID descending
                        select c;
            return query.FirstOrDefault();
        }

        public List<CookieLoginAppIG4Item> GetCookiebyCusId(string name, int id)
        {
            var query = from c in FDIDB.CookieLogins
                        where c.Name == name && c.CustomerID == id
                        select new CookieLoginItem
                        {
                            CustomerID = c.CustomerID,
                            Value = c.Value,
                            DateCreate = c.DateCreate,
                            IsActive = c.IsActive,
                            key = c.key,

                        };
            return query.ToList();
        }
        public List<CookieLogin> GetCookiebyCusIdBase(string name, int id)
        {
            var query = from c in FDIDB.CookieLogins
                        where c.Name == name && c.CustomerID == id
                        select c;
            return query.ToList();
        }
        public CookieLogin GetCookiebyKey(string name, string key)
        {
            var query = from c in FDIDB.CookieLogins
                        where c.Name == name && c.key == key && c.IsActive == true
                        select c;
            return query.FirstOrDefault();
        }

        public void DeleteCookie(CookieLogin obj)
        {
            FDIDB.CookieLogins.Remove(obj);
        }
        public void AddCookie(CookieLogin obj)
        {
            FDIDB.CookieLogins.Add(obj);
        }

        public bool CheckBookReading(int id, int cus)
        {
            var date = DateTime.Now;
            var query = from c in FDIDB.Product_Reading
                        where c.CustomerID == cus && c.ProductId == id
                        //&&((c.IsTime == true && c.DateEnd > date) || (!c.IsTime.HasValue || c.IsTime == false))
                        select c;
            return query.Any();
        }
        public Customer GetById(int customerId)
        {
            var query = from c in FDIDB.Customers where c.ID == customerId select c;
            return query.FirstOrDefault();
        }

        public Product_Reading GetBookById(int customerId, int productId)
        {
            var query = from c in FDIDB.Product_Reading where c.CustomerID == customerId && c.ProductId == productId select c;
            return query.FirstOrDefault();
        }
        public Customer GetByEmail(string email)
        {
            var query = from c in FDIDB.Customers where c.Email == email && c.IsDelete == false select c;
            return query.FirstOrDefault();
        }
        public Customer GetByUsername(string username)
        {
            var query = from c in FDIDB.Customers where c.Email == username && c.IsDelete == false select c;
            return query.FirstOrDefault();
        }
        public Customer GetByidUserFacebook(string idUserFacebook)
        {
            var query = from c in FDIDB.Customers where c.idUserFacebook == idUserFacebook && c.IsDelete == false select c;
            return query.FirstOrDefault();
        }
        public Customer GetByidUserGoogle(string idUserGoogle)
        {
            var query = from c in FDIDB.Customers where c.idUserGoogle == idUserGoogle && c.IsDelete == false select c;
            return query.FirstOrDefault();
        }
        public Customer GetByPhone(string phone)
        {
            var query = from c in FDIDB.Customers where c.Mobile == phone && c.IsDelete == false select c;
            return query.FirstOrDefault();
        }
        public bool CheckExitsByEmail(string email)
        {
            var query = from c in FDIDB.Customers
                        where c.Email.Equals(email) && c.IsDelete == false
                        select c;
            return query.Any();
        }
        public bool CheckExitsByUser(string username)
        {
            var query = (from c in FDIDB.Customers
                         where c.UserName.Equals(username) && c.IsDelete == false
                         select c).Count();
            return query > 0;
        }
        public bool CheckExitsByPhone(string phone)
        {
            var query = (from c in FDIDB.Customers
                         where c.Mobile.Equals(phone) && c.IsDelete == false
                         select c).Count();
            return query > 0;
        }
        public bool CheckExitsByPhone(int customerId, string phone)
        {
            return FDIDB.Customers.Any(c => c.Mobile.Equals(phone) && c.ID != customerId && c.IsDelete == false);
        }
        public bool CheckExitsByEmail(int customerId, string email)
        {
            return FDIDB.Customers.Any(c => c.Email.Equals(email) && c.ID != customerId && c.IsDelete == false);
        }
        public void Add(Customer customer)
        {
            FDIDB.Customers.Add(customer);
        }

        public void AddBookReading(Product_Reading obj)
        {
            FDIDB.Product_Reading.Add(obj);
        }
        public void Delete(Customer customer)
        {
            customer.IsDelete = true;
            FDIDB.Customers.Attach(customer);
            var entry = FDIDB.Entry(customer);
            entry.Property(e => e.IsDelete).IsModified = true;
            // DB.Customers.Remove(customer);
        }
        public List<CustomerAppIG4Item> GetPrestige(int page, int pagesize)
        {
            var query = from c in FDIDB.Customers
                        where c.IsPrestige
                        orderby c.Ratings descending
                        select new CustomerAppIG4Item
                        {
                            ID = c.ID,
                            Fullname = c.FullName,
                            Address = c.Address,
                            Ratings = c.Ratings,
                            AvgRating = c.AvgRating,
                            LikeTotal = c.LikeTotal,
                            ImageTimeline = c.ImageTimeline
                        };

            query = query.Skip(pagesize * (page - 1)).Take(pagesize);
            return query.ToList();
        }
        public List<CustomerAppIG4Item> ShopSame(int shopid, double minKm, double maxKm, double latitude, double longitude, int page, int pagesize)
        {
            var query = from c in FDIDB.ShopSame(shopid,maxKm,minKm,latitude,longitude,page,pagesize)
                orderby c.Ratings descending
                select new CustomerAppIG4Item
                {
                    ID = c.ID,
                    Fullname = c.FullName,
                    Address = c.Address,
                    Ratings = c.Ratings,
                    AvgRating = c.AvgRating,
                    LikeTotal = c.LikeTotal,
                    ImageTimeline = c.ImageTimeline,
                    Latitude = c.Latitude,
                    Longitude = c.Longitude,
                    Km = c.km,
                };

            query = query.Skip(pagesize * (page - 1)).Take(pagesize);
            return query.ToList();
        }
        public List<OrderCustomerAppItem> GetListOrderCustomer(int customerId, int status, int page, int take)
        {
            var list = new List<OrderCustomerAppItem>();
            var query = from c in FDIDB.Orders
                         where c.CustomerID == customerId &&
                              ( c.Status == status || status == 0)
                         orderby c.ID descending
                         select new OrderCustomerAppItem
                         {
                             Id = c.ID,
                             PriceShip = c.FeeShip,
                             Price = c.OrderTotal,
                             Discount = c.Discount,
                             CouponPrice = c.CouponPrice,
                             Customername = c.Customer.FullName,
                             Phone = c.Customer.Mobile,
                             TotalPrice = c.OrderTotal - (c.CouponPrice + (c.Discount * c.OrderTotal / 100)) + c.FeeShip,
                             UrlPicture = c.Customer.AvatarUrl,
                             Check = c.OrderDetails.Select(a => a.Check).FirstOrDefault(),
                             LisOrderDetailItems = c.OrderDetails.Select(v => new OrderDetailCustomerAppItem
                             {
                                 Id = v.ID,
                                 Shopname = v.Customer.FullName,
                                 Productname = v.Shop_Product.Name,
                                 UrlPicture = v.Shop_Product.Gallery_Picture.Folder + v.Shop_Product.Gallery_Picture.Url,
                                 Status = v.Status,
                                 Price = v.Price,
                                 Quantity = v.Quantity,
                                 DateCreate = v.DateCreate,
                                 Check = v.Check,
                             })
                         };
            query = query.Skip((page - 1) * take).Take(take);
            return query.ToList();
        }
        public CustomerAppIG4Item GetItemByID(int id)
        {
            var query = from c in FDIDB.Customers
                        where c.ID == id

                        select new CustomerAppIG4Item
                        {
                            ID = c.ID,
                            Fullname = c.FullName,
                            Address = c.Address,
                            Ratings = c.Ratings,
                            AvgRating = c.AvgRating,
                            LikeTotal = c.LikeTotal,
                            ImageTimeline = c.ImageTimeline,
                            AvartaUrl = c.AvatarUrl,
                            Mobile = c.Mobile,
                            ListID = c.ListID,
                            tokenDevice = c.TokenDevice,
                            Wallets = c.Customer_Reward.Sum(a => a.PriceReward - a.PriceReceive),
                            TotalWallets = c.Wallets.Sum(a=>a.WalletCus - a.CashOutWallet),

                        };

            return query.FirstOrDefault();
        }
        public List<BonusTypeItem> ListBonusTypeItems(int type = 1)
        {
            var query = from c in FDIDB.BonusTypes
                        where c.Type == type
                        orderby c.ID descending
                        select new BonusTypeItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            RootID = c.RootID,
                            Description = c.Description,
                            PercentParent = c.PercentParent ?? 0,
                            PercentRoot = c.PercentRoot ?? 0,
                            Percent = c.Percent ?? 0,

                        };
            return query.ToList();
        }
        public CustomerAppIG4Item ShopPresigeDetail(int id)
        {
            var date = DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.Customers
                where c.ID == id
                        select new CustomerAppIG4Item()
                        {
                            ID = c.ID,
                            Fullname = c.FullName,
                            Address = c.CustomerAddresses.Where(m => m.IsDefault).Select(m => m.Address).FirstOrDefault(),
                            Ratings = c.Ratings,
                            ListCateId = c.Shop_Product.Select(a=>a.CategoryId).ToList(),
                            //ListCateId1 = c.Shop_Product1.Select(a=>a.CategoryId).ToList(),
                            AvgRating = c.AvgRating,
                            LikeTotal = c.LikeTotal,
                            ImageTimeline = c.ImageTimeline,
                            Latitude = c.CustomerAddresses.Where(m => m.IsDefault).Select(m => m.Latitude).FirstOrDefault(),
                            Longitude = c.CustomerAddresses.Where(m => m.IsDefault).Select(m => m.Longitude).FirstOrDefault(),
                            TypeId = c.Order_Package.Where(a => a.DateStart <= date && a.DateEnd > date).Select(a => a.Customer_Type.Type).FirstOrDefault(),
                            TypePoint = c.Order_Package.Where(a => a.DateStart <= date && a.DateEnd > date).Select(a => a.Customer_Type.Customer_TypeGroup.Value).FirstOrDefault() ?? 0
                        };
            return query.FirstOrDefault();
        }

        public CategoryAppIG4Item GetCategoryByShop(int id)
        {
            var query = from c in FDIDB.Shop_Product
                        where c.IsDelete == false && c.IsShow == true && c.CustomerID == id
                        select new CategoryAppIG4Item()
                        {
                            ID = c.CategoryId,
                            Name = c.Name
                        };
            return query.FirstOrDefault();
        }
        public List<NewsAppIG4Item> GetListPackage()
        {
            var query = from c in FDIDB.News_News
                        where c.Categories.Any(m => m.Id == 196)
                        orderby c.Sort
                        select new NewsAppIG4Item()
                        {
                            ID = c.ID,
                            Details = c.Details,
                            Type = c.Sort,
                            OrderPackageItems = FDIDB.Customer_Type.Where(m => (!m.IsDelete.HasValue || !m.IsDelete.Value) && m.Type == c.Sort).OrderBy(m => m.Sort).Select(m => new CustomerTypeItem
                            {
                                ID = m.ID,
                                Price = m.Price,
                                Name = m.Name
                            })
                        };
            return query.ToList();
        }
        public TokenRefresh GetTokenByGuidId(Guid id)
        {
            return FDIDB.TokenRefreshes.FirstOrDefault(m => m.GuidId == id);
        }

        public void DeleteTokenRefresh(TokenRefresh token)
        {
            FDIDB.Entry(token).State = EntityState.Deleted;
        }
        public void InsertToken(TokenRefresh data)
        {
            FDIDB.TokenRefreshes.Add(data);
        }
        public List<CustomerAppIG4Item> GetCustomerForMap(double km, double Latitude, double Longitude, int page, int pagesize)
        {
            var query = from c in FDIDB.Customers
                        join a in FDIDB.CustomerAddresses on c.ID equals a.CustomerId
                        where (!c.IsDelete.HasValue || c.IsDelete == false)
                              && c.Type == 2 && a.IsDefault
                              && ConvertUtil.DistanceBetween((float)Latitude, (float)Longitude, (float)(a.Latitude ?? 0), (float)(a.Longitude ?? 0)) <= km
                        select new CustomerAppIG4Item
                        {
                            ID = c.ID,
                            UserName = c.UserName,
                            Fullname = c.FullName,
                            Address = c.CustomerAddresses.Where(g=>g.IsDefault == true).Select(b=>b.Address).FirstOrDefault(),
                            Mobile = c.Mobile,
                            AvartaUrl = c.AvatarUrl,
                            Km = ConvertUtil.DistanceBetween((float)Latitude, (float)Longitude, (float)a.Latitude, (float)a.Longitude) / 1000,
                            ImageTimeline = c.ImageTimeline,
                        };

            query = query.OrderBy(m => m.Km).Skip(pagesize * (page - 1)).Take(pagesize);
            return query.ToList();
        }

        public void AddComment(CustomerRating data)
        {
            FDIDB.CustomerRatings.Add(data);
        }
        public CustomerRating GetComment(int shopId, int customerId)
        {
            return FDIDB.CustomerRatings.FirstOrDefault(m => m.CustomerId == customerId && m.ShopId == shopId);
        }
        public List<RatingAppIG4Item> GetTotalRating(int shopId)
        {
            return FDIDB.CustomerRatings.Where(m => m.ShopId == shopId).GroupBy(m => new { m.ShopId, m.TypeRating }).Select(m => new RatingItem() { TypeRating = m.Key.TypeRating, Quantity = m.Count() }).ToList();
        }
        public List<RatingAppIG4Item> GetCommentRatings(int id)
        {
            var query = FDIDB.CustomerRatings.Where(m => m.ShopId == id)
                .Select(m => new RatingItem
                {
                    TypeRating = m.TypeRating,
                    Title = m.Title,
                    Comment = m.Comment,
                    CustomerName = m.Customer.FullName,
                    DateCreated = m.DateCreated,
                    Gallery_Picture = m.Gallery_Picture.OrderBy(g => g.ID).Select(g => new PictureItem
                    {
                        Url = g.Folder + g.Url
                    })
                });
            return query.ToList();
        }
    }
}