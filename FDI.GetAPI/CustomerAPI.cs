using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class CustomerAPI : BaseAPI
    {
        private readonly string _url = Domain;
        private readonly string _urlSv = DomainSv;
        public List<DNCardItem> GetListByCustomer(int customerId)
        {
            var urlJson = string.Format("{0}Customer/GetListByCustomer?key={1}&customerId={2}", _url, Keyapi, customerId);
            return GetObjJson<List<DNCardItem>>(urlJson);
        }
        public List<CustomerItem> GetList()
        {
            var urlJson = string.Format("{0}Customer/GetList?key={1}", _url, Keyapi);
            return GetObjJson<List<CustomerItem>>(urlJson);
        }
        public List<CustomerItem> GetListByParent(int parentId)
        {
            var urlJson = string.Format("{0}Customer/GetListByParent?key={1}&parentId={2}", _url, Keyapi, parentId);
            return GetObjJson<List<CustomerItem>>(urlJson);
        }
        public ModelCustomerItem ListItems(int agencyId, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Customer/ListItems{1}&key={2}&agencyId={3}", _url, url, Keyapi, agencyId);
            return GetObjJson<ModelCustomerItem>(urlJson);
        }

        public ModelCustomerItem GetDiscountRequest(string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Customer/GetDiscountRequest{1}&key={2}", _url, url, Keyapi);
            return GetObjJson<ModelCustomerItem>(urlJson);
        }

        public List<TreeViewItem> GetListTree(int id, int lv, int agencyId)
        {
            var urlJson = string.Format("{0}Customer/GetListTree?key={1}&id={2}&lv={3}&agencyId={4}", Domain, Keyapi, id, lv, agencyId);
            return GetObjJson<List<TreeViewItem>>(urlJson);
        }

        public List<SuggestionsProduct> GetListAuto(string keword, int agencyId)
        {
            var urlJson = string.Format("{0}Customer/GetListAutoFashion?key={1}&keword={2}&agencyId={3}", _url, Keyapi, keword, agencyId);
            return GetObjJson<List<SuggestionsProduct>>(urlJson);
        }
        public List<SuggestionsProduct> GetListAuto(string keword, int showLimit, int agencyId)
        {
            var urlJson = string.Format("{0}Customer/GetListAuto?key={1}&keword={2}&showLimit={3}&agencyId={4}", _urlSv, Keyapi, keword, showLimit, agencyId);
            return GetObjJson<List<SuggestionsProduct>>(urlJson);
        }
        public CustomerItem GetCustomerItem(int id)
        {
            var urlJson = string.Format("{0}Customer/GetCustomerItem?key={1}&id={2}", _url, Keyapi, id);
            return GetObjJson<CustomerItem>(urlJson);
        }
        public CustomerItem GetCustomerBySerial(string serial)
        {
            var urlJson = string.Format("{0}Customer/GetCustomerBySerial?key={1}&serial={2}", _url, Keyapi, serial);
            return GetObjJson<CustomerItem>(urlJson);
        }
        public CustomerItem GetCustomerItemBySerial(string serial)
        {
            var urlJson = string.Format("{0}Customer/GetCustomerItemBySerial?key={1}&serial={2}", _url, Keyapi, serial);
            return GetObjJson<CustomerItem>(urlJson);
        }
        public List<CustomerItem> GetAll()
        {
            var urlJson = string.Format("{0}Customer/GetAll?key={1}", _url, Keyapi);
            return GetObjJson<List<CustomerItem>>(urlJson);
        }
        public List<CityItem> GetListCity()
        {
            var urlJson = string.Format("{0}Customer/GetListCity?key={1}", Domain, Keyapi);
            return GetObjJson<List<CityItem>>(urlJson);
        }
        public List<DistrictItem> GetListDistrict(int cityId)
        {
            var urlJson = string.Format("{0}Customer/GetListDistrict?key={1}&cityId={2}", Domain, Keyapi, cityId);
            return GetObjJson<List<DistrictItem>>(urlJson);
        }
        public int CheckParent(string txt)
        {
            var urlJson = string.Format("{0}Customer/CheckParent?key={1}&txt={2}", Domain, Keyapi, txt);
            return GetObjJson<int>(urlJson);
        }
        public int CheckCardSerial(string sirial)
        {
            var urlJson = string.Format("{0}Customer/CheckCardSerial?key={1}&sirial={2}", Domain, Keyapi, sirial);
            return GetObjJson<int>(urlJson);
        }
        public int CheckUserName(string txt, int id)
        {
            var urlJson = string.Format("{0}Customer/CheckUserName?key={1}&txt={2}&id={3}", Domain, Keyapi, txt, id);
            return GetObjJson<int>(urlJson);
        }
        public int CheckEmail(string txt, int id)
        {
            var urlJson = string.Format("{0}Customer/CheckEmail?key={1}&txt={2}&id={3}", Domain, Keyapi, txt, id);
            return GetObjJson<int>(urlJson);
        }
        public int CheckPhone(string txt, int id)
        {
            var urlJson = string.Format("{0}Customer/CheckPhone?key={1}&txt={2}&id={3}", Domain, Keyapi, txt, id);
            return GetObjJson<int>(urlJson);
        }
        public JsonMessage AddCard(int id, string card, string code)
        {
            var urlJson = string.Format("{0}Customer/AddCard?key={1}&id={2}&card={3}&code={4}", Domain, Keyapi, id, card, code);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Add(string json)
        {
            var urlJson = string.Format("{0}Customer/Add?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Update(string json)
        {
            var urlJson = string.Format("{0}Customer/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage UpdateCustomerCare(string json, int agencyId)
        {
            var urlJson = string.Format("{0}Customer/UpdateCustomerCare?key={1}&{2}&agencyId={3}", Domain, Keyapi, json, agencyId);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}Customer/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
    }
}