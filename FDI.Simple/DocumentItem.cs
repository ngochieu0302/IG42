using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class DocumentItem : BaseSimple
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string NoiBanHanh { get; set; }
        public string NguoiKy { get; set; }
        public bool? IsShow { get; set; }
        public int? DocumentLevelId { get; set; }
        public string LevelName { get; set; }
        public int? DocumentRoomId { get; set; }
        public string RoomName { get; set; }
        public int? CabinetID { get; set; }
        public string CabinetName { get; set; }
        public int? DrawerID { get; set; }
        public string DrawerName { get; set; }
        public decimal? CreatedDate { get; set; }
        public string Code { get; set; }
        public string NameB { get; set; }
        public string Address { get; set; }
        public string MST { get; set; }
        public string STK { get; set; }
        public string BankName { get; set; }
        public string Department { get; set; }
        public decimal? Value { get; set; }
        public string NameCompany { get; set; }
        public bool? Isbill { get; set; }
        public string Numberbill  {get; set; }
        public string MobileB { get; set; }
        public decimal? Deposit { get; set; }
        public bool? IsCancel { get; set; }
        public string Usercancel { get; set; }
        public int? AgencyId { get; set; }
        public Guid? UserId { get; set; }
        public int? SupId { get; set; }
        public decimal? DateStart { get; set; }
        public decimal? DateEnd { get; set; }
        public int? Type { get; set; }
        public string UserCreate { get; set; }
        public bool? Status { get; set; }
        public string FileUrl { get; set; }
        public IEnumerable<DocumentFilesItem> DocumentFilesItem { get; set; }
    }

    public class ModelDocumentItem : BaseModelSimple
    {
        public IEnumerable<DNDrawerItem> ListDrawerItems { get; set; }
        public IEnumerable<DocumentItem> ListItem { get; set; }
    }

    public class StaticDocumentItem
    {
        public int Month { get; set; }
        public decimal? TotalValue { get; set; }
        public int? TotalDoc { get; set; }
        public decimal? TotalDeposit { get; set; }
    }
}

