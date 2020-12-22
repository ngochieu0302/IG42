using System.Linq;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class DNDrawerController : BaseController
    {
        private readonly DNDrawerAPI _api = new DNDrawerAPI();
        private readonly DNCabinetDocumentAPI _dnCabinetDocument = new DNCabinetDocumentAPI();
        
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.ListItems(UserItem.AgencyID, Request.Url.Query));
        }
        public ActionResult AjaxView()
        {
            return View();
        }
        public ActionResult AjaxForm()
        {
            var model = new DNDrawerItem();
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            ViewBag.CabinetDocumentID = _dnCabinetDocument.GetListSimple();
            if (DoAction == ActionType.Edit)
            {
                model = _api.GetItemsByID(ArrId.FirstOrDefault());
            }
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage { Erros = false };
            var url = Request.Form.ToString();
            switch (DoAction)
            {
                case ActionType.Add:
                    if (_api.Add(url) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra!";
                    }
                    msg.Message = "Cập nhật dữ liệu thành công !";
                    break;

                case ActionType.Edit:
                    if (_api.Update(url) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra!";
                    }
                    msg.Message = "Cập nhật dữ liệu thành công !";
                    break;

                case ActionType.Delete:
                    var lst1 = string.Join(",", ArrId);
                    _api.Delete(lst1);
                    msg.Message = "Cập nhật thành công.";
                    break;
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
