using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
    public class WalletCustomerAppIG4Item : BaseSimple
    {
        public string CustomerName { get; set; }
        public string Code { get; set; }
        public string TransactionNo { get; set; }
        public bool? IsActive { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public decimal? DateCreate { get; set; }
        public int? CustomerID { get; set; }
        public decimal? Totalprice { get; set; }
        public string Note { get; set; }
        public int? Type { get; set; }
        public int? TypeWalet { get; set; }
    }

    public class ModelWalletCustomerAppIG4Item : BaseModelSimple
    {
        public IEnumerable<WalletCustomerAppIG4Item> ListItems { get; set; }
        public CustomerItem CustomerItem { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}
