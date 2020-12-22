using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA.DA.CheckOrigin;
using FDI.GetAPI;
using FDI.GetAPI.CheckOrigin;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers.CheckOrigin
{
    public class SourceController : BaseController
    {
        //
        // GET: /Source/
        readonly SourceAPI _api = new SourceAPI();
        readonly SourceDA _da = new SourceDA("#");
        readonly StorageProductAPI _storageProductApi = new StorageProductAPI();
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
            var model = new Source();
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            if (DoAction == ActionType.Edit)
            {
                model = _da.GetById(ArrId.FirstOrDefault());
            }
            ViewBag.lstStoraproduct = _storageProductApi.GetListSimple();
            return View(model);
        }

        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var url = Request.Form.ToString();
            url = HttpUtility.UrlDecode(url);
            switch (DoAction)
            {
                case ActionType.Add:
                    msg = _api.Add(url);
                    break;
                case ActionType.Edit:
                    msg = _api.Update(url);
                    break;

                case ActionType.Delete:
                    var lst = string.Join(",", ArrId);
                    msg = _api.Delete(lst);
                    break;
                default:
                    msg.Message = "Bạn chưa được phân quyền cho chức năng này.";
                    msg.Erros = true;
                    break;
            }
            return Json(msg,JsonRequestBehavior.AllowGet);
        }

    }
}
