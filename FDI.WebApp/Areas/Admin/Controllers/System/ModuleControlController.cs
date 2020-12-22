using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.DA.DA;
using FDI.Simple;
using FDI.Utils;
using System;

namespace FDI.Areas.Admin.Controllers
{
    public class ModuleControlController : BaseController
    {
        //
        // GET: /Admin/CustomerContact/
        private readonly ModuleControlDA _da = new ModuleControlDA("#");
        readonly ModulePageDA _pageDa = new ModulePageDA();

        public ActionResult Index()
        {
            var model = _pageDa.GetChildByParentId();
            return View(model);
        }

        public ActionResult ListItems()
        {
            var lstNews = _da.GetListByRequest(Request);
            var model = new ModelModuleControlItem
            {
                Container = Request["Container"],
                ListItem = lstNews,
                PageHtml = _da.GridHtmlPage
            };
            return View(model);
        }

        public ActionResult AjaxForm()
        {
            var model = new ModelModuleControlItem
            {
                ModuleControlItem = new ModuleControlItem(),
                LstSection = new List<string>(),
                LstAction = new List<string>()
            };
            var lst = Config.GetAction(WebConfig.ModuleArea);
            if (DoAction == ActionType.Edit)
            {
                model.ModuleControlItem = _da.GetItemById(ArrId.FirstOrDefault());
                model.LstSection = Config.GetSectsion(model.ModuleControlItem.Layout).Where(c => c != "MenuAdmin" && c != "metas").ToList();
                model.LstAction = lst.Where(c => c.Controller == model.ModuleControlItem.Module).Select(c => c.Action).ToList();
            }
            model.Action = DoAction.ToString();
            model.ActionText = ActionText;
            model.LstModules = lst.GroupBy(c => c.Controller).Select(u => u.Key).ToList();
            model.SysPageItems = _pageDa.GetChildByParentId();
            return View(model);
        }

        public string AjaxGetSection(string layout)
        {
            var lst = Config.GetSectsion(layout).Where(c => c != "MenuAdmin" && c != "metas").ToList();
            return lst.Aggregate("", (current, item) => current + string.Format("<option value='{0}'>{0}</option>", item));
        }

        public string AjaxGetAction(string name)
        {
            var lst = Config.GetAction(WebConfig.ModuleArea).Where(c => c.Controller == name).ToList();
            return lst.Aggregate("", (current, item) => current + string.Format("<option value='{0}'>{0}</option>", item.Action));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage(false, "");
            var model = new ModuleControl();
            List<ModuleControl> lst;
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(model);
                        model.LanguageID = Fdisystem.LanguageId;
                        _da.Add(model);
                        _da.Save();
                        msg.Message = "Thêm mới dữ liệu thành công.";

                    }
                    catch (Exception ex)
                    {
                        msg.Erros = false;
                        msg.Message = "Thêm mới dữ liệu thất bại.";
                    }
                    break;

                case ActionType.Edit:
                    try
                    {
                        model = _da.GetById(ArrId.FirstOrDefault());
                        UpdateModel(model);
                        //model.LanguageID = Fdisystem.LanguageId;
                        _da.Add(model);
                        _da.Save();
                        msg.Message = "Cập nhật dữ liệu thành công.";
                    }
                    catch (Exception ex)
                    {
                        msg.Erros = false;
                        msg.Message = "Cập nhật dữ liệu thât bại.";
                    }
                    break;

                case ActionType.Delete:
                    try
                    {
                        lst = _da.GetByArrId(ArrId);
                        foreach (var item in lst)
                        {
                            _da.Delete(item);

                        }
                        _da.Save();
                        msg.Message = "Xóa dữ liệu thành công.";
                    }
                    catch (Exception ex)
                    {
                        msg.Erros = false;
                        msg.Message = "Xóa dữ liệu thât bại.";
                    }
                    break;
                default:
                    msg.Message = "Không có hành động nào được thực hiện.";
                    msg.Erros = true;
                    break;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
