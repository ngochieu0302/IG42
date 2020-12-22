using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class EventItem : BaseSimple
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public string Des { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Image { get; set; }
        public string SuDung { get; set; }
        public bool? IsGuiMail { get; set; }
        public bool? IsPopup { get; set; }
        public bool? IsMobile { get; set; }
        public int? TimeNhacViec { get; set; }
        public int? KieuLap { get; set; }
        public DateTime? NgayBatDauLap { get; set; }
        public DateTime? NgayKetThucLap { get; set; }
        public bool? isImportant { get; set; }
        public bool? isPriod { get; set; }
        public bool? isCancel { get; set; }
        public bool? isApprove { get; set; }
        public int? CreateUserID { get; set; }
        public string Location { get; set; }
        public int? ChuTri { get; set; }
        public string ThamGia { get; set; }
        public string ChuanBi { get; set; }
        public int? PortalID { get; set; }
        public string LanguageId { get; set; }
        public DateTime? CreateDate { get; set; }
        public IEnumerable<UserItem> ListUsers { get; set; }
        public UserItem UserItem { get; set; }

        public string Ngay { get; set; }
        public string Thang { get; set; }
        public int? Year { get; set; }
        public int? Gio { get; set; }
        public int? Phut { get; set; }
        public int? EndGio { get; set; }
        public int? EndPhut { get; set; }
        public string UserName { get; set; }
       
    }
    public class ModelEventItem : BaseModelSimple
    {
        public IEnumerable<EventItem> ListEventItem { get; set; }
        public IEnumerable<UserItem> ListUserItem { get; set; }
        public string DateText { get; set; }
        public string CodeColor { get; set; }
        public IEnumerable<string> ListYear { get; set; }
        public IEnumerable<string> ListDay { get; set; }
        public IEnumerable<object> DanhSachThu { get; set; }
        public string Content { get; set; }
        public int DayOfWeek { get; set; }
        public int FirstDayOfWeek { get; set; }
        public int LastDayOfWeek { get; set; }
        public string Day { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public int ALLDayOfMonth { get; set; }
        public int DayStartOfMonth { get; set; }
        public IList<int> ListWeekOfYear { get; set; }
        public IList<int> ListWeekOfWeek { get; set; }
        public IEnumerable<DateTime> ListtDayOfWeek { get; set; }
        public int GetWeekNumber { get; set; }
    }

    public class ModelDateItem : BaseModelSimple
    {
        public string DateText { get; set; }
        public string Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public bool Error { get; set; }
        public int GetWeekNumber { get; set; }
        public string FirstDayOfWeek { get; set; }
        public string LastDayOfWeek { get; set; }
       
    }
}
