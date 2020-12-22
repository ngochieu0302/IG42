using System;

namespace FDI.Simple
{
    [Serializable]
    public class BaseSimple
    {
        public int ID { get; set; }
    }

    public class BaseModelSimple
    {
        public string PageHtml { get; set; }
        public string Container { get; set; }
        public int PageId { get; set; }
        public string CtrUrl { get; set; }
        public int CtrId { get; set; }
        public string Action { get; set; }
        public string ActionText { get; set; }
    }
    public class BaseFormSimple
    {
        public string Action { get; set; }
        public int ID { get; set; }
    }
}
