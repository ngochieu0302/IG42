using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI.DN_Sales
{
    public class DNPromotionAPI:BaseAPI
    {
        public ModelDNPromotionItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}DNPromotion/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyid);
            return GetObjJson<ModelDNPromotionItem>(urlJson);
        }
        public DNPromotionItem GetDNPromotionItem(int id)
        {
            var urlJson = string.Format("{0}DNPromotion/GetDNPromotionItem?key={1}&Id={2}", Domain, Keyapi, id);
            return GetObjJson<DNPromotionItem>(urlJson);
        }
        public JsonMessage Add(string json, int agencyid,Guid userId,string codelogin)
        {
            var urlJson = string.Format("{0}DNPromotion/Add?key={1}&{2}&agencyId={3}&userId={4}&codeLogin={5}", Domain, Keyapi, json, agencyid, userId, codelogin);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Update(string json, Guid userId,string codelogin)
        {
            var urlJson = string.Format("{0}DNPromotion/Update?key={1}&{2}&userId={3}&codeLogin={4}", Domain, Keyapi, json, userId, codelogin);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}DNPromotion/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
    }
}
