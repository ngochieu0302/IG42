using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class AdvertisingPositionAPI : BaseAPI
    {
        public ModelAdvertisingPositionItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}AdvertisingPosition/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi,agencyid);
            return GetObjJson<ModelAdvertisingPositionItem>(urlJson);
        }

        public List<AdvertisingPositionItem> GetAll()
        {
            var urlJson = string.Format("{0}AdvertisingPosition/GetAll?key={1}", Domain, Keyapi);
            return GetObjJson<List<AdvertisingPositionItem>>(urlJson);
        }

        public AdvertisingPositionItem GetItemById(int id)
        {
            var urlJson = string.Format("{0}AdvertisingPosition/GetItemById?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<AdvertisingPositionItem>(urlJson);
        }

        public int Add(string json)
        {
            var urlJson = string.Format("{0}AdvertisingPosition/Add?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
        public int Update(string json)
        {
            var urlJson = string.Format("{0}AdvertisingPosition/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
        public int Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}AdvertisingPosition/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }
    }
}