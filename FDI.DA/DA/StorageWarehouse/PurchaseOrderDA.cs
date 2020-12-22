using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FDI.Base;
using FDI.CORE;
using FDI.Simple.Logistics;
using FDI.Simple.StorageWarehouse;

namespace FDI.DA.DA.StorageWarehouse
{
    public class PurchaseOrderDA : BaseDA
    {
        public List<PurchaseOrderItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.PurchaseOrders
                        where o.IsDelete == false
                        orderby o.ID descending
                        select new PurchaseOrderItem()
                        {
                            ID = o.ID,
                            Quantity = o.Quantity,
                            CreateDate = o.CreateDate,
                            UserCreate = o.DN_Users.UserName
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public PurchaseOrderItem GetById(int id)
        {
            var query = from o in FDIDB.PurchaseOrders
                        where o.IsDelete == false && o.ID == id
                        orderby o.ID descending
                        select new PurchaseOrderItem()
                        {
                            ID = o.ID,
                            Quantity = o.Quantity,
                            CreateDate = o.CreateDate,
                            UserCreate = o.DN_Users.UserName,
                            OrderCarID = o.OrderCarId ?? 0
                        };
            return query.FirstOrDefault();

        }
        public void Add(PurchaseOrder item)
        {
            FDIDB.PurchaseOrders.Add(item);
        }

        public bool CheckExistByOrderCarId(int orderCarId)
        {
            return FDIDB.PurchaseOrders.Any(m => m.IsDelete == false && m.OrderCarId == orderCarId);
        }
        public List<OrderCarProductDetailItem> GetByOrderCarId(int ordercarId)
        {
            var query = from o in FDIDB.Cate_Value
                        where o.IsDelete == false && o.PurchaseOrderId == ordercarId
                        orderby o.ID descending
                        select new OrderCarProductDetailItem()
                        {
                            ID = o.ID,
                            Quantity = o.Weight ?? 0,
                            Code = o.Barcode,
                            ProductName = o.Category.Name,
                            Status = o.Status != null ? (CateValueStatus)o.Status : CateValueStatus.NoneActive
                        };
            return query.ToList();
        }
    }
}
