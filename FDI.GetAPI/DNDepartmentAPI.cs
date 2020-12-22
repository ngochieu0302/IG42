using System.Collections.Generic;
using FDI.Utils;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class DepartmentAPI : BaseAPI
    {
        private readonly string _url = Domain;
        public ModelDepartmentItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}DNDepartment/ListItems{1}&key={2}&agencyId={3}", _url, url, Keyapi,agencyid);
            return GetObjJson<ModelDepartmentItem>(urlJson);
        }

        public List<DepartmentItem> GetAll(int agencyId)
        {
            var urlJson = string.Format("{0}DNDepartment/GetAll?key={1}&agencyId={2}", _url, Keyapi, agencyId);
            return GetObjJson<List<DepartmentItem>>(urlJson);
        }

        //public DN_Department GetById(int agencyid, int id = 0)
        //{
        //    var urlJson = string.Format("{0}DNDepartment/GetById?key={1}&agencyId={2}&id={3}", _url, Keyapi,agencyid, id);
        //    return GetObjJson<DN_Department>(urlJson);
        //}

        public DepartmentItem GetDepartmentItemById(int agencyid, int id = 0)
        {
            var urlJson = string.Format("{0}DNDepartment/GetDepartmentItemById?key={1}&agencyId={2}&id={3}", _url, Keyapi,agencyid, id);
            return GetObjJson<DepartmentItem>(urlJson);
        }

        public List<DepartmentItem> GetListByArrId(int agencyid, string lstId)
        {
            var urlJson = string.Format("{0}DNDepartment/GetListByArrId?key={1}&agencyId={2}&lstId={3}", _url, Keyapi,agencyid, lstId);
            return GetObjJson<List<DepartmentItem>>(urlJson);
        }

        public JsonMessage Add(int agencyid, string json)
        {
            var urlJson = string.Format("{0}DNDepartment/Add?key={1}&agencyId={2}&json={3}", _url, Keyapi,agencyid, json);
            return GetObjJson<JsonMessage>(urlJson);
        }

        public JsonMessage Update(int agencyid, string json, int id)
        {
            var urlJson = string.Format("{0}DNDepartment/Update?key={1}&agencyId={2}&json={3}&id={4}", _url, Keyapi,agencyid, json, id);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}DNDepartment/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
    }
}
