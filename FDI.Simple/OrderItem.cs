using System;
using System.Collections.Generic;
using FDI.CORE;

namespace FDI.Simple
{
    public class OrderItem : BaseSimple
    {
        public Guid? UserID { get; set; }
        public int? AgencyId { get; set; }
        public int? BedDeskID { get; set; }
        public int? LevelRoomId { get; set; }
        public decimal? TotalPrice { get; set; }
        public int? TotalMinute { get; set; }
        public Guid? UserIdBedDeskID { get; set; }
        public int? AddMinuteID { get; set; }
        public string UserName { get; set; }
        public string UserName1 { get; set; }
        public string UserName2 { get; set; }
        public string CodeUser { get; set; }
        public string BedDeskName { get; set; }
        public string CustomerName { get; set; }
        public string CutomerPhone { get; set; }
        public string CutomerAddress { get; set; }        
        public int? CustomerID { get; set; }        
        public decimal? DateCreated { get; set; }
        public decimal? StartDate { get; set; }
        public decimal? EndDate { get; set; }
        public string Note { get; set; }
        public string Mobile { get; set; }
        public int? Status { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Deposits { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsActive { get; set; }
        public decimal? PrizeMoney { get; set; }
        public decimal? Payments { get; set; }
        public decimal? SalePercent { get; set; }
        public decimal? SalePrice { get; set; }
        
        public string SaleCode { get; set; }
        public string Billnumber { get; set; }
        public string TaxCode { get; set; }
        public string CompanyName { get; set; }
        public IEnumerable<OrderDetailItem> LstOrderDetailItems { get; set; }
        public virtual IEnumerable<BedDeskItem> DN_Bed_Desk1 { get; set; }   
        
        public string Name { get; set; }
        public decimal? ReceiveDate { get; set; }

        public string ReceiveDateTxt => ReceiveDate.DecimalToString("dd/MM/yyyy");
    }

    public class OrderProcessItem : BaseSimple
    {
        public Guid? UserID { get; set; }
        public string UserName { get; set; }
        public int? BedDeskID { get; set; }
        public decimal? StartDate { get; set; }
        public decimal? EndDate { get; set; }
        public int? Minute { get; set; }
        public int? Status { get; set; }
        public bool IsEarly { get; set; }
        public int? ProductID { get; set; }
        public int? Value { get; set; }
        public int? AgencyId { get; set; }
        public int TimeWait { get; set; }
        public Guid? UserCheck { get; set; }
    }

    public class OrderGetItem : BaseSimple
    {
        public Guid? UserID { get; set; }
        public int? BedDeskID { get; set; }
        public decimal? StartDate { get; set; }
        public decimal? EndDate { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Deposits { get; set; }
        public decimal? PrizeMoney { get; set; }
        public decimal? Payments { get; set; }
        public int? Status { get; set; }
        public int? ProductID { get; set; }
        public int? Value { get; set; }
        public int? CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public bool? IsEarly { get; set; }
        public bool? IsDeposit { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public int? Time { get; set; }
        public decimal? Price { get; set; }
        public List<int> list { get; set; }
        public int ReceiveHour { get; set; }
        public IEnumerable<ContactOderDetailItem> Listproduct { get; set; }
        public IEnumerable<ProductItem> ListItem { get; set; }

        public decimal ReceiveDate { get; set; }

        public List<ProductAppItem> ListProductModel { get; set; }
    }

    public class ModelOrderGetItem : BaseSimple
    {
        public Guid? UserID { get; set; }
        public int? BedDeskID { get; set; }
        public Guid? UserIdBesk { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Deposits { get; set; }
        public decimal? PrizeMoney { get; set; }
        public decimal? Payments { get; set; }
        public decimal? TotalPrice { get; set; }
        public int? Status { get; set; }
        public int? ProductID { get; set; }
        public int? CustomerID { get; set; }
        public int? AddMinuteID { get; set; }
        public bool? IsEarly { get; set; }
        public bool? IsDeposit { get; set; }
        public string Note { get; set; }
        public List<ListBebDesk> list { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public decimal? EndDate { get; set; }
        public int? Value { get; set; }
        public string ContactId { get; set; }
        public int? Time { get; set; }
        public int? TimeEarly { get; set; }
        public int? TimeWait { get; set; }
        public int? AgencyID { get; set; }
        public decimal? Price { get; set; }
        public string CustomerName { get; set; }
        public decimal? StartDate { get; set; }
        public string Lstproduct { get; set; }
        public IEnumerable<DNProductPacketItem > LstProductPacketItems  {get; set; }
        public IEnumerable<ProductItem> ListItem { get; set; }
        public IEnumerable<ContactOderDetailItem> Listproduct { get; set; }
        public List<DiscountItem> DiscountItems { get; set; }
    }
    public class OrderNewItem : BaseSimple
    {
        public decimal? TotalPrice { get; set; }
    }

    public class ListBebDesk
    {
        public int? idbed { get; set; }
        public string UserName { get; set; }
        public Guid? userid { get; set; }
        public int c { get; set; }
    }
    public class ModelOrderItem : BaseModelSimple
    {
        public IEnumerable<OrderItem> ListOrderItem { get; set; }
        public IEnumerable<AddMinuteItem> ListAddMinuteItem { get; set; }
        public OrderItem OrderItem { get; set; }
        public int Count { get; set; }
        public int AgencyId { get; set; }
        public decimal? Total { get; set; }
        public decimal? TotalPay { get; set; }
        public decimal? TotalDiscount { get; set; }
    }
    public class ModelRewardItem : BaseModelSimple
    {
        public decimal? TotalRecive { get; set; }
        public decimal? TotalRewar { get; set; }
        public List<RewardHistoryItem> ListReward { get; set; }
        public List<RewardHistoryItem> ListReceive { get; set; }
        public IEnumerable<AgencyItem> ListAgency { get; set; }
        public int AgencyId { get; set; }
    }

    public class OrderAppSaleItem : BaseSimple
    {
        public string N { get; set; } // ten khac hhang 
        public string P { get; set; } // so dien thoai khach hang
        public string A { get; set; } // dia chi khach hang
        public int? CId { get; set; } // ID khach hhang
        public decimal? D { get; set; } //ngay tao don
        public decimal Pay {get; set; } // tien thanh toan
        public int? S { get; set; } // trang thai
        public decimal? Tp { get; set; } // tong tien don hang
        public string Note { get; set; } // ghi chu
    }

    public class ModelOrderAppSaleItem
    {
        public int Total { get; set; }
        public IEnumerable<OrderAppSaleItem> ListItems { get; set; }
    }


}
