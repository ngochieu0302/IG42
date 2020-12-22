using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FDI.Base;
using FDI.CORE;
using FDI.Simple;
using FDI.Simple.Order;
using FDI.Simple.StorageWarehouse;
using FDI.Utils;

namespace FDI.DA.DA.StorageWarehouse
{
    public class TotalProductToDayDA : BaseDA
    {
        #region Contruction
        public TotalProductToDayDA()
        {
        }
        public TotalProductToDayDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public TotalProductToDayDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion
        public List<TotalProductToDayItem> GetListSimpleByRequest(HttpRequestBase httpRequest, decimal code, int productId)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.TotalProductToDays
                        where o.ToDayCode == code && o.ProductId == productId
                        orderby o.ID descending
                        select new TotalProductToDayItem()
                        {
                            ID = o.ID,
                            Quantity = o.Quantity,
                            SupplierName = o.DN_Supplier.Name,
                            SupplierId = o.SupplierId,
                            ProductId = o.SupplierId,
                            ProductName = o.Category.Name,
                            Note = o.Note,
                            Status = o.Status
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public TotalStorageWare GetbyId(int id)
        {
            var query = from c in FDIDB.TotalStorageWares
                        where c.ID == id
                        select c;
            return query.FirstOrDefault();
        }
        public TotalProductToDay GetbyId(decimal todayCode, int productId, int supplierId)
        {
            return FDIDB.TotalProductToDays
                .FirstOrDefault(m => m.ToDayCode == todayCode && m.ProductId == productId && m.SupplierId == supplierId);
        }
        public TotalProductToDayItem GetItem(decimal todayCode, int productId, int supplierId)
        {
            var query = from m in FDIDB.TotalProductToDays
                        where m.ToDayCode == todayCode && m.ProductId == productId && m.SupplierId == supplierId
                        select new TotalProductToDayItem()
                        {
                            ID = m.ID,
                            Price = m.Price
                        };
            return query.FirstOrDefault();
        }

        public void AddSupplier(RequestWareSupplierRequest[] request)
        {
            foreach (var item in request)
            {
                FDIDB.TotalProductToDays.Add(new TotalProductToDay()
                {

                    ProductId = item.ProductId,
                    SupplierId = item.SupplierId,
                    IsDelete = false,
                    DateCreated = DateTime.Now.TotalSeconds(),
                    Status = 0,
                    Quantity = item.Quantity,
                    ToDayCode = item.ToDayCode,
                    UserId = item.UserId
                });
            }
        }
        public void AddSupplier(RequestWareSupplierRequest request)
        {
            FDIDB.TotalProductToDays.Add(new TotalProductToDay()
            {

                ProductId = request.ProductId,
                SupplierId = request.SupplierId,
                IsDelete = false,
                DateCreated = DateTime.Now.TotalSeconds(),
                Status = 0,
                Quantity = request.Quantity,
                ToDayCode = request.ToDayCode,
                Price = request.Price,
                UserId = request.UserId
            });

        }

        public List<TotalProductToDayItem> GetSummaryTotalByToDay(decimal todayCode)
        {
            var query = FDIDB.TotalProductToDays.Where(m => m.ToDayCode == todayCode && m.IsDelete == false)
                .GroupBy(m => new { m.ToDayCode, m.ProductId })
                .Select(m =>
                new TotalProductToDayItem()
                {
                    ToDayCode = m.Key.ToDayCode,
                    ProductId = m.Key.ProductId,
                    Quantity = m.Sum(x => x.Quantity)
                });
            return query.ToList();
        }
        public List<TotalProductToDayItem> GetListByToDay(decimal code)
        {
            var query = from o in FDIDB.TotalProductToDays
                        where o.ToDayCode == code && o.IsDelete == false
                        orderby o.ID descending
                        select new TotalProductToDayItem()
                        {
                            ID = o.ID,
                            Quantity = o.Quantity,
                            SupplierName = o.DN_Supplier.Name,
                            SupplierId = o.SupplierId,
                            ProductId = o.ProductId,
                            ProductName = o.Category.Name,
                            SupplierPhone = o.DN_Supplier.Mobile,
                            Price = o.Price,
                            Note = o.Note,
                            Status = o.Status
                        };
            return query.ToList();
        }
        public List<TotalProductToDayItem> GetListByToDay(decimal code, int[] products)
        {
            var query = from o in FDIDB.TotalProductToDays
                        where o.ToDayCode == code && o.IsDelete == false && products.Any(m => m == o.ProductId)
                        orderby o.ID descending
                        select new TotalProductToDayItem()
                        {
                            ID = o.ID,
                            Quantity = o.Quantity,
                            SupplierName = o.DN_Supplier.Name,
                            SupplierId = o.SupplierId,
                            ProductId = o.ProductId,
                            ProductName = o.Category.Name,
                            SupplierPhone = o.DN_Supplier.Mobile,
                            Price = o.Price,
                            Note = o.Note,
                            Status = o.Status
                        };
            return query.ToList();
        }


        public void AddStora(StorageProduct item)
        {
            FDIDB.StorageProducts.Add(item);
        }
        public decimal? GetTotalOrder(decimal totalCode)
        {
            return FDIDB.TotalProductToDays.Where(m => m.ToDayCode == totalCode).Sum(m => (decimal?)m.Quantity);
        }
        public void DeleteStora(StorageProduct item)
        {
            FDIDB.StorageProducts.Remove(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
