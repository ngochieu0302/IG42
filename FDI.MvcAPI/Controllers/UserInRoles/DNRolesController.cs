using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;
using Newtonsoft.Json;

namespace FDI.MvcAPI.Controllers
{
    public class DNRolesController : BaseApiController
    {
        //
        // GET: /DNRoles/
        private readonly DNRoleDA _dl = new DNRoleDA();

        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelDNRolesItem()
                : new ModelDNRolesItem { ListItem = _dl.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _dl.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAll(string key, int agencyId)
        {
            var obj = key != Keyapi ? new List<DNRolesItem>() : _dl.GetByAll(agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRoleItemById(string key, Guid id)
        {
            var obj = key != Keyapi ? new DNRolesItem() : _dl.GetRoleItemById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add(string key, int agencyId, string json)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var obj = new DN_Roles();
                    var role = JsonConvert.DeserializeObject<DNRolesJsonItem>(json);
                    if (role.RoleName.Trim().ToLower() == "admin")
                    {
                        msg.Erros = true;
                        msg.Message = "Quyền Admin đã tồn tại trong hệ thống.";
                        return Json(msg, JsonRequestBehavior.AllowGet);
                    }

                    obj = UpdateRole(obj, role);
                    var lsGuiId = GetListUser(role.Code);
                    obj.AgencyID = agencyId;
                    var list = lsGuiId.Select(guid => new DN_UsersInRoles
                    {
                        RoleId = obj.RoleId,
                        UserId = guid.UserId,
                        AgencyID = agencyId,
                        IsDelete = false,
                        DateCreated = ConvertDate.TotalSeconds(DateTime.Now)
                    }).ToList();
                    obj.DN_UsersInRoles = list;
                    obj.RoleId = role.RoleId;                    
                    _dl.Add(obj);
                    _dl.Save();
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được thêm mới";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(string key, int agencyId, string json)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var role = JsonConvert.DeserializeObject<DNRolesJsonItem>(json);
                    var obj = _dl.GetById(role.RoleId);
                    if (role.RoleName.Trim().ToLower() != "admin" && obj.RoleName.ToLower() == "admin")
                    {
                        msg.Erros = true;
                        msg.Message = "Bạn không được phép chình sửa tên quyền Admin trong hệ thống.";
                        return Json(msg, JsonRequestBehavior.AllowGet);
                    }
                    obj = UpdateRole(obj, role);
                    var lst = obj.DN_UsersInRoles.Where(c => c.IsDelete == false);
                    var lsGuiId = GetListUser(role.Code);
                    var result1 = lst.Where(p => lsGuiId.All(p2 => p2.UserId != p.UserId));
                    foreach (var i in result1)
                    {
                        i.IsDelete = true;
                    }
                    var result2 = lsGuiId.Where(p => lst.All(p2 => p2.UserId != p.UserId)).ToList();
                    var listAdd = result2.Select(guid => new DN_UsersInRoles
                    {
                        RoleId = obj.RoleId,
                        UserId = guid.UserId,
                        AgencyID = agencyId,
                        IsDelete = false,
                        DateCreated = ConvertDate.TotalSeconds(DateTime.Now)
                    }).ToList();
                    obj.DN_UsersInRoles.AddRange(listAdd);
                    _dl.Save();
                }

            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được cập nhật";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateActive(string key, Guid roleid, string ltrInts)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    _dl.UpdateRoleActive(roleid, ltrInts);
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được cập nhật";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateModuleActive(string key, Guid roleid, string ltrInts)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    _dl.UpdateRoleModuleActive(roleid, ltrInts);
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được cập nhật";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddModuleRole(string key, string listInt, Guid roleId)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var lstActive = _dl.GetListActive();
                    var lst = FDIUtils.StringToListInt(listInt);

                    var model = _dl.GetById(roleId);
                    model.DN_Module.Clear();

                    model.DN_Module = _dl.GetListModulebyListInt(lst);
                    while (model.DN_Role_ModuleActive.Count > 0)
                    {
                        var item = model.DN_Role_ModuleActive.FirstOrDefault();
                        _dl.Delete(item);
                    }
                    model.DN_Role_ModuleActive.Clear();
                    foreach (var activeitem in model.DN_Module.SelectMany(item => lstActive.Select(active => new DN_Role_ModuleActive
                    {
                        ModuleId = item.ID,
                        RoleId = model.RoleId,
                        ActiveId = active.ID,
                        Active = true,
                        Check = true,
                        AgencyId = Agencyid()
                    })))
                    {
                        model.DN_Role_ModuleActive.Add(activeitem);
                    }
                    _dl.Save();
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được cập nhật.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(string key, List<Guid> lstId)
        {
            var msg = new JsonMessage(false, "Xóa dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var list = _dl.GetListByArrId(lstId);
                    foreach (var item in list)
                    {
                        if (item.RoleName.ToLower() == "admin")
                            continue;
                        item.IsDeleted = true;
                        foreach (var itemnew in item.DN_UsersInRoles)
                        {
                            itemnew.IsDelete = true;
                        }
                    }
                    _dl.Save();
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được xóa";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public DN_Roles UpdateRole(DN_Roles activeRole, DNRolesJsonItem activeRoleItem)
        {
            activeRole.RoleName = activeRoleItem.RoleName;
            activeRole.Description = activeRoleItem.Description;
            activeRole.LoweredRoleName = FomatString.Slug(activeRole.RoleName);
            activeRole.LevelId = activeRoleItem.LevelRoomId;            
            activeRole.IsDeleted = false;
            return activeRole;
        }

        public List<ListUsersItem> GetListUser(string code)
        {
            const string url = "Utility/GetListUser?key=";
            var urlJson = string.Format("{0}{1}", UrlG + url, code);
            var list = Utility.GetObjJson<List<ListUsersItem>>(urlJson);
            return list;
        }
    }
}
