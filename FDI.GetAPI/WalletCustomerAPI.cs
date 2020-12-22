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
    public class WalletCustomerAPI : BaseAPI
    {
        private readonly string _url = Domain;
        public ModelWalletCustomerItem ListItems(int agencyId, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}WalletCustomer/ListItems{1}&key={2}&agencyId={3}", _url, url, Keyapi, agencyId);
            return GetObjJson<ModelWalletCustomerItem>(urlJson);
        }
        public ModelWalletCustomerItem GetListById(int agencyId, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}WalletCustomer/GetListById{1}&key={2}&agencyId={3}", _url, url, Keyapi, agencyId);
            return GetObjJson<ModelWalletCustomerItem>(urlJson);
        }

        public WalletCustomer GetCustomerItem(int id)
        {
            var urlJson = string.Format("{0}WalletCustomer/GetCustomerItem?key={1}&id={2}", _url, Keyapi, id);
            return GetObjJson<WalletCustomer>(urlJson);
        }

        public JsonMessage Add(string json)
        {
            var urlJson = string.Format("{0}WalletCustomer/Add?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public int AddTranfer(decimal tranfer, int agencyId, int id)
        {
            var urlJson = string.Format("{0}WalletCustomer/AddTranfer?key={1}&moneyTranfer={2}&agencyId={3}&customerId={4}", Domain, Keyapi, tranfer, agencyId, id);
            return GetObjJson<int>(urlJson);
        }
        public JsonMessage Update(string json)
        {
            var urlJson = string.Format("{0}WalletCustomer/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}WalletCustomer/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
    }
}
