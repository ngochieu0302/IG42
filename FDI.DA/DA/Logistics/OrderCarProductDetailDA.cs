using FDI.Base;
using FDI.Simple.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FDI.Simple.Logistics;

namespace FDI.DA.DA.Logistics
{
    public class OrderCarProductDetailDA : BaseDA
    {
        public List<OrderCarProductDetailItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int ordercarId)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.OrderCarProductDetails
                        where o.IsDelete == false && o.OrderCarId == ordercarId
                        orderby o.ID descending
                        select new OrderCarProductDetailItem()
                        {
                            ID = o.ID,
                            Quantity = o.Quantity,
                            Code = o.Code,
                            ProductName = o.OrderCar.Category.Name
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public void Add(OrderCarProductDetail item)
        {
            FDIDB.OrderCarProductDetails.Add(item);
        }
        public OrderCarProductDetail GetById(int id)
        {
            return FDIDB.OrderCarProductDetails.FirstOrDefault(m => m.ID == id);
        }

        public OrderCarProductDetailItem GetItemById(int id)
        {
            return FDIDB.OrderCarProductDetails.Where(m => m.ID == id).Select(o => new OrderCarProductDetailItem()
            {
                ID = o.ID,
                Quantity = o.Quantity,
                Code = o.Code
            }).FirstOrDefault();
        }
        public List<OrderCarProductDetailItem> GetItemById(IList<int> ids)
        {
            return FDIDB.OrderCarProductDetails.Where(m => ids.Any(n => n == m.ID)).Select(o => new OrderCarProductDetailItem()
            {
                ID = o.ID,
                Quantity = o.Quantity,
                OrderCarID = o.OrderCarId,
                Code = o.Code
            }).ToList();
        }
        public List<OrderCarProductDetailItem> GetItemByOrderCarId(int orderCarId)
        {
            return FDIDB.OrderCarProductDetails.Where(m => m.OrderCarId == orderCarId).Select(o => new OrderCarProductDetailItem()
            {
                ID = o.ID,
                Quantity = o.Quantity,
                OrderCarID = o.OrderCarId,
                Code = o.Code,
                ProductName = o.OrderCar.Category.Name,
                PriceUnit = o.OrderCar.Price
            }).ToList();
        }
        public List<CarItem> GetAll()
        {
            return FDIDB.Cars.Select(o => new CarItem()
            {
                ID = o.ID,
                Quantity = o.Quantity,
                Name = o.Name,
                Address = o.Address,
                CarType = o.CarType,
                Latitude = o.Latitude,
                Longitude = o.Longitude,
                Phone = o.Phone,
                UnitID = o.UnitID,
                UnitName = o.DN_Unit.Name
            }).ToList();
        }

        public List<CarItem> GetListAssign(int unitId)
        {
            return FDIDB.Cars.Where(m => m.UnitID == unitId && m.IsDelete == false).Select(o => new CarItem()
            {
                ID = o.ID,
                Quantity = o.Quantity,
                Name = o.Name,
                Address = o.Address,
                CarType = o.CarType,
                Latitude = o.Latitude,
                Longitude = o.Longitude,
                Phone = o.Phone,
                UnitID = o.UnitID,
                UnitName = o.DN_Unit.Name
            }).ToList();
        }

        public decimal? GetAmountRecevied(int ordercarId)
        {
            return FDIDB.OrderCarProductDetails.Where(m => m.IsDelete == false && m.OrderCarId == ordercarId)
                .Sum(m => (decimal?)m.Quantity);
        }
        public List<OrderCarItem> GetAmountRecevied(int[] ordercarIds)
        {
            return FDIDB.OrderCarProductDetails.Where(m => m.IsDelete == false && ordercarIds.Any(n => n == m.OrderCarId))
                .GroupBy(m => m.OrderCarId)
                .Select(m => new OrderCarItem()
                {
                    ID = m.Key,
                    QuantityReceived = m.Sum(n => n.Quantity)
                }).ToList();
        }
        public int CountRecevied(int ordercarId)
        {
            return FDIDB.OrderCarProductDetails
                .Count(m => m.IsDelete == false && m.OrderCarId == ordercarId);
        }

        public bool CheckEsixt(string code)
        {
            var query = from c in FDIDB.OrderCarProductDetails
                        where c.Code == code && c.IsDelete == false
                        select c;
            return query.Any();
        }


    }
}
