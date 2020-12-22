using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class CashAdvanceItem : BaseSimple
    {
        public Guid? UserID { get; set; }
        public Guid? UserActive { get; set; }
        public Guid? UserCashier { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string UsernameActive { get; set; }
        public string UserName { get; set; }
        public string UserNameCashier { get; set; }
        public string FullNameCashier { get; set; }
        public string FullNameActive { get; set; }
        public string FullNameReceipt{ get; set; }
        public decimal? DateCreated { get; set; }
        public decimal? DateReturn { get; set; }
        public decimal? DateActive { get; set; }
        public int? PaymentMethodId { get; set; }
        public string CustomerName { get; set; }
        public decimal? Price { get; set; }
        public string Note { get; set; }
        public int? AgencyId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }

        public decimal? TotalCash { get; set; }
        public decimal? TotalRepay { get; set; }
    }
    public class FormCashAdvanceItem : BaseFormSimple
    {
        public CashAdvanceItem ObjItem { get; set; }
        public bool IsAdmin { get; set; }
        public int AgencyId { get; set; }
        public Guid UserId { get; set; }
        public List<DNUserItem> Users { get; set; }
        public List<DNUserItem> Users1 { get; set; }
    }
    public class ModelCashAdvanceItem : BaseModelSimple
    {
        public IEnumerable<CashAdvanceItem> ListItem { get; set; }
        public Guid UserActive { get; set; }
        public decimal? TotalPrice { get; set; }
        public bool IsAdmin { get; set; }
        public decimal? TotalActive { get; set; }
        public decimal? TotalDelete { get; set; }
    }
}
