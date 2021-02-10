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
            var query = from c in FDIDB.Shop_Orders
                        select new OrderAppIG4Item
                        {
                            ID = c.ID,
                            OrderTotal = c.Total,
                            DateCreated = c.DateCreated,
                            Status = c.Status
                        };
            return query.ToList();
        }
        public List<OrderAppIG4Item> GetListSimpleByCusId(int CusId)
        {
            var query = from c in FDIDB.Shop_Orders
                        where c.IsDelete != true && c.CustomerID == CusId
                        select new OrderAppIG4Item
                        {
                            ID = c.ID,
                            Status = c.Status,
                            OrderTotal = c.TotalPrice,
                            DateCreated = c.DateCreated,
                            ShopID = c.ShopID,
                            //ShopName = c.Customer1.FullName,
                            LisOrderDetailItems = c.Shop_Order_Details.Select(a => new OrderDetailAppIG4Item
                            {
                                ID = a.ID,
                                UrlPicture = a.Shop_Product.Gallery_Picture.Folder + a.Shop_Product.Gallery_Picture.Url,
                            })
                        };
            return query.ToList();
        }
        public List<OrderShopAppIG4Item> GetListProductByNew(int shopid, int status, int page, int take)
        {
            var query = from c in FDIDB.Shop_Orders
                        where c.ShopID == shopid && (status == 0 || c.Status == status)
                        orderby c.ID descending
                        select new OrderShopAppIG4Item
                        {
                            ID = c.ID,
                            Customername = c.CustomerName,
                            Phone = c.Customer.Mobile,
                            TotalPrice = c.Total - (c.CouponPrice + (c.Discount * c.Total / 100)) + c.FeeShip,
                            UrlPicture = c.Customer.AvatarUrl,
                            Lat = c.CustomerAddress.Latitude,
                            Longt = c.CustomerAddress.Longitude,
                            Address = c.CustomerAddress.Address,
                            AddressType = c.CustomerAddress.AddressType,
                            FeeShip = c.FeeShip,
                            Level = c.Customer.CustomerPolicyID ?? 0,
                            Check = c.Check,
                            Note = c.Note,
                            ListItems = c.Shop_Order_Details.Select(a => new OrderDetailAppIG4Item
                            {
                                ID = a.ID,
                                //GID = a.GID,
                                ProductName = a.Shop_Product.Name,
                                Quantity = a.Quantity,
                                Price = a.Price,
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


        /// <summary>
        /// Lấy về mảng đơn giản qua mảng ID
        /// </summary>
        /// <param name="ltsArrID"></param>
        /// <returns></returns>
        public List<OrderAppIG4Item> GetListSimpleByArrID(List<int> ltsArrID)
        {
            var query = from c in FDIDB.Shop_Orders
                        where ltsArrID.Contains(c.ID)
                        orderby c.ID descending
                        select new OrderAppIG4Item
                        {
                            ID = c.ID,
                            OrderTotal = c.Total,
                            DateCreated = c.DateCreated,
                            Status = c.Status
                        };
            TotalRecord = query.Count();
            return query.ToList();
        }

        public OrderAppIG4Item GetOrderAppIG4ItemById(int orderId)
        {
            var query = from c in FDIDB.Shop_Orders
                        where c.ID == orderId
                        orderby c.ID descending
                        select new OrderAppIG4Item
                        {
                            ID = c.ID,
                            OrderTotal = c.Total,
                            DateCreated = c.DateCreated,
                            Status = c.Status,
                            Code = c.Code,
                            Note = c.Note,
                            FeeShip = c.FeeShip,
                            Payment = c.Payments,
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
            var countProduct = FDIDB.Shop_Orders.Count();
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
        public Shop_Orders GetById(int id)
        {
            var query = from c in FDIDB.Shop_Orders where c.ID == id select c;
            return query.FirstOrDefault();
        }
        public Shop_Order_Details GetOrderDetailById(int id)
        {
            var query = from c in FDIDB.Shop_Order_Details where c.ID == id select c;
            return query.FirstOrDefault();
        }
        /// <summary>
        /// Lấy về danh sách qua mảng id
        /// </summary>
        /// <param name="ltsArrID">Mảng ID</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<Shop_Orders> GetListByArrID(List<int> ltsArrID)
        {
            var query = from c in FDIDB.Shop_Orders where ltsArrID.Contains(c.ID) select c;
            return query.ToList();
        }
        public List<Shop_Order_Details> GetListOrderDetailByArrID(List<long> ltsArrID)
        {
            var query = from c in FDIDB.Shop_Order_Details where ltsArrID.Contains(c.ID) select c;
            return query.ToList();
        }
        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="systemCountry">bản ghi cần thêm</param>
        public void Add(Shop_Orders order)
        {
            FDIDB.Shop_Orders.Add(order);
        }

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="systemCountry">Xóa bản ghi</param>
        public void Delete(Shop_Orders order)
        {
            FDIDB.Shop_Orders.Remove(order);
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
