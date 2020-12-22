using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class BarcodeSouceItem 
    {
        public Guid? GID { get; set; }
        public decimal? Value { get; set; } // trọng lượng
        public decimal? Price { get; set; }// Giá
        public decimal? PriceNew { get; set; }// Thành tiền.
        public string BarCode { get; set; }// Mã vạch
        public string NameAgency { get; set; }// Tên cửa hàng bán.
        public int? IDStorage { get; set; }// Mã phiếu nhập hàng
        public int? CountSource1 { get; set; }// Số giai đoạn(bài viết liên quan)
        public int? CountSource2 { get; set; }// Số giai đoạn(bài viết liên quan)
        public int? CountSource3 { get; set; }// Số giai đoạn(bài viết liên quan)
        public int? CountSource4 { get; set; }// Số giai đoạn(bài viết liên quan)
        public string AddressAgency { get; set; }// Địa chỉ cửa hàng
        public string PhoneAgency { get; set; }// SĐT cửa hàng
        public string NameSupplier { get; set; }// Tên trang trại nuôi
        public string AddressSupplier { get; set; }// Địa chỉ trang trại nuôi
        public bool? IsCheck { get; set; }// Trạng thái check mã vạch
        public string FullName { get; set; }// Tên khách hàng mua sản phẩm
        public string UserName { get; set; }// Tên đăng nhập(SĐT) KH mua SP
        public int Status { get; set; }// Trạng thái = 1 thành công hay KH đã đc 2%
        public decimal? DatePack { get; set; }
        public decimal? DateImport { get; set; }
        public decimal? DateOrder { get; set; }
    }

    public class ModelBarcodeSouceItem : BaseModelSimple
    {
        public IEnumerable<BarcodeSouceItem> ListItem { get; set; }
    }
}
