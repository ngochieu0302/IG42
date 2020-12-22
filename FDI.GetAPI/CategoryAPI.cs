using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class CategoryAPI : BaseAPI
    {
        public CategoryItem GetBySlug(string slug, int agencyId)
        {
            var urlJson = string.Format("{0}DNCategory/GetBySlug?key={1}&slug={2}&agencyId={3}", Domain, Keyapi, slug, agencyId);
            return GetObjJson<CategoryItem>(urlJson);
        }
        public ModelCategoryItem ListItems(string url, int type)
        {
            var urlJson = string.Format("{0}DNCategory/ListItems?key={1}&type={2}&{3}", Domain, Keyapi, type, url);
            return GetObjJson<ModelCategoryItem>(urlJson);
        }
        public CategoryItem GetItemById(int id)
        {
            var urlJson = string.Format("{0}DNCategory/GetItemById?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<CategoryItem>(urlJson);
        }

        public List<CategoryItem> GetAll()
        {
            var urlJson = string.Format("{0}DNCategory/GetAll?key={1}", Domain, Keyapi);
            return GetObjJson<List<CategoryItem>>(urlJson);
        }
        public List<CategoryItem> GetListSimpleByAutoComplete(string keyword, int showLimit, bool isShow, int agencyId)
        {
            var urlJson = string.Format("{0}DNCategory/GetListSimpleByAutoComplete?key={1}&keyword={2}&showLimit={3}&isShow={4}&agencyId={5}", Domain, Keyapi, keyword, showLimit, isShow, agencyId);
            return GetObjJson<List<CategoryItem>>(urlJson);
        }

        public List<CategoryItem> GetChildByParentId(bool setTitle)
        {
            var urlJson = string.Format("{0}DNCategory/GetChildByParentId?key={1}&setTitle={2}", Domain, Keyapi, setTitle);
            return GetObjJson<List<CategoryItem>>(urlJson);
        }

        public List<TreeViewItem> GetListTree(int agencyId)
        {
            var urlJson = string.Format("{0}DNCategory/GetListTree?key={1}&agencyId={2}", Domain, Keyapi, agencyId);
            return GetObjJson<List<TreeViewItem>>(urlJson);
        }

        public List<TreeViewItem> GetListTreeByType(int type, int agencyId)
        {
            var urlJson = string.Format("{0}DNCategory/GetListTreeByType?key={1}&type={2}&agencyId={3}", Domain, Keyapi, type, agencyId);
            return GetObjJson<List<TreeViewItem>>(urlJson);
        }

        public List<TreeViewItem> GetListTreeByTypeListId(int type, string lstId, int agencyId)
        {
            var urlJson = string.Format("{0}DNCategory/GetListTreeByTypeListId?key={1}&type={2}&lstId={3}&agencyId={4}", Domain, Keyapi, type, lstId, agencyId);
            return GetObjJson<List<TreeViewItem>>(urlJson);
        }

        public List<CategoryItem> GetAllListSimpleByParentId(int id, int agencyId)
        {
            var urlJson = string.Format("{0}DNCategory/GetAllListSimpleByParentId?key={1}&id={2}&agencyId={3}", Domain, Keyapi, id, agencyId);
            return GetObjJson<List<CategoryItem>>(urlJson);
        }

        public bool CheckTitleAsciiExits(string slug, int id)
        {
            var urlJson = string.Format("{0}DNCategory/CheckTitleAsciiExits?key={1}&slug={2}&id={3}", Domain, Keyapi, slug, id);
            GetObjJson<int>(urlJson);
            return true;
        }

        public CategoryItem GetCategoryById(int id)
        {
            var urlJson = string.Format("{0}DNCategory/GetCategoryById?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<CategoryItem>(urlJson);
        }

        public CategoryItem GetCategoryParentId(int id)
        {
            var urlJson = string.Format("{0}DNCategory/GetCategoryParentId?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<CategoryItem>(urlJson);
        }

        public List<CategoryItem> GetListByArrId(string lstId)
        {
            var urlJson = string.Format("{0}DNCategory/GetListByArrId?key={1}&keyword={2}", Domain, Keyapi, lstId);
            return GetObjJson<List<CategoryItem>>(urlJson);
        }

        public JsonMessage Add(string json, string code)
        {
            var urlJson = string.Format("{0}DNCategory/Add?key={1}&codelogin={2}&{3}", Domain, Keyapi, code, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Update(string json, string code)
        {
            var urlJson = string.Format("{0}DNCategory/Update?key={1}&codelogin={2}&{3}", Domain, Keyapi, code, json);
            return GetObjJson<JsonMessage>(urlJson);
        }

        public JsonMessage ShowHide(string json, bool showhide)
        {
            var urlJson = string.Format("{0}DNCategory/ShowHide?key={1}&{2}&showhide={3}", Domain, Keyapi, json, showhide);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Delete(string json)
        {
            var urlJson = string.Format("{0}DNCategory/Delete?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
    }
}
