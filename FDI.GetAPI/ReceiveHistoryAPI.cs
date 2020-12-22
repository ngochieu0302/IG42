using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class ReceiveHistoryAPI : BaseAPI
    {
        public ModelRewardHistoryItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}ReceiveHistory/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi,agencyid);
            return GetObjJson<ModelRewardHistoryItem>(urlJson);
        }

        public List<RewardHistoryItem> GetListByCustomer(int page, int id, int agencyId)
        {
            var urlJson = string.Format("{0}ReceiveHistory/GetListByCustomer?key={1}&page={2}&id={3}&agencyId={4}", Domain, Keyapi, page, id, agencyId);
            return GetObjJson<List<RewardHistoryItem>>(urlJson);
        }

	    public RewardHistoryItem GetDetailByOrderById(int id)
	    {
            var urlJson = string.Format("{0}ReceiveHistory/GetDetailByOrderById?key={1}&id={2}", Domain, Keyapi,id);
		    return GetObjJson<RewardHistoryItem>(urlJson);
	    }
        public int AddTranfer(decimal tranfer, int agencyId, int id)
        {
            var urlJson = string.Format("{0}ReceiveHistory/AddTranfer?key={1}&moneyTranfer={2}&agencyId={3}&customerId={4}", Domain, Keyapi, tranfer, agencyId, id);
            return GetObjJson<int>(urlJson);
        }
        public JsonMessage AddReciveLocal(string json)
        {
            var urlJson = string.Format("{0}ReceiveHistory/AddReciveLocal?key={1}&json={2}", DomainSv, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
	}
}
