using System;

namespace FDI.Simple
{
    [Serializable]
    public class HtmlMapItem : BaseSimple
    {
        public int? IdHtml { get; set; }
        public int? IdModule { get; set; }
        public int? IdCopy { get; set; }
        public string LanguageId { get; set; }
        public string Value { get; set; }
    }

    public class ModelHtmlMapItem : BaseModelSimple
    {
        public HtmlMapItem HtmlMapItem { get; set; }
    }

}
