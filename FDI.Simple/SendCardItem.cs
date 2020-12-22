using System.Collections.Generic;

namespace FDI.Simple
{
    public class SendCardItem : BaseSimple
    {
        public int? CustomerID { get; set; }
        public string CustomerName { get; set; }
        public decimal? DateCreate { get; set; }
    }

    public class CardCustomerItem : BaseSimple
    {
        public int? CardID { get; set; }
        public int? SendCardID { get; set; }
    }
    public class ModelSendCardItem : BaseModelSimple
    {
        public IEnumerable<SendCardItem> ListItems { get; set; }
    }
}
