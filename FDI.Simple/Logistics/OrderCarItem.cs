using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;
using FDI.CORE;

namespace FDI.Simple.Logistics
{
    public class OrderCarItem : BaseSimple
    {
        public int SupplierId { get; set; }
        public int ProductId { get; set; }
        public int CarId { get; set; }

        [DataType(DataType.DateTime)]
        public decimal DepartureDate { get; set; }
        [DataType(DataType.DateTime)]
        public decimal ReceiveDate { get; set; }
        [DataType(DataType.DateTime)]
        public decimal ReturnDate { get; set; }
        public decimal Quantity { get; set; }
        public decimal QuantityReceived { get; set; }
        public decimal CountReceived { get; set; }
        public string UnitName { get; set; }
        public  int UnitID { get; set; }
        public string Code { get; set; }
        public OrderCarStatus Status { get; set; }
        public decimal? DateUpdate { get; set; }
        public decimal Price { get; set; }
        public decimal PriceNow { get; set; }
        public bool IsDelete { get; set; }
        public Guid UserCreateId { get; set; }
        public Guid? UserUpdateId { get; set; }
        public decimal DateCreate { get; set; }
        public string CarName { get; set; }
        public  string CarType { get; set; }
        public  decimal? TodayCode { get; set; }
        public  string CarPhone { get; set; }
        public string SupplierName { get; set; }
        public  int WorkshopID { get; set; }
        public  string WorkshopName { get; set; }
        public string ProductName { get; set; }
    }

    public class OrderCarModel
    {
        public OrderCarModel()
        {
            Supplier = new SupplieItem();
            OrderCar = new OrderCarItem();
            Category = new CategoryItem();
            Cars = new List<CarItem>();
        }
        public CategoryItem Category { get; set; }
        public OrderCarItem OrderCar { get; set; }
        public SupplieItem Supplier { get; set; }
        public List<CarItem> Cars { get; set; }
        public  List<WorkShopItem> Workshops { get; set; }

    }
    public class OrderCarResponse : BaseModelSimple
    {
        public List<OrderCarItem> ListItem { get; set; }
    }
}
