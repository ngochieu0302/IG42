using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class AgencyDA : BaseDA
    {
        #region Constructer
        public AgencyDA()
        {
        }

        public AgencyDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public AgencyDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion
        public List<AgencyItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int enterprisesid, int areaId)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.DN_Agency
                        where (enterprisesid == 0 || o.EnterpriseID == enterprisesid) && o.IsDelete == false
                        //  && (areaId == 0 || o.Areas.Any(c => c.ID == areaId) || o.Market.AreaID == areaId)
                        orderby o.ID descending
                        select new AgencyItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            Email = o.Email,
                            Address = o.Address,
                            Phone = o.Phone,
                            IsShow = o.IsShow,
                            IsLock = o.IsLock,
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<AgencyItem> GetListSimpleByRequestStatic(HttpRequestBase httpRequest, int enterprisesid, int areaId)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.DN_Agency
                        where (enterprisesid == 0 || o.EnterpriseID == enterprisesid) && o.IsDelete == false
                        && (areaId == 0 || o.Areas.Any(c => c.ID == areaId) || o.Market.AreaID == areaId)
                        && o.DN_ImportProduct.Where(a => a.QuantityOut < a.Quantity && a.IsDelete == false).Sum(c => c.Quantity) > 0
                        orderby o.DN_ImportProduct.Count(a => a.QuantityOut < a.Quantity && a.IsDelete == false) descending
                        select new AgencyItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            Address = o.Address,
                            Phone = o.Phone,
                            MarketID = o.MarketID,
                            AreaID = o.Market.AreaID,
                            Total = o.DN_ImportProduct.Count(a => a.QuantityOut < a.Quantity && a.IsDelete == false)
                        };
            var area = httpRequest["Area_ID"];
            if (!string.IsNullOrEmpty(area))
            {
                var ida = int.Parse(area);
                query = query.Where(c => c.AreaID == ida);
            }
            var market = httpRequest["Market_ID"];
            if (!string.IsNullOrEmpty(area))
            {
                var idm = int.Parse(market);
                query = query.Where(c => c.MarketID == idm);
            }
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<AgencyItem> GetAll()
        {
            var query = from o in FDIDB.DN_Agency
                        where o.IsShow == true
                        orderby o.ID descending
                        select new AgencyItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            Email = o.Email,
                            Address = o.Address,
                            Phone = o.Phone,
                            IsShow = o.IsShow
                        };
            return query.ToList();
        }

        public List<AgencyItem> GetAll(int eid)
        {
            var query = from o in FDIDB.DN_Agency
                        where o.IsShow == true && o.EnterpriseID == eid && o.IsDelete == false
                        orderby o.ID descending
                        select new AgencyItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim()
                        };
            return query.ToList();
        }
        public List<AgencyItem> GetByCustomer(int customerId)
        {
            var query = from o in FDIDB.DN_Agency
                        where o.Shop_Orders.Any(c => c.CustomerID == customerId)
                        orderby o.ID descending
                        select new AgencyItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            PriceReceive = o.Customer_Reward.Where(u => u.CustomerID == customerId).Sum(u => u.PriceReceive),
                            PriceReward = o.Customer_Reward.Where(u => u.CustomerID == customerId).Sum(u => u.PriceReward)
                        };
            return query.ToList();
        }
        public List<SuggestionsProduct> GetListAuto(string keword, int showLimit, int agencyId)
        {
            //var price = FDIDB.DN_Agency.Where(c => c.ID == agencyId).Select(c => c.DN_Enterprises.PercentOrder ?? 0).FirstOrDefault();
            const int stt = (int)Card.Released;
            var query = from c in FDIDB.DN_Agency
                        where c.IsDelete == false && (c.Phone.Contains(keword) || c.FullName.Contains(keword))
                        select new SuggestionsProduct
                        {
                            ID = c.ID,
                            //value = c.CardID.HasValue ? "%" + c.DN_Card.Code + "?" : "",
                            title = c.FullName,
                            UrlImg = "/Content/Admin/images/auto-custommer.jpg",
                            data = c.Phone,
                            name = c.Address,
                            keword = "%" + keword + "?",
                            //Serial = c.CardID.HasValue ? c.DN_Card.Code : "",
                            phone = c.Phone,
                            code = c.Address,
                            //Birthday = c.Birthday,
                            pricenew = c.AgencyLevelId.HasValue ? c.DN_GroupAgency.Discount : 0,
                            //Unit = c.Customer_Care.Where(v => v.AgencyId == agencyId).Select(v => v.Note).FirstOrDefault(),
                            //TotalPrice = c.Customer_Reward.Where(u => u.AgencyID == agencyId).Select(v => v.PriceReward - v.PriceReceive).FirstOrDefault(),
                            //TotalWallet = c.Wallets.Sum(v => v.WalletCus - v.WalletOrder),
                            Type = 4
                        };
            return query.Take(showLimit).ToList();
        }
        public List<int> GetAllID()
        {
            var query = from o in FDIDB.DN_Agency
                        where o.IsShow == true
                        orderby o.ID descending
                        select o.ID;
            return query.ToList();
        }
        public AgencyItem GetItemById(int id)
        {
            var query = from c in FDIDB.DN_Agency
                        where c.ID == id
                        select new AgencyItem
                        {
                            ID = c.ID,
                            Code = c.Code,
                            Name = c.Name,
                            Address = c.Address,
                            FullName = c.FullName,
                            
                            GroupID = c.GroupID,
                            WalletValue = c.WalletValue,
                            Phone = c.Phone,
                            Email = c.Email,
                            IPTimekeep = c.IPTimekeep,
                            Port = c.Port,
                            MarketID = c.MarketID,
                            IsShow = c.IsShow,
                            IsFdi = c.IsFdi,
                            AgencyLevelId = c.AgencyLevelId,
                            UserName = c.DN_Users.OrderBy(m => m.CreateDate).Select(m => m.UserName).FirstOrDefault(),
                            LstDocumentItems = c.Documents.Select(v => new DocumentItem
                            {
                                Name = v.Name,
                                Value = v.Value,
                                DateStart = v.DateStart,
                                DateEnd = v.DateEnd,
                                Deposit = v.Deposit
                            })
                        };
            return query.FirstOrDefault();
        }
        public AgencyItem GetItem(string phone)
        {
            var query = from c in FDIDB.DN_Agency
                        where c.Phone == phone
                        select new AgencyItem
                        {
                            ID = c.ID,
                            Code = c.Code,
                            Name = c.Name,
                            Address = c.Address,
                            GroupID = c.GroupID,
                            WalletValue = c.WalletValue,
                            Phone = c.Phone,
                            Email = c.Email,
                            IPTimekeep = c.IPTimekeep,
                            Port = c.Port,
                            MarketID = c.MarketID,
                            IsShow = c.IsShow,
                            IsFdi = c.IsFdi,
                            AgencyLevelId = c.AgencyLevelId,

                        };
            return query.FirstOrDefault();
        }
        public AgencyItem GetItemByStatic(int id)
        {
            var query = from c in FDIDB.DN_Agency
                        where c.ID == id
                        select new AgencyItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            Address = c.Address,
                            Phone = c.Phone,
                            LstImportProductItems = c.DN_ImportProduct.Where(a => a.Quantity > a.QuantityOut && a.IsDelete == false).Select(v => new ImportProductItem
                            {
                                Name = v.Product_Value.Shop_Product_Detail.Name,
                                Value = v.Value,
                                PriceNew = v.PriceNew,
                                Date = v.Date,
                                DateEnd = v.DateEnd,
                            })
                        };
            return query.FirstOrDefault();
        }
        public DN_Agency GetById(int id)
        {
            var query = from c in FDIDB.DN_Agency where c.ID == id select c;
            return query.FirstOrDefault();
        }
        public void InsertDNModule(int? groupid, int agencyid, bool isdelete = false)
        {
            FDIDB.InsertDNModule(groupid, agencyid, isdelete);
        }
        public List<DN_Agency> GetListByArrId(string ltsArrID)
        {
            var arrId = FDIUtils.StringToListInt(ltsArrID);
            var query = from c in FDIDB.DN_Agency where arrId.Contains(c.ID) select c;
            return query.ToList();
        }
        #region tạo mã số doanh nghiệp
        public string GetCodeAgency()
        {
            int countProduct = FDIDB.DN_Agency.OrderByDescending(m => m.ID).Select(m => m.ID).FirstOrDefault();
            string newCode = "DL";
            var nextNumber = countProduct + 1;
            for (var i = 0; i < countProduct.ToString().Length; i++)
            {
                newCode += "0";
            }
            return string.Concat(newCode, nextNumber.ToString());

        }
        #endregion
        #region App
        public List<CustomerAppIG4Item> GetListAgencyListID(List<int> ltsArrId)
        {

            var query = from c in FDIDB.DN_Agency
                where (!c.IsDelete.HasValue || !c.IsDelete.Value) && ltsArrId.Contains(c.ID)
                orderby c.ID descending
                select new CustomerAppIG4Item
                {
                    ID = c.ID,
                    ParentID = c.ParentID,
                    //GroupID = c.GroupID,
                    ListID = c.ListID,
                    Email = c.Email,
                    
                    tokenDevice = c.TokenDevice,
                    //PrizeMoney = c.Customer_Groups.Discount,
                    //IsActive = c.WalletCustomers.Where(m => m.DateCreate > date && m.DateCreate < dateend && m.IsDelete == false).Sum(m => m.TotalPrice) >= 500000,
                    //WalletCus = c.WalletCustomers.Where(m => m.DateCreate > date && m.DateCreate < dateend && m.IsDelete == false).Sum(m => m.TotalPrice),
                    //WalletOr = c.WalletOrder_History.Where(m => m.DateCreate > date && m.DateCreate < dateend && m.IsDelete == false).Sum(m => m.TotalPrice),
                };
            return query.ToList();
        }
        public CustomerAppIG4Item GetItemByIdApp(int id)
        {
            var query = from c in FDIDB.DN_Agency
                where c.ID == id
                select new CustomerAppIG4Item
                {
                    ID = c.ID,
                    Address = c.Address,
                    Fullname = c.FullName,
                    Wallets = c.WalletValue,
                    Mobile = c.Phone,
                    Email = c.Email,
                    ParentID = c.ParentID,
                    ListID = c.ListID,
                    IsVerify =  c.IsVerify,
                    IsBank = c.IsBank,
                    Bankname = c.BankName,
                    FullnameBank = c.FullnameBank,
                    Branchname = c.Branchname,
                    IsActive = c.IsActive,
                    Level = c.Level,
                    UserName = c.DN_Users.OrderBy(m => m.CreateDate).Select(m => m.UserName).FirstOrDefault(),
                    
                };
            return query.FirstOrDefault();
        }

        public StaticWalletsTotal GetStaticChartsTotal(int id, decimal fo, decimal to)
        {
            var query = from c in FDIDB.StaticChartsAgencyTotalPrice(fo,to,id)
                        select new StaticWalletsTotal
                        {
                            Total = c.Total??0,
                            TotalCustomer = c.totalcustomer??0,
                            TotalAgency = c.totalagency??0,
                            TotalSouce = c.totalsouce??0,
                            Percent = ((c.Total - c.TotalOld)*c.TotalOld/100) ?? 0
                        };
            return query.FirstOrDefault();
        }
        public List<StaticWalletsTotal> GetStaticChartsTotalDay(int id, decimal fo, decimal to,int type)
        {
            var query = from c in FDIDB.StaticChartsbyDayAgencyTotalPrice(fo, to, id,type)
                select new StaticWalletsTotal
                {
                    Total = c.Total ?? 0,
                    DateCreate = c.DateCreate
                };
            return query.ToList();

        }
        public  List<ListRewardAgencyApp> GetListRewardApp(int id,int type ,int page, int take,decimal fr,decimal to,string key, ref decimal total)
        {
            var query = (from c in FDIDB.RewardHistories
                where c.IsActive == true && (!c.IsDeleted.HasValue || c.IsDeleted == false)
                      && c.AgencyId == id && (type == 0 || c.Type == type)
                      && c.DateCreate >= fr && c.DateCreate <= to
                      orderby c.DateCreate descending 
                select new ListRewardAgencyApp
                {
                    Fullname = c.Type == (int)Reward.Cus || c.Type == (int)Reward.Agency ? (c.Shop_Orders.Customer.FullName?? "Không xác định") : (c.Order_Package.Customer.FullName ?? "Không xác định"),
                    Avatar = c.Type == (int)Reward.Cus || c.Type == (int)Reward.Agency ? c.Shop_Orders.Customer.AvatarUrl : c.Order_Package.Customer.AvatarUrl,
                    Total = c.Price,
                    Des = c.Type == (int)Reward.Cus || c.Type == (int)Reward.Agency ? "Đã mua "+ c.Shop_Orders.Shop_Order_Details.Sum(a=>a.Quantity ?? 0)+ " sản phẩm" : "Đã gia hạn gói"+ c.Order_Package.Customer_Type.Customer_TypeGroup.Name
                    ,Date = c.DateCreate,
                }).ToList();
            if (!string.IsNullOrEmpty(key))
            {
                query = query.Where(a => a.Fullname.ToLower().Contains(key.ToLower())).ToList();
            }
            total = query.Sum(a => a.Total ?? 0);
            return query.Skip(take * (page - 1)).Take(take).ToList();
        }

        public bool CheckExitsByPhone(string phone,int id)
        {
            var query = (from c in FDIDB.DN_Agency
                         where c.Phone.Equals(phone) && c.IsDelete == false && (id == 0 || c.ID != id)
                         select c).Count();
            return query > 0;
        }
        public DN_Agency GetByPhone(string phone)
        {
            var query = from c in FDIDB.DN_Agency where c.Phone == phone && c.IsDelete == false select c;
            return query.FirstOrDefault();
        }
        public TokenRefresh GetTokenByGuidId(Guid id)
        {
            return FDIDB.TokenRefreshes.FirstOrDefault(m => m.GuidId == id);
        }

        public void DeleteTokenRefresh(TokenRefresh token)
        {
            FDIDB.Entry(token).State = System.Data.Entity.EntityState.Deleted;
        }
        public void InsertToken(TokenRefresh data)
        {
            FDIDB.TokenRefreshes.Add(data);
        }
        #endregion
        public void Add(DN_Agency agency)
        {
            agency.Code = GetCodeAgency();
            FDIDB.DN_Agency.Add(agency);
        }
        public void Delete(DN_Agency agency)
        {
            FDIDB.DN_Agency.Remove(agency);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }

    }
}