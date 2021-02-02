using FDI.Base;
using FDI.Simple.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FDI.DA.DA.Supplier
{
    public class SupplierAmountProductDA : BaseDA
    {
        public List<SupplierAmountProductItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.SupplierAmountProducts
                        where o.IsDelete == false
                        orderby o.AmountEstimate descending
                        select new SupplierAmountProductItem
                        {
                            ID = o.ID,
                            CategoryName = o.Category.Name,
                            AmountEstimate = o.AmountEstimate,
                            AmountPayed = o.AmountPayed,
                            SupplierName = o.DN_Supplier.Name,
                            PublicationDate = o.PublicationDate
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public void Add(SupplierAmountProduct item)
        {
            FDIDB.SupplierAmountProducts.Add(item);
        }
        public void AddCommingsoon(List<Shop_Product_Comingsoon> item)
        {
            FDIDB.Shop_Product_Comingsoon.AddRange(item);
        }
        public SupplierAmountProduct GetById(int id)
        {
            return FDIDB.SupplierAmountProducts.FirstOrDefault(m => m.ID == id);
        }

        public SupplierAmountProductItem GetItemById(int id)
        {
            return FDIDB.SupplierAmountProducts.Where(m => m.ID == id).Select(m => new SupplierAmountProductItem()
            {
                ID = m.ID,
                AmountEstimate = m.AmountEstimate,
                AmountPayed = m.AmountPayed,
                CallDate = m.CallDate,
                ExpireDate = m.ExpireDate,
                IsAlwayExist = m.IsAlwayExist,
                Note = m.Note,
                PublicationDate = m.PublicationDate,
                SupplierId = m.SupplierId,
                ProductID = m.ProductID,
                UserActiveId = m.UserActiveId
            }).FirstOrDefault();
        }

        public List<SupplierAmountProductItem> GetSupplierByCategoryId(int id)
        {
            var query = from o in FDIDB.SupplierAmountProducts
                        where o.IsDelete == false && o.ProductID == id
                        orderby o.AmountEstimate descending
                        select new SupplierAmountProductItem
                        {
                            ID = o.ID,
                            CategoryName = o.Category.Name,
                            AmountEstimate = o.AmountEstimate,
                            AmountPayed = o.AmountPayed,
                            SupplierName = o.DN_Supplier.Name,
                            SupplierId = o.SupplierId,
                            ProductID = o.ProductID,
                            PublicationDate = o.PublicationDate,
                            SupplierPhone = o.DN_Supplier.Mobile,
                            SupplierAdress = o.DN_Supplier.Address
                        };
            return query.Distinct().ToList();
        }

        public List<SupplierAmountProductItem> GetAmount(int productId, decimal todayCode)
        {
            var query = from o in FDIDB.SupplierAmountProducts
                        where o.ProductID == productId && todayCode >= o.PublicationDate && todayCode <= o.ExpireDate
                        group o by new { o.ProductID, o.SupplierId, o.DN_Supplier.Name } into g
                        select new SupplierAmountProductItem
                        {
                            ProductID = g.Key.ProductID,
                            AmountEstimate = g.Sum(x => x.AmountEstimate),
                            AmountPayed = g.Sum(x => x.AmountPayed),
                            SupplierName = g.Key.Name,
                            SupplierId = g.Key.SupplierId
                        };
            return query.ToList();
        }
        public SupplierAmountProductItem GetAmount(int productId, decimal todayCode, int supplierId)
        {
            var query = from o in FDIDB.SupplierAmountProducts
                        where o.ProductID == productId && todayCode >= o.PublicationDate && todayCode <= o.ExpireDate && supplierId == o.SupplierId
                        group o by new { o.ProductID, o.SupplierId } into g
                        select new SupplierAmountProductItem
                        {
                            ProductID = g.Key.ProductID,
                            AmountEstimate = g.Sum(x => x.AmountEstimate),
                            AmountPayed = g.Sum(x => x.AmountPayed),
                        };
            return query.FirstOrDefault();
        }

        public List<SupplierAmountProduct> GetItem(decimal todaycode, int supplierId, int productId)
        {
            return FDIDB.SupplierAmountProducts.Where(m =>
                 todaycode >= m.PublicationDate && todaycode <= m.ExpireDate && m.SupplierId == supplierId &&
                 m.ProductID == productId).ToList();
        }

    }
}
