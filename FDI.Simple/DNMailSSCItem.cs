using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class DNMailSSCItem : BaseSimple
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public decimal? CreateDate { get; set; }
        public decimal? UpdateDate { get; set; }
        public int? Status { get; set; }
        public int? Type { get; set; }
        public string ListUserReceiveIds { get; set; }
        public Guid? UserSendId { get; set; }
        public Guid? UserReceiveId { get; set; }
        public int? CustomerSendId { get; set; }
        public int? CustomerReceiveId { get; set; }
        public string CustomerSendName { get; set; }
        public string CustomerReceiveName { get; set; }
        public int? AgencyID { get; set; }
        public string UserSendName { get; set; }
        public string UserReceiveName { get; set; }
        public string UserSendEmail { get; set; }
        public string UserReceiveEmail { get; set; }
        public string TimeAgo { get; set; }
        public bool? StatusEmail { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsDraft { get; set; }
        public bool? IsSpam { get; set; }
        public bool? IsRecycleBin { get; set; }
        public bool? IsImportant { get; set; }
        public string ListID { get; set; }
        public string ListUrlPicture { get; set; }
        public IEnumerable<DNFileMailItem> ListDNFileMailItem { get; set; }
        public IEnumerable<CustomerItem> ListCustomerItem { get; set; }
    }
    public class ModelDNMailSSCItem : BaseModelSimple
    {
        public IEnumerable<DNMailSSCItem> ListItem { get; set; }
        public int? TotalMailInbox { get; set; }
        public int? TotalMailDrafts { get; set; }
        public int? TotalMailSpam { get; set; }
        public int? TotalNewsSsc { get; set; }
        public string UserReceiveId { get; set; }
        public int? CustomerSendId { get; set; }
        public int? CustomerReceiveId { get; set; }
        public int? Type { get; set; }
        public int? UserId { get; set; }
        public IEnumerable<DNUserItem> ListDNUserItem { get; set; }
        public IEnumerable<CustomerItem> ListCustomerItem { get; set; }
        public IEnumerable<DNGroupMailSSCItem> ListDNGroupMailSSCItem { get; set; }
    }

    public class MailContent
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }

    public class MailType
    {
        public int ID { get; set; }
        public string ListID { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsDraft { get; set; }
        public bool? IsSpam { get; set; }
        public bool? IsRecycleBin { get; set; }
        public bool? IsImportant { get; set; }
    }
}
