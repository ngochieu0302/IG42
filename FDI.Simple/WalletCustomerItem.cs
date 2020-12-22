using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;

namespace FDI.Simple
{
    public class WalletCustomerItem : BaseSimple
    {
        public int? CustomerID { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? DateCreate { get; set; }
        public int? AgencyId { get; set; }
        public bool? IsDelete { get; set; }
        public string Name { get; set; }
        public string CMTND { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
    public class ModelWalletCustomerItem : BaseModelSimple
    {
        public IEnumerable<WalletCustomerItem> ListItem { get; set; }
        public WalletCustomer CustomerItem { get; set; }
    }
}
