using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
    public class TemplateDocumentItem:BaseSimple
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public int? Type { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsDelete { get; set; }
        public decimal? DateCreate { get; set; }
    }

    public class ModelTemplateDocumentItem : BaseModelSimple
    {
        public IEnumerable<TemplateDocumentItem> ListItems { get; set; }
    }
}
