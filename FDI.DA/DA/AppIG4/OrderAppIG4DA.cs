using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.CORE;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public partial class OrderAppIG4DA : BaseDA
    {
        #region Constructer
        public OrderAppIG4DA()
        {
        }

        public OrderAppIG4DA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public OrderAppIG4DA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        /// <summary>
        /// Lấy về tất cả kiểu đơn giản
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        public List<OrderAppIG4Item> GetListSimpleAll()
        {
            var query = from c in FDIDB.Orders
                        select new OrderAppIG4Item
                        {
                            ID = c.ID,
                            OrderTotal = c.OrderTotal,
                            CreatedOnUtc = c.CreatedOnUtc,
                            Status = c.Status
                        };
            return query.ToList();
        }
        // đây
        public List<OrderShopAppIG4Item> GetListProductByNew(int shopid, int status, int page, int take)
        {
            var query = from c in FDIDB.Orders
                        where c.ShopID == shopid && (c.Status == status || status == 0)
                        orderby c.ID descending
                        select new OrderShopAppIG4Item
                        {
                            ID = c.ID,
                            Customername = c.CustomerName,
                            Phone = c.Customer.Mobile,
                            TotalPrice = c.OrderTotal - (c.CouponPrice + (c.Discount * c.OrderTotal / 100)) + c.FeeShip,
                            UrlPicture = c.Customer.AvatarUrl,
                            Lat = c.CustomerAddress.Latitude,
                            Longt = c.CustomerAddress.Longitude,
                            Address = c.CustomerAddress.Address,
                            AddressType = c.CustomerAddress.AddressType,
                            FeeShip = c.FeeShip,
                            Level = c.Customer.CustomerPolicyID ?? 0,
                            Check = c.Check,
                            Note = c.Note,
                            ListItems = c.OrderDetails.Select(a => new OrderDetailAppIG4Item
                            {
                                ID = a.ID,
                                ProductName = a.Shop_Product.Name,
                                Quantity = a.Quantity,
                                Price = a.Price,
                                DateCreate = a.DateCreate,
                                Status = a.Status,
                                UrlPicture = a.Shop_Product.Gallery_Picture.Folder + a.Shop_Product.Gallery_Picture.Url,
                                Check = a.Check
                            })
                        };
            query = query.Skip((page - 1) * take).Take(take);
            return query.ToList();
        }
        /// <summary>
        /// Lấy về kiểu đơn giản, phân trang
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        public List<OrderAppIG4Item> GetListSimpleByRequest(HttpRequestBase httpRequest, ref decimal? total, ref decimal? totalsuccess, ref decimal? totalpending, ref decimal? totalcancel)
        {
            var isDelete = false;
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.Orders
                        where !c.IsDelete.HasValue || c.IsDelete == false
                        orderby c.ID descending
                        select new OrderAppIG4Item
                        {
                            ID = c.ID,
                            NoOrder = c.NoOrder,
                            Shopname = c.Customer.FullName,
                            Code = c.Code,
                            StatusPayment = c.StatusPayment,
                            OrderTotal = c.OrderTotal,
                            CreatedOnUtc = c.CreatedOnUtc,
                            Discount = c.Discount,
                            CouponPrice = c.CouponPrice,
                            Status = c.Status,
                            Payment = c.Payment,
                            IsDelete = c.IsDelete,
                            FeeShip = c.FeeShip,
                            CityName = c.System_City.Name,
                            District = c.District,
                            Wards = c.Wards,
                            PaymentmethodId = c.PaymentMethodId,
                            Note = c.Note,
                            CustomerName = c.CustomerName,
                            Mobile = c.Phone,
                            Address = c.Address,
                            Email = c.Email,
                            vpc_TransactionNo = c.vpc_TransactionNo,
                            ContactOrderID = c.ContactOrderID,

                        };

            var status = httpRequest["status"];

            if (!string.IsNullOrEmpty(status))
            {
                var id = int.Parse(status);
                query = query.Where(c => c.Status == id);
            }

            //var SearchIn = httpRequest["SearchIn"];

            #region search by CreatedOnUtc
            if (!string.IsNullOrEmpty(httpRequest["_dateStart"]))
            {
                var startDate = Convert.ToDateTime(httpRequest["_dateStart"]);
                query = query.Where(c => c.CreatedOnUtc >= startDate);
            }
            if (!string.IsNullOrEmpty(httpRequest["_dateEnd"]))
            {
                var endDate = Convert.ToDateTime(httpRequest["_dateEnd"]);
                query = query.Where(c => c.CreatedOnUtc <= endDate);
            }
            #endregion
            //if (!string.IsNullOrEmpty(httpRequest["method"]))
            //{
            //    var method = int.Parse(httpRequest["method"]);
            //    query = query.Where(c => c.PaymentmethodId == method);
            //}

            total = query.Sum(c => c.Payment);
            totalsuccess = query
                .Where(c => c.StatusPayment == (int)PaymentOrder.Complete && c.Status == (int)StatusOrder.Complete)
                .Sum(c => c.Payment);
            totalpending = query
                .Where(c => c.StatusPayment == (int)PaymentOrder.Process && c.Status == (int)StatusOrder.Process)
                .Sum(c => c.Payment);
            totalcancel = query
                .Where(c => c.Status == (int)StatusOrder.Cancel)
                .Sum(c => c.Payment);
            query = query.SelectByRequest(Request, ref TotalRecord);

            return query.ToList();
        }

        public List<StaticOrderAppIG4Item> GetListSimpleByRequestStatic(HttpRequestBase httpRequest, decimal perkygui, decimal percent, ref decimal? total, ref decimal? totalg)
        {

            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.Orders
                where c.IsDelete == false || !c.IsDelete.HasValue
                        orderby c.ID descending
                        select new StaticOrderAppIG4Item
                        {
                            ID = c.ID,
                            DateCreate = c.CreatedOnUtc,
                            Code = c.Code,
                            Seller = c.Customer.FullName,
                            Buyer = c.Customer.FullName,
                            Status = c.Status,
                            Payment = c.OrderTotal - (c.CouponPrice) + c.FeeShip,
                            Total = c.OrderTotal ?? 0,
                            FeeShip = c.FeeShip,
                            CouponPrice = c.CouponPrice,
                            Whsname = c.Customer.FullName,
                            Address = c.Address,
                            LstCateInts = c.OrderDetails.Select(a => a.Shop_Product.CategoryId).ToList(),
                            Quantity = c.OrderDetails.Sum(a => a.Quantity) ?? 1,
                            Productname = c.OrderDetails.Select(a => a.Shop_Product.Name).ToList(),
                            Categoryname = c.OrderDetails.Select(a => a.Shop_Product.Category.Name).ToList(),
                            Iskygui = c.Customer.Type == 2,
                            TotalKygui = c.OrderDetails.Where(a => a.IsPrestige == true).Sum(a => a.TotalPrice) ?? 0,
                            TotalNoKygui = c.OrderDetails.Where(a => a.IsPrestige == false || !a.IsPrestige.HasValue).Sum(a => a.TotalPrice) ?? 0,
                            Gstore = c.Customer.Type == 2 ? (c.OrderDetails.Where(a => a.IsPrestige == true).Sum(a => a.TotalPrice) ?? 0) * perkygui / 100 : (c.OrderTotal ?? 0) * percent / 100
                        };

            #region search by CreatedOnUtc
            if (!string.IsNullOrEmpty(httpRequest["_dateStart"]))
            {
                var startDate = Convert.ToDateTime(httpRequest["_dateStart"]);
                query = query.Where(c => c.DateCreate >= startDate);
            }
            if (!string.IsNullOrEmpty(httpRequest["_dateEnd"]))
            {
                var endDate = Convert.ToDateTime(httpRequest["_dateEnd"]);
                query = query.Where(c => c.DateCreate <= endDate);
            }
            #endregion
            var status = httpRequest["_status"];

            if (!string.IsNullOrEmpty(status))
            {
                var id = int.Parse(status);
                query = query.Where(c => c.Status == id);
            }
            var cate = httpRequest["_category"];
            if (!string.IsNullOrEmpty(cate))
            {
                var lstInt = FDIUtils.StringToListInt(cate);
                query = query.Where(a => a.LstCateInts.Intersect(lstInt).Any());
            }


            total = query.Sum(c => c.Payment);
            totalg = query.Sum(c => c.Gstore);
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<OrderAppIG4Item> GetListSimpleByRequestStaticExcel(HttpRequestBase httpRequest, ref decimal? total, ref decimal? totalVNpay, ref decimal? totalWallets, ref decimal? totalCod)
        {
            var isDelete = false;
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.Orders
                        where c.StatusPayment == (int)StatusOrder.Complete && c.Status == (int)StatusOrder.Complete
                      && (!c.IsDelete.HasValue || c.IsDelete == false)
                        orderby c.CreatedOnUtc descending

                        select new OrderAppIG4Item
                        {
                            ID = c.ID,
                            NoOrder = c.NoOrder,
                            Code = c.Code,
                            StatusPayment = c.StatusPayment,
                            OrderTotal = c.OrderTotal,
                            CreatedOnUtc = c.CreatedOnUtc,
                            Status = c.Status,
                            Payment = c.Payment,
                            IsDelete = c.IsDelete,
                            FeeShip = c.FeeShip,
                            CityName = c.System_City.Name,
                            District = c.District,
                            Wards = c.Wards,
                            PaymentmethodId = c.PaymentMethodId,
                            Note = c.Note,
                            CustomerName = c.CustomerName,
                            Mobile = c.Phone,
                            Address = c.Address,
                            Email = c.Email,
                            vpc_TransactionNo = c.vpc_TransactionNo,
                        };

            #region search by CreatedOnUtc
            if (!string.IsNullOrEmpty(httpRequest["_dateStart"]))
            {
                var startDate = Convert.ToDateTime(httpRequest["_dateStart"]);
                query = query.Where(c => c.CreatedOnUtc >= startDate);
            }
            if (!string.IsNullOrEmpty(httpRequest["_dateEnd"]))
            {
                var endDate = Convert.ToDateTime(httpRequest["_dateEnd"]);
                query = query.Where(c => c.CreatedOnUtc <= endDate);
            }
            #endregion
            if (!string.IsNullOrEmpty(httpRequest["method"]))
            {
                var method = int.Parse(httpRequest["method"]);
                query = query.Where(c => c.PaymentmethodId == method);
            }
            query = query.Where(c => c.IsDelete == isDelete);
            total = query.Sum(c => c.Payment);
            totalVNpay = query
                .Where(c => c.PaymentmethodId == 2)
                .Sum(c => c.Payment);
            totalWallets = query
                .Where(c => c.PaymentmethodId == 3)
                .Sum(c => c.Payment);
            totalCod = query
                .Where(c => c.PaymentmethodId == 1)
                .Sum(c => c.Payment);
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public List<OrderAppIG4Item> GetListExcelSimpleByRequest(HttpRequestBase httpRequest, ref decimal? total, ref decimal? totalsuccess, ref decimal? totalpending, ref decimal? totalcancel)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.Orders
                        where c.IsDelete == false
                        orderby c.CreatedOnUtc descending
                        select new OrderAppIG4Item
                        {
                            ID = c.ID,
                            NoOrder = c.NoOrder,
                            Code = c.Code,
                            StatusPayment = c.StatusPayment,
                            OrderTotal = c.OrderTotal,
                            CreatedOnUtc = c.CreatedOnUtc,
                            Status = c.Status,
                            Payment = c.Payment,

                            FeeShip = c.FeeShip,
                            CityName = c.System_City.Name,
                            District = c.District,
                            Wards = c.Wards,
                            PaymentmethodId = c.PaymentMethodId,
                            Note = c.Note,
                            CustomerName = c.CustomerName,
                            Mobile = c.Phone,
                            Address = c.Address,
                            Email = c.Email,
                            vpc_TransactionNo = c.vpc_TransactionNo,
                        };
            #region search by CreatedOnUtc

            if (!string.IsNullOrEmpty(httpRequest["_dateStart"]))
            {
                var startDate = Convert.ToDateTime(httpRequest["_dateStart"]);
                query = query.Where(c => c.CreatedOnUtc >= startDate);
            }
            if (!string.IsNullOrEmpty(httpRequest["_dateEnd"]))
            {
                var endDate = Convert.ToDateTime(httpRequest["_dateEnd"]);
                query = query.Where(c => c.CreatedOnUtc <= endDate);
            }
            #endregion

            if (!string.IsNullOrEmpty(httpRequest["status"]))
            {
                var status = int.Parse(httpRequest["status"]);
                query = query.Where(c => c.Status == status);
            }
            if (!string.IsNullOrEmpty(httpRequest["method"]))
            {
                var method = int.Parse(httpRequest["method"]);
                query = query.Where(c => c.PaymentmethodId == method);
            }

            total = query.Sum(c => c.Payment);
            totalsuccess = query
                .Where(c => c.StatusPayment == (int)PaymentOrder.Complete && c.Status == (int)StatusOrder.Complete)
                .Sum(c => c.Payment);
            totalpending = query
                .Where(c => c.StatusPayment == (int)PaymentOrder.Process && c.Status == (int)StatusOrder.Process)
                .Sum(c => c.Payment);
            totalcancel = query
                .Where(c => c.Status == (int)StatusOrder.Cancel)
                .Sum(c => c.Payment);
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        /// <summary>
        /// Lấy về mảng đơn giản qua mảng ID
        /// </summary>
        /// <param name="ltsArrID"></param>
        /// <returns></returns>
        public List<OrderAppIG4Item> GetListSimpleByArrID(List<int> ltsArrID)
        {
            var query = from c in FDIDB.Orders
                        where ltsArrID.Contains(c.ID)
                        orderby c.ID descending
                        select new OrderAppIG4Item
                        {
                            ID = c.ID,
                            OrderTotal = c.OrderTotal,
                            CreatedOnUtc = c.CreatedOnUtc,
                            Status = c.Status
                        };
            TotalRecord = query.Count();
            return query.ToList();
        }

        public OrderAppIG4Item GetOrderAppIG4ItemById(int orderId)
        {
            var query = from c in FDIDB.Orders
                        where c.ID == orderId
                        orderby c.ID descending
                        select new OrderAppIG4Item
                        {
                            ID = c.ID,
                            OrderTotal = c.OrderTotal,
                            CreatedOnUtc = c.CreatedOnUtc,
                            Status = c.Status,
                            Code = c.Code,
                            Note = c.Note,
                            FeeShip = c.FeeShip,
                            Payment = c.Payment,
                        };
            return query.FirstOrDefault();
        }
        public List<OrderAppIG4Item> GetOrderAppIG4ItemByCustomer(int cusId)
        {
            var query = from c in FDIDB.Orders
                        where c.CustomerID == cusId
                        orderby c.ID descending
                        select new OrderAppIG4Item
                        {
                            ID = c.ID,
                            Address = c.Address,
                            LisOrderDetailItems = c.OrderDetails.Where(a => a.Type == 2 && a.DateEnd > DateTime.Now.TotalSeconds())
                            .Select(v => new OrderDetailAppIG4Item
                            {
                                DateEnd = v.DateEnd,
                                DateCreate = v.DateCreate,
                                Type = v.Type
                            })
                        };
            return query.ToList();
        }
        public OrderAppIG4Item GetOrderLastestItemByCustomer(int cusId)
        {
            var query = from c in FDIDB.Orders
                        where c.CustomerID == cusId
                        orderby c.ID descending
                        select new OrderAppIG4Item
                        {
                            ID = c.ID,
                            Address = c.Address,
                        };
            return query.FirstOrDefault();
        }
        public PushNotifyItem GetNotifyById(int id)
        {
            var query = from c in FDIDB.PushNotifications
                        where c.ID == id
                        select new PushNotifyItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            Title = c.Title,
                            Content = c.Content
                        };
            return query.FirstOrDefault();
        }
        #region tạo số đơn hàng
        public string GetNoOrder()
        {
            const int maxCodeLength = 6;
            var countProduct = FDIDB.Orders.Count();
            var newCode = "";
            var nextNumber = countProduct + 1;
            for (var i = 0; i < maxCodeLength - countProduct.ToString().Length; i++)
            {
                newCode += "0";
            }
            return string.Concat(newCode, nextNumber.ToString());

        }
        #endregion

        #region Check Exits, Add, Update, Delete
        /// <summary>
        /// Lấy về bản ghi qua khóa chính
        /// </summary>
        /// <param name="id">ID bản ghi</param>
        /// <returns>Bản ghi</returns>
        public Base.Order GetById(int id)
        {
            var query = from c in FDIDB.Orders where c.ID == id select c;
            return query.FirstOrDefault();
        }
        public OrderDetail GetOrderDetailById(int id)
        {
            var query = from c in FDIDB.OrderDetails where c.ID == id select c;
            return query.FirstOrDefault();
        }
        /// <summary>
        /// Lấy về danh sách qua mảng id
        /// </summary>
        /// <param name="ltsArrID">Mảng ID</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<Base.Order> GetListByArrID(List<int> ltsArrID)
        {
            var query = from c in FDIDB.Orders where ltsArrID.Contains(c.ID) select c;
            return query.ToList();
        }
        public List<OrderDetail> GetListOrderDetailByArrID(List<int> ltsArrID)
        {
            var query = from c in FDIDB.OrderDetails where ltsArrID.Contains(c.ID) select c;
            return query.ToList();
        }
        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="systemCountry">bản ghi cần thêm</param>
        public void Add(Base.Order order)
        {
            FDIDB.Orders.Add(order);
        }

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="systemCountry">Xóa bản ghi</param>
        public void Delete(Base.Order order)
        {
            FDIDB.Orders.Remove(order);
        }

        /// <summary>
        /// save bản ghi vào DB
        /// </summary>
        public void Save()
        {
            FDIDB.SaveChanges();
        }
        #endregion
    }
}
