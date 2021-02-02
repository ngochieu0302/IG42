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
    public partial class OrdersDA : BaseDA
    {
        public OrdersDA()
        {
        }
        public OrdersDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public OrdersDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        public List<OrderItem> GetListSimpleAll()
        {
            var query = from c in FDIDB.Shop_Orders
                        select new OrderItem
                        {
                            ID = c.ID,
                            CustomerID = c.CustomerID,
                            DateCreated = c.DateCreated,
                            IsDelete = c.IsDelete,
                            UserID = c.UserId,
                            Note = c.Note,
                        };
            return query.ToList();
        }
        public OrderItem ContactToOrder(int id, Guid? UserId, int date, int dateend, int totalm)
        {
            var list = FDIDB.ContactToOrder(id, UserId, date, dateend, totalm).ToList();
            var contactToOrderResult = list.FirstOrDefault();
            if (contactToOrderResult != null)
            {
                var query = new OrderItem
                {
                    ID = contactToOrderResult.OrderID,
                    DN_Bed_Desk1 = list.Select(m => new BedDeskItem
                    {
                        ID = m.BedDeskID
                    })
                };
                return query;
            }
            return new OrderItem();
        }
        public List<OrderItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyid, out decimal? total, out decimal? totalPay, out decimal? totalDiscount)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var now = DateTime.Now;
            var to = httpRequest["toDate"];
            var status = int.Parse(httpRequest["Status"] ?? "0");
            var fromDate = !string.IsNullOrEmpty(from) ? DateTime.Parse(from).TotalSeconds() : new DateTime(now.Year, now.Month, 1).TotalSeconds();
            var toDate = !string.IsNullOrEmpty(to) ? DateTime.Parse(to).TotalSeconds() : now.AddDays(1).TotalSeconds();
            var query = from c in FDIDB.Shop_Orders
                        where (c.IsDelete.HasValue || c.IsDelete == false)
                        //   && c.DateCreated >= fromDate && c.DateCreated <= toDate
                        orderby c.ID descending
                        select new OrderItem
                        {
                            ID = c.ID,
                            CustomerName = c.CustomerName,
                            CutomerPhone = c.Mobile,
                            IsActive = c.IsActive,
                            DateCreated = c.DateCreated,
                            StartDate = c.StartDate,
                            EndDate = c.EndDate,
                            BedDeskID = c.BedDeskID,
                            UserIdBedDeskID = c.UserIdBedDeskID,
                            Mobile = c.Mobile,
                            UserName = c.DN_Users.UserName,
                            UserName1 = c.DN_Users2.UserName,
                            CodeUser = c.DN_Users2.CodeCheckIn,
                            TotalMinute = c.TotalMinute,
                            TotalPrice = c.TotalPrice ?? 0,
                            PrizeMoney = c.PrizeMoney ?? 0,
                            Payments = c.Payments ?? 0,
                            Discount = c.Discount,
                            Deposits = c.Deposits ?? 0,
                            IsDelete = c.IsDelete,
                            UserID = c.UserId,
                            Note = c.Note,
                            Status = c.Status,
                        };
            //if (status > 0 && status == (int)FDI.CORE.OrderStatus.Pending)
            //{
            //    const int t1 = (int)FDI.CORE.OrderStatus.Pending;
            //    const int t2 = (int)FDI.CORE.OrderStatus.Processing;
            //    query = query.Where(m => m.Status == t1 || m.Status == t2);
            //}
            ////else if (status == (int)FDI.CORE.OrderStatus.Debt)
            ////{
            ////    const int t3 = (int)FDI.CORE.OrderStatus.Cancelled;
            ////    query = query.Where(m => m.Status != t3 && m.Payments < m.TotalPrice);
            ////}
            //else if (status > 0 && status != (int)FDI.CORE.OrderStatus.Pending)
            //{
            //    query = query.Where(m => m.Status == status);
            //}

            //if (!string.IsNullOrEmpty(Request.Keyword))
            //{
            //    query = Request.SearchInField.Aggregate(query, (current, propSearch) => current.HasOne(propSearch, Request.Keyword));
            //}
            //if (!isadmin)
            //{
            //    //query = query.Where(m => m.UserID == userid || m.UserIdBedDeskID == userid);
            //    total = query.Sum(m => m.TotalPrice);
            //    totalPay = query.Where(m => m.UserID == userid && m.Status < 4).Sum(m => m.TotalPrice);
            //}
            //else
            //{
            total = query.Sum(m => m.TotalPrice);
            totalPay = query.Where(m => m.Status < 4).Sum(m => m.TotalPrice);
            //}
            totalDiscount = query.Where(m => m.Status < 4).Sum(m => m.Discount);
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public List<OrderItem> GetListSimpleByRequestAll(HttpRequestBase httpRequest, out decimal? total, out decimal? totalPay, out decimal? totalDiscount)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var now = DateTime.Now;
            var to = httpRequest["toDate"];
            var status = int.Parse(httpRequest["Status"] ?? "0");
            var fromDate = !string.IsNullOrEmpty(from) ? DateTime.Parse(from).TotalSeconds() : new DateTime(now.Year, now.Month, 1).TotalSeconds();
            var toDate = !string.IsNullOrEmpty(to) ? DateTime.Parse(to).TotalSeconds() : now.AddDays(1).TotalSeconds();
            var query = from c in FDIDB.Shop_Orders
                        where (c.IsDelete.HasValue || c.IsDelete == false)
                        && c.StartDate >= fromDate && c.StartDate <= toDate
                        orderby c.ID descending
                        select new OrderItem
                        {
                            ID = c.ID,
                            CustomerName = c.CustomerName,
                            CutomerPhone = c.Mobile,
                            IsActive = c.IsActive,
                            DateCreated = c.DateCreated,
                            StartDate = c.StartDate,
                            EndDate = c.EndDate,
                            BedDeskID = c.BedDeskID,
                            UserIdBedDeskID = c.UserIdBedDeskID,
                            UserName = c.DN_Users.UserName,
                            UserName1 = c.DN_Users2.UserName,
                            CodeUser = c.DN_Users2.CodeCheckIn,
                            TotalMinute = c.TotalMinute,
                            TotalPrice = c.TotalPrice ?? 0,
                            PrizeMoney = c.PrizeMoney ?? 0,
                            Payments = c.Payments ?? 0,
                            Discount = c.Discount,
                            Deposits = c.Deposits ?? 0,
                            IsDelete = c.IsDelete,
                            UserID = c.UserId,
                            Note = c.Note,
                            AgencyId = c.AgencyId,
                            Status = c.Status,
                        };
            var agency = httpRequest["agency_Id"];
            if (!string.IsNullOrEmpty(agency))
            {
                var ida = int.Parse(agency);
                query = query.Where(c => c.AgencyId == ida);
            }
            if (status > 0 && status == (int)FDI.CORE.OrderStatus.Pending)
            {
                const int t1 = (int)FDI.CORE.OrderStatus.Pending;
                const int t2 = (int)FDI.CORE.OrderStatus.Processing;
                query = query.Where(m => m.Status == t1 || m.Status == t2);
            }
            else if (status > 0 && status != (int)FDI.CORE.OrderStatus.Pending)
            {
                query = query.Where(m => m.Status == status);
            }

            if (!string.IsNullOrEmpty(Request.Keyword))
            {
                query = Request.SearchInField.Aggregate(query, (current, propSearch) => current.HasOne(propSearch, Request.Keyword));
            }
            //if (!isadmin)
            //{
            //    //query = query.Where(m => m.UserID == userid || m.UserIdBedDeskID == userid);
            //    total = query.Sum(m => m.TotalPrice);
            //    totalPay = query.Where(m => m.UserID == userid && m.Status < 4).Sum(m => m.TotalPrice);
            //}
            //else
            //{
            total = query.Sum(m => m.TotalPrice);
            totalPay = query.Where(m => m.Status < 4).Sum(m => m.TotalPrice);
            //}
            totalDiscount = query.Where(m => m.Status < 4).Sum(m => m.Discount);
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }


        public List<OrderItem> GetListSimpleByRequestDate(HttpRequestBase httpRequest, int agencyid)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var now = DateTime.Now;
            var fromDate = !string.IsNullOrEmpty(from) ? DateTime.Parse(from).TotalSeconds() : DateTime.Today.TotalSeconds();
            var toDate = now.AddDays(1).TotalSeconds();
            var query = from c in FDIDB.Shop_Orders
                        where c.AgencyId == agencyid && (c.IsDelete.HasValue || c.IsDelete == false)
                        && c.StartDate >= fromDate && c.StartDate <= toDate
                        orderby c.ID descending
                        select new OrderItem
                        {
                            ID = c.ID,
                            CustomerName = c.CustomerName,
                            CutomerPhone = c.Mobile,
                            IsActive = c.IsActive,
                            CutomerAddress = c.Address,
                            DateCreated = c.DateCreated,
                            CustomerID = c.CustomerID,
                            StartDate = c.StartDate,
                            EndDate = c.EndDate,
                            UserName = c.DN_Users.UserName,
                            UserName1 = c.DN_Users2.UserName,
                            IsDelete = c.IsDelete,
                            UserID = c.UserId,
                            Note = c.Note,
                            Status = c.Status,
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        #region Thống kê
        /// <summary>
        /// Tầng, Khu
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="agencyid"></param>
        /// <param name="total"></param>
        /// <param name="totalPay"></param>
        /// <param name="totalDiscount"></param>
        /// <returns></returns>
        public List<DNLevelRoomItem> OrderByLevelRequest(HttpRequestBase httpRequest, int agencyid, out decimal? total, out decimal? totalPay, out decimal? totalDiscount)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var date = DateTime.Today;
            var to = httpRequest["toDate"];
            var fromDate = !string.IsNullOrEmpty(from) ? from.StringToDecimal() : 0;
            var toDate = !string.IsNullOrEmpty(from) ? to.StringToDecimal(1) : DateTime.Now.TotalSeconds();
            var query = from o in FDIDB.DN_Level
                        where o.AgencyId == agencyid && FDIDB.Shop_Orders.Any(c => (c.DN_Bed_Desk1.Any(m => m.DN_Room.LevelID == o.ID) || c.DN_Bed_Desk.DN_Room.LevelID == o.ID) && c.AgencyId == agencyid && c.IsDelete == false && c.StartDate >= fromDate && c.StartDate <= toDate && c.EndDate <= toDate)
                        orderby o.Sort descending
                        select new DNLevelRoomItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Total = FDIDB.Shop_Orders.Where(c => (c.DN_Bed_Desk1.Any(m => m.DN_Room.LevelID == o.ID) || c.DN_Bed_Desk.DN_Room.LevelID == o.ID) && c.AgencyId == agencyid && c.IsDelete == false && c.StartDate >= fromDate && c.StartDate <= toDate && c.EndDate <= toDate).Sum(m => m.TotalPrice),
                            TotalPay = FDIDB.Shop_Orders.Where(c => (c.DN_Bed_Desk1.Any(m => m.DN_Room.LevelID == o.ID) || c.DN_Bed_Desk.DN_Room.LevelID == o.ID) && c.Status < 4 && c.AgencyId == agencyid && c.IsDelete == false && c.StartDate >= fromDate && c.StartDate <= toDate && c.EndDate <= toDate).Sum(m => m.TotalPrice),
                            TotalDisCount = FDIDB.Shop_Orders.Where(c => (c.DN_Bed_Desk1.Any(m => m.DN_Room.LevelID == o.ID) || c.DN_Bed_Desk.DN_Room.LevelID == o.ID) && c.Status < 4 && c.AgencyId == agencyid && c.IsDelete == false && c.StartDate >= fromDate && c.StartDate <= toDate && c.EndDate <= toDate).Sum(m => m.Discount)
                        };

            if (!string.IsNullOrEmpty(Request.Keyword))
            {
                query = Request.SearchInField.Aggregate(query, (current, propSearch) => current.HasOne(propSearch, Request.Keyword));
            }
            total = query.Sum(m => m.Total);
            totalPay = query.Sum(m => m.TotalPay);
            totalDiscount = query.Sum(m => m.TotalDisCount);
            //query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        /// <summary>
        /// Phòng
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="agencyid"></param>
        /// <param name="total"></param>
        /// <param name="totalPay"></param>
        /// <param name="totalDiscount"></param>
        /// <returns></returns>
        public List<DNRoomItem> OrderByRoomRequest(HttpRequestBase httpRequest, int agencyid, out decimal? total, out decimal? totalPay, out decimal? totalDiscount)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var date = DateTime.Today;
            var to = httpRequest["toDate"];
            var fromDate = !string.IsNullOrEmpty(from) ? from.StringToDecimal() : 0;
            var toDate = !string.IsNullOrEmpty(from) ? to.StringToDecimal(1) : DateTime.Now.TotalSeconds();
            var query = from o in FDIDB.DN_Room
                        where o.AgencyID == agencyid && FDIDB.Shop_Orders.Any(c => (c.DN_Bed_Desk1.Any(m => m.RoomId == o.ID) || c.DN_Bed_Desk.RoomId == o.ID) && c.AgencyId == agencyid && c.IsDelete == false && c.StartDate >= fromDate && c.StartDate <= toDate && c.EndDate <= toDate)
                        orderby o.Sort descending
                        select new DNRoomItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            NameLevel = o.DN_Level.Name,
                            Total = FDIDB.Shop_Orders.Where(c => (c.DN_Bed_Desk1.Any(m => m.RoomId == o.ID) || c.DN_Bed_Desk.RoomId == o.ID) && c.AgencyId == agencyid && c.IsDelete == false && c.StartDate >= fromDate && c.StartDate <= toDate && c.EndDate <= toDate).Sum(m => m.TotalPrice),
                            TotalPay = FDIDB.Shop_Orders.Where(c => (c.DN_Bed_Desk1.Any(m => m.RoomId == o.ID) || c.DN_Bed_Desk.RoomId == o.ID) && c.Status < 4 && c.AgencyId == agencyid && c.IsDelete == false && c.StartDate >= fromDate && c.StartDate <= toDate && c.EndDate <= toDate).Sum(m => m.TotalPrice),
                            TotalDisCount = FDIDB.Shop_Orders.Where(c => (c.DN_Bed_Desk1.Any(m => m.RoomId == o.ID) || c.DN_Bed_Desk.RoomId == o.ID) && c.Status < 4 && c.AgencyId == agencyid && c.IsDelete == false && c.StartDate >= fromDate && c.StartDate <= toDate && c.EndDate <= toDate).Sum(m => m.Discount)
                        };

            if (!string.IsNullOrEmpty(Request.Keyword))
            {
                query = Request.SearchInField.Aggregate(query, (current, propSearch) => current.HasOne(propSearch, Request.Keyword));
            }
            total = query.Sum(m => m.Total);
            totalPay = query.Sum(m => m.TotalPay);
            totalDiscount = query.Sum(m => m.TotalDisCount);
            //query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        /// <summary>
        /// Giường or Bàn
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="agencyid"></param>
        /// <param name="total"></param>
        /// <param name="totalPay"></param>
        /// <param name="totalDiscount"></param>
        /// <returns></returns>
        public List<BedDeskItem> OrderByBedDeskRequest(HttpRequestBase httpRequest, int agencyid, out decimal? total, out decimal? totalPay, out decimal? totalDiscount)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var date = DateTime.Today;
            var to = httpRequest["toDate"];
            var fromDate = !string.IsNullOrEmpty(from) ? from.StringToDecimal() : 0;
            var toDate = !string.IsNullOrEmpty(from) ? to.StringToDecimal(1) : DateTime.Now.TotalSeconds();
            var query = from o in FDIDB.DN_Bed_Desk
                        where o.AgencyId == agencyid && FDIDB.Shop_Orders.Any(c => (c.DN_Bed_Desk1.Any(m => m.ID == o.ID) || c.DN_Bed_Desk.ID == o.ID) && c.AgencyId == agencyid && c.IsDelete == false && c.StartDate >= fromDate && c.StartDate <= toDate && c.EndDate <= toDate)
                        orderby o.Sort descending
                        select new BedDeskItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            RoomName = o.DN_Room.Name,
                            LevelName = o.DN_Room.DN_Level.Name,
                            Total = FDIDB.Shop_Orders.Where(c => (c.DN_Bed_Desk1.Any(m => m.ID == o.ID) || c.DN_Bed_Desk.ID == o.ID) && c.AgencyId == agencyid && c.IsDelete == false && c.StartDate >= fromDate && c.StartDate <= toDate && c.EndDate <= toDate).Sum(m => m.TotalPrice),
                            TotalPay = FDIDB.Shop_Orders.Where(c => (c.DN_Bed_Desk1.Any(m => m.ID == o.ID) || c.DN_Bed_Desk.ID == o.ID) && c.Status < 4 && c.AgencyId == agencyid && c.IsDelete == false && c.StartDate >= fromDate && c.StartDate <= toDate && c.EndDate <= toDate).Sum(m => m.TotalPrice),
                            TotalDisCount = FDIDB.Shop_Orders.Where(c => (c.DN_Bed_Desk1.Any(m => m.ID == o.ID) || c.DN_Bed_Desk.ID == o.ID) && c.Status < 4 && c.AgencyId == agencyid && c.IsDelete == false && c.StartDate >= fromDate && c.StartDate <= toDate && c.EndDate <= toDate).Sum(m => m.Discount)
                        };

            if (!string.IsNullOrEmpty(Request.Keyword))
            {
                query = Request.SearchInField.Aggregate(query, (current, propSearch) => current.HasOne(propSearch, Request.Keyword));
            }
            total = query.Sum(m => m.Total);
            totalPay = query.Sum(m => m.TotalPay);
            totalDiscount = query.Sum(m => m.TotalDisCount);
            //query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        /// <summary>
        /// Thu ngân
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="agencyid"></param>
        /// <param name="total"></param>
        /// <param name="totalPay"></param>
        /// <param name="totalDiscount"></param>
        /// <returns></returns>
        public List<DNUserItem> OrderByUserRequest(HttpRequestBase httpRequest, int agencyid, out decimal? total, out decimal? totalPay, out decimal? totalDiscount)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var date = DateTime.Today;
            var to = httpRequest["toDate"];
            var fromDate = !string.IsNullOrEmpty(from) ? from.StringToDecimal() : 0;
            var toDate = !string.IsNullOrEmpty(from) ? to.StringToDecimal(1) : DateTime.Now.TotalSeconds();
            var query = from o in FDIDB.DN_Users
                        where o.AgencyID == agencyid && o.Shop_Orders.Any(c => c.AgencyId == agencyid && c.IsDelete == false && c.StartDate >= fromDate && c.StartDate <= toDate && c.EndDate <= toDate)
                        orderby o.UserName descending
                        select new DNUserItem
                        {
                            UserName = o.UserName,
                            LoweredUserName = o.LoweredUserName,
                            Rolename = o.DN_UsersInRoles.Where(v => v.IsDelete == false).Select(m => m.DN_Roles.RoleName).FirstOrDefault(),
                            Total = o.Shop_Orders.Where(c => c.AgencyId == agencyid && c.IsDelete == false && c.StartDate >= fromDate && c.StartDate <= toDate && c.EndDate <= toDate).Sum(m => m.TotalPrice),
                            TotalPay = o.Shop_Orders.Where(c => c.Status < 4 && c.AgencyId == agencyid && c.IsDelete == false && c.StartDate >= fromDate && c.StartDate <= toDate && c.EndDate <= toDate).Sum(m => m.TotalPrice),
                            TotalDisCount = o.Shop_Orders.Where(c => c.Status < 4 && c.AgencyId == agencyid && c.IsDelete == false && c.StartDate >= fromDate && c.StartDate <= toDate && c.EndDate <= toDate).Sum(m => m.Discount)
                        };

            if (!string.IsNullOrEmpty(Request.Keyword))
            {
                query = Request.SearchInField.Aggregate(query, (current, propSearch) => current.HasOne(propSearch, Request.Keyword));
            }
            total = query.Sum(m => m.Total);
            totalPay = query.Sum(m => m.TotalPay);
            totalDiscount = query.Sum(m => m.TotalDisCount);
            //query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<DNUserItem> OrderByUserPVRequest(HttpRequestBase httpRequest, int agencyid, out decimal? total, out decimal? totalPay, out decimal? totalDiscount)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var date = DateTime.Today;
            var to = httpRequest["toDate"];
            var fromDate = !string.IsNullOrEmpty(from) ? from.StringToDecimal() : 0;
            var toDate = !string.IsNullOrEmpty(from) ? to.StringToDecimal(1) : DateTime.Now.TotalSeconds();
            var query = from o in FDIDB.DN_Users
                        where o.AgencyID == agencyid && o.Shop_Orders2.Any(c => c.AgencyId == agencyid && c.IsDelete == false && c.StartDate >= fromDate && c.StartDate <= toDate && c.EndDate <= toDate)
                        orderby o.UserName descending
                        select new DNUserItem
                        {
                            UserName = o.UserName,
                            LoweredUserName = o.LoweredUserName,
                            Rolename = o.DN_UsersInRoles.Where(v => v.IsDelete == false).Select(m => m.DN_Roles.RoleName).FirstOrDefault(),
                            Total = o.Shop_Orders2.Where(c => c.AgencyId == agencyid && c.IsDelete == false && c.StartDate >= fromDate && c.StartDate <= toDate && c.EndDate <= toDate).Sum(m => m.TotalPrice),
                            CountOrder = o.Shop_Orders2.Count(c => c.AgencyId == agencyid && c.IsDelete == false && c.StartDate >= fromDate && c.StartDate <= toDate && c.EndDate <= toDate),
                            TotalPay = o.Shop_Orders2.Where(c => c.Status < 4 && c.AgencyId == agencyid && c.IsDelete == false && c.StartDate >= fromDate && c.StartDate <= toDate && c.EndDate <= toDate).Sum(m => m.TotalPrice),
                            TotalDisCount = o.Shop_Orders2.Where(c => c.Status < 4 && c.AgencyId == agencyid && c.IsDelete == false && c.StartDate >= fromDate && c.StartDate <= toDate && c.EndDate <= toDate).Sum(m => m.Discount)
                        };

            if (!string.IsNullOrEmpty(Request.Keyword))
            {
                query = Request.SearchInField.Aggregate(query, (current, propSearch) => current.HasOne(propSearch, Request.Keyword));
            }
            total = query.Sum(m => m.Total);
            totalPay = query.Sum(m => m.TotalPay);
            totalDiscount = query.Sum(m => m.TotalDisCount);
            //query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        /// <summary>
        /// Phục vụ
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="agencyid"></param>
        /// <param name="total"></param>
        /// <param name="totalPay"></param>
        /// <param name="totalDiscount"></param>
        /// <returns></returns>
        public List<DNUserItem> OrderByUser1Request(HttpRequestBase httpRequest, int agencyid, out decimal? total, out decimal? totalPay, out decimal? totalDiscount)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var date = DateTime.Today;
            var to = httpRequest["toDate"];
            var fromDate = !string.IsNullOrEmpty(from) ? from.StringToDecimal() : 0;
            var toDate = !string.IsNullOrEmpty(from) ? to.StringToDecimal(1) : DateTime.Now.TotalSeconds();
            var query = from o in FDIDB.DN_Users
                        where o.AgencyID == agencyid && o.Shop_Orders.Any(c => c.AgencyId == agencyid && c.IsDelete == false && c.StartDate >= fromDate && c.StartDate <= toDate && c.EndDate <= toDate)
                        orderby o.UserName descending
                        select new DNUserItem
                        {
                            UserName = o.UserName,
                            LoweredUserName = o.LoweredUserName,
                            Rolename = o.DN_UsersInRoles.Where(v => v.IsDelete == false).Select(m => m.DN_Roles.RoleName).FirstOrDefault(),
                            Total = o.Shop_Orders1.Where(c => c.AgencyId == agencyid && c.IsDelete == false && c.StartDate >= fromDate && c.StartDate <= toDate && c.EndDate <= toDate).Sum(m => m.TotalPrice),
                            TotalPay = o.Shop_Orders1.Where(c => c.Status < 4 && c.AgencyId == agencyid && c.IsDelete == false && c.StartDate >= fromDate && c.StartDate <= toDate && c.EndDate <= toDate).Sum(m => m.TotalPrice),
                            TotalDisCount = o.Shop_Orders1.Where(c => c.Status < 4 && c.AgencyId == agencyid && c.IsDelete == false && c.StartDate >= fromDate && c.StartDate <= toDate && c.EndDate <= toDate).Sum(m => m.Discount)
                        };

            if (!string.IsNullOrEmpty(Request.Keyword))
            {
                query = Request.SearchInField.Aggregate(query, (current, propSearch) => current.HasOne(propSearch, Request.Keyword));
            }
            total = query.Sum(m => m.Total);
            totalPay = query.Sum(m => m.TotalPay);
            totalDiscount = query.Sum(m => m.TotalDisCount);
            //query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        #endregion
        #region Thống kê Doanh nghiệp
        public List<MonthEItem> OrderByEnterprisesRequest(int id, int year)
        {
            if (year == 0) year = DateTime.Now.Year;
            var query = FDIDB.GeneralYearByEnterprises(year, id).Select(o => new MonthEItem
            {
                T = o.I,
                Percent = o.Percent,
                Total = o.TotalPriceO,
                TotalPay = o.TotalPriceRV,
                TotalAward = o.TotalPricePV,
                TotalSSC = o.TotalPriceO * o.Percent / 100
            }).ToList();
            return query;
        }
        #endregion
        #region Thống kê đại lý
        public List<MonthItem> OrderByAgencyRequest(int id, int year, out decimal? total, out decimal? totalPay, out decimal? totalDiscount)
        {
            var date = DateTime.Now;
            var month = 1;
            if (year == 0 || year == date.Year)
            {
                year = date.Year;
                month = date.Month;
            }
            var dates = new DateTime(year, 1, 1);
            var datee = new DateTime(year, month, 1).AddMonths(1);
            if (year < date.Year)
            {
                month = 12;
                dates = new DateTime(year, 1, 1);
                datee = new DateTime(year, month, 31);
            }

            var ds = dates.TotalSeconds();
            var de = datee.TotalSeconds();
            var query = from o in FDIDB.Shop_Orders
                        where o.AgencyId == id && o.IsDelete == false && o.StartDate >= ds && o.EndDate <= de
                        orderby o.StartDate descending
                        select new OrderItem
                        {
                            StartDate = o.StartDate,
                            EndDate = o.EndDate,
                            TotalPrice = o.TotalPrice,
                            Status = o.Status,
                            Discount = o.Discount
                        };
            total = query.Sum(m => m.TotalPrice);
            totalPay = query.Where(m => m.Status == 3).Sum(m => m.TotalPrice);
            totalDiscount = query.Sum(m => m.Discount);
            var list = new List<MonthItem>();
            for (int i = 1; i <= month; i++)
            {
                var datem = new DateTime(year, i, 1);
                ds = datem.TotalSeconds();
                de = datem.AddMonths(1).TotalSeconds();
                if (year < date.Year && i == 12)
                {
                    datem = new DateTime(year, i, 31);
                    de = datem.AddDays(1).TotalSeconds();
                }
                var obj = new MonthItem
                {
                    Item = new DateItem { I = i, S = ds, E = de },
                    Total = query.Where(m => m.EndDate > ds && m.EndDate < de).Sum(m => m.TotalPrice),
                    TotalPay = query.Where(m => m.Status == 3 && m.EndDate > ds && m.EndDate < de).Sum(m => m.TotalPrice),
                    TotalDisCount = query.Where(m => m.EndDate > ds && m.EndDate < de).Sum(m => m.Discount),
                    Month = i,
                };
                list.Add(obj);
            }
            return list;
        }
        #endregion
        #region Thống kê đơn hàng theo Khách hàng
        //public List<AgencyItem> OrderByCustomerRequest(HttpRequestBase httpRequest, out decimal? total, out decimal? totalPay, out decimal? totalDiscount)
        //{
        //    Request = new ParramRequest(httpRequest);
        //    var from = httpRequest["fromDate"];
        //    var date = DateTime.Today;
        //    var to = httpRequest["toDate"];
        //    var fromDate = !string.IsNullOrEmpty(from) ? ConvertDate.TotalSeconds(ConvertUtil.ToDate(from)) : ConvertDate.TotalSeconds(date);
        //    var toDate = !string.IsNullOrEmpty(to) ? ConvertDate.TotalSeconds(ConvertUtil.ToDate(to)) : ConvertDate.TotalSeconds(date.AddDays(1));
        //    var query = from o in FDIDB.Customers
        //                orderby o.Name descending
        //                select new AgencyItem
        //                {
        //                    ID = o.ID,
        //                    Name = o.Name,
        //                    Total = FDIDB.Shop_Orders.Where(c => c.AgencyId == o.ID && c.IsDelete == false && c.StartDate >= fromDate && c.StartDate <= toDate && c.EndDate <= toDate).Sum(m => m.TotalPrice),
        //                    TotalPay = FDIDB.Shop_Orders.Where(c => c.Status < 4 && c.AgencyId == o.ID && c.IsDelete == false && c.StartDate >= fromDate && c.StartDate <= toDate && c.EndDate <= toDate).Sum(m => m.TotalPrice),
        //                    TotalDisCount = FDIDB.Shop_Orders.Where(c => c.Status < 4 && c.AgencyId == o.ID && c.IsDelete == false && c.StartDate >= fromDate && c.StartDate <= toDate && c.EndDate <= toDate).Sum(m => m.Discount)
        //                };

        //    if (!string.IsNullOrEmpty(Request.Keyword))
        //    {
        //        query = Request.SearchInField.Aggregate(query, (current, propSearch) => current.HasOne(propSearch, Request.Keyword));
        //    }
        //    total = query.Sum(m => m.Total);
        //    totalPay = query.Sum(m => m.TotalPay);
        //    totalDiscount = query.Sum(m => m.TotalDisCount);
        //    //query = query.SelectByRequest(Request, ref TotalRecord);
        //    return query.ToList();
        //}
        #endregion
        public List<OrderItem> GetListOrderFashion(HttpRequestBase httpRequest, int agencyid)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var to = httpRequest["toDate"];
            var fromDate = !string.IsNullOrEmpty(from) ? from.StringToDecimal() : 0;
            var toDate = !string.IsNullOrEmpty(from) ? to.StringToDecimal(1) : DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.Shop_Orders
                        where c.AgencyId == agencyid && c.IsDelete == false && c.DateCreated >= fromDate && c.DateCreated <= toDate && c.IsActive == false
                        orderby c.ID descending
                        select new OrderItem
                        {
                            ID = c.ID,
                            CustomerName = c.CustomerName,
                            CutomerAddress = c.Address,
                            CutomerPhone = c.Mobile,
                            IsActive = c.IsActive,
                            StartDate = c.StartDate,
                            EndDate = c.EndDate,
                            UserName = c.DN_Users.UserName,
                            TotalPrice = c.TotalPrice ?? 0,
                            Payments = c.Payments ?? 0,
                            Note = c.Note,
                            Status = c.Status
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<OrderDetailItem> GetListOrderFashionDetail(HttpRequestBase httpRequest, int agencyid)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var to = httpRequest["toDate"];
            var fromDate = !string.IsNullOrEmpty(from) ? from.StringToDecimal() : 0;
            var toDate = !string.IsNullOrEmpty(from) ? to.StringToDecimal(1) : DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.Shop_Order_Details
                        where c.Shop_Orders.AgencyId == agencyid && c.Shop_Orders.IsDelete == false
                        && c.Shop_Orders.DateCreated >= fromDate && c.Shop_Orders.DateCreated <= toDate && c.Shop_Orders.IsActive == false
                        orderby c.DateCreated descending
                        select new OrderDetailItem
                        {
                            //GID = c.GID,
                            OrderID = c.OrderID,
                            ProductID = c.ProductID,
                            ProductName = c.Shop_Product.Shop_Product_Detail.Name,
                            Quantity = c.Quantity,
                            StartDate = c.Shop_Orders.StartDate,
                            EndDate = c.Shop_Orders.EndDate,
                            Status = c.Status,
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<OrderItem> GetListByCustomer(HttpRequestBase httpRequest, int id, out decimal? totalPrice, out int count)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var to = httpRequest["toDate"];
            var fromDate = !string.IsNullOrEmpty(from) ? from.StringToDecimal() : 0;
            var toDate = !string.IsNullOrEmpty(from) ? to.StringToDecimal(1) : DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.Shop_Orders
                        where c.IsDelete == false && c.CustomerID == id && c.DateCreated >= fromDate && c.DateCreated <= toDate
                        orderby c.ID descending
                        select new OrderItem
                        {
                            ID = c.ID,
                            CustomerName = c.CustomerName,
                            DateCreated = c.DateCreated,
                            StartDate = c.StartDate,
                            UserName = c.DN_Users.UserName,
                            TotalPrice = c.TotalPrice,
                            IsDelete = c.IsDelete,
                            UserID = c.UserId,
                            Note = c.Note,
                            AgencyId = c.AgencyId,
                            Status = c.Status
                        };
            var agencyId = httpRequest.QueryString["AgencyId"];
            if (!string.IsNullOrEmpty(agencyId))
            {
                var idg = int.Parse(agencyId);
                query = query.Where(c => c.AgencyId == idg);
            }
            query = query.SelectByRequest(Request);
            totalPrice = query.Sum(c => c.TotalPrice);
            query = query.SelectPageByRequest(Request, ref TotalRecord);
            count = TotalRecord;
            return query.ToList();
        }
        public List<OrderItem> GetListSimpleByUser(HttpRequestBase httpRequest, int agencyid, Guid? guid, out decimal? total, out decimal? totalPay, out decimal? totalDiscount)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var to = httpRequest["toDate"];
            var now = DateTime.Now;
            var fromDate = !string.IsNullOrEmpty(from) ? from.StringToDecimal() : 0;
            var toDate = !string.IsNullOrEmpty(from) ? to.StringToDecimal(1) : DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.Shop_Orders
                        where c.UserId == guid && c.IsDelete == false && c.DateCreated >= fromDate && c.DateCreated <= toDate
                        orderby c.ID descending
                        select new OrderItem
                        {
                            ID = c.ID,
                            CustomerName = c.CustomerName,
                            CutomerPhone = c.Mobile,
                            IsActive = c.IsActive,
                            DateCreated = c.DateCreated,
                            StartDate = c.StartDate,
                            EndDate = c.EndDate,
                            BedDeskID = c.BedDeskID,
                            UserIdBedDeskID = c.UserIdBedDeskID,
                            UserName = c.DN_Users.UserName,
                            UserName1 = c.DN_Users2.UserName,
                            TotalMinute = c.TotalMinute,
                            TotalPrice = c.TotalPrice ?? 0,
                            PrizeMoney = c.PrizeMoney ?? 0,
                            Payments = c.Payments ?? 0,
                            Discount = c.Discount,
                            IsDelete = c.IsDelete,
                            UserID = c.UserId,
                            Note = c.Note,
                            Status = c.Status,
                        };
            var status = Convert.ToInt32(httpRequest["Status"]);
            if (status > 0)
            {
                query = status == (int)FDI.CORE.OrderStatus.Complete ? query.Where(m => m.Status < 4) : query.Where(m => m.Status == status);
            }
            query = query.SelectByRequest(Request);
            total = query.Sum(m => m.TotalPrice);
            totalPay = query.Where(m => m.Status < 4).Sum(m => m.TotalPrice);
            totalDiscount = query.Where(m => m.Status < 4).Sum(m => m.Discount);
            query = query.SelectPageByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<OrderItem> GetListSimple(int agencyId)
        {
            var query = from c in FDIDB.Shop_Orders
                        orderby c.ID descending
                        where c.IsDelete == false && c.AgencyId == agencyId
                        select new OrderItem
                        {
                            ID = c.ID,
                            CustomerName = c.CustomerName,
                            DateCreated = c.DateCreated,
                            StartDate = c.StartDate,
                            UserName = c.DN_Users.UserName,
                            TotalPrice = c.TotalPrice,
                            IsDelete = c.IsDelete,
                            UserID = c.UserId,
                            Note = c.Note,
                            Status = c.Status
                        };
            return query.ToList();
        }
        public List<PriceAgencyItem> ListPriceAgencyByAgencyId(int id)
        {
            var query = from c in FDIDB.PriceAgencies
                        where c.AgencyID == id && c.IsShow == true
                        select new PriceAgencyItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            Price = c.Price
                        };
            return query.ToList();
        }
        public List<OrderProcessItem> ListOrderByDateNow()
        {
            var date = DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.Shop_Orders
                        where c.EndDate >= date && c.Status == 3 && (!c.IsDelete.HasValue || !c.IsDelete.Value)
                        select new OrderProcessItem
                        {
                            ID = c.ID,
                            BedDeskID = c.BedDeskID,
                            Minute = c.TotalMinute,
                            UserName = c.DN_Users2.UserName,
                            StartDate = (int)c.StartDate,
                            EndDate = (int)c.EndDate,
                            IsEarly = c.IsEarly.HasValue && c.IsEarly.Value,
                            AgencyId = c.AgencyId,
                            Status = 0
                        };
            return query.ToList();
        }
        public OrderGetItem OrderByBedIdContactId(int bedid, int agencyid)
        {
            var date = DateTime.Now.TotalSeconds();
            var dates = DateTime.Now.AddMinutes(5).TotalSeconds();
            const int before = (int)Utils.Order.Before;
            var query = from c in FDIDB.Shop_ContactOrder
                        where c.StartDate - before < date && c.EndDate > date && c.Status == 1 && (!c.IsDelete.HasValue || c.IsDelete == false) && c.BedDeskID == bedid
                        select new OrderGetItem
                        {
                            ID = c.ID,
                            BedDeskID = c.BedDeskID,
                            Value = c.TotalMinute,
                            ProductID = c.Shop_ContactOrder_Details.Select(m => m.ProductID).FirstOrDefault(),
                            Listproduct = c.Shop_ContactOrder_Details.Select(m => new ContactOderDetailItem
                            {
                                ProductId = m.ProductID ?? 0,
                            }),
                            StartDate = c.StartDate,
                            CustomerName = c.CustomerName,
                            Address = c.Address,
                            Mobile = c.Mobile,
                            Time = c.TotalMinute,
                            Price = c.TotalPrice,
                            IsEarly = c.IsEarly,
                            ListItem = FDIDB.Shop_Product.Where(m => m.IsDelete == false && m.IsShow == true)
                                    .Select(v => new ProductItem
                                    {
                                        ID = v.ID,
                                        Name = v.Shop_Product_Detail.Name,
                                        //Value = v.Value,
                                        PriceNew = (v.Shop_Product_Detail.Price * (decimal)v.Product_Size.Value / 1000) ?? 0,
                                        Quantity = 0
                                    })
                        };
            if (query.Any())
            {
                return query.FirstOrDefault();
            }
            var list = (from c in FDIDB.DN_Packet
                        where c.AgencyID == agencyid && c.DN_Bed_Desk.Any(u => u.ID == bedid)
                        select new OrderGetItem
                        {
                            ID = 0,
                            BedDeskID = bedid,
                            IsEarly = c.IsEarly,
                            Time = 0,
                            StartDate = dates,
                            Price = 0,
                            ListItem = FDIDB.Shop_Product.Where(m => m.IsDelete != true).Select(m => new ProductItem
                            {
                                ID = m.ID,
                                Name = m.Shop_Product_Detail.Name,
                                //Value = m.Value,
                                PriceNew = (m.Shop_Product_Detail.Price * (decimal)m.Product_Size.Value / 1000) ?? 0,
                            })
                        }).FirstOrDefault();
            return list;
        }

        public PacketItem ProductDefaultbyBedid(int bedId, int agencyId, int packetId)
        {

            if (packetId > 0)
            {
                var query = from c in FDIDB.DN_Packet
                            where c.AgencyID == agencyId && c.ID == packetId
                            select new PacketItem
                            {
                                ListProductPacketItems = c.DN_Product_Packet.Where(a => a.IsDefault == true).Select(z => new DNProductPacketItem
                                {
                                    ProductId = z.ProductId,
                                    NameProduct = z.Shop_Product.Shop_Product_Detail.Name,
                                    Price = z.Shop_Product.Shop_Product_Detail.Price * (decimal)z.Shop_Product.Product_Size.Value / 1000,
                                    Time = 0,
                                }),
                                IsEarly = c.IsEarly ?? false,
                                TimeEarly = c.TimeEarly ?? 0,
                                TimeWait = c.TimeWait ?? 0
                            };
                return query.FirstOrDefault();
            }
            else
            {
                var query = from c in FDIDB.DN_Packet
                            where c.AgencyID == agencyId && c.IsDefault == true
                            select new PacketItem
                            {
                                ListProductPacketItems = c.DN_Product_Packet.Where(a => a.IsDefault == true).Select(z => new DNProductPacketItem
                                {
                                    ProductId = z.ProductId,
                                    NameProduct = z.Shop_Product.Shop_Product_Detail.Name,
                                    Price = z.Shop_Product.Shop_Product_Detail.Price * (decimal)z.Shop_Product.Product_Size.Value / 1000,
                                    Time = 0,
                                }),
                                IsEarly = c.IsEarly ?? false,
                                TimeEarly = c.TimeEarly ?? 0,
                                TimeWait = c.TimeWait ?? 0
                            };
                return query.FirstOrDefault();
            }


        }
        public OrderGetItem OrderByBedIdContactIdSpa(int bedid, int agencyid)
        {
            var date = DateTime.Now.TotalSeconds();
            var dates = DateTime.Now.AddMinutes(5).TotalSeconds();
            const int before = (int)Utils.Order.Before;
            var query = from c in FDIDB.Shop_ContactOrder
                        where c.StartDate - before < date && c.EndDate > date && c.Status == 1 && (!c.IsDelete.HasValue || c.IsDelete == false) && c.BedDeskID == bedid
                        select new OrderGetItem
                        {
                            ID = c.ID,
                            BedDeskID = c.BedDeskID,
                            Value = c.TotalMinute,
                            ProductID = c.Shop_ContactOrder_Details.Select(m => m.ProductID).FirstOrDefault(),
                            Listproduct = c.Shop_ContactOrder_Details.Select(m => new ContactOderDetailItem
                            {
                                ProductId = m.ProductID ?? 0,
                            }),
                            StartDate = c.StartDate,
                            CustomerName = c.CustomerName,
                            Address = c.Address,
                            Mobile = c.Mobile,
                            Time = c.TotalMinute,
                            Price = c.TotalPrice,
                            IsEarly = c.IsEarly,
                            ListItem = FDIDB.Shop_Product.Where(m => m.IsDelete == false && m.IsShow == true)
                                    .Select(v => new ProductItem
                                    {
                                        ID = v.ID,
                                        Name = v.Shop_Product_Detail.Name,
                                        Value = 0,
                                        PriceNew = (v.Shop_Product_Detail.Price * (decimal)v.Product_Size.Value / 1000) ?? 0,
                                    })
                        };
            if (query.Any())
            {
                return query.FirstOrDefault();
            }
            var list = (from c in FDIDB.DN_Packet
                        where c.AgencyID == agencyid && c.DN_Bed_Desk.Any(u => u.ID == bedid)
                        select new OrderGetItem
                        {
                            ID = 0,
                            BedDeskID = bedid,
                            IsEarly = c.IsEarly,
                            Time = 0,
                            StartDate = dates,
                            Price = 0,
                            ListItem = FDIDB.Shop_Product.Where(m => m.IsDelete != true).Select(m => new ProductItem
                            {
                                ID = m.ID,
                                Name = m.Shop_Product_Detail.Name,
                                Value = 0,
                                PriceNew = (m.Shop_Product_Detail.Price * (decimal)m.Product_Size.Value / 1000) ?? 0,
                            })
                        }).FirstOrDefault();
            return list;
        }
        public OrderGetItem OrderByBedId(int bedid, int agencyid)
        {
            var date = DateTime.Now.TotalSeconds();
            const int before = (int)Utils.Order.Before;
            var query = from c in FDIDB.Shop_ContactOrder
                        where c.StartDate - before < date && c.EndDate > date && c.Status == 1 && (!c.IsDelete.HasValue || c.IsDelete == false) && c.BedDeskID == bedid
                        select new OrderGetItem
                        {
                            ID = c.ID,
                            BedDeskID = bedid,
                            Status = 1
                        };

            if (!query.Any())
            {
                query = from c in FDIDB.Shop_Orders
                        where c.StartDate - before < date && c.EndDate > date && c.Status == 1 && (!c.IsDelete.HasValue || c.IsDelete == false) && c.BedDeskID == bedid
                        select new OrderGetItem
                        {
                            ID = c.ID,
                            BedDeskID = bedid,
                            Status = 2
                        };
            }
            if (query.Any())
            {
                return query.FirstOrDefault();
            }
            return new OrderGetItem
            {
                BedDeskID = bedid,
                Status = 0
            };
        }
        public BonusTypeItem GetBonusTypeItem()
        {
            var query = from c in FDIDB.BonusTypes
                        orderby c.ID descending
                        select new BonusTypeItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            RootID = c.RootID,
                            Description = c.Description,
                            PercentParent = c.PercentParent ?? 0,
                            PercentRoot = c.PercentRoot ?? 0,
                            Percent = c.Percent ?? 0
                        };
            return query.FirstOrDefault();
        }
        public OrderItem GetOrderItem(int id)
        {
            var query = from c in FDIDB.Shop_Orders
                        where c.ID == id
                        select new OrderItem
                        {
                            ID = c.ID,
                            DateCreated = c.DateCreated,
                            BedDeskName = c.DN_Bed_Desk.Name,
                            CustomerName = c.CustomerName,
                            CutomerPhone = c.Mobile,
                            CutomerAddress = c.Address,
                            SaleCode = c.SaleCode,
                            SalePercent = c.SalePercent,
                            //CutomerPrice = c.Customer.Customer_Reward.Where(u => u.AgencyID == c.AgencyId).Select(v => v.PriceReward - v.PriceReceive).FirstOrDefault(),
                            PrizeMoney = c.PrizeMoney,
                            SalePrice = c.SalePrice,
                            StartDate = c.StartDate,
                            UserName = c.DN_Users.UserName,
                            UserName1 = c.DN_Users1.UserName,
                            UserName2 = c.DN_Users2.UserName,
                            Discount = c.Discount,
                            Deposits = c.Deposits,
                            Payments = c.Payments,
                            TotalPrice = c.TotalPrice,
                            LstOrderDetailItems = from v in c.Shop_Order_Details
                                                  where c.ID == id
                                                  select new OrderDetailItem
                                                  {
                                                      //GID = v.GID,
                                                      ID = v.ID,
                                                      Price = v.Price,
                                                      Quantity = v.Quantity,
                                                      ProductName = v.Shop_Product.Shop_Product_Detail.Name,
                                                      ComboName = v.DN_Combo.Name,
                                                      UnitName = v.Shop_Product.Shop_Product_Detail.DN_Unit.Name,
                                                      Status = v.Status,
                                                      Discount = v.Discount,
                                                      ContentPromotion = v.ContentPromotion,
                                                      TotalPrice = v.TotalPrice,
                                                  },
                            DN_Bed_Desk1 = from v in c.DN_Bed_Desk1
                                           where c.ID == id
                                           select new BedDeskItem
                                           {
                                               ID = v.ID,
                                               Name = v.Name
                                           }
                        };
            return query.FirstOrDefault();
        }
        public OrderDetailItem GetOrderDetailItem(long id)
        {
            var query = from c in FDIDB.Shop_Order_Details
                        where c.ID == id
                        select new OrderDetailItem
                        {
                            //GID = c.GID,
                            ID = c.ID,
                            OrderID = c.OrderID,
                            ProductID = c.ProductID,
                            ProductName = c.Shop_Product.Shop_Product_Detail.Name,
                            Quantity = c.Quantity,
                            StartDate = c.Shop_Orders.StartDate,
                            EndDate = c.Shop_Orders.EndDate,
                            Status = c.Status,
                            //LstRecipeItems = c.Shop_Product.Product_Recipe.Where(v => v.IsDelete == false).Select(v => new RecipeItem
                            //{
                            //    ValueName = v.Shop_Product_Value.Name,
                            //    Quantity = v.Quantity,
                            //    ValueQuantity = v.Shop_Product_Value.Quantity
                            //})
                        };
            return query.FirstOrDefault();
        }
        public OrderItem GetItemById(int id)
        {
            var query = from c in FDIDB.Shop_Orders
                        where c.ID == id
                        select new OrderItem
                        {
                            ID = c.ID,
                            DateCreated = c.DateCreated,
                            CustomerName = c.CustomerName,
                            CutomerPhone = c.Mobile,
                            CutomerAddress = c.Address,
                            CustomerID = c.CustomerID,
                            StartDate = c.StartDate,
                            EndDate = c.EndDate,
                            UserName = c.DN_Users.UserName,
                            UserName1 = c.DN_Users1.UserName,
                            UserName2 = c.DN_Users2.UserName,
                            Note = c.Note,
                            Deposits = c.Deposits.HasValue ? c.Deposits : 0,
                            Discount = c.Discount,
                            Payments = c.Payments.HasValue ? c.Payments : 0,
                            TotalPrice = c.TotalPrice,
                            AgencyId =  c.AgencyId,
                            Status = c.Status,
                            ReceiveDate = c.ReceiveDate,
                            
                            LstOrderDetailItems = c.Shop_Order_Details.Where(m => m.Quantity > 0).Select(v => new OrderDetailItem
                            {
                                ID = v.ProductID ?? 0,
                                Price = v.Price,
                                Quantity = v.Quantity,
                                UrlImg = v.Shop_Product.Shop_Product_Detail.Gallery_Picture.Folder + v.Shop_Product.Shop_Product_Detail.Gallery_Picture.Url,
                                QuantityOld = v.QuantityOld,
                                ProductName = v.Shop_Product.Shop_Product_Detail.Name,
                                CateName = v.Shop_Product.Shop_Product_Detail.Category.Name,
                                ProductCode = v.Shop_Product.CodeSku,
                                Status = v.Status
                            }),
                            DN_Bed_Desk1 = c.DN_Bed_Desk1.Select(m => new BedDeskItem
                            {
                                ID = m.ID,
                                Name = m.Name
                            })
                        };
            return query.FirstOrDefault();
        }
        public OrderItem GetMassageItemById(int id)
        {
            var query = from c in FDIDB.Shop_Orders
                        where c.ID == id
                        select new OrderItem
                        {
                            ID = c.ID,
                            DateCreated = c.DateCreated,
                            CustomerName = c.CustomerName,
                            CutomerPhone = c.Mobile,
                            CutomerAddress = c.Address,
                            StartDate = c.StartDate,
                            UserName2 = c.DN_Users2.UserName,
                            EndDate = c.EndDate,
                            Note = c.Note,
                            TotalPrice = c.TotalPrice,
                            Discount = c.Discount,
                            Payments = c.Payments.HasValue ? c.Payments : 0,
                            PrizeMoney = c.PrizeMoney.HasValue ? c.PrizeMoney : 0,
                            UserIdBedDeskID = c.UserIdBedDeskID,
                            UserName = c.DN_Users2.UserName,
                            UserName1 = c.DN_Users.UserName,
                            BedDeskID = c.BedDeskID,
                            BedDeskName = c.DN_Bed_Desk.DN_Room.Name + "/ G" + c.DN_Bed_Desk.Name,
                            AddMinuteID = c.AddMinuteID
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
                            Address = c.Address,
                            Phone = c.Phone,
                            Email = c.Email
                        };

            return query.FirstOrDefault();
        }
        public ProductItem GetProductItem(int id)
        {
            var query = from c in FDIDB.Shop_Product
                        where c.ID == id
                        select new ProductItem
                        {
                            ID = c.ID,
                            PriceNew = (c.Shop_Product_Detail.Price * (decimal)c.Product_Size.Value / 1000) ?? 0,
                            Value = 0,
                        };
            return query.FirstOrDefault();
        }
        public bool CheckOrder(string lstId, decimal sdate, decimal eDate, int timedo)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            timedo = timedo * 60;
            var query = (from c in FDIDB.Shop_Orders
                         where c.BedDeskID.HasValue && ltsArrId.Contains(c.BedDeskID.Value) && c.Status == 3 && ((c.StartDate >= sdate && c.StartDate < eDate) || ((c.IsEarly == true ? c.EndDate - timedo : c.EndDate) > sdate && (c.IsEarly == true ? c.EndDate - timedo : c.EndDate) <= eDate) || (sdate >= c.StartDate && sdate < (c.IsEarly == true ? c.EndDate - timedo : c.EndDate)) || (eDate > c.StartDate && eDate <= (c.IsEarly == true ? c.EndDate - timedo : c.EndDate)))
                         select new OrderItem
                         {
                             ID = c.ID
                         }).Any();
            return query;
        }
        public List<OrderItem> GetOrder()
        {
            var date = DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.Shop_Orders
                        where c.StartDate < date && c.EndDate > date && c.Status == 1
                        orderby c.ID descending
                        select new OrderItem
                        {
                            ID = c.ID,
                            CustomerID = c.CustomerID,
                            StartDate = c.StartDate,
                            EndDate = c.EndDate,
                            TotalMinute = c.TotalMinute,
                            BedDeskID = c.BedDeskID
                        };
            return query.ToList();
        }
        public List<DNUserItem> GetListByAgencyId(int agencyId, int month)
        {
            var monthstart = ConvertDate.TotalSecondsMonth(month);
            var monthend = ConvertDate.TotalSecondsMonth(month + 1);
            var query = from c in FDIDB.DN_Users
                        where c.AgencyID == agencyId && (c.Shop_Orders.Any(m => m.AgencyId == agencyId && m.DateCreated >= monthstart && m.DateCreated < monthend && m.Status == 1) || c.Shop_Orders2.Any(m => m.AgencyId == agencyId && m.DateCreated >= monthstart && m.DateCreated < monthend && m.Status == 1))
                        select new DNUserItem
                        {
                            UserId = c.UserId,
                            lstLevelRoom = c.DN_UsersInRoles.Where(v => v.IsDelete == false).Select(m => new DNLevelRoomItem
                            {
                                ID = m.DN_Roles.LevelId.HasValue ? m.DN_Roles.LevelId.Value : 0
                            }),
                        };
            return query.ToList();
        }
        public List<OrderProcessItem> ListRestaurantByDateNow()
        {
            var date = DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.DN_Bed_Desk
                        from m in c.Shop_Orders1
                        where m.EndDate >= date && m.Status < 3 && (!m.IsDelete.HasValue || m.IsDelete == false)
                        select new OrderProcessItem
                        {
                            ID = m.ID,
                            BedDeskID = c.ID,
                            Minute = m.TotalMinute,
                            StartDate = (int)m.StartDate,
                            EndDate = (int)m.EndDate,
                            AgencyId = m.AgencyId,
                            Status = m.Status
                        };
            return query.ToList();
        }
        public Shop_Orders GetById(int id)
        {
            var query = from c in FDIDB.Shop_Orders where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public List<Shop_Orders> GetListByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.Shop_Orders where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }
        public List<DN_Bed_Desk> GetListArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.DN_Bed_Desk where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }
        public List<OrderItem> GetListByCustomer(int pageIndex, int pageSize, int id, out int count)
        {
            var query = from c in FDIDB.Shop_Orders
                        where c.IsDelete == false && c.CustomerID == id
                        orderby c.ID descending
                        select new OrderItem
                        {
                            ID = c.ID,
                            CustomerName = c.CustomerName,
                            DateCreated = c.DateCreated,
                            StartDate = c.StartDate,
                            UserName = c.DN_Users.UserName,
                            TotalPrice = c.TotalPrice,
                            IsDelete = c.IsDelete,
                            UserID = c.UserId,
                            Note = c.Note,
                            AgencyId = c.AgencyId,
                            Status = c.Status,
                            ReceiveDate = c.ReceiveDate,
                            LstOrderDetailItems = c.Shop_Order_Details.Where(m => m.Quantity > 0).Select(v => new OrderDetailItem
                            {
                                ID = v.ProductID ?? 0,
                                Price = v.Price,
                                Quantity = v.Quantity,
                                UrlImg = v.Shop_Product.Shop_Product_Detail.Gallery_Picture.Folder + v.Shop_Product.Shop_Product_Detail.Gallery_Picture.Url,
                                QuantityOld = v.QuantityOld,
                                ProductName = v.Shop_Product.Shop_Product_Detail.Name,
                                CateName = v.Shop_Product.Shop_Product_Detail.Category.Name,
                                ProductCode = v.Shop_Product.CodeSku,
                                Status = v.Status
                            }),
                        };
            count = query.Count();
            query = query.Skip(pageSize * (pageIndex - 1)).Take(pageSize);

            return query.ToList();
        }

        public OrderItem GetItemById(int id, int customerId)
        {
            var query = from c in FDIDB.Shop_Orders
                        where c.ID == id && customerId == c.CustomerID
                        select new OrderItem
                        {
                            ID = c.ID,
                            DateCreated = c.DateCreated,
                            CustomerName = c.CustomerName,
                            CutomerPhone = c.Mobile,
                            CutomerAddress = c.Address,
                            CustomerID = c.CustomerID,
                            StartDate = c.StartDate,
                            EndDate = c.EndDate,
                            UserName = c.DN_Users.UserName,
                            UserName1 = c.DN_Users1.UserName,
                            UserName2 = c.DN_Users2.UserName,
                            Note = c.Note,
                            Deposits = c.Deposits.HasValue ? c.Deposits : 0,
                            Discount = c.Discount,
                            Payments = c.Payments.HasValue ? c.Payments : 0,
                            TotalPrice = c.TotalPrice,
                            LstOrderDetailItems = c.Shop_Order_Details.Where(m => m.Quantity > 0).Select(v => new OrderDetailItem
                            {
                                ID = v.ProductID ?? 0,
                                Price = v.Price * v.Value,
                                Quantity = v.Quantity,
                                UrlImg = v.Shop_Product.Shop_Product_Detail.Gallery_Picture.Folder + v.Shop_Product.Shop_Product_Detail.Gallery_Picture.Url,
                                QuantityOld = v.QuantityOld,
                                ProductName = v.Shop_Product.Shop_Product_Detail.Name,
                                CateName = v.Shop_Product.Shop_Product_Detail.Category.Name,
                                ProductCode = v.Shop_Product.CodeSku,
                                Status = v.Status,

                            }),
                            DN_Bed_Desk1 = c.DN_Bed_Desk1.Select(m => new BedDeskItem
                            {
                                ID = m.ID,
                                Name = m.Name
                            })
                        };
            return query.FirstOrDefault();
        }

        public void Add(Shop_Orders item)
        {
            FDIDB.Shop_Orders.Add(item);
        }
        public void Delete(Shop_Orders item)
        {
            FDIDB.Shop_Orders.Remove(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}