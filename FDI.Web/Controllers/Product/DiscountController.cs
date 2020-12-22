using System.Linq;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class DiscountController : BaseController
    {

        private readonly DiscountAPI _api = new DiscountAPI();
        //readonly SupplieAPI supplieAPI = new SupplieAPI();
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult ListItems()
        {
            return View(_api.ListItems(Request.Url.Query, UserItem.AgencyID));
        }

        public ActionResult AjaxView()
        {
            return View();
        }
        public ActionResult AjaxForm()
        {
            var model = new ModelDiscountItem
            {
                Action = DoAction.ToString(),                
                DiscountItem = new DiscountItem()
            };
            if (DoAction == ActionType.Edit)
                model.DiscountItem = _api.GetItemById(ArrId.FirstOrDefault());           
            return View(model);
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
                    msg = _api.Add(url, UserItem.AgencyID);
                    break;

                case ActionType.Edit:
                    msg = _api.Update(url);
                    break;
	            case ActionType.Delete:
                    msg = _api.Delete(url);
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
