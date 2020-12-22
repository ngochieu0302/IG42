using System;
using FDI.Simple;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FDI.CORE;

namespace FDI.Utils
{
    public enum Agency
    {
        Agency1 = 1006,
    }

    public enum ActionType
    {
        NoRole = 0,
        Add = 1,
        Edit = 2,
        Delete = 3,
        Active = 4,
        View = 5,
        Show = 6,
        Hide = 7,
        Order = 8,
        Public = 9,
        Complete = 10,
        UserModule = 11,
        RoleModule = 12,
        Excel = 13,
        NotActive = 14,
    }
    public enum Card
    {
        Create = 0, // Khởi tạo
        Excel = 1,//Thẻ đã in
        Released = 2,//Đã phát hành
        Lock = 3 // khóa thẻ
    }
    public enum Position
    {
        Detailnews = 17,
    }
    public enum Reward
    {
        Root = 0,    //Thưởng gốc
        Parent = 1,  //Thưởng cha
        Cus = 2,     // thưởng trực tiếp
        Receive1 = 0,// trừ tích lũy vào đơn hàng
        Receive2 = 1 // Rút tiền trực tiếp
    }
    public static class TypeSale
    {
        public static Dictionary<string, int> Image()
        {
            var list = new Dictionary<string, int>
            {
                {"Giám giá trên đơn hàng", 1},
                {"Giám giá theo sản phẩm", 2},
                {"Giảm giá tạo Voucher", 3},
                {"Giảm giá theo Ngày/Tháng sinh", 4},
            };
            return list;
        }
    }
    public static class TypeHours
    {
        public static Dictionary<string, int> Hours()
        {
            var list = new Dictionary<string, int>
            {
                {"4h-5h", 4},
                {"8h-9h", 8},
                {"15h-16h", 15},
            };
            return list;
        }
    }

    public static class TypeTime
    {
        public static List<TimeItem> Hours(decimal? date, decimal datenow)
        {
            var list = new List<TimeItem>
            {
                new TimeItem{n="4-5h", h = 4, s= date +4*3600> datenow},
                new TimeItem{n="8-9h", h = 8,s= date+8*3600> datenow},
                new TimeItem{n="15-16h", h = 15,s= date+15*3600> datenow},
            };
            return list.ToList();
        }
    }
    public static class TypeDocument
    {
        public static Dictionary<string, int> Document()
        {
            var list = new Dictionary<string, int>
            {
                {"Hợp đồng đại lý", 1},
                {"Hợp đồng nhân sự", 2},
                {"Hợp đồng đối tác", 3},
                //{"Hợp đồng cho thuê", 4},
            };
            return list;
        }
    }
    public enum TypeEnumSale
    {
        Order = 1,
        Product = 2,
        Voucher = 3,
        Birthday = 4,
    }

   
    public enum TypeOrder
    {
        Banle = 1,
        Banbuon = 2,
    }

    public static class TypePromotion
    {
        public static Dictionary<string, int> Image()
        {
            var list = new Dictionary<string, int>
            {
                {"Giám giá trên đơn hàng", 1},
                {"Giám giá theo sản phẩm", 2},
            };
            return list;
        }
    }
    public enum TypeEnumPromotion
    {
        Order = 1,
        Product = 2,
    }
    public enum PaymentMethod
    {
        Transfer = 0,    //Chuyển khoản
        Cash = 1,  //Tiền mặt
        Other = 2,  //hình thức khác
    }
    public enum Vouchers
    {
        Payment = 0,//phiếu chi
        Receipt = 1,//Phiếu thu
        ReceiptPayment = 2,//Phiếu chuyển
        BiasProduce = 3,//Phiếu chuyển
        BiasUser = 4//Phiếu chuyển
    }

    public enum Order // s
    {
        Before = 600,
        Order = 300,
        Temp = 100
    }

    public enum FolderImage
    {
        Thumbs = 1,
        Mediums = 2,
        Images = 3,
        Originals = 4,
    }

    // Product

    public enum TypeSendMail
    {
        InfoPayment = 1,    // gửi mail cho admin thông tin mua hàng của khách
        OrderCustomer = 2    // gửi thông tin đơn hàng cho khách hàng
    }

    public static class ListFoder
    {
        public static Dictionary<string, int> Image()
        {
            var list = new Dictionary<string, int>
            {
                {"Thumbs (Ảnh nhỏ < 257px)", 1},
                {"Mediums (Ảnh vừa < 640px)", 2},
                {"Images (Ảnh lớn < 1280px)", 3},
                {"Originals (Ảnh rất lớn < 1920px)", 4},
            };
            return list;
        }
    }
    public static class ListAnalysis
    {
        public static Dictionary<string, int> Names()
        {
            var list = new Dictionary<string, int>
            {
                {"Đơn hàng", 1},
                {"Sản phẩm", 2},
                {"Lợi nhuận", 3},
                {"Khách hàng", 4},
            };
            return list;
        }
    }
    public static class ListStatus
    {
        public static Dictionary<string, int> StatusInts()
        {
            var list = new Dictionary<string, int>
            {
                {"Active", 1},
                {"Reset", 2},
                {"Return", 3},
                {"Cancel", 4},
                {"Not", 0},
            };
            return list;
        }
    }

    public enum Criteria
    {
        Level = 1,
        Live = 2,
        Roles = 3,
        AllOrder = 4,
    }

    public enum MailSsc
    {
        Inbox = 1, // hộp thư đến
        Sent = 2, // hộp thư đã gửi
        Drafts = 3,  // hộp thư nháp
        Spam = 4,  // hộp thư nháp
        RecycleBin = 5  // thùng rác
    }

    public enum SlideType
    {
        MainSlide = 4,
        AdvertiseRight = 5,
        AdvertiseLeft = 6,
        Partner = 2,
        LstPartner = 10,
        AdvertHome = 7,
        DanhbaWebsite = 9
    }

    public enum ModuleType
    {
        News = 1,
        Product = 2,
        Document = 3,
        Gallery = 4,
        Video = 5,
        FAQ = 6,
    }



    public enum Template : int
    {
        PurchaseOrder = 2
    }

    public enum UnitID:int
    {
        KG=1,
        GAM=5
    }

}