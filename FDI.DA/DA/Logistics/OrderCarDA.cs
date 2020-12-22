using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.CORE;
using FDI.Simple;
using FDI.Simple.Logistics;

namespace FDI.DA.DA.Logistics
{
    public class OrderCarDa : BaseDA
    {
        public List<OrderCarItem> GetListSimpleByRequest(HttpRequestBase httpRequest, decimal todayCode)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.OrderCars
                        where o.IsDelete == false && o.TodayCode == todayCode
                        orderby o.ID descending
                        select new OrderCarItem()
                        {
                            ID = o.ID,
                            Quantity = o.Quantity,
                            CarId = o.CarId,
                            SupplierId = o.SupplierId,
                            Status = (OrderCarStatus)o.Status,
                            Code = o.Code,
                            DateCreate = o.DateCreate,
                            DateUpdate = o.DateUpdate,
                            DepartureDate = o.DepartureDate,
                            PriceNow = o.PriceNow,
                            Price = o.Price,
                            ProductId = o.ProductId,
                            ReturnDate = o.ReturnDate,
                            ReceiveDate = o.ReceiveDate,
                            CarName = o.Car.Name,
                            CarType = o.Car.CarType,
                            CarPhone = o.Car.Phone,
                            UnitName = o.Category.DN_Unit.Name,
                            SupplierName = o.DN_Supplier.Name,
                            WorkshopName =  o.P_Workshop.Name,
                            ProductName  = o.Category.Name

                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public List<OrderCarItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int[] status)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.OrderCars
                where o.IsDelete == false && status.Any(m=>m == o.Status)
                        orderby o.ID descending
                select new OrderCarItem()
                {
                    ID = o.ID,
                    Quantity = o.Quantity,
                    CarId = o.CarId,
                    SupplierId = o.SupplierId,
                    Status = (OrderCarStatus)o.Status,
                    Code = o.Code,
                    DateCreate = o.DateCreate,
                    DateUpdate = o.DateUpdate,
                    DepartureDate = o.DepartureDate,
                    PriceNow = o.PriceNow,
                    Price = o.Price,
                    ProductId = o.ProductId,
                    ReturnDate = o.ReturnDate,
                    ReceiveDate = o.ReceiveDate,
                    CarName = o.Car.Name,
                    CarType = o.Car.CarType,
                    CarPhone = o.Car.Phone,
                    UnitName = o.Category.DN_Unit.Name,
                    SupplierName = o.DN_Supplier.Name,
                    WorkshopName = o.P_Workshop.Name

                };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public OrderCar GetById(int id)
        {
            return FDIDB.OrderCars.FirstOrDefault(m => m.ID == id);
        }
        public OrderCarItem GetItemById(int id)
        {
            var query = from o in FDIDB.OrderCars
                        where o.IsDelete == false && o.ID == id
                        orderby o.ID descending
                        select new OrderCarItem()
                        {
                            ID = o.ID,
                            Quantity = o.Quantity,
                            CarId = o.CarId,
                            SupplierId = o.SupplierId,
                            Status = (OrderCarStatus)o.Status,
                            Code = o.Code,
                            DateCreate = o.DateCreate,
                            DateUpdate = o.DateUpdate,
                            DepartureDate = o.DepartureDate,
                            PriceNow = o.PriceNow,
                            Price = o.Price,
                            ProductId = o.ProductId,
                            ReturnDate = o.ReturnDate,
                            ReceiveDate = o.ReceiveDate,
                            CarName = o.Car.Name,
                            CarType = o.Car.CarType,
                            CarPhone = o.Car.Phone,
                            UnitName = o.Category.DN_Unit.Name,
                            UnitID =  o.Category.DN_Unit.ID,
                            SupplierName = o.DN_Supplier.Name
                        };
            return query.FirstOrDefault();
        }

        public List<OrderCarItem> GetByToday(decimal todayCode)
        {
            var query = from o in FDIDB.OrderCars
                        where o.IsDelete == false && o.TodayCode == todayCode
                        orderby o.ID descending
                        select new OrderCarItem()
                        {
                            ID = o.ID,
                            Quantity = o.Quantity,
                            CarId = o.CarId,
                            SupplierId = o.SupplierId,
                            Status = (OrderCarStatus)o.Status,
                            Code = o.Code,
                            DateCreate = o.DateCreate,
                            DateUpdate = o.DateUpdate,
                            DepartureDate = o.DepartureDate,
                            PriceNow = o.PriceNow,
                            Price = o.Price,
                            ProductId = o.ProductId,
                            ReturnDate = o.ReturnDate,
                            ReceiveDate = o.ReceiveDate,
                            CarName = o.Car.Name,
                            CarType = o.Car.CarType,
                            CarPhone = o.Car.Phone,
                            UnitName = o.Category.DN_Unit.Name
                        };
            return query.ToList();
        }

        public void Add(OrderCar itemCar)
        {
            FDIDB.OrderCars.Add(itemCar);
        }
    }
}
