using System;
using System.Collections.Generic;
using FDI.Base;

namespace FDI.Simple
{
    [Serializable]
    public class ShopStatusItem : BaseSimple
    {
        public string Name { get; set; }
        public string Color { get; set; }
    }

    public class ModelShopItem : BaseModelSimple
    {
        public IEnumerable<ShopStatusItem> ListItem { get; set; }
    }
    [Serializable]
    public class NhapkhoItem : BaseSimple
    {
        public DateTime? NgayNhap { get; set; }
        public int? MaPhieu { get; set; }
        public string NguoiNhap { get; set; }
        public string NguoiBan { get; set; }
        public string TrangThai { get; set; }
        public int? Status { get; set; }
        public string MaKho { get; set; }
        public string GhiChu { get; set; }
        public string HTTT { get; set; }
        public decimal? PayMent { get; set; }
        public int? VAT { get; set; }
        public bool? IsDelete { get; set; }
        public int? SoLuong { get; set; }
        public int? ShopId { get; set; }
        public int? ShopNewId { get; set; }
        public int? BrandId { get; set; }
        public string BrandName { get; set; }
        public string ShopName { get; set; }
        public string ShopNameN { get; set; }
        public int? CusomerID { get; set; }
        public string CusomerName { get; set; }
        public decimal? TongTien { get; set; }
        public decimal? TongTienTra { get; set; }
        public IEnumerable<ChiTietNhapkhoItem> ListChiTiet { get; set; }

    }

    [Serializable]
    public class ChiTietNhapkhoItem : BaseSimple
    {
        public int? ProductID { get; set; }
        public int? NhapKhoId { get; set; }
        public int? SoLuong { get; set; }
        public decimal? GiaVon { get; set; }
        public decimal? GiaBan { get; set; }
        public string MaKho { get; set; }
        public int? SoLuongNhap { get; set; }
        public int? Type { get; set; }
        public bool? IsDelete { get; set; }

        public decimal? Sale { get; set; }
        public int? MaPhieu { get; set; }
        public int? OrderCode { get; set; }
        public DateTime? NgayNhap { get; set; }
        public string NhaCC { get; set; }
        public string KhoChuyen { get; set; }
        public string User { get; set; }
        public string Code { get; set; }
        public string ProductName { get; set; }
        public string PhieuNhapKho { get; set; }
    }
    
}
