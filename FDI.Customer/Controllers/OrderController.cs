using System.Linq;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;

namespace FDI.Customer.Controllers
{
    public class OrderController : BaseController
    {
        readonly OrderAPI _api = new OrderAPI();
        readonly DNAgencyAPI _agencyApi = new DNAgencyAPI();
        readonly CustomerRewardAPI _customerRewardApi = new CustomerRewardAPI();
        readonly ReceiveHistoryAPI _receiveHistoryApi = new ReceiveHistoryAPI();
        readonly RewardHistoryAPI _rewardHistoryApi = new RewardHistoryAPI();

        public ActionResult Index()
        {
            var listAgency = _agencyApi.GetByCustomer(UserItem.ID);
            var agencyId = Request["AgencyId"] != null ? int.Parse(Request["AgencyId"]) : listAgency.OrderByDescending(c => c.PriceReward).Select(c => c.ID).FirstOrDefault();
            var listReceive = _receiveHistoryApi.GetListByCustomer(1, UserItem.ID, agencyId);
            var listReward = _rewardHistoryApi.GetListByCustomer(1, UserItem.ID, agencyId);
            var listCus = _customerRewardApi.GetListByCustomer(1, UserItem.ID, agencyId);
            var model = new ModelRewardItem
            {
                TotalRecive = listCus != null ? listCus.Sum(c => c.TotalReceipt) : 0,
                TotalRewar = listCus != null ? listCus.Sum(c => c.TotalReward) : 0,
                ListAgency = listAgency,
                ListReceive = listReceive,
                ListReward = listReward,
                AgencyId = !string.IsNullOrEmpty(Request["AgencyId"]) ? int.Parse(Request["AgencyId"]) : 0
            };
            return View(model);
        }
        public ActionResult AjaxView(int id)
        {
            var model = _api.GetOrderItem(id);
            return PartialView(model);
        }

        public ActionResult ListReceive(int page, int agencyId)
        {
            ViewBag.STT = (page - 1) * 10 + 1;
            var model = _receiveHistoryApi.GetListByCustomer(page, UserItem.ID, agencyId);
            return PartialView(model);
        }
        public ActionResult ListReward(int page, int agencyId)
        {
            ViewBag.STT = (page - 1) * 10 + 1;
            var model = _rewardHistoryApi.GetListByCustomer(page, UserItem.ID, agencyId);
            return PartialView(model);
        }

    }
}
