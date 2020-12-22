using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class MenuAPI : BaseAPI
    {
        public List<MenusItem> GetListMenus(string url ,int groupId)
        {
            var urlJson = string.Format("{0}Menu/GetListMenus?key={1}&groupId={2}", url, Keyapi, groupId);
            return GetObjJson<List<MenusItem>>(urlJson);
        }

        public List<CategoryItem> GetCategories(string url, int groupId)
        {
            var urlJson = string.Format("{0}Menu/GetCategories?key={1}&groupId={2}", url, Keyapi, groupId);
            return GetObjJson<List<CategoryItem>>(urlJson);
        }
        public ModuleSettingItem GetKey(string url, int moduleId)
        {
            var urlJson = string.Format("{0}Menu/GetKey?key={1}&moduleId={2}", url, Keyapi, moduleId);
            return GetObjJson<ModuleSettingItem>(urlJson);
        }

        public List<TreeViewItem> GetListTree(int groupId)
        {
            var urlJson = string.Format("{0}Menu/GetListTree?key={1}&groupId={2}", Domain, Keyapi, groupId);
            return GetObjJson<List<TreeViewItem>>(urlJson);
        }
        
        public List<MenusItem> GetAll()
        {
            var urlJson = string.Format("{0}Menu/GetAll?key={1}", Domain, Keyapi);
            return GetObjJson<List<MenusItem>>(urlJson);
        }

        public List<MenusItem> GetListByParentId(int groupId)
        {
            var urlJson = string.Format("{0}Menu/GetListByParentId?key={1}&groupId={2}", Domain, Keyapi, groupId);
            return GetObjJson<List<MenusItem>>(urlJson);
        }

        public ModelMenusItem ListItems(int agencyid, string url, int groupId)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Menu/ListItems{1}&key={2}&agencyId={3}&groupId={4}", Domain, url, Keyapi,agencyid, groupId);
            return GetObjJson<ModelMenusItem>(urlJson);
        }

        public MenusItem GetItemById(int id)
        {
            var urlJson = string.Format("{0}Menu/GetItemById?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<MenusItem>(urlJson);
        }

        public int Add(string json)
        {
            var urlJson = string.Format("{0}Menu/Add?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
        public int Update(string json)
        {
            var urlJson = string.Format("{0}Menu/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
        public int Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}Menu/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }

    }
}
