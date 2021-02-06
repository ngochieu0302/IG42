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
    public class ContactOrderDA : BaseDA
    {
        public ContactOrderDA()
        {
        }
        public ContactOrderDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public ContactOrderDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }

        public List<ContactOrderItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyid)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var to = httpRequest["toDate"];
            var fromDate = !string.IsNullOrEmpty(from) ? from.StringToDecimal() : 0;
            var toDate = !string.IsNullOrEmpty(from) ? to.StringToDecimal(1) : DateTime.Now.TotalSeconds();
            var query = from o in FDIDB.Shop_ContactOrder
                        where o.AgencyId == agencyid && o.IsDelete == false && o.DateCreated >= fromDate && o.DateCreated <= toDate
                        orderby o.ID descending
                        select new ContactOrderItem
                        {
                            ID = o.ID,
                            DateCreated = o.DateCreated,
                            UserID = o.UserID,
                            UserName = o.DN_Users.UserName,
                            BedDeskID = o.BedDeskID,
                            StartDate = o.StartDate,
                            CustomerName = o.Customer.FullName,
                            Status = o.Status,
                            TotalPrice = o.TotalPrice,
                            Discount = o.Discount,
                            BedDeskName = o.DN_Bed_Desk.Name,
                           AgencyName = o.DN_Agency.Name
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<ContactOrderItem> GetListSimple(int agencyId)
        {
            var query = from o in FDIDB.Shop_ContactOrder
                        where o.AgencyId == agencyId
                        orderby o.ID descending
                        select new ContactOrderItem
                        {
                            ID = o.ID,
                            DateCreated = o.DateCreated,
                            UserID = o.UserID,
                            BedDeskID = o.BedDeskID,
                            StartDate = o.StartDate,
                            CustomerName = o.Customer.FullName,
                            Status = o.Status,
                            BedDeskName = o.DN_Bed_Desk.Name
                        };
            return query.ToList();
        }

        public List<ContactOrderItem> ListItemByDay(int agencyId)
        {
            var day = DateTime.Now;
            var date = day.TotalSeconds();
            var query = from o in FDIDB.Shop_ContactOrder
                        where o.AgencyId == agencyId && o.StartDate >= date && o.Status == 1 && (o.IsDelete == false || !o.IsDelete.HasValue)
                        orderby o.ID descending
                        select new ContactOrderItem
                        {
                            ID = o.ID,
                            DateCreated = o.DateCreated,
                            UserID = o.UserID,
                            BedDeskID = o.BedDeskID,
                            StartDate = o.StartDate,
                            CutomerCode = o.Customer.DN_Card.Serial,
                            CustomerName = o.CustomerName,

                            CutomerPhone = o.Mobile,
                            Quantity = o.Quantity,
                            TotalMinute = o.TotalMinute,
                            TotalPrice = o.TotalPrice,
                            CutomerAddress = o.Address,
                            Status = o.Status,
                            BedDeskName = o.DN_Bed_Desk.Name
                        };
            return query.ToList();
        }

        public List<OrderProcessItem> ListContactOrderByDateNow()
        {
            var date = DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.Shop_ContactOrder
                        where c.EndDate >= date && c.Status == 1 && (!c.IsDelete.HasValue || c.IsDelete == false)
                        select new OrderProcessItem
                        {
                            ID = c.ID,
                            BedDeskID = c.BedDeskID,
                            Minute = c.TotalMinute,
                            StartDate = (int)c.StartDate,
                            EndDate = (int)c.EndDate,
                            IsEarly = c.IsEarly.HasValue && c.IsEarly.Value,
                            AgencyId = c.AgencyId,
                            Status = c.Status
                        };
            return query.ToList();
        }

        public List<OrderProcessItem> ListRestaurantByDateNow()
        {
            var date = DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.DN_Bed_Desk
                        from m in c.Shop_ContactOrder1
                        where m.EndDate >= date && m.Status == 1 && (!m.IsDelete.HasValue || m.IsDelete == false)
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
                            //PriceNew = c.PriceNew,
                            //Value = c.Value,
                        };
            return query.FirstOrDefault();
        }
        public ContactOrderItem GetContactOrderItem(int id)
        {
            var query = from c in FDIDB.Shop_ContactOrder
                        where c.ID == id
                        select new ContactOrderItem
                        {
                            DateCreated = c.DateCreated,
                            StartDate = c.StartDate,
                            //CutomerID = c.CustomerID,
                            EndDate = c.EndDate,
                            BedDeskName = c.DN_Bed_Desk.Name,
                            CustomerName = c.CustomerName,
                            CutomerPhone = c.Mobile,
                            CutomerAddress = c.Address,
                            Discount = c.Discount,
                            DN_Bed_Desk1 = c.DN_Bed_Desk1.Select(m => new BedDeskItem
                            {
                                ID = m.ID,
                                Name = m.Name
                            }),
                            Note = c.Content,
                            LstOrderDetailItems = from v in c.Shop_ContactOrder_Details
                                                  where c.ID == id
                                                  select new OrderDetailItem
                                                  {
                                                      //ID = v.ID,
                                                      Price = v.Price,
                                                      Quantity = v.Quantity,
                                                      ProductName = v.Shop_Product_Detail.Name,
                                                      ComboName = v.DN_Combo.Name
                                                  },
                        };
            return query.FirstOrDefault();
        }
        public Shop_ContactOrder GetById(int id)
        {
            var query = from c in FDIDB.Shop_ContactOrder where c.ID == id select c;
            return query.FirstOrDefault();
        }
        public bool CheckOrder(string lstId, decimal? sdate, decimal? eDate, int timedo)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            timedo = timedo * 60;
            var query = from c in FDIDB.Shop_ContactOrder
                        where c.BedDeskID.HasValue && ltsArrId.Contains(c.BedDeskID.Value) && c.IsDelete == false && c.Status < 3 && ((c.StartDate >= sdate && c.StartDate <= eDate) || ((c.IsEarly == true ? c.EndDate - timedo : c.EndDate) > sdate && (c.IsEarly == true ? c.EndDate - timedo : c.EndDate) <= eDate) || (sdate >= c.StartDate && sdate < (c.IsEarly == true ? c.EndDate - timedo : c.EndDate)) || (eDate > c.StartDate && eDate <= (c.IsEarly == true ? c.EndDate - timedo : c.EndDate)))
                        select new ContactOrderItem
                        {
                            ID = c.ID
                        };
            return query.Any();
        }
        public List<DN_Bed_Desk> GetListArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.DN_Bed_Desk where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }

        public List<ContactOrderItem> GetListByCustomer(int pageIndex, int pageSize, int id, out int count)
        {
            var query = from c in FDIDB.Shop_ContactOrder
                        where c.IsDelete == false && c.CustomerID == id
                        orderby c.ID descending
                        select new ContactOrderItem
                        {
                            ID = c.ID,
                            CustomerName = c.CustomerName,
                            DateCreated = c.DateCreated,
                            StartDate = c.StartDate,
                            UserName = c.DN_Users.UserName,
                            TotalPrice = c.TotalPrice,

                            AgencyId = c.AgencyId,
                            Status = c.Status,
                            LstOrderDetailItems = c.Shop_ContactOrder_Details.Where(m => m.Quantity > 0).Select(v => new OrderDetailItem
                            {
                                ID = v.ProductID ?? 0,
                                Price = v.Price,
                                Quantity = v.Quantity,
                                UrlImg = v.Shop_Product_Detail.Gallery_Picture.Folder + v.Shop_Product_Detail.Gallery_Picture.Url,
                                QuantityOld = v.QuantityOld,
                                ProductName = v.Shop_Product_Detail.Name,
                                CateName = v.Shop_Product_Detail.Category.Name,
                                Status = v.Status
                            }),
                        };
            count = query.Count();
            query = query.Skip(pageSize * (pageIndex - 1)).Take(pageSize);

            return query.ToList();
        }
        public ContactOrderItem GetContactOrderItem(int id, int customerId)
        {
            var query = from c in FDIDB.Shop_ContactOrder
                        where c.ID == id && c.CustomerID == customerId
                        select new ContactOrderItem
                        {
                            ID = c.ID,
                            DateCreated = c.DateCreated,
                            StartDate = c.StartDate,
                            //CutomerID = c.CustomerID,
                            EndDate = c.EndDate,
                            BedDeskName = c.DN_Bed_Desk.Name,
                            CustomerName = c.CustomerName,
                            CutomerPhone = c.Mobile,
                            CutomerAddress = c.Address,
                            Discount = c.Discount,
                            TotalPrice = c.TotalPrice,
                            ReceiveDate = c.ReceiveDate,
                            DN_Bed_Desk1 = c.DN_Bed_Desk1.Select(m => new BedDeskItem
                            {
                                ID = m.ID,
                                Name = m.Name
                            }),
                            Note = c.Content,
                            Status = c.Status,
                            LstOrderDetailItems = from v in c.Shop_ContactOrder_Details
                                                  where c.ID == id
                                                  select new OrderDetailItem
                                                  {
                                                      //ID = v.ID,
                                                      Price = v.Price,
                                                      Quantity = v.Quantity,
                                                      ProductName = v.Shop_Product_Detail.Name,
                                                      ComboName = v.DN_Combo.Name,
                                                      Weight = v.Weight
                                                  },
                        };
            return query.FirstOrDefault();
        }

        public List<OrderDetailItem> GetProductDetail(int agencyId, decimal todayCode)
        {
            var query = from c in FDIDB.Shop_ContactOrder_Details
                        join orderContact in FDIDB.Shop_ContactOrder on c.ContactOrderID equals orderContact.ID
                        where orderContact.AgencyId == agencyId
                              && orderContact.ReceiveDate == todayCode
                        select new OrderDetailItem
                        {
                            ProductID = c.ProductID,
                            Price = c.Price,
                            Quantity = c.Quantity,
                            ProductName = c.Shop_Product_Detail.Name,
                            ComboName = c.DN_Combo.Name,
                            Weight = c.Weight
                        };
            return query.ToList();
        }
        public List<OrderDetailItem> GetProductDetail(int agencyId, decimal todayCode, int categoryId)
        {
            var query = from c in FDIDB.Shop_ContactOrder_Details
                        join orderContact in FDIDB.Shop_ContactOrder on c.ContactOrderID equals orderContact.ID
                        where orderContact.AgencyId == agencyId
                              && orderContact.ReceiveDate == todayCode
                              && c.Shop_Product_Detail.CateID == categoryId
                        select new OrderDetailItem
                        {
                            ProductID = c.ProductID,
                            Price = c.Price,
                            Quantity = c.Quantity,
                            ProductName = c.Shop_Product_Detail.Name,
                            ComboName = c.DN_Combo.Name,
                            Weight = c.Weight
                        };
            return query.ToList();
        }

        public List<OrderDetailItem> GetProductToDay(decimal todayCode)
        {
            var query = from c in FDIDB.Shop_ContactOrder_Details
                        where c.TodayCode == todayCode
                        orderby c.ContactOrderID descending
                        select new OrderDetailItem()
                        {
                            ProductName = c.Shop_Product_Detail.Name,
                            Price = c.Price,
                            Weight = c.Weight,
                            Quantity = c.Quantity,
                            OrderType = c.OrderType,
                            ReceiveDate = c.ReceiveDate,
                            GID = c.GID,
                            ID = c.Shop_Product_Detail.ID
                        };
            return query.ToList();
        }

        public void Add(Shop_ContactOrder item)
        {
            FDIDB.Shop_ContactOrder.Add(item);
        }
        public void Delete(Shop_ContactOrder item)
        {
            FDIDB.Shop_ContactOrder.Remove(item);
        }

        public List<OrderDetailItem> GetFastOrder(HttpRequestBase httpRequest, decimal todayCode)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.Shop_ContactOrder_Details
                        where !c.IsDeleted && c.TodayCode == todayCode
                        && (c.Shop_ContactOrder.IsDelete == null || c.Shop_ContactOrder.IsDelete.Value == false)
                        orderby c.ContactOrderID
                        select new OrderDetailItem
                        {
                            OrderID = c.ContactOrderID,
                            GID = c.GID,
                            Price = c.Price,
                            Quantity = c.Quantity,
                            ReceiveDate = c.ReceiveDate,
                            UrlImg = c.Shop_Product_Detail.Gallery_Picture.Folder + c.Shop_Product_Detail.Gallery_Picture.Url,
                            QuantityOld = c.QuantityOld,
                            ProductName = c.Shop_Product_Detail.Name,
                            CateName = c.Shop_Product_Detail.Category.Name,
                            Status = c.Status,
                            CustomerName = c.Shop_ContactOrder.CustomerName,
                            CustomerPhone =c.Shop_ContactOrder.Mobile,
                   
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public Shop_ContactOrder_Details GetDetailById(Guid id)
        {
            return FDIDB.Shop_ContactOrder_Details.FirstOrDefault(m => m.GID == id);
        }
    }
}
