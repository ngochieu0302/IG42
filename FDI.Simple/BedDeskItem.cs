using System.Collections.Generic;

namespace FDI.Simple
{
    public class BedDeskItem : BaseSimple
    {
        public string Name { get; set; }
        //public string Color { get; set; }
        public int? LevelRoomId { get; set; }
        public string LevelName { get; set; }
        public int? RoomId { get; set; }
        public string RoomName { get; set; }
        public int? PacketId { get; set; }
        public string PacketName { get; set; }
        public int? Value { get; set; }
        public int? ProductId { get; set; }
        public int? Row { get; set; }
        public int? Quantity { get; set; }
        public int? Column { get; set; }
        public int? CountOrder { get; set; }
        public int? CountContactOrder { get; set; }
        public decimal? Total { get; set; }
        public decimal? TotalPay { get; set; }
        public decimal? TotalDisCount { get; set; }
        public bool? IsShow { get; set; }
        public bool IsEarly { get; set; }
        public int? Status { get; set; }
        public int? Sort { get; set; }
        public int? PacketSort { get; set; }
        //public virtual IEnumerable<DNUserItem> DN_Users { get; set; }
        public virtual DNUserBedDeskItem DN_User_BedDesk { get; set; }
        //public virtual DNPacketItem DN_Packet { get; set; }
        public virtual IEnumerable<int> Shop_Orders { get; set; }
        public virtual IEnumerable<int> ContactOrders { get; set; }
        public IEnumerable<PacketItem> LstPacketItems { get; set; }
    }

    public class ShowDeskItem : BaseSimple
    {
        public int? Q { get; set; }
        public bool? S { get; set; }
        public string Key { get; set; }
    }

    public class ModelBedDeskItem : BaseModelSimple
    {
        public int? AgencyId { get; set; }
        public string NameAgency { get; set; }
        public string AddressAgency { get; set; }
        public string UserName { get; set; }
        public IEnumerable<BedDeskItem> ListItem { get; set; }
        public IEnumerable<DNLevelRoomShowItem> ListRoomItem { get; set; }
        public IEnumerable<CategoryItem> CategoryItems { get; set; }
        public DiscountItem DiscountPItem { get; set; }
        public List<DiscountItem> DiscountItems { get; set; }
        public decimal? Total { get; set; }
        public decimal? TotalPay { get; set; }
        public decimal? TotalDisCount { get; set; }
        public IEnumerable<int> Listid { get; set; }
        public OrderItem OrderItem { get; set; }
        public string Lstid { get; set; }
        public IEnumerable<PacketItem>  lstPacket { get; set; }
    }
}
