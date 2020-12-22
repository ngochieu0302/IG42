using System.Collections.Generic;

namespace FDI.Simple
{
    public class DNLevelRoomItem : BaseSimple
    {
        public string Name { get; set; }
        //public int? ParentId { get; set; }
        public int? AgencyId { get; set; }
        public bool? IsShow { get; set; }
        public decimal? Total { get; set; }
        public decimal? TotalPay { get; set; }
        public decimal? TotalDisCount { get; set; }
        public int? Sort { get; set; }
        public int Count { get; set; }
        //public IEnumerable<BedDeskItem> LstBedDeskItems { get; set; }
    }

    public class DNLevelRoomShowItem : BaseSimple
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public IEnumerable<int> LiInts { get; set; }
    }

    public class ModelDNLevelRoomItem : BaseModelSimple
    {
        public List<DNLevelRoomItem> ListItems { get; set; }
        public List<BedDeskItem> ListbBedDeskItems { get; set; }
        public IEnumerable<OrderItem> ListOrder { get; set; }
        public IEnumerable<ContactOrderItem> ListContacOrder { get; set; }
        public int? AgencyId { get; set; }
        public decimal? Total { get; set; }
        public decimal? TotalPay { get; set; }
        public decimal? TotalDisCount { get; set; }
    }
}
