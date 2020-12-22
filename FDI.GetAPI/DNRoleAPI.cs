using System;
using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class DNRoleAPI : BaseAPI
    {
        private readonly string _url = Domain;

        public ModelDNRolesItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}DNRoles/ListItems{1}&key={2}&agencyId={3}", _url, url, Keyapi, agencyid);
            return GetObjJson<ModelDNRolesItem>(urlJson);
        }

        public DNRolesItem GetBySlug(string slug)
        {
            var urlJson = string.Format("{0}DNRoles/GetBySlug?key={1}&slug={2}", _url, Keyapi, slug);
            return GetObjJson<DNRolesItem>(urlJson);
        }

        public DNRolesItem GetByid(Guid id)
        {
            var urlJson = string.Format("{0}DNRoles/GetRoleItemById?key={1}&id={2}", _url, Keyapi, id);
            return GetObjJson<DNRolesItem>(urlJson);
        }

        public List<DNRolesItem> GetAll(int agencyId)
        {
            var urlJson = string.Format("{0}DNRoles/GetAll?key={1}&agencyId={2}", _url, Keyapi, agencyId);
            return GetObjJson<List<DNRolesItem>>(urlJson);
        }

        public JsonMessage Add(int agencyid, string json, string code)
        {
            var urlJson = string.Format("{0}DNRoles/Add?key={1}&agencyId={2}&json={3}&code={4}", _url, Keyapi, agencyid, json, code);
            return GetObjJson<JsonMessage>(urlJson);
        }

        public JsonMessage Update(string json, Guid id, int agencyid, string code)
        {
            var urlJson = string.Format("{0}DNRoles/Update?key={1}&json={2}&id={3}&agencyId={4}&code={5}", _url, Keyapi, json, id, agencyid, code);
            return GetObjJson<JsonMessage>(urlJson);
        }

        public JsonMessage UpdateActive(Guid roleid, string ltrInts)
        {
            var urlJson = string.Format("{0}DNRoles/UpdateActive?key={1}&roleid={2}&ltrInts={3}", _url, Keyapi, roleid, ltrInts);
            return GetObjJson<JsonMessage>(urlJson);
        }

        public JsonMessage UpdateModuleActive(Guid roleid, string ltrInts)
        {
            var urlJson = string.Format("{0}DNRoles/UpdateModuleActive?key={1}&roleid={2}&ltrInts={3}", _url, Keyapi, roleid, ltrInts);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage AddModuleRole(string listInt, string roleId, int agencyid)
        {
            var urlJson = string.Format("{0}DNRoles/AddModuleRole?key={1}&agencyId={2}&listInt={3}&roleId={4}", _url, Keyapi, agencyid, listInt, roleId);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Delete(string lstId)
        {
            var urlJson = string.Format("{0}DNRoles/Delete?key={1}&lstId={2}", _url, Keyapi, lstId);
            return GetObjJson<JsonMessage>(urlJson);
        }
    }
}
