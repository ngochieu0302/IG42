using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Admin.Controllers
{
    public class ModuleController : BaseController
    {

        private readonly ModuleDA _moduleDa;
        private readonly RoleDA _roleDa;
        private readonly UserDA _userDa;

        public ModuleController()
        {
            
            _userDa = new UserDA("#");
            _roleDa = new RoleDA("#");
            _moduleDa = new ModuleDA("#");
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult JsonTreeCategorySelect()
        {
            var ltsCategory = _moduleDa.GetListAdminTree();
            return Json(ltsCategory, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AjaxTreeSelect()
        {
            return View();
        }

        public ActionResult AjaxSort()
        {
            var ltsSourceCategory = _moduleDa.GetAllListSimpleByParentID(ArrId.FirstOrDefault());
            ViewData.Model = ltsSourceCategory;
            return View();
        }

        public ActionResult ListItems()
        {
            var ltsSourceModule = _moduleDa.GetAllListSimple();
            return View(ltsSourceModule);
        }

        public ActionResult AjaxForm()
        {
            var moduleModel = new Module
            {
                IsShow = true,
                Ord = 0,
                PrarentID = (ArrId.Any()) ? ArrId.FirstOrDefault() : 0
            };

            if (DoAction == ActionType.Edit)
                moduleModel = _moduleDa.GetById(ArrId.FirstOrDefault());
            var ltsAllItems = _moduleDa.GetAllListSimple();
            ltsAllItems = _moduleDa.GetAllSelectList(ltsAllItems, moduleModel.ID, true);
            ViewBag.RoleId = _roleDa.GetListAll();
            ViewBag.UserID = _userDa.GetListAll();
            ViewBag.PrarentID = ltsAllItems;
            ViewData.Model = moduleModel;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }

        public ActionResult AjaxRoleForm()
        {
            var id = Request["id"];
            var role = _roleDa.GetListItemAll();
            ViewBag.Id = id;
            ViewBag.Action = DoAction;
            return View(role);
        }

        public ActionResult AjaxUserForm()
        {
            var id = Request["id"];
            var user = _userDa.GetListAll();
            ViewBag.Id = id;
            return View(user);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var module = new Module();
            List<Module> ltsModuleItems;
            StringBuilder stbMessage;
            var LstUserIds = Request["UserId"];
            var LstRoleIds = Request["RoleId"];
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(module);
                        var parent = _moduleDa.GetById(module.PrarentID??0);
                        if (parent !=null)
                        {
                            module.Level = parent.Level + 1;
                        }
                        else
                        {
                            module.Level = 1;
                        }
                        module.IsDelete = false;
                        _moduleDa.Add(module);
                        foreach (var rolemo in ConvertUtil.LsGuiId(LstRoleIds).Select(item => _moduleDa.GetByRoleId(item)))
                        {
                            module.aspnet_Roles.Add(rolemo);
                            if (rolemo.ActiveRoles.Any())
                            {
                                foreach (var roleM in rolemo.ActiveRoles.Select(roleActive => new Role_ModuleActive
                                {
                                    ModuleId = module.ID,
                                    RoleId = rolemo.RoleId,
                                    Check = false,
                                    ActiveRoleId = roleActive.Id,
                                    Active = true
                                }))
                                {
                                    module.Role_ModuleActive.Add(roleM);
                                }
                            }
                        }
                        foreach (var item in ConvertUtil.LsGuiId(LstRoleIds))
                        {
                            var user = _moduleDa.GetByUserId(item);
                            module.aspnet_Users.Add(user);
                            var firstOrDefault = user.aspnet_Roles.FirstOrDefault();
                            if (firstOrDefault != null && firstOrDefault.ActiveRoles.Any())
                            {
                                foreach (var userModuleActive in firstOrDefault.ActiveRoles.Select(moduleactive => new User_ModuleActive
                                {
                                    ID = 1,
                                    ModuleId = module.ID,
                                    UserId = item,
                                    ActiveRoleId = moduleactive.Id,
                                    Active = true,
                                    Check = 1
                                }))
                                {
                                    module.User_ModuleActive.Add(userModuleActive);
                                }
                            }
                        }
                        
                        _moduleDa.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = module.ID.ToString(),
                            Message = string.Format("Đã thêm mới: <b>{0}</b>", Server.HtmlEncode(module.NameModule))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;
                
                case ActionType.Edit:
                    try
                    {
                        module = _moduleDa.GetById(ArrId.FirstOrDefault());
                        UpdateModel(module);
                        var parent = _moduleDa.GetById(module.PrarentID ?? 0);
                        if (parent != null)
                        {
                            module.Level = parent.Level + 1;
                        }
                        else
                        {
                            module.Level = 1;
                        }
                        var listrole = ConvertUtil.LsGuiId(LstRoleIds);
                        foreach (var rolemo in listrole.Where(m => module.aspnet_Roles.All(t => t.RoleId != m)).Select(item => _moduleDa.GetByRoleId(item)))
                        {
                            module.aspnet_Roles.Add(rolemo);
                            if (rolemo.ActiveRoles.Any())
                            {
                                foreach (var roleM in rolemo.ActiveRoles.Select(roleActive => new Role_ModuleActive
                                {
                                    ModuleId = module.ID,
                                    RoleId = rolemo.RoleId,
                                    Check = false,
                                    ActiveRoleId = roleActive.Id,
                                    Active = true
                                }))
                                {
                                    module.Role_ModuleActive.Add(roleM);
                                }
                            }
                        }
                        var listuser = ConvertUtil.LsGuiId(LstUserIds);
                        foreach (var user in listuser.Where(m => module.aspnet_Users.All(t => t.UserId != m)).Select(item => _moduleDa.GetByUserId(item)))
                        {
                            module.aspnet_Users.Add(user);
                            var firstOrDefault = user.aspnet_Roles.FirstOrDefault();
                            if (firstOrDefault != null && firstOrDefault.ActiveRoles.Any())
                            {
                                foreach (var userModuleActive in firstOrDefault.ActiveRoles.Select(moduleactive => new User_ModuleActive
                                {
                                    ID = 1,
                                    ModuleId = module.ID,
                                    UserId = user.UserId,
                                    ActiveRoleId = moduleactive.Id,
                                    Active = true,
                                    Check = 1
                                }))
                                {
                                    module.User_ModuleActive.Add(userModuleActive);
                                }
                            }
                        }
                        var list = module.aspnet_Roles.Where(m => listrole.All(n => n != m.RoleId)).Select(m => m.RoleId).ToList();
                        var list1 = module.aspnet_Users.Where(m => listuser.All(n => n != m.UserId)).Select(m => m.UserId).ToList();
                        _moduleDa.Save();
                        if (list.Any() || list1.Any())
                            _moduleDa.DeleteAdminModuleUserRole(module.ID, string.Join(",", list1), string.Join(",", list));
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = module.ID.ToString(),
                            Message = string.Format("Đã cập nhật chuyên mục: <b>{0}</b>", Server.HtmlEncode(module.NameModule))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;


                case ActionType.Delete:
                    ltsModuleItems = _moduleDa.GetListByArrID(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsModuleItems)
                    {
                        item.IsDelete = true;
                        stbMessage.AppendFormat("Đã xóa chuyên mục <b>{0}</b>.<br />", Server.HtmlEncode(item.NameModule));
                    }
                    msg.ID = string.Join(",", ArrId);
                    _moduleDa.Save();
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Show:
                    ltsModuleItems = _moduleDa.GetListByArrID(ArrId).Where(o => o.IsShow != null && !o.IsShow.Value).ToList(); //Chỉ lấy những đối tượng ko được hiển thị
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsModuleItems)
                    {
                        item.IsShow = true;
                        stbMessage.AppendFormat("Đã hiển thị chuyên mục <b>{0}</b>.<br />", Server.HtmlEncode(item.NameModule));
                    }
                    _moduleDa.Save();
                    msg.ID = string.Join(",", ltsModuleItems.Select(o => o.ID));
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Hide:
                    ltsModuleItems = _moduleDa.GetListByArrID(ArrId).Where(o => o.IsShow != null && o.IsShow.Value).ToList(); //Chỉ lấy những đối tượng được hiển thị
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsModuleItems)
                    {
                        item.IsShow = false;
                        stbMessage.AppendFormat("Đã ẩn chuyên mục <b>{0}</b>.<br />", Server.HtmlEncode(item.NameModule));
                    }
                    _moduleDa.Save();
                    msg.ID = string.Join(",", ltsModuleItems.Select(o => o.ID));
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Order:
                    if (!string.IsNullOrEmpty(Request["OrderValues"]))
                    {
                        var orderValues = Request["OrderValues"];
                        if (orderValues.Contains("|"))
                        {
                            foreach (var keyValue in orderValues.Split('|'))
                            {
                                if (keyValue.Contains("_"))
                                {
                                    var tempCategory = _moduleDa.GetById(Convert.ToInt32(keyValue.Split('_')[0]));
                                    tempCategory.Ord = Convert.ToInt32(keyValue.Split('_')[1]);
                                    _moduleDa.Save();
                                }
                            }
                        }
                        msg.ID = string.Join(",", orderValues);
                        msg.Message = "Đã cập nhật lại thứ tự chuyên mục";
                    }
                    break;
            }

            if (string.IsNullOrEmpty(msg.Message))
            {
                msg.Message = "Không có hành động nào được thực hiện.";
                msg.Erros = true;
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutoComplete()
        {
            var term = Request["term"];
            var ltsResults = _moduleDa.GetListSimpleByAutoComplete(term, 10, true);
            return Json(ltsResults, JsonRequestBehavior.AllowGet);
        }
    }
}
