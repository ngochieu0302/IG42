using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;

namespace FDI.Simple
{
    public class WalletOrderHistoryItem : BaseSimple
    {
        public int? CustomerID { get; set; }
        public int? OrderID { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? DateCreate { get; set; }
        public int? AgencyId { get; set; }
        public bool? IsDelete { get; set; }
        public virtual Customer Customer { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string CMTND { get; set; }
        public string Phone { get; set; }
        public string Product { get; set; }
    }
    public class ModelWalletOrderHistoryItem : BaseModelSimple
    {
        public IEnumerable<WalletOrderHistoryItem> ListItem { get; set; }
        public WalletOrder_History OrderHistoryItem { get; set; }
    }
}
