using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class ProfitItem : BaseSimple
    {
        public DateTime? ThoiGian { get; set; }
        public int? SlBan { get; set; }
        public int? SlTra { get; set; }
        public int? SoHD { get; set; }
        public int? SoHDTra { get; set; }
        public decimal? GiamGia { get; set; }
        public decimal? TienBH { get; set; }
        public decimal? TienTH { get; set; }
        public decimal? Von { get; set; }
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public string TenCH { get; set; }
        public string User { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
    }

    public class ModelProfitItem : BaseModelSimple
    {
        public DateTime ToDate { get; set; }
        public DateTime EndDate { get; set; }
        public int View { get; set; }
        public IEnumerable<ProfitItem> ListItem { get; set; }
        public IEnumerable<ProfitItem> ListFirstItem { get; set; }
    }

    public class StatisticItem : BaseModelSimple
    {
        public IEnumerable<ProfitItem> ListUserItem { get; set; }
        public IEnumerable<ProfitItem> ListShopItem { get; set; }
    }
}
