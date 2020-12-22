using System;
using System.Linq;
using System.Web.Mvc;
using FDI.Areas.Admin.Controllers;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Areas.Setting.Controllers
{
    public class ModulesController : Controller
    {
        readonly ModuleControlDL _da = new ModuleControlDL();
        readonly ModulePageDA _pageDa = new ModulePageDA();

        public PartialViewResult ReWrite(object model)
        {
            return PartialView(model);
        }

        public PartialViewResult EditModule(string doAction, string layout, int ctrId = 0)
        {
            //Delete
            var model = new ModelPageItem
            {
                DoAction = doAction,
                CtrId = ctrId,
                ModeItem = new ModeItem()
            };
            if (doAction == "delete") return PartialView(model);

            //Edit
            var lst = Config.GetAction(WebConfig.ModuleArea);
            model.LstSection = Config.GetSectsion(layout).Where(c => c != "MenuAdmin" && c != "metas").ToList();
            model.LstModules = lst.GroupBy(c => c.Controller).Select(u => u.Key).ToList();
            if (doAction != "edit") return PartialView(model);

            //Add
            model.ModeItem = _da.GetModulById(ctrId);
            model.LstAction = lst.Where(c => c.Controller == model.ModeItem.Module).Select(c => c.Action).ToList();
            return PartialView(model);
        }

        public string AjaxGetAction(string name)
        {
            var lst = Config.GetAction(WebConfig.ModuleArea).Where(c => c.Controller == name).ToList();
            return lst.Aggregate("", (current, item) => current + string.Format("<option value='{0}'>{0}</option>", item.Action));
        }

        public string AjaxGetSection(string layout)
        {
            var lst = Config.GetSectsion(layout).Where(c => c != "MenuAdmin" && c != "metas").ToList();
            return lst.Aggregate("", (current, item) => current + string.Format("<option value='{0}'>{0}</option>", item));
        }

        public PartialViewResult ModuleCopy(int ctrId)
        {
            var model = _pageDa.GetChildByParentId();
            return PartialView(model);
        }

        public bool SaveModule(string doAction)
        {
            try
            {
                var sys = new ModuleControl();
                var ctrId = Request["ctrId"];
                switch (doAction)
                {
                    case "add":
                        UpdateModel(sys);
                        sys.CreateDate = DateTime.Now;
                        sys.LanguageID = Fdisystem.LanguageId;
                        _da.Add(sys);
                        _da.Save();
                        break;
                    case "edit":
                        sys = _da.GetItemModule(int.Parse(ctrId));
                        UpdateModel(sys);
                        _da.Save();
                        break;
                    case "copy":
                        var cp = _da.GetItemModule(int.Parse(ctrId));
                        UpdateModel(sys);
                        sys.Module = cp.Module;
                        sys.Action = cp.Action;
                        sys.Sort = cp.Sort;
                        sys.CreateDate = DateTime.Now;
                        _da.Add(sys);
                        _da.Save();
                        if (cp.Module == "Html")
                        {
                            var itemHtml = _da.GetHtmlMap(int.Parse(ctrId));
                            var newhtml = new HtmlMap
                            {
                                IdCopy = int.Parse(ctrId),
                                IdHtml = itemHtml.IdHtml,
                                IdModule = sys.Id,
                                LanguageId = Utility.Getcookie("LanguageId")
                            };
                            _da.Add(newhtml);
                            _da.Save();
                        }
                        break;
                    case "delete":
                        sys = _da.GetItemModule(int.Parse(ctrId));
                        if (sys.Module == "Html")
                        {
                            var idhtml = sys.HtmlMaps.FirstOrDefault().IdHtml;
                            var count = _da.GetCountHtmlMap(idhtml ?? 0);
                            if (count <= 1)
                            {
                                var htmlcontent = _da.GetHtmlContent(idhtml ?? 0);
                                _da.Delete(htmlcontent);
                            }
                        }
                        _da.Delete(sys);
                        _da.Save();
                        break;
                    case "layout":
                        var pageId = Request["pageId"];
                        var layout = Request["LayoutNew"];
                        var page = _pageDa.Get(int.Parse(pageId));
                        page.Layout = layout;
                        _pageDa.Save();
                        break;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public ActionResult EditPage(string name)
        {
            if (name == "view")
            {
                Session["editPage"] = 0;
            }
            else
            {
                Session["editPage"] = 1;
            }
            return null;
        }

    }
}
