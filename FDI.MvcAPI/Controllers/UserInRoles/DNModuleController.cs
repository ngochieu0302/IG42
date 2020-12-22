using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;
using Newtonsoft.Json;

namespace FDI.MvcAPI.Controllers
{
    public class DNModuleController : BaseApiController
    {
        //
        // GET: /DNRoles/
        private readonly DNModuleDA _da = new DNModuleDA("#");
        private readonly DNLoginDA _dllogin = new DNLoginDA("#");
        public ActionResult GetRouter(string key)
        {
            var obj = key != Keyapi ? new List<RouterItem>() : _da.GetRouter();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListTree(string key, string code)
        {
            var obj = key != Keyapi ? new List<TreeViewItem>() : _da.GetListTree(Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListTreeNew(string key, string lstId)
        {
            var obj = key != Keyapi ? new List<TreeViewItem>() : _da.GetListTreeNew(lstId, Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRoleItemById(string key, int id)
        {
            var obj = key != Keyapi ? new DN_Module() : _da.GetById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new ModuleItem() : _da.GetItemById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DNModule_GetChildByParentId(string key, string code, int id)
        {
            var obj = key != Keyapi ? new List<ModuleItem>() : _da.DNModule_GetChildByParentId(id, Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllListSimpleByParentID(string key, int id)
        {
            var obj = key != Keyapi ? new List<ModuleItem>() : _da.GetAllListSimpleByParentID(id, Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListByParentID(string key, int parentId, Guid UserId,string listRole)
        {
            var lst = FDIUtils.StringToListInt(listRole);
            var obj = key != Keyapi ? new List<ModuleItem>() : _da.GetListByParentID(parentId, Agencyid(), UserId, lst);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListByParentIdAdmin(string key, int parentId)
        {            
            var obj = key != Keyapi ? new List<ModuleItem>() : _da.GetListByParentIdAdmin(parentId, Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListSimpleByAutoComplete(string key, string keyword, int showLimit, bool isShow)
        {
            var obj = key != Keyapi ? new List<ModuleItem>() : _da.GetListSimpleByAutoComplete(keyword, showLimit, isShow, Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetlistByTagUserId(string key, string keyword, Guid guid, int agencyId, string listRole, int parentId, int moduleId)
        {
            //var user = _dllogin.GetUserItemByCode(code);
            var lst = FDIUtils.StringToListInt(listRole);
            var obj = key != Keyapi ? new List<ActionActiveItem>() : _da.GetlistByTagUserId(keyword, guid, lst, agencyId, parentId, moduleId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllListSimpleItems(string key, int agencyid)
        {
            var obj = key != Keyapi ? new List<ModuleadminItem>() : _da.GetAllListSimpleItems(agencyid);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllModuleByUserName(string key, Guid userId, int agencyId)
        {
            var obj = key != Keyapi ? new List<ModuleadminItem>() : _da.GetAllModuleByUserName(userId, agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update(string key, string json, string code)
        {
            try
            {
                if (key == Keyapi)
                {
                    
                    var moduleItem = JsonConvert.DeserializeObject<ModuleUpdateItem>(json);
                    var obj = _da.GetById(moduleItem.ID);
                    var lstActive = _da.GetListActive();
                    var lsGuiId1 = GetListRole(code);
                    var listRole = lsGuiId1.Select(c => c.RoleId).ToList();

                    obj.DN_Roles.Clear();
                    obj.DN_Roles = _da.GetListRolesbyListGuid(listRole);
                    while (obj.DN_Role_ModuleActive.Count > 0)
                    {
                        var item = obj.DN_Role_ModuleActive.FirstOrDefault();
                        _da.Delete(item);
                    }
                    obj.DN_Role_ModuleActive.Clear();
                    foreach (var activeitem in obj.DN_Roles.SelectMany(item => lstActive.Select(active => new DN_Role_ModuleActive
                    {
                        ModuleId = obj.ID,
                        RoleId = item.RoleId,
                        ActiveId = active.ID,
                        Active = true,
                        Check = false,
                        AgencyId = Agencyid()
                    })))
                    {
                        obj.DN_Role_ModuleActive.Add(activeitem);
                    }

                    // quyền cho nhân viên
                    var lsGuiId2 = GetListUser(code);
                    var lstg = lsGuiId2.Select(c => c.UserId).ToList();
                    obj.DN_Users.Clear();
                    obj.DN_Users = _da.GetListUserbyListGuid(lstg);
                    while (obj.DN_User_ModuleActive.Count > 0)
                    {
                        var item = obj.DN_User_ModuleActive.FirstOrDefault();
                        _da.Delete(item);
                    }
                    obj.DN_User_ModuleActive.Clear();
                    foreach (var activeitem in obj.DN_Users.SelectMany(item => lstActive.Select(active => new DN_User_ModuleActive
                    {
                        ModuleId = obj.ID,
                        UserId = item.UserId,
                        ActiveId = active.ID,
                        Active = true,
                        Check = 1,
                        AgencyId = Agencyid()
                    })))
                    {
                        obj.DN_User_ModuleActive.Add(activeitem);
                    }
                    _da.Save();
                    //if (list.Any() || list1.Any())
                    //    _da.DeleteModuleUserRole(moduleItem.ID, string.Join(",", list1), string.Join(",", list));
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }        

        public ActionResult Show(string key, string listint, string code)
        {
            if (key == Keyapi)
            {
                var lit = JsonConvert.DeserializeObject<List<int?>>(listint);
                var list = _da.GetListAgencyModuleByArrID(lit, Agencyid());
                foreach (var item in list)
                {
                    item.IsShow = true;
                }
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Hide(string key, string listint)
        {
            if (key == Keyapi)
            {
                var lit = JsonConvert.DeserializeObject<List<int?>>(listint);
                var list = _da.GetListAgencyModuleByArrID(lit, Agencyid());
                foreach (var item in list)
                {
                    item.IsShow = false;
                }
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        
        public List<ListUsersItem> GetListUser(string code)
        {
            const string url = "Utility/GetListUser?key=";
            var urlJson = string.Format("{0}{1}", UrlG + url, code);
            var list = Utility.GetObjJson<List<ListUsersItem>>(urlJson);
            return list;
        }

        public List<ListRolesItem> GetListRole(string code)
        {
            const string url = "Utility/GetListRole?key=";
            var urlJson = string.Format("{0}{1}", UrlG + url, code);
            var list = Utility.GetObjJson<List<ListRolesItem>>(urlJson);
            return list;
        }
    }
}
