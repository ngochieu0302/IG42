using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;
using System;
using FDI.CORE;

namespace FDI.Web.Controllers
{
    public class GeneralContentVoteController : BaseController
    {
        // GET: /Admin/Order/
        private readonly ContentVoteAPI _api = new ContentVoteAPI();
        private readonly DNUserAPI _userApi = new DNUserAPI();

        public ActionResult Index()
        {            
            return View();
        }
        public ActionResult ListItems()
        {
            var year = Request["year"];            
            var y = string.IsNullOrEmpty(year) ? DateTime.Now.Year : int.Parse(year);
            var model = new ModelGeneralVoteItem
            {
                ListItems = _api.GeneralListTotal(y, UserItem.AgencyID),
                LstMonthItem = ConvertDateItem.ListMonthinYear(y),
                ListUser = _userApi.GetListSimple(UserItem.AgencyID)
            };
            return View(model);
        }
    }
}
