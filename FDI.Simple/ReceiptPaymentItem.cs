using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class ReceiptPaymentItem : BaseSimple
    {
        public Guid? UserID { get; set; }

        public string Name { get; set; }
        public string Code { get; set; }
        public Guid? UserCashier { get; set; }
        public string UserNameCashier { get; set; }
        public string UserName { get; set; }
        public string FullNameCashier { get; set; }
        public string FullNameReceipt { get; set; }
        public decimal? DateCreated { get; set; }
        public decimal? DateReturn { get; set; }
        public decimal? DateActive { get; set; }
        public string CostTypeName { get; set; }
        public int? CostTypeID { get; set; }
        public int? PaymentMethodId { get; set; }
        public string CustomerName { get; set; }
        public decimal? Price { get; set; }
        public decimal? TotalReceip { get; set; }
        public decimal? TotalPayment { get; set; }
        public decimal? TotalCashAdvance { get; set; }
        public decimal? TotalOrder { get; set; }
        public string Note { get; set; }
        public int? AgencyId { get; set; }
        public int? OrderId { get; set; }
        public int? Type { get; set; }
        public int? Status { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
    }
    public class ModelReceiptPaymentItem : BaseModelSimple
    {
        public List<ReceiptPaymentItem> ListItem { get; set; }
        public Guid UserActive { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? TotalActive { get; set; }
        public decimal? TotalDelete { get; set; }
        public DateTime ToDate { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class FormReceiptPaymentItem : BaseModelSimple
    {
        public ReceiptPaymentItem ObjItem { get; set; }
        public bool IsAdmin { get; set; }
        public int AgencyId { get; set; }
        public Guid UserId { get; set; }
        public List<DNUserItem> Users { get; set; }
        public List<CostTypeItem> CostTypeItems { get; set; }
    }
    public class GeneralUserItem
    {
        public Guid? UserId { get; set; }
        public decimal? UngMonney { get; set; }
        public decimal? TraUngMoney { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? TotalAward { get; set; }
        public decimal? TotalAwardKH { get; set; }
    }   
}
