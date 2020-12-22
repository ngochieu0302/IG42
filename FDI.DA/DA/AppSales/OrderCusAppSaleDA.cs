using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FDI.Base;
using FDI.CORE;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA.DA
{
    public class OrderCusAppSaleDA : BaseDA
    {
        #region Contruction

        public OrderCusAppSaleDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public OrderCusAppSaleDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }

        #endregion
        public List<OrderAppSaleItem> GetListOrderAppSale(int rowPerPage, int page, int aid, string cus, string fromd, string tod, ref int total)
        {
            var datef = !string.IsNullOrEmpty(fromd) ? ConvertDate.TotalSeconds(ConvertUtil.ToDateTime(fromd)) : ConvertDate.TotalSeconds(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
            var datet = !string.IsNullOrEmpty(tod) ? ConvertDate.TotalSeconds(ConvertUtil.ToDateTime(tod).AddDays(1)) : ConvertDate.TotalSeconds(DateTime.Now.AddDays(1));
            var query = from c in FDIDB.Shop_ContactOrder
                        where c.AgencyId == aid && datef <= c.DateCreated && c.DateCreated <= datet
                        && c.IsDelete != true
                        orderby c.ID descending
                        select new OrderAppSaleItem
                        {
                            ID = c.ID,
                            P = c.Mobile,
                            N = c.CustomerName,
                            A = c.Address,
                            CId = c.CustomerID,
                            D = c.DateCreated,
                            S = c.Status,
                            Note = c.Content,
                            Pay = c.Payments ?? 0,
                            Tp = c.TotalPrice ?? 0,
                        };
            if (!string.IsNullOrEmpty(cus))
            {
                query = query.Where(c => c.N.Contains(cus));
            }
            return query.Paging(page, rowPerPage, ref total).ToList();

        }
        public List<ContactOrderAppItem> ListAll(int aid, int status)
        {
            var query1 = from c in FDIDB.Shop_ContactOrder
                         where c.AgencyId == aid && c.Status == status
                         select new ContactOrderAppItem
                         {
                             ID = c.ID,
                             Mobile = c.Mobile,
                             Latitute = c.Latitute,
                             Longitude = c.Longitude,
                             Address = c.Address,
                             CustomerID = c.CustomerID,
                             CustomerItem = new CustomerAppItem
                             {
                                 FullName = c.Customer.FullName,
                                 Phone = c.Customer.Phone,
                                 NameGroup = c.Customer.Customer_Groups.Name,
                             },
                             StartDate = c.StartDate,
                             EndDate = c.EndDate,
                             Status = c.Status,
                             Note = c.Content,
                             Time = c.TotalMinute
                         };
            return query1.ToList();
        }
        public List<OrderDetailItem> GetListDetailsById(int id)
        {
            var query = from c in FDIDB.Shop_ContactOrder_Details
                        where c.ContactOrderID == id
                        select new OrderDetailItem
                        {
                            ProductName = c.Shop_Product_Detail.Name,
                            Quantity = c.Quantity,
                            Price = c.Price,
                            Status = c.Status,
                            UrlImg = c.Shop_Product_Detail.Gallery_Picture.Folder + c.Shop_Product_Detail.Gallery_Picture.Url,
                            Weight = c.Weight
                            
                        };
            return query.ToList();
        }

        public Shop_ContactOrder GetByID(int id)
        {
            var query = from c in FDIDB.Shop_ContactOrder
                        where c.ID == id
                        select c;
            return query.FirstOrDefault();
        }

        public ContactOrderItem getDetailById(int id)
        {
            var queryContact = from c in FDIDB.Shop_ContactOrder
                               join order in FDIDB.Shop_Orders on c.ID equals order.ContactOrderID into lj
                               from x in lj.DefaultIfEmpty()
                               where c.ID == id
                               select new ContactOrderItem()
                               {
                                   ID = c.ID,
                                   Status = c.Status,
                                   ReceiveDate = c.ReceiveDate,
                                   TotalPrice = (x == null ? c.TotalPrice : x.TotalPrice),
                                   CustomerName = c.CustomerName,
                                   AgencyId = c.AgencyId,
                                   CutomerID = c.CustomerID,
                                   CutomerPhone = c.Mobile,
                                   CutomerAddress = c.Address
                               };
            return queryContact.FirstOrDefault();

        }
        public CustomerAppItem GetCusbyQrCode(string qrcode)
        {
            var query = from c in FDIDB.Customers
                        where c.QRCode == qrcode && (!c.IsDelete.HasValue || c.IsDelete == false)
                        select new CustomerAppItem
                        {
                            ID = c.ID,
                            UserName = c.UserName,
                            Gender = c.Gender == true ? 1 : 0,
                            Address = c.Address,
                            Birthday = c.Birthday,
                            FullName = c.FullName,
                            Reward = c.Reward,
                            GroupID = c.GroupID,
                            Phone = c.Phone,
                        };
            return query.FirstOrDefault();
        }

        public List<ContactOrderAppItem> GetAll(int agencyId, int rowPerPage, int page, int status, string txt, decimal startDate, decimal endDate, bool? orderbyPrice, ref int total)
        {
            var query = from c in FDIDB.Shop_Orders
                        where c.ContactOrderID == null && c.AgencyId == agencyId
                        select new ContactOrderAppItem()
                        {
                            ID = c.ID,
                            Status = c.Status,
                            ReceiveDate = c.ReceiveDate,
                            TotalPrice = c.TotalPrice,
                            ReceiveHour = 0,
                            CustomerName = c.CustomerName,
                            IsContactOrder = false
                        };
            var queryContact = from c in FDIDB.Shop_ContactOrder
                               join order in FDIDB.Shop_Orders on c.ID equals order.ContactOrderID into lj
                               from x in lj.DefaultIfEmpty()
                               where c.AgencyId == agencyId
                               select new ContactOrderAppItem()
                               {
                                   ID = c.ID,
                                   Status = c.Status,
                                   ReceiveDate = c.ReceiveDate,
                                   TotalPrice = (x == null ? c.TotalPrice : x.TotalPrice),
                                   ReceiveHour = c.ReceiveHour,
                                   CustomerName = c.CustomerName,
                                   IsContactOrder = true
                               };
            query = query.Union(queryContact);
            query = query.Where(m =>
                    (m.CustomerName.Contains(txt) || string.IsNullOrEmpty(txt)) &&
                    (m.Status == status || status == 0)
                    && m.ReceiveDate >= startDate && m.ReceiveDate <= endDate
                    );
            if (orderbyPrice != null)
            {
                if (orderbyPrice == true)
                {
                    query = query.OrderByDescending(m => m.TotalPrice).ThenByDescending(m => m.ReceiveDate)
                        .ThenByDescending(m => m.ReceiveHour);
                }
                else
                {
                    query = query.OrderBy(m => m.TotalPrice).ThenByDescending(m => m.ReceiveDate)
                        .ThenByDescending(m => m.ReceiveHour);
                }
            }
            else
            {
                query = query.OrderByDescending(m => m.ReceiveDate)
                    .ThenByDescending(m => m.ReceiveHour);
            }
            query = query.Paging(page, rowPerPage, ref total);

            return query.ToList();
        }
    }
}
