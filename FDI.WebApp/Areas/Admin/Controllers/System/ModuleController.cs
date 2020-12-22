using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Areas.Admin.Controllers
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
            return View(SystemActionItem);
        }

        public ActionResult AjaxTreeSelect()
        {
            var ltsSourceModule = _moduleDa.GetAllListSimple();
            var ltsValues = FDIUtils.StringToListInt(Request["ValuesSelected"]);
            var stbHtml = new StringBuilder();
            _moduleDa.BuildTreeViewCheckBox(ltsSourceModule, 1, true, ltsValues, ref stbHtml);

            var model = new ModelModuleItem
            {
                Container = Request["Container"],
                SelectMutil = Convert.ToBoolean(Request["SelectMutil"]),
                SystemActionItem = SystemActionItem,
                StbHtml = stbHtml.ToString()
            };
            ViewData.Model = model;
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
            //var stbHtml = new StringBuilder();
            //_moduleDa.BuildTreeView(ltsSourceModule, 1, false, ref stbHtml);
            //ViewData.Model = stbHtml.ToString();
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
            ViewBag.PrarentID = ltsAllItems;
            ViewData.Model = moduleModel;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }

        public ActionResult AjaxRoleForm()
        {
            var id = ArrId.FirstOrDefault();
            var role = _roleDa.GetListItemAll();
            ViewBag.Id = id;
            ViewBag.ActionText = ActionText;
            return View(role);
        }

        public ActionResult AjaxUserForm()
        {
            var id = ArrId.FirstOrDefault();
            var user = _userDa.GetListAll();
            ViewBag.Id = id;
            ViewBag.ActionText = ActionText;
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

            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(module);
                        _moduleDa.Add(module);
                        module.IsShow = true;
                        module.IsDelete = false;
                        _moduleDa.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = module.ID.ToString(),
                            Message = string.Format("Đã thêm mới chuyên mục: <b>{0}</b>", Server.HtmlEncode(module.NameModule))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;
                case ActionType.RoleModule:
                    try
                    {
                        var rId = Guid.Parse(Request["UserID"]);
                        int moduleId = Convert.ToInt16(Request["ItemID"]);
                        module = _moduleDa.GetById(moduleId);
                        var list = module.aspnet_Roles.Any(m => m.RoleId == rId);
                        if (!list)
                        {
                            var rolemo = _moduleDa.GetByRoleId(rId);
                            module.aspnet_Roles.Add(rolemo);
                            foreach (var roleM in rolemo.ActiveRoles.Select(role => new Role_ModuleActive
                            {
                                ModuleId = moduleId,
                                RoleId = rId,
                                Check = false,
                                ActiveRoleId = role.Id,
                                Active = true
                            }))
                            {
                                module.Role_ModuleActive.Add(roleM);
                                _moduleDa.Save();
                            }
                        }

                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = module.ID.ToString(),
                            Message = string.Format("Đã Gán Module Cho Role: <b>{0}</b>", Server.HtmlEncode(rId.ToString()))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;

                case ActionType.UserModule:
                    try
                    {
                        var userId = Guid.Parse(Request["PrarentID"]);
                        int id = Convert.ToInt16(Request["ItemID"]);
                        module = _moduleDa.GetById(id);
                        var user = _moduleDa.GetByUserId(userId);
                        var kiemtra = module.aspnet_Users.Select(c => c.UserId == userId);
                        if (!kiemtra.Any())
                        {

                            module.aspnet_Users.Add(user);
                            var firstOrDefault = user.aspnet_Roles.FirstOrDefault();
                            if (firstOrDefault != null)
                                foreach (var userModuleActive in firstOrDefault.ActiveRoles.Select(moduleactive => new User_ModuleActive
                                {
                                    ID = 1,
                                    ModuleId = id,
                                    UserId = userId,
                                    ActiveRoleId = moduleactive.Id,
                                    Active = true,
                                    Check = 1
                                }))
                                {
                                    module.User_ModuleActive.Add(userModuleActive);
                                    _moduleDa.Save();
                                }
                            else
                            {
                                msg = new JsonMessage
                                {
                                    Erros = true,
                                    ID = module.ID.ToString(),
                                    Message = string.Format("<b>{0}</b> Phải gán Role: ", Server.HtmlEncode(user.UserName))
                                };
                                break;
                            }
                        }
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = module.ID.ToString(),
                            Message = string.Format("Đã Gán Module: <b>{0}</b>", Server.HtmlEncode(user.UserName))
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
                        _moduleDa.Save();
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
