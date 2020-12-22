using System.Linq;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class ProductionCostController : BaseController
    {

        private readonly ProductionCostAPI _api = new ProductionCostAPI();
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
            var model = new ProductionCostItem();
            if (DoAction == ActionType.Edit)
                model = _api.GetUnitItem(ArrId.FirstOrDefault());
            ViewBag.Action = DoAction;
            ViewBag.AgencyId = UserItem.AgencyID;
            return View(model);
        }
        
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage {Erros = false};
            var url = Request.Form.ToString();
            switch (DoAction)
            {
                case ActionType.Add:
                    if (_api.AddUnit(url) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra!";
                    }else
                    msg.Message = "Cập nhật dữ liệu thành công !";
                    break;

                case ActionType.Edit:
                    if (_api.UpdateUnit(url) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra!";
                    }else
                    msg.Message = "Cập nhật dữ liệu thành công !";
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
