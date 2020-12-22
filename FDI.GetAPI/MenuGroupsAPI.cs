using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class MenuGroupsAPI : BaseAPI
    {
        public ModelMenuGroupsItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}MenuGroups/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi,agencyid);
            return GetObjJson<ModelMenuGroupsItem>(urlJson);
        }

        public MenuGroupsItem GetItemById(int id)
        {
            var urlJson = string.Format("{0}MenuGroups/GetItemById?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<MenuGroupsItem>(urlJson);
        }

        public List<MenuGroupsItem> GetListSimpleAll()
        {
            var urlJson = string.Format("{0}MenuGroups/GetListSimpleAll?key={1}", Domain, Keyapi);
            return GetObjJson<List<MenuGroupsItem>>(urlJson);
        }

        public int Add(string json)
        {
            var urlJson = string.Format("{0}MenuGroups/Add?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
        public int Update(string json)
        {
            var urlJson = string.Format("{0}MenuGroups/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
        public int Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}MenuGroups/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }
    }
}