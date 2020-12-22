using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.GetAPI.QLCN;
using FDI.Simple;
using FDI.Simple.QLCN;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class CateCNController : BaseController
    {
        //
        // GET: /CateCN/
        private readonly CateCNAPI _api = new CateCNAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.ListItems(Request.Url.Query));
        }
        public ActionResult AjaxForm()
        {
            var model = new CateCNItem();
            if (DoAction == ActionType.Edit)
                model = _api.GetCateCNItem(ArrId.FirstOrDefault());
            ViewBag.Action = DoAction;
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage { Erros = false, Message = "Cập nhật dữ liệu thành công." };
            var url = Request.Form.ToString();
            url = HttpUtility.UrlDecode(url);
            switch (DoAction)
            {
                case ActionType.Add:
                    if (_api.Add(url) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Cập nhật thất bại.";
                    }
                    break;

                case ActionType.Edit:
                    if (_api.Update(url) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Cập nhật thất bại.";
                    }
                    break;
                case ActionType.Delete:
                    if (_api.Delete(url) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Cập nhật thất bại.";
                    }
                    break;
                default:
                    msg.Erros = true;
                    msg.Message = "Bạn không được phần quyển cho chức năng này.";
                    break;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
