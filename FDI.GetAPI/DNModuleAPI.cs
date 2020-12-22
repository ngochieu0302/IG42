using System;
using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class DNModuleAPI : BaseAPI
    {
        public List<ActionActiveItem> GetlistByTagUserId(string keyword, Guid guid, int agencyId, string listRole, string parentId, string moduleId)
        {
            var urlJson = string.Format("{0}DNModule/GetlistByTagUserId?key={1}&keyword={2}&agencyId={3}&listRole={4}&guid={5}&parentId={6}&moduleId={7}", Domain, Keyapi, keyword, agencyId, listRole, guid, parentId, moduleId);
            return GetObjJson<List<ActionActiveItem>>(urlJson);
        }
        public List<TreeViewItem> GetListTree(int agencyid)
        {
            var urlJson = string.Format("{0}DNModule/GetListTree?key={1}&agencyId={2}", Domain, Keyapi, agencyid);
            return GetObjJson<List<TreeViewItem>>(urlJson);
        }
        public List<ModuleItem> GetAllListSimpleByParentID(int agencyid, int id)
        {
            var urlJson = string.Format("{0}DNModule/GetAllListSimpleByParentID?key={1}&agencyId={2}&id={3}", Domain, Keyapi, agencyid, id);
            return GetObjJson<List<ModuleItem>>(urlJson);
        }
        public List<ModuleItem> GetListByParentID(int agencyid, string parentId, Guid UserId, string listRole)
        {
            var urlJson = string.Format("{0}DNModule/GetListByParentID?key={1}&agencyId={2}&parentId={3}&UserId={4}&listRole={5}", Domain, Keyapi, agencyid, parentId, UserId, listRole);
            return GetObjJson<List<ModuleItem>>(urlJson);
        }

        public List<ModuleItem> GetListByParentIdAdmin(int agencyid, string parentId)
        {
            var urlJson = string.Format("{0}DNModule/GetListByParentIdAdmin?key={1}&agencyId={2}&parentId={3}", Domain, Keyapi, agencyid, parentId);
            return GetObjJson<List<ModuleItem>>(urlJson);
        }
        public ModuleItem GetItemById(int id)
        {
            var urlJson = string.Format("{0}DNModule/GetItemById?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<ModuleItem>(urlJson);
        }
        public List<TreeViewItem> GetListTreeNew(string lstId, int agencyId)
        {
            var urlJson = string.Format("{0}DNModule/GetListTreeNew?key={1}&lstId={2}&agencyId={3}", Domain, Keyapi, lstId, agencyId);
            return GetObjJson<List<TreeViewItem>>(urlJson);
        }
        public List<ModuleadminItem> GetAllListSimpleItems(int agencyid)
        {
            var urlJson = string.Format("{0}DNModule/GetAllListSimpleItems?key={1}&agencyId={2}", Domain, Keyapi, agencyid);
            var key = string.Format("DNModuleAPI_GetAllListSimpleItems?key={0}&agencyId={1}", Keyapi, agencyid);
            return GetCache<List<ModuleadminItem>>(key, urlJson, ConfigCache.TimeExpire360);
        }
        public List<ModuleadminItem> GetAllModuleByUserName(Guid userId, int agencyid)
        {
            var urlJson = string.Format("{0}DNModule/GetAllModuleByUserName?key={1}&userId={2}&agencyId={3}", Domain, Keyapi, userId, agencyid);
            var key = string.Format("DNModuleAPI_GetAllModuleByUserName_{0}_{1}", userId, agencyid);
            return GetCache<List<ModuleadminItem>>(key, urlJson, ConfigCache.TimeExpire360);
        } 
        public int Update(int agencyid, string json, string code)
        {
            var urlJson = string.Format("{0}DNModule/Update?key={1}&agencyId={2}&json={3}&code={4}", Domain, Keyapi, agencyid, json, code);
            return GetObjJson<int>(urlJson);
        }
        public int Show(string listint, int agencyid)
        {
            var urlJson = string.Format("{0}DNModule/Show?key={1}&listint={2}&agencyId={3}", Domain, Keyapi, listint, agencyid);
            return GetObjJson<int>(urlJson);
        }
        public int Hide(string listint, int agencyid)
        {
            var urlJson = string.Format("{0}DNModule/Hide?key={1}&listint={2}&agencyId={3}", Domain, Keyapi, listint, agencyid);
            return GetObjJson<int>(urlJson);
        }        
    }
}
