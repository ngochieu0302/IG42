using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class DNUserItem
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public string UserName { get; set; }
        public string Rolename { get; set; }
        public string LoweredUserName { get; set; }
        public string Mobile { get; set; }
        public decimal? BirthDay { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Url { get; set; }
        public bool? Gender { get; set; }
        public bool? IsLockedOut { get; set; }
        public decimal? FixedSalary { get; set; }
        public decimal? Total { get; set; }
        public decimal? StartDate { get; set; }
        public decimal? TotalPay { get; set; }
        public decimal? TotalReward { get; set; }
        public decimal? TotalReceip { get; set; }
        public bool IsOnline { get; set; }
        public decimal? TotalDisCount { get; set; }
        public int? EnterprisesID { get; set; }
        public int CountOrder { get; set; }
        public IEnumerable<DNUserOrderItem> Times { get; set; }
        public IEnumerable<DNLevelRoomItem> lstLevelRoom { get; set; }
        public bool IsBed { get; set; }
        public int AgencyID { get; set; }
        public string Comment { get; set; }
        public string CardSerial { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsApproved { get; set; }
        public string AgencyName { get; set; }
        public decimal? AgencyWallet { get; set; }
        public decimal? AgencyDeposit { get; set; }
        public string AgencyAddress { get; set; }
        public bool? IsAgency { get; set; }
        public bool? IsService { get; set; }
        public string CodeCheckIn { get; set; }
        public string CodeLogin { get; set; }
        public int? CustomerID { get; set; }
        public int? Sort { get; set; }
        public int AreaID { get; set; }
        public int MarketID { get; set; }
        public string CustomerName { get; set; }
        public bool IsActive { get; set; }

        public virtual IEnumerable<ModuleItem> DN_Module { get; set; }
        public virtual IEnumerable<DNUserModuleActiveItem> DN_User_ModuleActive { get; set; }
        public virtual IEnumerable<EditScheduleItem> DN_EDIT_Schedule { get; set; }
        public virtual IEnumerable<EditScheduleItem> DN_EDIT_Schedule1 { get; set; }
        public virtual IEnumerable<string> listRole { get; set; }
        public virtual IEnumerable<int> listRoleID { get; set; }
        public IEnumerable<int> ListModuleID { get; set; }
        public IEnumerable<string> ListModule { get; set; }
        public virtual IEnumerable<DNCalendarItem> ListCalendarItem { get; set; }
        public IEnumerable<DNGroupMailSSCItem> ListDNGroupMailSSCItem { get; set; }
    }

    public class DNUserAddItem
    {
        public Guid UserId { get; set; }
        public int? CustomerID { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public decimal? StartDate { get; set; }
        public decimal? BirthDay { get; set; }
        public string LoweredUserName { get; set; }
        public int? FixedSalary { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsAgency { get; set; }
        public bool? IsLockedOut { get; set; }
        public bool? Gender { get; set; }
        public bool? IsOut { get; set; }
        public string CodeCheckIn { get; set; }
        public string Comment { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public bool? IsService { get; set; }
    }

    public class DNUserAppItem
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public int AreaID { get; set; }
        public int MarketID { get; set; }
        public decimal? AgencyWallet { get; set; }
        public decimal? AgencyDeposit { get; set; }
        public string AgencyAddress { get; set; }
        public int AgencyID { get; set; }
        public string CodeLogin { get; set; }
        public int? EnterprisesID { get; set; }
        public int? GroupID { get; set; }
        public int Status { get; set; }
        public string Company { get; set; }
        public string MST { get; set; }
        public string STK { get; set; }
        public string BankName { get; set; }
        public string FullName { get; set; }
        public string Depart { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int? Gender { get; set; }
        public decimal? Birthday { get; set; }
        public string Phone { get; set; }
        public int? AgencyLevelId { get; set; }
        public bool? isLock { get; set; }
    }
    public class ListUsersItem
    {
        public Guid UserId { get; set; }
        public string Key { get; set; }

    }
    public class ListRolesItem
    {
        public Guid RoleId { get; set; }
        public string Key { get; set; }

    }

    public class DNUserCalendarItem
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string LoweredUserName { get; set; }
        public bool IsOnline { get; set; }
        public int Countdate { get; set; }
        public int? TotalLate { get; set; }
        public virtual IEnumerable<DateOffItem> DateOffItems { get; set; }
        public virtual IEnumerable<CDNTimeJobItem> DN_Time_Job { get; set; }
        public int? FixedSalary { get; set; }
        public virtual string NameRole { get; set; }
        public virtual IEnumerable<string> ListNameTree { get; set; }
        public virtual IEnumerable<CalendarItem> CalendarItems { get; set; }
        public virtual IEnumerable<RCalendarItem> RCalendarItems { get; set; }
        public IEnumerable<CEditScheduleItem> EditScheduleItems { get; set; }
        public virtual TotalSalaryMonthItem TotalSalaryMonthItem { get; set; }

    }

    public class DNUserRolesMonthDetail
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string LoweredUserName { get; set; }
        public string NameRole { get; set; }
        public string DepartmentName { get; set; }
        public virtual IEnumerable<CDNTimeJobItem> DN_Time_Job { get; set; }
        public virtual TotalSalaryMonthItem TotalSalaryMonthItem { get; set; }
        public virtual IEnumerable<CalendarItem> CalendarItems { get; set; }
        public virtual IEnumerable<RCalendarItem> RCalendarItems { get; set; }
    }

    public class ModelDNUserRolesSalaryMonthItem
    {
        public List<DNUserRolesMonthDetail> ListItems { get; set; }
        public DateTime DateStart { get; set; }
    }

    public class DNUserTotalItem
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string LoweredUserName { get; set; }
        public int CountDay { get; set; }
        public int TotalDay { get; set; }
        public virtual IEnumerable<string> ListNameTree { get; set; }
        public IEnumerable<CEditScheduleItem> EditScheduleItems { get; set; }
    }

    public class ModelDNUserCalendarItem
    {
        public virtual IEnumerable<DNUserCalendarItem> ListItems { get; set; }
        public virtual IEnumerable<WeeklyScheduleItem> WeeklyScheduleItems { get; set; }
        public virtual IEnumerable<DayOffItem> DayOffItems { get; set; }
        public DateTime DateStart { get; set; }
    }

    public class DNUserVoteItem
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string LoweredUserName { get; set; }
        public IEnumerable<string> ListTree { get; set; }
        public decimal? TotalValue { get; set; }
    }
    public class DNUserTimeJobItem
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string LoweredUserName { get; set; }
        public int CountDay { get; set; }
        public int Totale { get; set; }
        public int Totall { get; set; }
        public IEnumerable<DNTimeJobItem> DNTimeJobItems { get; set; }
        public virtual IEnumerable<CalendarItem> CalendarItems { get; set; }
        public virtual IEnumerable<CkeckEditScheduleItem> EditScheduleItems { get; set; }
        public virtual IEnumerable<DateOffItem> DateOffItem { get; set; }
    }
    public class ModelDNUserTimeJobItem
    {
        public IEnumerable<DayItem> DayItems { get; set; }
        public IEnumerable<DNUserTimeJobItem> ListItems { get; set; }
    }
    public class ModelDNUserVoteItem
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string LoweredUserName { get; set; }
        public IEnumerable<string> ListTree { get; set; }
        public decimal? TotalValue { get; set; }
    }

    public class ModelDNUserAddItem
    {
        public string Dates { get; set; }
        public string Datee { get; set; }
        public IEnumerable<DNUserVoteItem> ListItems { get; set; }
    }
    public class DNUserOrderItem
    {
        public int? BId { get; set; }
        public decimal? S { get; set; }
        public decimal? E { get; set; }
    }

    public class DNUserSimpleItem
    {
        public Guid? UserId { get; set; }
        public string UserName { get; set; }
        public string LoweredUserName { get; set; }
    }

    public class ModelDNUserItem : BaseModelSimple
    {
        public string UserName { get; set; }
        public Guid UserID { get; set; }
        public int Year { get; set; }
        public DNUserItem Item { get; set; }
        public decimal? TotalFixedSalary { get; set; }
        public IEnumerable<ActiveRoleItem> ActiveRoleItems { get; set; }
        public IEnumerable<DNUserItem> ListItem { get; set; }
        public IEnumerable<WeekItem> ListWeekItems { get; set; }
        public decimal? Total { get; set; }
        public decimal? TotalPay { get; set; }
        public decimal? TotalDiscount { get; set; }
    }
}
