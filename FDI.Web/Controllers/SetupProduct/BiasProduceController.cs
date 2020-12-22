using System.Linq;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;
using NPOI.SS.Formula.Functions;

namespace FDI.Web.Controllers
{
    public class BiasProduceController : BaseController
    {

        private readonly BiasProduceAPI _api = new BiasProduceAPI();
        private readonly SetupProductionAPI _setupAPI = new SetupProductionAPI();
        readonly DNUserAPI _dnUserApi = new DNUserAPI();

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
            var model = _api.GetBiasProduceItem(ArrId.FirstOrDefault());
            return View(model);
        }

        public ActionResult AjaxForm()
        {
            var i = new int();
            var model = new BiasProduceItem();
            if (DoAction == ActionType.Edit)
                model = _api.GetBiasProduceItem(ArrId.FirstOrDefault());
            model.SetupProductionItems = _setupAPI.GetList(UserItem.AgencyID);
            ViewBag.Action = DoAction;
            ViewBag.AgencyId = UserItem.AgencyID;
            return View(model);
        }

        public ActionResult AutoUser()
        {
            var query = Request["query"];
            var ltsResults = _dnUserApi.GetListAuto(query, 10, UserItem.AgencyID);
            var resulValues = new AutoCompleteProduct
            {
                query = query,
                suggestions = ltsResults
            };
            return Json(resulValues, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var url = Request.Form.ToString();
            switch (DoAction)
            {
                case ActionType.Add:
                    msg = _api.Add(url, UserItem.AgencyID, CodeLogin());
                    break;

                case ActionType.Edit:
                    msg = _api.Update(url, UserItem.AgencyID, CodeLogin());
                    break;
                case ActionType.Delete:
                    msg = _api.Delete(string.Join(",", ArrId));
                    msg.ID = string.Join(",", ArrId);
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
