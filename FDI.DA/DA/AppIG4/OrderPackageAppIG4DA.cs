using System;
using System.Collections.Generic;
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
    public class OrderPackageAppIG4DA : BaseDA
    {
        #region Constructer
        public OrderPackageAppIG4DA()
        {
        }

        public OrderPackageAppIG4DA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public OrderPackageAppIG4DA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        public OrderPackageAppIG4Item GetOrderPackage(int customerid)
        {
            var date = DateTime.Now.TotalSeconds();
            var query = from o in FDIDB.Order_Package
                where o.CustomerID == customerid && o.DateStart< date && o.DateEnd> date
                        orderby o.Price descending
                select new OrderPackageAppIG4Item
                {
                    TypeID = o.TypeID,
                    CustomerID = o.CustomerID,
                    DateStart = o.DateStart,
                    DateEnd = o.DateEnd,
                    Name = o.Customer_Type.Name,
                    ImagesUrl = o.Customer_Type.Gallery_Picture.Folder + o.Customer_Type.Gallery_Picture.Url,
                    Color = o.Customer_Type.Color,
                    Price = o.Price
                };
            return query.FirstOrDefault();
        }

        public List<OrderPackageAppIG4Item> GetListOrderPacketStatic(HttpRequestBase httpRequest, ref decimal? total)
        {
            Request = new ParramRequest(httpRequest);
            var datef = httpRequest["_dateStart"];
            var datet = httpRequest["_dateEnd"];
            var df = !string.IsNullOrEmpty(datef) ? datef.StringToDecimal() : 0;
            var dt = !string.IsNullOrEmpty(datet) ? datet.StringToDecimal() : DateTime.Now.AddDays(1).TotalSeconds();
            var query = from c in FDIDB.Order_Package
                where c.Datecreate >= df && c.Datecreate <= dt
                      orderby c.ID descending 
                select new OrderPackageAppIG4Item
                {
                    Datecreate = c.Datecreate,
                    Name = c.Customer_Type.Customer_TypeGroup.Name + " " + c.Customer_Type.Name,
                    Packet = c.Customer_Type.Customer_TypeGroup.ID,
                    DateStart = c.DateStart,
                    DateEnd = c.DateEnd,
                    CustomerPolicy = c.Customer.Customer_Policy.Name,
                    CustomerPolicyID = c.Customer.CustomerPolicyID,
                    Price = c.Price,
                    Address = c.Customer.CustomerAddresses.Where(a=>a.IsDefault == true).Select(a=>a.Address).FirstOrDefault(),
                    Customername = c.Customer.FullName,
                };
            var cus = httpRequest["_customer"];
            if (!string.IsNullOrEmpty(cus))
            {
                query = query.Where(c => c.Customername.ToLower().Contains(cus));
            }

            var hang = httpRequest["_type"];
            if (!string.IsNullOrEmpty(hang) && hang != "null")
            {
                var type = FDIUtils.StringToListInt(hang);
                query = query.Where(c => type.Contains(c.CustomerPolicyID ?? 0));
            }
            var packet = httpRequest["_packet"];
            if (!string.IsNullOrEmpty(packet) && packet != "null")
            {
                var type = FDIUtils.StringToListInt(packet);
                query = query.Where(c => type.Contains(c.Packet ?? 0));
            }
            total = query.Sum(c => c.Price ?? 0);
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public decimal? GetDateStartByCustomerID(int customerid)
        {
            var date = DateTime.Now.TotalSeconds();
            var query = from o in FDIDB.Order_Package
                where o.CustomerID == customerid && o.DateStart < date && o.DateEnd > date
                orderby o.DateEnd descending
                select o.DateEnd;
            return query.FirstOrDefault();
        }
        public void Add(Order_Package orderPackage)
        {
            FDIDB.Order_Package.Add(orderPackage);
        }

        public void Delete(Order_Package orderPackage)
        {
            FDIDB.Order_Package.Remove(orderPackage);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
        #endregion
    }
}
