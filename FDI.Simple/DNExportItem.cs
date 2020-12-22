using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
    public class DNExportItem : BaseSimple
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
        public virtual IEnumerable<ExportProductValueItem> Export_Product_Value { get; set; }
    }
    public class ModelDNExportItem : BaseModelSimple
    {
        public virtual IEnumerable<DNExportItem> ListItems { get; set; }
    }
}
