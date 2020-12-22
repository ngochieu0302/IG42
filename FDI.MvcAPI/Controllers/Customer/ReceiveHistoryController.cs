using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers
{
	public class ReceiveHistoryController : BaseApiController
	{
		//
		// GET: /RewardHistory/
		readonly ReceiveHistoryDA _da = new ReceiveHistoryDA("#");
		public ActionResult ListItems()
		{
			var obj = Request["key"] != Keyapi
				? new ModelRewardHistoryItem()
                : new ModelRewardHistoryItem { ListItems = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
			return Json(obj, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetListByCustomer(int page, int id, int agencyId)
		{
			var obj = Request["key"] != Keyapi
			   ? new List<RewardHistoryItem>()
			   : _da.GetListByCustomer(page, id, agencyId);
			return Json(obj, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetDetailByOrderById(int id)
		{
			var obj = Request["key"] != Keyapi ? new RewardHistoryItem() : _da.GetDetailByOrderById(id);
			return Json(obj, JsonRequestBehavior.AllowGet);
		}
        public ActionResult AddTranfer(string key, string moneyTranfer, string agencyId, string customerId)
        {
            // var model = new WalletCustomer();
            try
            {
                var wallet = new ReceiveHistory
                {
                    AgencyId = int.Parse(agencyId),
                    CustomerID = int.Parse(customerId),
                    DateCreate = DateTime.Now.TotalSeconds(),
                    IsDeleted = false,
                    Date = DateTime.Now.TotalSeconds(),
                    Month = DateTime.Now.Month,
                    Year = DateTime.Now.Year,
                    Type = (int)Reward.Receive1,
                    Price = decimal.Parse(moneyTranfer)
                };
                _da.Add(wallet);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
	}

}