using System.Collections.Generic;
using FDI.Simple;

namespace FDI.Simple
{
    public class DNComboItem : BaseSimple
    {
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public decimal? DateStart { get; set; }
        public decimal? DateEnd { get; set; }
        public string IsDeleted { get; set; }
        public decimal? Percent { get; set; }
        public int? AgencyId { get; set; }
        public int? PictureId { get; set; }
        public string UrlPicture { get; set; }
        public IEnumerable<ProductItem> LstProducts { get; set; }
    }

    public class ModelDNComboItem : BaseModelSimple
    {
        public IEnumerable<DNComboItem> ListItem { get; set; }
    }
}
