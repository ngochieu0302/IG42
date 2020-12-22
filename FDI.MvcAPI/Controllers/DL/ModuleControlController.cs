using System;
using System.Linq;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers
{
    public class ModuleControlController : BaseApiController
    {
        readonly ModuleControlDL _dl = new ModuleControlDL();
        readonly ModulePageDL _pageDa = new ModulePageDL();
        public JsonResult GetModulById(int id)
        {
            var obj = Request["key"] != Keyapi
                ? new ModeItem() : _dl.GetModulById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetCountHtmlMap(int id)
        {
            var obj = Request["key"] != Keyapi
                ? new int() : _dl.GetCountHtmlMap(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public int SaveModule(string key)
        {
            try
            {
                if (key != Keyapi) return 0;
                var doAction = Request["DoAction"];
                var ctrId = Convert.ToInt32(Request["ctrId"]);
                var pageId = Convert.ToInt32(Request["PageID"]);
                var action = Request["Action"];
                var module = Request["Module"];
                var section = Request["Section"];
                var sort = Convert.ToInt32(Request["Sort"]);
                var sys = new ModuleControl();
                switch (doAction)
                {
                    case "add":
                        UpdateModel(sys);
                        sys.CreateDate = DateTime.Now;
                        sys.Action = action;
                        sys.AgencyID = Agencyid();
                        sys.PageID = pageId;
                        sys.LanguageID = "vi";
                        _dl.Add(sys);
                        _dl.Save();
                        break;
                    case "edit":
                        sys = _dl.GetItemModule(ctrId);
                        sys.Action = action;
                        sys.Module = module;
                        sys.Section = section;
                        sys.Sort = sort;
                        _dl.Save();
                        break;
                    case "copy":
                        var cp = _dl.GetItemModule(ctrId);
                        UpdateModel(sys);
                        sys.Module = cp.Module;
                        sys.Action = cp.Action;
                        sys.AgencyID = Agencyid();
                        sys.LanguageID = "vi";
                        sys.Sort = cp.Sort;
                        sys.CreateDate = DateTime.Now;
                        _dl.Add(sys);
                        _dl.Save();
                        if (cp.Module == "Html")
                        {
                            var itemHtml = _dl.GetHtmlMap(ctrId);
                            var newhtml = new HtmlMap
                            {
                                IdCopy = ctrId,
                                IdHtml = itemHtml.IdHtml,
                                IdModule = sys.Id,
                                LanguageId = Utility.Getcookie("LanguageId")
                            };
                            _dl.Add(newhtml);
                            _dl.Save();
                        }
                        break;
                    case "delete":
                        sys = _dl.GetItemModule(ctrId);
                        if (sys.Module == "Html")
                        {
                            var idhtml = sys.HtmlMaps.FirstOrDefault().IdHtml;
                            var count = _dl.GetCountHtmlMap(idhtml ?? 0);
                            if (count <= 1)
                            {
                                var htmlcontent = _dl.GetHtmlContent(idhtml ?? 0);
                                _dl.Delete(htmlcontent);
                            }
                        }
                        _dl.Delete(sys);
                        _dl.Save();
                        break;
                    case "layout":
                        var layout = Request["LayoutNew"];
                        var page = _pageDa.Get(pageId);
                        page.Layout = layout;
                        _pageDa.Save();
                        break;
                }
                return 1;
            }
            catch
            {
                return 0;
            }
        }
    }
}
