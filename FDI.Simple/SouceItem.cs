using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;

namespace FDI.Simple
{
    public class SourceItem:BaseSimple
    {
        public string Name { get; set; }
        public decimal? DateCreate { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public string Code { get; set; }
        public int? AgencyID { get; set; }
        public int? StorageProductID { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsDeleted { get; set; }
        public string SupplierName { get; set; }
        public StorageProductItem StorageProduct { get; set; }
        public IEnumerable<GalleryVideoItem> SourceVideo { get; set; }
        public GalleryPictureItem GalleryPictureItem { get; set; }
        public string PictureUrl { get; set; }
    }

    public class ModelSourceItem : BaseModelSimple
    {
        public IEnumerable<SourceItem> ListItems  { get; set; }
    }
}
