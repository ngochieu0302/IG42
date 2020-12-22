using System.Linq;
using System.Text;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;
namespace FDI.Web.Controllers
{
    public class ProductCodeController : BaseController
    {
        // GET: /Admin/Order/
        private readonly BiasProduceAPI _api = new BiasProduceAPI();
        public ActionResult Index()
        {
            ViewBag.BiasId = Request["BiasId"];
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.GetListProductCode(UserItem.AgencyID, Request.Url.Query));
        }

        public ActionResult AjaxView()
        {
            var model = _api.GetListEvaluate(ArrId.FirstOrDefault());
            //ViewBag.Agency = _agencyApi.GetItemById(UserItem.AgencyID ?? 0);
            return View(model);
        }
        public ActionResult AjaxForm(int biasId)
        {
            var model = _api.GetListCostProductUser(biasId);
            _api.GetListEvaluate(ArrId.FirstOrDefault());
            ViewBag.itemId = Request["itemId"];          
            ViewBag.Action = DoAction;            
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var lstRet = Request["lstRet"];
            var itemId = Request["itemId"];
            var status = Request["status"];
            switch (DoAction)
            {
                case ActionType.Add:

                    if (_api.AddEvaluate(UserItem.AgencyID, status, itemId, lstRet, UserItem.UserId) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra!";
                    }
                    else
                        msg.Message = "Cập nhật dữ liệu thành công !";
                    break;
                case ActionType.Edit:
                    if (_api.AddEvaluate(UserItem.AgencyID, status, itemId, lstRet, UserItem.UserId) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra!";
                    }
                    else
                        msg.Message = "Cập nhật dữ liệu thành công !";
                    break;
                case ActionType.Active:
                    msg.Message = "Đơn đặt hàng đã được duyệt!";
                    break;
                case ActionType.Delete:
                    var stbMessage = new StringBuilder();

                    msg.ID = string.Join(",", ArrId);
                    msg.Message = stbMessage.ToString();
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