using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class WalletOrderAPI : BaseAPI
    {
        private readonly string _url = Domain;
        public ModelWalletOrderHistoryItem ListItems(int agencyId, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}WalletOrder/ListItems{1}&key={2}&agencyId={3}", _url, url, Keyapi, agencyId);
            return GetObjJson<ModelWalletOrderHistoryItem>(urlJson);
        }
        public ModelWalletOrderHistoryItem GetListById(int agencyId, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}WalletOrder/GetListById{1}&key={2}&agencyId={3}", _url, url, Keyapi, agencyId);
            return GetObjJson<ModelWalletOrderHistoryItem>(urlJson);
        }
        public WalletOrder_History GetCustomerItem(int id)
        {
            var urlJson = string.Format("{0}WalletOrder/GetCustomerItem?key={1}&id={2}", _url, Keyapi, id);
            return GetObjJson<WalletOrder_History>(urlJson);
        }

        public JsonMessage Add(string json)
        {
            var urlJson = string.Format("{0}WalletOrder/Add?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Update(string json)
        {
            var urlJson = string.Format("{0}WalletOrder/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}WalletOrder/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
    }
}
