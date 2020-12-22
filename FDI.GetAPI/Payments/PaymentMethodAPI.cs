using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class PaymentMethodAPI:BaseAPI
    {
        private readonly string _url = Domain;
        public ModelPaymentMethodItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}PaymentMethod/ListItems{1}&key={2}", _url, url, Keyapi);
            return GetObjJson<ModelPaymentMethodItem>(urlJson);
        }
        public List<PaymentMethodItem> GetAll()
        {
            var urlJson = string.Format("{0}PaymentMethod/GetAll?key={1}", _url, Keyapi);
            return GetObjJson<List<PaymentMethodItem>>(urlJson);
        }

        public PaymentMethodItem GetItemById(int agencyid, int id = 0)
        {
            var urlJson = string.Format("{0}PaymentMethod/GetItemById?key={1}&id={2}", _url, Keyapi,  id);
            return GetObjJson<PaymentMethodItem>(urlJson);
        }
        public JsonMessage Add(string json)
        {
            var urlJson = string.Format("{0}PaymentMethod/Add?key={1}&json={2}", _url, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }

        public JsonMessage Update(string json, int id = 0)
        {
            var urlJson = string.Format("{0}PaymentMethod/Update?key={1}&json={2}&id={3}", _url, Keyapi, json, id);
            return GetObjJson<JsonMessage>(urlJson);
        }

        public JsonMessage Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}PaymentMethod/Delete?key={1}&listint={2}", _url, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
    }
}
