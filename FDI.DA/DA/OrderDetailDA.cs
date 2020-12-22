using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class OrderDetailDA : BaseDA
    {
        #region Constructer
        public OrderDetailDA()
        {
        }

        public OrderDetailDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public OrderDetailDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<Shop_Order_Details> GetListByArrId(List<Guid> lstId)
        {
            var query = from o in FDIDB.Shop_Order_Details
                        where lstId.Contains(o.GID)
                select o;
            return query.ToList();
        }
        public List<OrderDetailItem> GetList(int orderId)
        {
            var query = from o in FDIDB.Shop_Order_Details
                where o.OrderID == orderId
                select new OrderDetailItem()
                {
                    ProductName = o.Shop_Product.Shop_Product_Detail.Name,
                    Weight = o.Value,
                    TotalPrice = o.TotalPrice,
                    Quantity = o.Quantity,
                    Price = o.Shop_Product.Shop_Product_Detail.PriceCost,
                    UrlImg = o.Shop_Product.Shop_Product_Detail.Gallery_Picture.Folder + o.Shop_Product.Shop_Product_Detail.Gallery_Picture.Url
                };
            return query.ToList();
        }

        public void Add(Shop_Order_Details obj)
        {
            FDIDB.Shop_Order_Details.Add(obj);
        }
        
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
