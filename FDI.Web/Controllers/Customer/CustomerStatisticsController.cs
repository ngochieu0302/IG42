using System.Web.Mvc;
using FDI.GetAPI;

namespace FDI.Web.Controllers.Product
{
    public class CustomerStatisticsController : BaseController
    {
        // GET: /Admin/Order/       
        readonly CustomerRewardAPI _api = new CustomerRewardAPI();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            return View(_api.GetListCustomerByRequest(UserItem.AgencyID, Request.Url.Query));
        }

        public ActionResult AjaxView()
        {
            return View();
        }
    }
}
