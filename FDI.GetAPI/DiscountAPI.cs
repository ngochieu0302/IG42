using FDI.Simple;
using FDI.Utils;
using System.Collections.Generic;

namespace FDI.GetAPI
{
    public class DiscountAPI:BaseAPI
    {
        private readonly string _url = Domain;
        public DiscountItem GetItemById(int id)
        {
            var urlJson = string.Format("{0}Discount/GetItemById?key={1}&id={2}", _url, Keyapi, id);
            return GetObjJson<DiscountItem>(urlJson);
        }

        public List<DiscountItem> GetDiscountItem(int type, int agencyid)
        {
            var urlJson = string.Format("{0}Discount/GetDiscountItem?key={1}&type={2}&agencyid={3}", _url, Keyapi, type, agencyid);
            return GetObjJson<List<DiscountItem>>(urlJson);
        } 
        public List<DiscountItem> GetByAllKHDiscount()
        {
            var urlJson = string.Format("{0}Discount/GetByAllKHDiscount?key={1}", _url, Keyapi);
            return GetObjJson<List<DiscountItem>>(urlJson);
        }
        public ModelDiscountItem ListItems(string url, int agencyid)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Discount/ListItems{1}&key={2}&agencyid={3}", Domain, url, Keyapi, agencyid);
            return GetObjJson<ModelDiscountItem>(urlJson);
        }

        public JsonMessage Add(string json, int agencyid)
        {
            var urlJson = string.Format("{0}Discount/Add?key={1}&{2}&agencyid={3}", Domain, Keyapi, json, agencyid);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Update(string json)
        {
            var urlJson = string.Format("{0}Discount/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Delete(string json)
        {
            var urlJson = string.Format("{0}Discount/Delete?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
    }
}
