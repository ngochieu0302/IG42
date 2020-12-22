using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class WalletAPI : BaseAPI
    {
        public ModelWalletItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Wallets/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyid);
            return GetObjJson<ModelWalletItem>(urlJson);
        }
        public ModelWalletItem GetListCustomerByRequest(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Wallets/GetListCustomerByRequest{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyid);
            return GetObjJson<ModelWalletItem>(urlJson);
        }
        public List<WalletCustomerItem> GetListWalletCusByCustomer(int page, int id, int agencyId)
        {
            var urlJson = string.Format("{0}Wallets/GetListWalletCusByCustomer?key={1}&page={2}&id={3}&agencyId={4}", Domain, Keyapi, page, id, agencyId);
            return GetObjJson<List<WalletCustomerItem>>(urlJson);
        }
        public List<CashOutWalletItem> GetListWalletCashByCustomer(int page, int id, int agencyId)
        {
            var urlJson = string.Format("{0}Wallets/GetListWalletCashByCustomer?key={1}&page={2}&id={3}&agencyId={4}", Domain, Keyapi, page, id, agencyId);
            return GetObjJson<List<CashOutWalletItem>>(urlJson);
        }
        public List<WalletOrderHistoryItem> GetListWalletOrderByCustomer(int page, int id, int agencyId)
        {
            var urlJson = string.Format("{0}Wallets/GetListWalletOrderByCustomer?key={1}&page={2}&id={3}&agencyId={4}", Domain, Keyapi, page, id, agencyId);
            return GetObjJson<List<WalletOrderHistoryItem>>(urlJson);
        }
    }
}
