using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
    public class AppSimple
    {
        public int id { get; set; }
    }
    public class ModelSWItem // 
    {
        public int t { get; set; } // Tổng bản ghi
        public IEnumerable<SWItem> LstItem { get; set; } // danh sách Phiếu
    }
    public class SWItem : AppSimple // Phiếu đơn hàng
    {
        public string n { get; set; } // Name
        public decimal w { get; set; } // Tích lũy
        public string c { get; set; }// Code
        public decimal wr { get; set; } // Rút Tích lũy
        public decimal wc { get; set; } // Rút Tích lũy
        public decimal? d { get; set; } // Ngày nhận
        public bool s { get; set; } // trạng thái
        
        public IEnumerable<DNRWHItem> LstItem { get; set; } // danh sách đơn hàng
        public IEnumerable<TimeItem> LstTimes { get; set; } // danh sách thời gian
    }
    public class DNRWHItem
    {
        //public Guid id { get; set; }
        public string n { get; set; } // Name
        public bool s { get; set; } // trạng thái
        public int? c { get; set; }// id cate
        public decimal q { get; set; }// số lượng
        public int h { get; set; }// giờ nhận
        public int t { get; set; } // trạng thái sẻ
    }
    public class TimeItem
    {
        public string n { get; set; } // Tên
        public int h { get; set; } // Tên
        public bool s { get; set; } // Trạng thái
    }
}
