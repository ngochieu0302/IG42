using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
    public class WalletsAppIG4Item : BaseSimple
    {
        public string Customername { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public decimal? WalletsCus { get; set; }
        public decimal? CashOutWallet { get; set; }
        public bool? IsDelete { get; set; }
        public decimal? Wallets { get; set; }

    }

    public class ModelWalletsAppIG4Item : BaseModelSimple
    {
        public IEnumerable<WalletsAppIG4Item> ListItems { get; set; }
    }

    public class CashOutWalletAppIG4Item:BaseSimple
    {
        public string Customername { get; set; }
        public decimal? DateCreate { get; set; }
        public int? CustomerId { get; set; }
        public decimal? Total { get; set; }
        public int? Type { get; set; }
        public int? TypeCash { get; set; }
        public string Query { get; set; }
    }

    public class WalletsAppAppIG4Item
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public decimal? Price { get; set; }
        public int? Type { get; set; }
    }
}
