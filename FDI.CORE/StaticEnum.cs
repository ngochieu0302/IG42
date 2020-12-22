using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FDI.CORE
{

    [Flags]
    public enum OrderCarStatus : int
    {
        [Display(Name = "Tạo mới")]
        News = 1,
        [Display(Name = "Đang di chuyển")]
        Process = 2,
        [Display(Name = "Nhận hàng")]
        AtSupplier = 4,
        [Display(Name = "Về xưởng")]
        AtWrokShop = 8,
        [Display(Name = "Hoàn thành")]
        Done = 16,

    }
    public enum CateValueStatus : int
    {
        NoneActive = 0,
        Active = 1,
    }
    public enum ProduceStatus : int
    {
        [Display(Name = "Tạo mới"), Css("label-primary")]
        New = 1,

        [Display(Name = "Lấy sản phẩm"), Css("label-info")]
        GetProduct = 2
    }
    public enum DNRequestStatus : int
    {
        [Display(Name = "Tạo mới"), Css("label-primary")]
        New = 1,

        [Display(Name = "Đang sản xuất"), Css("label-info")]
        Processing = 2
    }

    [Flags]
    public enum OrderStatus : int
    {
        [Display(Name = "Tạo mới"), Css("label-primary")]
        Pending = 1,    // đang chờ

        [Display(Name = "Đang xử lý"), Css("label-primary")]
        Processing = 2, // đang xư lý
        [Display(Name = "Đang giao")]
        Shipper = 4, // Chuyển hàng

        [Display(Name = "Đã giao"), Css("label-primary")]
        Complete = 8,   // đã hoàn thành

        [Display(Name = "Đã hủy"), Css("label-primary")]
        Cancelled = 16,  // đã hủy

        [Display(Name = "Trả hàng"), Css("label-primary")]
        Refunded = 32,   // trả hàng


    }

    public enum OrderType : int
    {
        [Order(1)]
        TOMORROW = 1,
        [Order(4)]
        TWOHOURS = 2,
        //INTODAY = 3,
        [Order(3)]
        BEFORE12H = 4,
        [Order(2)]
        BEFORE17H = 5,
    }

    [Flags]
    public enum StatusWarehouse
    {
        [Display(Name = "Tạo mới"), Css("label-primary")]
        New = 1,
        [Display(Name = "Đang xử lý"), CssAttribute("label-info")]
        Pending = 2,
        [Display(Name = "Chờ xác nhận"), CssAttribute("label-warning")]
        WattingConfirm = 4,
        [Display(Name = "Đã xác nhận"), CssAttribute("label-default")]
        AgencyConfirmed = 8,
        [Display(Name = "Đang chờ sản xuất"), CssAttribute("label-warning")]
        Waitting = 16,
        [Display(Name = "Đã nhận hàng"), CssAttribute("label-danger")]
        Imported = 32,
        [Display(Name = "Đã duyệt"), CssAttribute("label-success")]
        Complete = 64,
    }

    public enum CategoryModule : int
    {
        Product = 2
    }
}
