using System.Web.Mvc;
using FDI.GetAPI;

namespace FDI.Web.Controllers
{
    public class CustomerRewardController : BaseController
    {
        // GET: /Admin/Order/       
        CustomerRewardAPI _api = new CustomerRewardAPI();
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
    }
}
