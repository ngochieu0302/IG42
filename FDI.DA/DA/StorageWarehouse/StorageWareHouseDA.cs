using FDI.Base;
using FDI.Simple;
using FDI.Simple.Order;
using FDI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Simple.StorageWarehouse;
using FDI.CORE;
using ConvertDate = FDI.CORE.ConvertDate;

namespace FDI.DA.DA.StorageWarehouse
{
    public class StorageWareHouseDA : BaseDA
    {
        #region Contruction
        public StorageWareHouseDA()
        {
        }
        public StorageWareHouseDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public StorageWareHouseDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<StorageWarehousingItem> GetListSimple(int agencyId)
        {
            var query = from o in FDIDB.StorageWarehousings
                        where o.AgencyId == agencyId && (!o.IsDelete.HasValue || o.IsDelete == false)
                        orderby o.ID descending
                        select new StorageWarehousingItem
                        {
                            ID = o.ID,
                            Code = o.Code,
                            TotalPrice = o.TotalPrice,
                            DateCreated = o.DateCreated,
                        };
            return query.ToList();
        }
        #region UserLogin
        public SWItem GetStorageByUser(int agencyId)
        {
            var date = ConvertDate.TotalSeconds(DateTime.Now.AddHours(12));// .TotalSeconds();
            var query = from o in FDIDB.StorageWarehousings
                        where o.AgencyId == agencyId && (!o.IsDelete.HasValue || o.IsDelete == false)
                        orderby o.ID descending
                        select new SWItem
                        {
                            id = o.ID,
                            n = o.Name,
                            d = o.DateRecive ?? 0,
                            LstItem = o.DN_RequestWare.Select(m => new DNRWHItem
                            {
                                //id = m.GID,
                                h = m.Hour ?? 0,
                                //d = m.Day ?? 0,
                                q = m.Quantity ?? 0,
                                s = date < (m.Today + m.Day * 86400 + m.Hour * 3600),
                            })
                        };
            var obj = query.FirstOrDefault() ?? new SWItem { d = ConvertDate.TotalSeconds(DateTime.Today.AddDays(1)) };
            obj.LstTimes = TypeTime.Hours(obj.d, date);
            return obj;
        }
        public SWItem GetStorageByID(int agencyId, int id)
        {
            var date = ConvertDate.TotalSeconds(DateTime.Now.AddHours(12));
            var query = from o in FDIDB.StorageWarehousings
                        where o.AgencyId == agencyId && (!o.IsDelete.HasValue || !o.IsDelete.Value) && o.ID == id
                        select new SWItem
                        {
                            id = o.ID,
                            n = o.Name,
                            d = o.DateRecive ?? 0,
                            LstItem = o.DN_RequestWare.Select(m => new DNRWHItem
                            {
                                //id = m.GID,
                                h = m.Hour ?? 0,
                                //d = m.Day ?? 0,
                                q = m.Quantity ?? 0,
                                s = date < (m.Today + m.Day * 86400 + m.Hour * 3600),
                            })
                        };
            var obj = query.FirstOrDefault() ?? new SWItem { d = ConvertDate.TotalSeconds(DateTime.Today.AddDays(1)) };
            obj.LstTimes = TypeTime.Hours(obj.d, date);
            return obj;
        }
        public List<SWItem> ListStorageByUser(HttpRequestBase httpRequest, int agencyId)
        {
            var date = ConvertDate.TotalSeconds(DateTime.Now.AddHours(12));
            var query = from o in FDIDB.StorageWarehousings
                        where o.AgencyId == agencyId && (!o.IsDelete.HasValue || !o.IsDelete.Value)
                        orderby o.ID descending
                        select new SWItem
                        {
                            id = o.ID,
                            n = o.Name,
                            d = o.DateRecive ?? 0,
                            s = o.DN_RequestWare.Any(m => m.DateEnd > date)
                        };
            return query.SelectByRequest(Request, ref TotalRecord).ToList();
        }
        #endregion
        public List<SuggestionsProduct> GetListAuto(string keword, int showLimit, int agencyId, int type = 1)
        {
            var name = FomatString.Slug(keword);
            var date = DateTime.Now;
            var query = FDIDB.AutoSelectProductDetail(name, showLimit).Select(c => new SuggestionsProduct
            {
                ID = c.ID,
                name = "<span>Mã SP: <b>" + (c.Code ?? "") + "</b> " + "</span>",
                UrlImg = c.UrlImg ?? "/Content/Admin/images/auto-default.jpg",
                value = c.Code,
                QuantityDay = c.QuantityDay,
                DateS = date.ToString("yyyy-MM-dd"),
                DateE = date.AddMonths(c.QuantityDay ?? 0).ToString("yyyy-MM-dd"),
                code = c.Code,
                data = c.Price.ToString(),
                title = c.Name,
                PriceCost = c.Price,
                pricenew = c.Price,
                Type = type
            });
            return query.ToList();
        }
        public List<SuggestionsProduct> GetListAutoProductValue(string keword, int showLimit, int agencyId, int type = 1)
        {
            var name = FomatString.Slug(keword);
            var date = DateTime.Now;
            var query = FDIDB.AutoSelectCateValueFull(name, showLimit, type).Select(c => new SuggestionsProduct
            {
                ID = c.ID,
                name = "<span>Giá bán: <b>" + (c.Price.Money()) + "</b> " + "</span>" + "<span>Đơn vị: <b>Con</b> " + "</span>",
                UrlImg = c.UrlImg ?? "/Content/Admin/images/auto-default.jpg",
                data = c.Price.ToString(),
                Unit = "Con",
                title = c.Name,
                pricenew = c.Price,
                Type = type
            }).ToList();
            return query.ToList();
        }
        public List<StorageWarehousingItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var to = httpRequest["toDate"];
            var fromDate = !string.IsNullOrEmpty(from) ? ConvertDate.TotalSeconds(ConvertUtil.ToDateTime(from)) : 0;
            var toDate = !string.IsNullOrEmpty(from) ? ConvertDate.TotalSeconds(ConvertUtil.ToDateTime(to)) : ConvertDate.TotalSeconds(DateTime.Now.AddDays(1));
            var query = from o in FDIDB.StorageWarehousings
                        where o.DateCreated >= fromDate && o.DateCreated <= toDate
                        && (!o.IsDelete.HasValue || o.IsDelete == false)
                        orderby o.ID descending
                        select new StorageWarehousingItem()
                        {
                            ID = o.ID,
                            Code = o.Code,
                            TotalPrice = o.TotalPrice ?? 0,
                            //Payment = o.Payment ?? 0,
                            DateCreated = o.DateCreated,
                            //DateImport = o.DateImport,
                            //UserID = o.UserID,
                            //Note = o.Note,
                            //HoursRecive = o.HoursRecive,
                            IsDelete = o.IsDelete,
                            //IsActive = o.IsActive,
                            Status = o.Status,
                            DateRecive = o.DateRecive,
                            Users = o.StorageWarehousingUsers.Select(m => new UserItem() { UserId = m.UserId.Value, Name = m.DN_Users.LoweredUserName, UserName = m.DN_Users.UserName }),
                            //CustomerName = o.Customer.UserName
                            AgencyName = o.DN_Agency.Name
                            //UsernameActive = o.DN_Users1.UserName,
                            //DateActive = o.DateActive,
                            //UsernameCreate = o.DN_Users.UserName,
                        };
            var status = httpRequest["IsDelete"];
            if (!string.IsNullOrEmpty(status))
            {
                var tus = int.Parse(status);
                query = query.Where(c => c.Status == tus);
            }
            var hours = httpRequest["hours"];
            if (!string.IsNullOrEmpty(hours))
            {
                var h = int.Parse(hours);
                query = query.Where(c => c.HoursRecive == h);
            }

