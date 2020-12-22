using System.Collections.Generic;

namespace FDI.Simple
{
    public class PacketItem : BaseSimple
    {
        public string Name { get; set; }
        public int? Sort { get; set; }
        public int? Time { get; set; }
        public decimal? Price { get; set; }
        public int? AgencyID { get; set; }
        //public int? Value { get; set; }
        public bool? IsEarly { get; set; }
        public bool? IsDefault { get; set; }
        public int? TimeEarly { get; set; }
        public int? TimeWait { get; set; }
        public ProductItem Product { get; set; }
        public IEnumerable<DNProductPacketItem> ListProductPacketItems { get; set; }
        public IEnumerable<ProductItem> LstProduct { get; set; }
        public IEnumerable<BedDeskItem> LstBedDesk { get; set; }
    }

    public class ModelPacketItem : BaseModelSimple
    {
        public List<PacketItem> ListItem { get; set; }
    }

    public class DNProductPacketItem:BaseSimple
    {
        public int? ProductId { get; set; }
        public string NameProduct { get; set; }
        public decimal? Price { get; set; }
        public bool? IsDefault { get; set; }
        public int? Time { get; set; }
    }
}
