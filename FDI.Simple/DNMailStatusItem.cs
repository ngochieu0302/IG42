using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class DNMailStatusItem : BaseSimple
    {
        public int? CustomerId { get; set; }
        public int? MailId { get; set; }
        public bool? Status { get; set; }
        
    }
}