            //ParramRequest parramRequest = Request;
            //if (parramRequest.SearchInField.IsNullOrEmpty())
            //{
            //    query = query.Where(m=>m.CustomerName)
            //}
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<DNRequestWareHouseItem> GetListSimpleAllByRequest(HttpRequestBase httpRequest, int area)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var query = from o in FDIDB.DN_RequestWare
                        where (!o.IsDelete.HasValue || o.IsDelete == false)
                        && o.AreaID == area
                        //    && o.StorageProductID == 1
                        orderby o.Hour descending
                        select new DNRequestWareHouseItem
                        {
                            GID = o.GID,
                            ProductName = o.Category.Name,
                            Today = o.Today,
                            Hours = o.Hour,
                            DateUpdate = o.DateUpdate,
                            UserUpdate = o.UserUpdate,
                            Quantity = o.Quantity,
                            QuantityActive = o.QuantityActive,
                            //Usercreate = o.StorageWarehousing.DN_Users.LoweredUserName,
                            Date = o.Date,
                        };
            if (!string.IsNullOrEmpty(from))
            {
                var fromDate = ConvertDate.TotalSeconds(ConvertUtil.ToDateTime(from));
                query = query.Where(c => c.Today == fromDate);
            }
            var hours = httpRequest["hours"];
            if (!string.IsNullOrEmpty(hours))
            {
                var h = int.Parse(hours);
                query = query.Where(c => c.Hours == h);
            }
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<DNRequestWareHouseItem> GetRequestWareByOrderId(int orderId)
        {
            var query = from o in FDIDB.DN_RequestWare
                        where o.IsDelete == false

                            && o.StorageProductID == orderId
                        orderby o.Hour descending
                        select new DNRequestWareHouseItem
                        {
                            GID = o.GID,
                            ProductName = o.Category.Name,
                            Today = o.Today,
                            Hours = o.Hour,
                            DateUpdate = o.DateUpdate,
                            UserUpdate = o.UserUpdate,
                            Quantity = o.Quantity,
                            QuantityActive = o.QuantityActive,
                            Details = o.DN_RequestWareDetail.Select(m => new RequestWareDetail() { ProductName = m.Shop_Product.Shop_Product_Detail.Name, ProductId = m.ProductId, Quantity = m.Quantity, Weight = m.Weight ?? 0 }),
                            RequestWareSuppliers = o.DN_RequestWareSupplier.Where(m => m.IsDelete != true).Select(m => new RequestWareSupplier() { Quantity = m.Quantity, SupplierName = m.DN_Supplier.Name, RequestWareId = m.RequestWareId, Id = m.Id }),
                            Date = o.Date,
                        };

            return query.ToList();
        }
        public List<DNRequestWareHouseItem> GetListSimpleStaticByRequest(HttpRequestBase httpRequest, int area)
        {
            Request = new ParramRequest(httpRequest);
            var date = ConvertDate.TotalSeconds(DateTime.Today);

            var query = from o in FDIDB.DN_RequestWare
                        where (!o.IsDelete.HasValue || o.IsDelete == false)
                        && (area == 0 || o.AreaID == area) && o.Today > date
                        group o by new
                        {
                            o.Hour,
                            o.CateID
                        } into g
                        orderby g.Key descending
                        select new DNRequestWareHouseItem
                        {
                            GID = g.Select(c => c.GID).FirstOrDefault(),
                            //Marketname = g.Select(c => c.Market.Name).FirstOrDefault(),
                            Today = g.Select(c => c.Today).FirstOrDefault(),
                            Hours = g.Select(c => c.Hour).FirstOrDefault(),
                            QuantityActive = g.Sum(c => c.QuantityActive),
                            ProductName = g.Select(c => c.Category.Name).FirstOrDefault(),

                        };
            var hours = httpRequest["hours"];
            if (!string.IsNullOrEmpty(hours))
            {
                var h = int.Parse(hours);
                query = query.Where(c => c.Hours == h);
            }
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<StorageWarehousingItem> GetListExcel(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var to = httpRequest["toDate"];
            var fromDate = !string.IsNullOrEmpty(from) ? ConvertDate.TotalSeconds(ConvertUtil.ToDateTime(from)) : 0;
            var toDate = !string.IsNullOrEmpty(from) ? ConvertDate.TotalSeconds(ConvertUtil.ToDateTime(to)) : ConvertDate.TotalSeconds(DateTime.Now.AddDays(1));
            var query = from o in FDIDB.StorageWarehousings
                        where (!o.IsDelete.HasValue || o.IsDelete == false) && o.DateCreated >= fromDate && o.DateCreated <= toDate
                        && o.Status == 1

                        orderby o.DateCreated descending
                        select new StorageWarehousingItem()
                        {
                            ID = o.ID,
                            TotalPrice = o.TotalPrice ?? 0,
                            DateCreated = o.DateCreated,
                            IsDelete = o.IsDelete,
                            DateRecive = o.DateRecive,
                            Status = o.Status,
                            LstImport = from v in o.DN_RequestWare
                                        select new DNRequestWareHouseItem()
                                        {
                                            GID = v.GID,
                                            Date = v.Date,
                                            ProductName = v.Category.Name,
                                            CateID = v.CateID,
                                            Quantity = v.Quantity,
                                            Price = v.Price,
                                            IsDelete = v.IsDelete,
                                            TotalPrice = v.TotalPrice,
                                        },
                        };
            var status = httpRequest["IsDelete"];
            if (!string.IsNullOrEmpty(status))
            {
                var tus = int.Parse(status);
                query = query.Where(c => c.Status == tus);
            }
            var hours = httpRequest["hours"];
            if (!string.IsNullOrEmpty(hours))
            {
                var h = int.Parse(hours);
                query = query.Where(c => c.HoursRecive == h);
            }
            return query.ToList();
        }
        public StorageWarehousingItem GetStorageWarehousingItem(int id)
        {
            var query = from c in FDIDB.StorageWarehousings
                        where c.ID == id
                        select new StorageWarehousingItem
                        {
                            ID = c.ID,
                            DateCreated = c.DateCreated,
                            AgencyId = c.AgencyId,
                            Code = c.Code,
                            TotalPrice = c.TotalPrice,
                            DateRecive = c.DateRecive,
                            PrizeMoney = c.PrizeMoney,
                            UrlConfirm = c.UrlConfirm,
                            Status = c.Status,
                            CustomerName = c.Customer.FullName,
                            Users = c.StorageWarehousingUsers.Select(m => new UserItem() { UserId = m.UserId.Value, Name = m.DN_Users.LoweredUserName, UserName = m.DN_Users.UserName }),
                            LstImport = from v in c.DN_RequestWare
                                        select new DNRequestWareHouseItem()
                                        {
                                            GID = v.GID,
                                            Date = v.Date,
                                            ProductName = v.Category.Name,
                                            CateID = v.CateID,
                                            Quantity = v.Quantity,
                                            QuantityActive = v.QuantityActive,
                                            Price = v.Price,
                                            IsDelete = v.IsDelete,
                                            TotalPrice = v.TotalPrice,
                                            Hours = v.Hour,
                                            RequestWareSuppliers = v.DN_RequestWareSupplier.Where(m => m.IsDelete != true).Select(m => new RequestWareSupplier() { Quantity = m.Quantity, SupplierId = m.SupplierId }),
                                        },
                            LstImportActive = from v in c.DN_RequestWarehousing
                                              select new DNRequestWareHouseActiveItem()
                                              {
                                                  ID = v.ID,
                                                  Code = v.Cate_Value.Code,
                                                  Date = v.Date,
                                                  ProductName = v.Cate_Value.Category.Name,
                                                  CateValueID = v.CateValueID,
                                                  Quantity = v.Quantity,
                                                  Price = v.Price,
                                                  BarCode = v.BarCode,
                                                  DateEnd = v.DateEnd,
                                                  IsDelete = v.IsDelete,
                                              },
                        };
            return query.FirstOrDefault();
        }

        public int[] GetProductInOrder(int id)
        {
            var query = from order in FDIDB.DN_RequestWare
                        where order.StorageProductID == id
                        select order.CateID ?? 0;
            return query.ToArray();
        }

        public List<TotalProductToDayItem> GetProductSummaryConfirm(int[] productids, decimal todayCode)
        {
            var query = from order in FDIDB.DN_RequestWare
                        where order.Today == todayCode && (order.IsDelete == false || order.IsDelete == null) && order.StorageWarehousing.Status == (int)StatusWarehouse.AgencyConfirmed && productids.Any(m => m == order.CateID)
                        select order;
            return query.GroupBy(m => m.CateID).
                Select(m => new TotalProductToDayItem() { ProductId = m.Key ?? 0, Quantity = m.Sum(n => n.QuantityActive) ?? 0 }).ToList();

        }

        public StorageWarehousingItem GetStorageWarehousingItem(decimal todayCode, int agencyId)
        {
            var query = from c in FDIDB.StorageWarehousings
                        where c.DateRecive == todayCode && c.CustomerId == agencyId && c.IsDelete == false
                        select new StorageWarehousingItem
                        {
                            ID = c.ID,
                            DateCreated = c.DateCreated,
                            CustomerId = c.CustomerId,
                            Code = c.Code,
                            Status = c.Status,
                        };
            return query.FirstOrDefault();
        }

        public StorageWarehousing GetById(string code, int customerId)
        {
            var query = from o in FDIDB.StorageWarehousings where o.Code == code && o.CustomerId == customerId && o.IsDelete == false select o;
            return query.FirstOrDefault();
        }
        public bool CheckExistOrder(string code, int agencyId)
        {
            return FDIDB.StorageWarehousings.Any(m =>
                m.Code == code && m.CustomerId == agencyId && m.IsDelete == false);
        }
        public List<DN_ImportProduct> GetListDNImportItem(int id)
        {
            var query = from c in FDIDB.DN_ImportProduct
                        where c.ProductValueID == id
                        select c;
            return query.ToList();
        }
        public List<DN_RequestWarehousing> GetListProductImport(List<int> lst)
        {
            var query = from o in FDIDB.Shop_Product
                        where lst.Contains(o.ID) && o.IsDelete == false
                        select new DN_RequestWarehousing
                        {
                            ID = o.ID,
                            Date = o.CreateDate,
                            DateEnd = o.EndDate,
                            Price = o.Shop_Product_Detail.Price * (decimal)o.Product_Size.Value / 1000
                        };
            return query.ToList();
        }
        public StorageWarehousing GetById(int id)
        {
            var query = from o in FDIDB.StorageWarehousings where o.ID == id select o;
            return query.FirstOrDefault();
        }
        public DN_RequestWare GetByIdDetai(Guid id)
        {
            var query = from o in FDIDB.DN_RequestWare where o.GID == id select o;
            return query.FirstOrDefault();
        }
        public StorageWarehousing GetByKey(Guid keyGuid)
        {
            var query = from o in FDIDB.StorageWarehousings select o;
            return query.FirstOrDefault();
        }
        public List<StorageWarehousing> GetListArrId(List<int> lst)
        {
            var query = from o in FDIDB.StorageWarehousings
                        where lst.Contains(o.ID)
                        orderby o.DateCreated
                        select o;
            return query.ToList();
        }
        public List<DN_ImportProduct> GetListArrIdImport(List<int> lst)
        {
            var query = from o in FDIDB.DN_ImportProduct where lst.Contains(o.ProductValueID ?? 0) select o;
            return query.ToList();
        }
        public List<Product_Value> GetListProductValueArr(int id)
        {
            var query = from o in FDIDB.Product_Value where o.CateValueID == id select o;
            return query.ToList();
        }
        public List<int> GetListProductValueListArr(List<int> lst)
        {
            var query = from o in FDIDB.Product_Value where lst.Contains(o.CateValueID ?? 0) select o.ID;
            return query.ToList();
        }
        public List<DN_ImportProduct> GetListImport(int id)
        {
            var query = from o in FDIDB.DN_ImportProduct where o.ProductValueID == id select o;
            return query.ToList();
        }
        public List<StorageWarehousingUser> GetAssignUser(int orderId)
        {
            return FDIDB.StorageWarehousingUsers.Where(m => m.StorageWarehousingId == orderId).ToList();
        }

        public void AddAssignUser(int orderId, IList<Guid> usersId)
        {
            foreach (var user in usersId)
            {
                var users = FDIDB.StorageWarehousingUsers.Add(new StorageWarehousingUser()
                {
                    UserId = user,
                    StorageWarehousingId = orderId
                });
            }
        }
        public void RemoveAssignUser(int orderId, IList<Guid> usersId)
        {
            foreach (var user in usersId)
            {
                var usersAssign = FDIDB.StorageWarehousingUsers
                    .Where(m => m.UserId == user && m.StorageWarehousingId == orderId).ToList();

                foreach (var item in usersAssign)
                {
                    FDIDB.StorageWarehousingUsers.Remove(item);
                }
            }
        }
        public void RemoveAssignUser(IList<StorageWarehousingUser> users)
        {
            foreach (var user in users)
            {
                FDIDB.StorageWarehousingUsers.Remove(user);
            }
        }

        public DN_RequestWare GetRequestWareById(Guid id)
        {
            return FDIDB.DN_RequestWare.FirstOrDefault(m => m.GID == id);
        }

        public List<DN_RequestWareSupplier> GetAllRequestWareByRequestWareId(Guid id)
        {
            return FDIDB.DN_RequestWareSupplier.Where(m => m.RequestWareId == id && m.IsDelete != true).ToList();
        }

        public IList<DNRequestWareHouseItem> GetRequestWareSummary(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var toDay = httpRequest["ToDay"];
            var fromDate = !string.IsNullOrEmpty(toDay) ? ConvertDate.TotalSeconds(ConvertDate.StringToDate(toDay)) : 0;

            var queryRequest = from c in FDIDB.DN_RequestWare
                               group c by new { c.Today, c.CateID, c.Category.Name }
                into g
                               select new
                               {
                                   ToDay = g.Key.Today,
                                   CateID = g.Key.CateID,
                                   ProductName = g.Key.Name,
                                   Quantity = g.Sum(m => m.Quantity)
                               };

            var queryBuy = from c in FDIDB.TotalProductToDays
                           group c by new { c.ToDayCode, c.ProductId, c.Category.Name } into g
                           select new
                           {
                               ToDayCode = g.Key.ToDayCode,
                               ProductName = g.Key.Name,
                               ProductId = g.Key.ProductId,
                               Quantity = g.Sum(m => m.Quantity)
                           };

            var query = queryRequest.GroupJoin(queryBuy, x =>
                new DNRequestWareHouseKey { ToDay = x.ToDay, ProductId = x.CateID },
                y => new DNRequestWareHouseKey { ToDay = y.ToDayCode, ProductId = y.ProductId },
                (a, b) => new
                {
                    a,
                    b
                }).SelectMany(x => x.b.DefaultIfEmpty(), (x, y) => new DNRequestWareHouseItem()
                {
                    Today = x.a.ToDay,
                    CateID = x.a.CateID,
                    ProductName = x.a.ProductName,
                    QuantityActive = y.Quantity,
                    Quantity = x.a.Quantity
                });

            query = query.OrderByDescending(m => m.Today).SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public IList<DNRequestWareHouseItem> GetRequestWareSummaryByProduct(decimal today, int productId)
        {
            var query = from c in FDIDB.DN_RequestWare
                        where c.Today == today && c.CateID == productId
                        group c by new { c.Today, c.CateID, c.Category.Name }
                into g
                        orderby g.Key.Today descending
                        select new DNRequestWareHouseItem
                        {
                            Today = g.Key.Today,
                            CateID = g.Key.CateID,
                            ProductName = g.Key.Name,

                            Quantity = g.Sum(m => m.Quantity)
                        };
            return query.ToList();
        }

        public DN_RequestWareSupplier GetRequestWareBySupplier(Guid id, int supplierId)
        {
            return FDIDB.DN_RequestWareSupplier.FirstOrDefault(m => m.RequestWareId == id && m.SupplierId == supplierId && m.IsDelete != true);
        }
        public DN_RequestWareSupplier GetRequestWareBySupplierById(int id)
        {
            return FDIDB.DN_RequestWareSupplier.FirstOrDefault(m => m.Id == id);
        }
        public void AddRequestWareSupplier(DN_RequestWareSupplier item)
        {
            FDIDB.DN_RequestWareSupplier.Add(item);
        }

        public void Add(StorageWarehousing item)
        {
            FDIDB.StorageWarehousings.Add(item);
        }
        public void AddStoragePrudct(StorageProduct item)
        {
            FDIDB.StorageProducts.Add(item);
        }
        public void Delete(DN_RequestWarehousing item)
        {
            FDIDB.DN_RequestWarehousing.Remove(item);
        }

        public IEnumerable<DNRequestWareItem> GetSummaryDetailConfirmed(decimal todayCode)
        {
            return from c in FDIDB.StorageWarehousings
                   join b in FDIDB.DN_RequestWare on c.ID equals b.StorageProductID
                   where c.DateRecive == todayCode && c.Status == (int)StatusWarehouse.AgencyConfirmed && b.Status == (int)CORE.DNRequestStatus.New
                   && (c.IsDelete == null || c.IsDelete == false) && (b.IsDelete == null || b.IsDelete == false)
                   group b by new { b.CateID, b.Category.Name, UnitName = b.Category.DN_Unit.Name }
                into g
                   select new DNRequestWareItem()
                   {
                       Quantity = g.Sum(m => m.QuantityActive),
                       UnitName = g.Key.UnitName,
                       ProductName = g.Key.Name,
                       CateID = g.Key.CateID
                   };

        }

        public List<StorageWarehousingItem> GetAll(decimal todayCode, int status)
        {
            var query = from o in FDIDB.StorageWarehousings
                        where o.DateRecive == todayCode && o.Status == status && (!o.IsDelete.HasValue || o.IsDelete == false)
                        orderby o.ID descending
                        select new StorageWarehousingItem
                        {
                            ID = o.ID,
                            Code = o.Code,
                            TotalPrice = o.TotalPrice ?? 0,
                            DateCreated = o.DateCreated,
                            IsDelete = o.IsDelete,
                            Status = o.Status,
                            DateRecive = o.DateRecive,
                            CustomerName = o.Customer.UserName
                        };
            return query.ToList();
        }

    }

}
