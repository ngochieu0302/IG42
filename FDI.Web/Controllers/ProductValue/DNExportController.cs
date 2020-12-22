using System.Linq;
using System.Web.Mvc;
using FDI.CORE;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers.Product
{
    public class DNExportController : BaseController
    {
        private readonly DNExportAPI _api = new DNExportAPI();
        private readonly DNUserAPI _userApi = new DNUserAPI();
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
            var model = _api.GetItemById(ArrId.FirstOrDefault());
            return View(model);
        }
        public ActionResult AjaxForm()
        {
            var model = new DNExportItem();
            if (DoAction == ActionType.Edit)
                model = _api.GetItemById(ArrId.FirstOrDefault());            
            ViewBag.UserID = UserItem.UserId;
            ViewBag.User = UserItem.UserName;
            ViewBag.Action = DoAction;
            ViewBag.LstUser = _userApi.GetAll(UserItem.AgencyID);
            return View(model);
        }
        public ActionResult AutoInport()
        {
            var query = Request["query"];
            var ltsResults = _api.GetListAuto(query, 10, UserItem.AgencyID, 3);
            var resulValues = new AutoCompleteProduct
            {
                query = query,
                suggestions = ltsResults.Where(x => (x.Quantity ?? 0) - (x.QuantityOut ?? 0) > 0).Select(c => new SuggestionsProduct
                {
                    ID = c.ID,
                    value = c.value,
                    title = c.title,
                    code = c.code,
                    QuantityDay = c.QuantityDay,
                    Date = c.Date,
                    StrDate = c.Date.DecimalToString("dd/MM/yyyy"),
                    StrDateEnd = c.DateEnd.DecimalToString("dd/MM/yyyy"),
                    data = "Giá: " + string.Format("{0:0,0}", c.pricenew),
                    name = "SL: " + string.Format("{0:0.##}", (c.Quantity ?? 0) - (c.QuantityOut ?? 0)) + " - Ngày nhập: " + c.Date.DecimalToString("dd/MM/yyyy") + " - HSD: " + c.DateEnd.DecimalToString("dd/MM/yyyy"),
                    Type = c.Type,
                    Unit = c.Unit,
                    pricenew = c.pricenew,
					Quantity = c.Quantity,
					QuantityOut = c.QuantityOut
                }),
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
                    var lst1 = string.Join(",", ArrId);
                    msg = _api.Delete(lst1);
                    break;
                default:
                    msg.Erros = true;
                    msg.Message = "Bạn không có quyền thực hiện chứ năng này.";
                    break;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
