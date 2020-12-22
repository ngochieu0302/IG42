using System.Linq;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class CustomerController : BaseController
    {
        //
        // GET: /Admin/Customer/
        private readonly CustomerAPI _api = new CustomerAPI();
        private readonly CustomerGroupAPI _groupApi = new CustomerGroupAPI();
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
            var customerType = _api.GetCustomerItem(ArrId.FirstOrDefault());
            var model = customerType;
            return View(model);
        }
        public ActionResult AjaxHistory()
        {
            return View();
        }
        public ActionResult AjaxForm()
        {
            var model = new CustomerItem
            {
                IsActive = true
            };
            ViewBag.ListCity = _api.GetListCity();
            if (DoAction == ActionType.Edit)
            {
                model = _api.GetCustomerItem(ArrId.FirstOrDefault());
                ViewBag.ListDistrict = _api.GetListDistrict(model.CityID ?? 0);
            }
            ViewBag.Action = DoAction;
            ViewBag.AgencyId = UserItem.AgencyID;
            ViewBag.Group = _groupApi.GetList(UserItem.AgencyID);
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
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult District(int cityId, int districtId = 0)
        {
            ViewBag.districtId = districtId;
            var model = _api.GetListDistrict(cityId);
            return PartialView(model);
        }
        public ActionResult GetDistrict(int cityId)
        {
            var model = _api.GetListDistrict(cityId);
            return Json(model);
        }
        public string CheckByUserName(string UserName, int id)
        {
            var result = _api.CheckUserName(UserName.Trim(), id);
            return result == 1 ? "false" : "true";
        }
        public string CheckByEmail(string Email, int id)
        {
            var result = _api.CheckEmail(Email.Trim(), id);
            return result == 1 ? "false" : "true";
        }
        public string CheckByPhone(string Phone, int id)
        {
            var result = _api.CheckPhone(Phone.Trim(), id);
            return result == 1 ? "false" : "true";
        }
        public string CheckCardSerial(string CardSerial)
        {
            var result = _api.CheckCardSerial(CardSerial);
            return result == 1 ? "true" : "false";
        }

    }
}
