using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class ProductComingController : BaseController
    {
        readonly ProductComingDA _da = new ProductComingDA("#");
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            var model = new ModelProductComingItem();
            model.ListItems = _da.GetListSimpleByRequest(Request);
            model.PageHtml = _da.GridHtmlPage;
            return View(model);
        }
        public ActionResult AjaxForm()
        {
            var model = new Shop_Product_Comingsoon();
            if (DoAction == ActionType.Edit)
            {
                model = _da.GetById(ArrId.FirstOrDefault());
            }
            ViewBag.Action = DoAction;
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Actions()
        {
            var msg = new JsonMessage();
            switch (DoAction)
            {
                case ActionType.Edit:
                   var model = _da.GetById(ArrId.FirstOrDefault());
                    var date = Request["_DateEx"];
                    if (!string.IsNullOrEmpty(date))
                    {
                        model.DateEx = date.StringToDecimal();
                    }
                    UpdateModel(model);
                    _da.Save();
                    msg = new JsonMessage()
                    {
                        Erros = false,
                        Message = "Cập nhật thành công."
                    };
                    break;

                case ActionType.Delete:
                    var lstArrId = string.Join(",", ArrId);
                    var lst = _da.GetById(FDIUtils.StringToListInt(lstArrId));
                    foreach (var item in lst)
                    {
                        _da.Delete(item);
                    }
                    _da.Save();
                    msg = new JsonMessage()
                    {
                        Erros = false,
                        Message = "Cập nhật thành công."
                    };
                    break;
                default:
                    msg.Message = "Bạn chưa được phân quyền cho chức năng này.";
                    msg.Erros = true;
                    break;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
