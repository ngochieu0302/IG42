using System.Linq;
using System.Web.Mvc;
using FDI.GetAPI;

namespace FDI.Web.Controllers
{
    public class CustomerByGroupController : BaseController
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
            return View(_api.GetDiscountRequest(Request.Url.Query));
        }
        public ActionResult AjaxView()
        {
            var customerType = _api.GetCustomerItem(ArrId.FirstOrDefault());
            var model = customerType;
            return View(model);
        } 
    }
}
