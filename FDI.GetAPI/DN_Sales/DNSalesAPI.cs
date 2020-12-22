using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI.DN_Sales
{
    public class DNSalesAPI : BaseAPI
    {
        public ModelDNSalesItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}DNSales/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyid);
            return GetObjJson<ModelDNSalesItem>(urlJson);
        }
        public ModelSaleCodeItem ListItemsCode(int agencyid, string url, int id)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}DNSales/ListItemsCode{1}&key={2}&agencyId={3}&id={4}", Domain, url, Keyapi, agencyid, id);
            return GetObjJson<ModelSaleCodeItem>(urlJson);
        }
        public DNSalesItem GetDNSalesItem(int id)
        {
            var urlJson = string.Format("{0}DNSales/GetDNSalesItem?key={1}&Id={2}", Domain, Keyapi, id);
            return GetObjJson<DNSalesItem>(urlJson);
        }
        public SaleCodeItem GetDNSalesItembyCode(string code, int agencyId)
        {
            var urlJson = string.Format("{0}DNSales/GetDNSalesItembyCode?key={1}&code={2}&agencyId={3}", Domain, Keyapi, code, agencyId);
            return GetObjJson<SaleCodeItem>(urlJson);
        }
        public JsonMessage Add(string json, int agencyid, Guid userId)
        {
            var urlJson = string.Format("{0}DNSales/Add?key={1}&{2}&agencyId={3}&userId={4}", Domain, Keyapi, json, agencyid, userId);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Update(string json, Guid userId)
        {
            var urlJson = string.Format("{0}DNSales/Update?key={1}&{2}&userId={3}", Domain, Keyapi, json, userId);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}DNSales/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
    }
}
