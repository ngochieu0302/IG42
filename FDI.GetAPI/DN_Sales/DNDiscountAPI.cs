using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI.DN_Sales
{
    public class DNDiscountAPI :BaseAPI
    {
        public ModelDNDiscountItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}DNDiscount/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyid);
            return GetObjJson<ModelDNDiscountItem>(urlJson);
        }
        public DNDiscountItem GetDNDiscountItem(int id)
        {
            var urlJson = string.Format("{0}DNDiscount/GetDNDiscountItem?key={1}&Id={2}", Domain, Keyapi, id);
            return GetObjJson<DNDiscountItem>(urlJson);
        }
        public JsonMessage Add(string json,  Guid userId)
        {
            var urlJson = string.Format("{0}DNDiscount/Add?key={1}&{2}&userId={3}", Domain, Keyapi, json,  userId);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Update(string json, Guid userId)
        {
            var urlJson = string.Format("{0}DNDiscount/Update?key={1}&{2}&userId={3}", Domain, Keyapi, json, userId);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}DNDiscount/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
    }
}
