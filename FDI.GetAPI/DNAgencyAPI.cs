using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class DNAgencyAPI : BaseAPI
    {
        private readonly string _url = Domain;
        public AgencyItem GetItemById(int id)
        {
            var urlJson = string.Format("{0}DNAgency/GetItemById?key={1}&id={2}", _url, Keyapi, id);
            return GetObjJson<AgencyItem>(urlJson);
        }
        public AgencyItem GetItemByStatic(int id)
        {
            var urlJson = string.Format("{0}DNAgency/GetItemByStatic?key={1}&id={2}", _url, Keyapi, id);
            return GetObjJson<AgencyItem>(urlJson);
        }
        public List<SuggestionsProduct> GetListAuto(string keword, int showLimit, int agencyId)
        {
            var urlJson = string.Format("{0}DNAgency/GetListAuto?key={1}&keword={2}&showLimit={3}&agencyId={4}", _url, Keyapi, keword, showLimit, agencyId);
            return GetObjJson<List<SuggestionsProduct>>(urlJson);
        }
        public ModelAgencyItem ListItems(int enterprisesId, string url,int areaId)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}DNAgency/ListItems{1}&key={2}&enterprisesId={3}&areaId={4}", Domain, url, Keyapi, enterprisesId, areaId);
            return GetObjJson<ModelAgencyItem>(urlJson);
        }
        public ModelAgencyItem ListItemsStatic(int enterprisesId, string url, int areaId)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}DNAgency/ListItemsStatic{1}&key={2}&enterprisesId={3}&areaId={4}", Domain, url, Keyapi, enterprisesId, areaId);
            return GetObjJson<ModelAgencyItem>(urlJson);
        }
        public List<AgencyItem> GetAll(int enterprisesId)
        {
            var urlJson = string.Format("{0}DNAgency/GetAll?key={1}&enterprisesId={2}", Domain, Keyapi, enterprisesId);
            return GetObjJson<List<AgencyItem>>(urlJson);
        }
        public List<AgencyItem> GetByCustomer(int customerId)
        {
            var urlJson = string.Format("{0}DNAgency/GetByCustomer?key={1}&customerId={2}", Domain, Keyapi, customerId);
            return GetObjJson<List<AgencyItem>>(urlJson);
        }
        public JsonMessage Add(int enterprisesId, string json)
        {
            var urlJson = string.Format("{0}DNAgency/Add?key={1}&enterprisesId={2}&json={3}", Domain, Keyapi, enterprisesId, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Addapp(string json)
        {
            var urlJson = string.Format("{0}DNAgency/Addapp?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }

        public JsonMessage Update(int enterprisesId, string json)
        {
            var urlJson = string.Format("{0}DNAgency/Update?key={1}&enterprisesId={2}&json={3}", Domain, Keyapi, enterprisesId, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage LockAgency(string lst)
        {
            var urlJson = string.Format("{0}DNAgency/LockAgency?key={1}&lstInt={2}", Domain, Keyapi,lst);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage UnLockAgency(string lst)
        {
            var urlJson = string.Format("{0}DNAgency/UnLockAgency?key={1}&lstInt={2}", Domain, Keyapi,lst);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Delete(int enterprisesId, string listint)
        {
            var urlJson = string.Format("{0}DNAgency/Delete?key={1}&enterprisesId={2}&listint={3}", Domain, Keyapi, enterprisesId, listint);
            return GetObjJson<JsonMessage>(urlJson);
        }
    }
}
