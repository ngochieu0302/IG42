using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;

namespace FDI.Simple
{
    public class WalletItem : BaseSimple
    {
        public int? CustomerID { get; set; }
        public decimal? WalletOrder { get; set; }
        public decimal? WalletCus { get; set; }
        public decimal? CashOut { get; set; }
        public bool? IsDelete { get; set; }
        public int? AgencyId { get; set; }
        public string CustomerName { get; set; }
        public string Customerphone { get; set; }
        public string Username { get; set; }
        public virtual Customer Customer { get; set; }
    }
    public class ModelWalletItem : BaseModelSimple
    {
        public IEnumerable<WalletItem> ListItems { get; set; }
        public string DateMonth { get; set; }
        public string DateYear { get; set; }
        public decimal? TotalWalletOrder { get; set; }
        public decimal? TotalWalletCustomer { get; set; }
    }
}
