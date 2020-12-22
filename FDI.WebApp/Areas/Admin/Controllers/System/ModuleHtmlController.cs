using System;
using System.Linq;
using System.Web.Mvc;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Areas.Admin.Controllers
{
    public class ModuleHtmlController : BaseController
    {
        readonly HtmlDA _da = new HtmlDA();
        readonly ModulePageDA _sysPageDa = new ModulePageDA();
        public ActionResult Index()
        {
            var model = _sysPageDa.GetChildByParentId();
            return View(model);
        }

        public ActionResult ListItems()
        {
            var id = Request["PageID"]??"0";
            var model = _da.GetList(int.Parse(id));
            return View(model);
        }

        public ActionResult AjaxForm()
        {
            var model = new HtmlItem();
            if (DoAction == ActionType.Edit)
                model = _da.GetHtmlid(ArrId.FirstOrDefault());
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View(model);
        }

        public ActionResult AjaxView()
        {
            var model = _da.GetHtmlid(ArrId.FirstOrDefault());
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            try
            {
                switch (DoAction)
                {
                    case ActionType.Edit:
                        try
                        {
                            var html = _da.GetByid(ArrId.FirstOrDefault());
                            UpdateModel(html);
                            _da.Save();
                            msg = new JsonMessage
                            {
                                Erros = false,
                                ID = html.ID.ToString(),
                                Message = "Đã cập nhật bài viết"
                            };
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Instance.LogError(GetType(), ex);
                        }

                        break;

                    case ActionType.Delete:
                        var module = _da.GetItemModule(ArrId.FirstOrDefault());
                        var htmlMap = module.HtmlMaps.FirstOrDefault();
                        if (htmlMap != null)
                        {
                            var idhtml = htmlMap.IdHtml;
                            var count = _da.GetCountHtmlMap(idhtml ?? 0);
                            _da.Delete(module);
                            _da.Save();
                            if (count <= 1)
                            {
                                var htmlcontent = _da.GetByid(idhtml ?? 0);
                                _da.Delete(htmlcontent);
                                _da.Save();
                            }
                        }
                        msg = new JsonMessage
                        {
                            Erros = false,
                            Message = "Đã cập xóa module Html"
                        };
                        break;
                }
            }
            catch (Exception ex)
            {
            }
            if (string.IsNullOrEmpty(msg.Message))
            {
                msg.Message = "Không có hành động nào được thực hiện.";
                msg.Erros = true;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

    }
}
