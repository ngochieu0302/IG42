using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class CustomerContactItem : BaseSimple
    {

        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsShow { get; set; }
        public bool IsDelete { get; set; }
        public int? TypeContact { get; set; }
        public DateTime? CreatedOnUtc { get; set; }
        public bool? Status { get; set; }
    }
    public class ModelCustomerContactItem : BaseModelSimple
    {
        public IEnumerable<CustomerContactItem> ListItem { get; set; }
        public int TotalNotifications { get; set; }
        public int TotalNotificationsContact { get; set; }
        public int TotalNotificationsRegisterContact { get; set; }
        public string TypeContact { get; set; }
        public string TypeRegisterContact { get; set; }
    }
}
