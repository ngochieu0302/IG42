using System;
using System.Collections.Generic;
using FDI.CORE;

namespace FDI.Simple
{
    public class ContactOrderItem : BaseSimple
    {
        public int? CutomerID { get; set; }
        public string CutomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CutomerPhone { get; set; }
        public string CutomerAddress { get; set; }
        public Guid? UserID { get; set; }
        public string UserName { get; set; }
        public string Note { get; set; }
        public int? BedDeskID { get; set; }
        public string BedDeskName { get; set; }
        public decimal? DateCreated { get; set; }
        public decimal ReceiveDate { get; set; }
        public decimal? StartDate { get; set; }
        public decimal? EndDate { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? Payments { get; set; }
        public decimal? Discount { get; set; }
        public int? TotalMinute { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Weight { get; set; }
        public int? AgencyId { get; set; }
        public string AgencyName { get; set; }

        public int? Status { get; set; }

        public string StatusTxt => Status == null ? "" : ((OrderStatus) Status.Value).GetDisplayName();

        public CustomerItem CustomerItem { get; set; }
        public int? LevelRoomID { get; set; }
        public virtual IEnumerable<BedDeskItem> DN_Bed_Desk1 { get; set; }
        public IEnumerable<OrderDetailItem> LstOrderDetailItems { get; set; }
    }

    public class ContactProcessItem : BaseSimple
    {
        public Guid? UserID { get; set; }
        public int? BedDeskID { get; set; }
        public decimal? StartDate { get; set; }
        public decimal? EndDate { get; set; }
        public int? Minute { get; set; }
        public bool IsEarly { get; set; }
        public int? Status { get; set; }
        public int? ProductID { get; set; }
        public int? Value { get; set; }
        public Guid? UserCheck { get; set; }
    }
    public class ContactOrderAppItem : BaseSimple
    {
        public decimal? StartDate { get; set; }
        public decimal? EndDate { get; set; }
        public int? CustomerID { get; set; }
        public CustomerAppItem CustomerItem { get; set; }
        public int? Status { get; set; }

        public string StatusStr => Status == null ? string.Empty : ((OrderStatus)Status.Value).GetDisplayName();

        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Latitute { get; set; }
        public string Longitude { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? TotalPrice { get; set; }
        public string Note { get; set; }
        public int? Time { get; set; }
        public decimal? ReceiveDate { get; set; }
        public int ReceiveHour { get; set; }
        public string CustomerName { get; set; }
        public bool IsContactOrder { get; set; }
    }
    public class ModelContactOrderItem : BaseModelSimple
    {
        public IEnumerable<ContactOrderItem> ListItem { get; set; }
        //public IEnumerable<WeeklyScheduleItem> ListWeeklyScheduleItem { get; set; }
    }
}
