using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class DNExportProductItem : BaseSimple
    {
        public string Name { get; set; }
        public int? AgencyId { get; set; }
        public string Code { get; set; }
        public decimal? DateCreated { get; set; }
        public string Note { get; set; }
        public decimal? TotalPrice { get; set; }
        public Guid? UserID { get; set; }
        public Guid? UserGet { get; set; }
        public string UserName { get; set; }
        public string UserGetName { get; set; }
        public decimal? DateExport { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsOrder { get; set; }
        public virtual DNUserItem DN_Users { get; set; }
        public virtual IEnumerable<ExportProductItem> ExportProduct { get; set; }
    }
    public class ModelDNExportProductItem : BaseModelSimple
    {
        public virtual IEnumerable<DNExportProductItem> ListItems { get; set; }
    }
}
