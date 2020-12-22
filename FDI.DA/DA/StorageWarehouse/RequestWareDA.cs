using System;
using System.Collections.Generic;
using System.Linq;
using FDI.Base;
using CORE = FDI.CORE;
using FDI.Simple;
using FDI.Simple.Order;
using FDI.Simple.StorageWarehouse;
using FDI.Utils;
using FDI.CORE;

namespace FDI.DA.DA.StorageWarehouse
{
    public class RequestWareDA : BaseDA
    {
        public decimal? GetTotalOrder(decimal todayCode)
        {
            return FDIDB.DN_RequestWare.Where(m => m.Today == todayCode && m.IsDelete == false).Sum(m => m.Quantity);
        }

        //get summary product in today
        public List<DNRequestWareItem> GetSummaryProductsByToDayCode(decimal todayCode)
        {
            var lst = FDIDB.DN_RequestWare.Where(m => m.Today == todayCode && m.IsDelete == false)
                .GroupBy(m => new { m.Today, m.CateID, m.Category.Name }).Select(m =>
                    new DNRequestWareItem()
                    {
                        Today = m.Key.Today,
                        Quantity = m.Sum(x => x.Quantity),
                        CateID = m.Key.CateID,
                        ProductName = m.Key.Name
                    });
            return lst.ToList();
        }
        public List<DNRequestWareItem> GetTotalProductNotConfirm(decimal todayCode)
        {
            var query = from c in FDIDB.StorageWarehousings
                        join rq in FDIDB.DN_RequestWare
                            on c.ID equals rq.StorageProductID
                        where rq.Today == todayCode && (c.Status == (int)StatusWarehouse.New || c.Status == (int)StatusWarehouse.Pending ||
                              c.Status == (int)StatusWarehouse.WattingConfirm)
                              && rq.IsDelete == false
                        group rq by new { rq.Today, rq.CateID } into g
                        select new DNRequestWareItem()
                        {
                            CateID = g.Key.CateID,
                            Quantity = g.Sum(m => m.Quantity)
                        };
            return query.ToList();
        }

        public List<ProductConfirm> GetTotalProductConfirm(decimal todayCode)
        {
            var query = from c in FDIDB.TotalProductToDays
                        join s in FDIDB.DN_Supplier
                            on c.SupplierId equals s.ID
                        where c.ToDayCode == todayCode && c.IsDelete == false
                        group c by new { c.SupplierId, c.ToDayCode, c.ProductId, c.Category.Name } into g
                        select new ProductConfirm()
                        {
                            ProductId = g.Key.ProductId,
                            ProductName = g.Key.Name,
                            SupplierId = g.Key.SupplierId,
                            Quantity = g.Sum(m => m.Quantity)
                        };

            return query.ToList();
        }

        public IList<OrderDetailProductItem> GetSummary(decimal todayCode)
        {
            return FDIDB.sp_GetOrderDetail(todayCode).Select(m => new OrderDetailProductItem()
            {
                ProductId = m.ProductDetailID,
                ProductName = m.Name,
                Quantity = m.Quantity ?? 0,
                CateID = m.CateID,
                CategoryName = m.CateName,
                SizeId = m.SizeID ?? 0,
                UnitName = m.UnitName,
                UnitValue = m.UnitValue ?? 0
            }).ToList();
        }

        public IEnumerable<RequestWareDetail> GetDetails(Guid[] ids)
        {
            return from c in FDIDB.DN_RequestWareDetail
                   where ids.Any(n => n == c.RequestWareId)
                   select new RequestWareDetail()
                   {
                       ProductName = c.Shop_Product.Shop_Product_Detail.Name,
                       Quantity = c.Quantity,
                       ProductId = c.ProductId
                   };
        }

        public IEnumerable<DN_RequestWare> GetWaittingProduceByTodayCode(decimal todayCode, int productId)
        {
            var query = from c in FDIDB.DN_RequestWare
                        where c.Today == todayCode && c.CateID == productId && c.StorageWarehousing.Status == (int)StatusWarehouse.AgencyConfirmed
                                                   && c.Status == (int)CORE.DNRequestStatus.New
                        select c;
            return query;
        }

        public List<DNRequestWareItem> GetItemsBySupplierId(IList<int> supplierids)
        {
            var query = from c in FDIDB.DN_RequestWare
                        join b in FDIDB.DN_RequestWareSupplier on c.GID equals b.RequestWareId

                        where (c.IsDelete == null || c.IsDelete == false)
                              && (c.StorageWarehousing.IsDelete == null || c.StorageWarehousing.IsDelete == false)
                              && b.IsDelete == false
                              && supplierids.Any(m => m == b.SupplierId)

                        select new DNRequestWareItem()
                        {
                            GID = c.GID,
                            ProductName = c.Category.Name,
                            Quantity = b.Quantity,
                            Today = c.Today,
                            StorageProductID = c.StorageProductID,
                            OrderStatus = (StatusWarehouse)c.StorageWarehousing.Status,
                            UnitName = c.Category.DN_Unit.Name,
                            AgencyMobile =  c.StorageWarehousing.DN_Agency.Phone,
                            AgencyAddress = c.StorageWarehousing.DN_Agency.Address,
                            Price = c.Price,
                            Sale = c.Sale
                        };
            return query.ToList();
        }

        public decimal GetQuantityFinish(int agencyId, int categoryId, int month)
        {
            var date = new DateTime(DateTime.Now.Year, month, 1);
            var endTime = date.AddMonths(1).TotalSeconds();
            var startTime = date.TotalSeconds();

            var query = from c in FDIDB.DN_RequestWare
                            //join b in FDIDB.DN_RequestWareSupplier on c.GID equals b.RequestWareId

                        where (c.IsDelete == null || c.IsDelete == false)
                              && (c.StorageWarehousing.IsDelete == null || c.StorageWarehousing.IsDelete == false)
                              //&& b.IsDelete == false
                              //&& supplierId == b.SupplierId
                              && c.StorageWarehousing.AgencyId == agencyId
                              && c.CateID == categoryId
                              && c.StorageWarehousing.Status == (int)StatusWarehouse.Imported
                              && c.StorageWarehousing.DateRecive != null
                              && c.StorageWarehousing.DateRecive >= startTime && c.StorageWarehousing.DateRecive < endTime
                        select new DNRequestWareItem()
                        {
                            Quantity = c.Quantity ?? 0
                        };
            var tmp = query.Sum(m => m.Quantity);
            return tmp ?? 0;
        }

    }
}
