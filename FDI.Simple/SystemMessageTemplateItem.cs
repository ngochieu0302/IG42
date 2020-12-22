using System.Collections.Generic;

namespace FDI.Simple
{
    public class SystemMessageTemplateItem : BaseSimple
    {
        public string Name { get; set; }
        public string Subject { get; set; }
        public string BodyContent { get; set; }
        public string BccEmail { get; set; }
        public bool? IsActive { get; set; }
        public int? EmailAccountId { get; set; }
        public bool? IsDeleted { get; set; }
        public int? Type { get; set; }
        public string EmailSend { get; set; }
        public string PassEmailSend { get; set; }
        public string EmailReceive { get; set; }
        public string CcEmail { get; set; }
    }
    public class ModelSystemMessageTemplateItem : BaseModelSimple
    {
        public IEnumerable<SystemMessageTemplateItem> ListItem { get; set; }
    }
}
