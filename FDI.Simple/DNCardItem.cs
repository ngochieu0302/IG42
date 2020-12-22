using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class DNCardItem : BaseSimple
    {
        public string Code { get; set; }
        public string Serial { get; set; }
        public string PinCard { get; set; }
        public int? CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string ListID { get; set; }
        public int? AgencyId { get; set; }
        public int? Level { get; set; }
        public int? Status { get; set; }
        public decimal? CreateDate { get; set; }
    }
    public class ModelDNCardItem : BaseModelSimple
    {
        public IEnumerable<DNCardItem> ListItems { get; set; }
    }
}
