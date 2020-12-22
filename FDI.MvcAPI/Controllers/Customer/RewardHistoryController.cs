using System.Collections.Generic;
using System.Web.Mvc;
using FDI.DA;
using FDI.Simple;

namespace FDI.MvcAPI.Controllers
{
    public class RewardHistoryController : BaseApiController
    {
        //
        // GET: /RewardHistory/
        readonly RewardHistoryDA _da = new RewardHistoryDA("#");
        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelRewardHistoryItem()
                : new ModelRewardHistoryItem { ListItems = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetList(int agencyId, int month, int year, int cusId)
        {
            var obj = Request["key"] != Keyapi
                ? new List<RewardHistoryItem>()
                : _da.GetList(Agencyid(), month, year, cusId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListByCustomer(int page, int id, int agencyId)
        {
            var obj = Request["key"] != Keyapi
               ? new List<RewardHistoryItem>()
               : _da.GetListByCustomer(page, id, agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }
}
