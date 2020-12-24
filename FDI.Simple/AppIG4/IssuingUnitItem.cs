using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
    public class IssuingUnitItem : BaseSimple
    {
        public string Name { get; set; }
        public string Descrition { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsDeleted { get; set; }
        public int? PictureID { get; set; }
        public string UrlPicture { get; set; }
        public int? TotalProduct { get; set; }
        public string Slug { get; set; }
        public bool? IsHot { get; set; }
        public IEnumerable<ProductItem> LstProductItems { get; set; }
    }
    public class ModelIssuingUnitItem : BaseModelSimple
    {

        public IEnumerable<IssuingUnitItem> ListItems { get; set; }
        public string keyword { get; set; }
        public string QueryString { get; set; }
    }
}
