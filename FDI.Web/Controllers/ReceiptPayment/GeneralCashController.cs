using System.Linq;
using System.Web.Mvc;
using FDI.GetAPI;

namespace FDI.Web.Controllers
{
    public class GeneralCashController : BaseController
    {
        // GET: /Admin/Order/
        private readonly CashAdvanceAPI _api = new CashAdvanceAPI();        
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.GetListGeneralCash(UserItem.AgencyID, Request.Url.Query));
        }
        public ActionResult AjaxView()
        {            
            return View();
        }      
        
    }
}
