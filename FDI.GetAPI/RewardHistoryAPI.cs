using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class RewardHistoryAPI : BaseAPI
    {
        public ModelRewardHistoryItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}RewardHistory/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyid);
            return GetObjJson<ModelRewardHistoryItem>(urlJson);
        }

        public List<RewardHistoryItem> GetList(int agencyId, int month, int year, int cusId)
        {

            var urlJson = string.Format("{0}RewardHistory/GetList?key={1}&agencyId={2}&month={3}&year={4}&cusId={5}", Domain, Keyapi, agencyId, month, year, cusId);
            return GetObjJson<List<RewardHistoryItem>>(urlJson);
        }
        public List<RewardHistoryItem> GetListByCustomer(int page, int id, int agencyId)
        {
            var urlJson = string.Format("{0}RewardHistory/GetListByCustomer?key={1}&page={2}&id={3}&agencyId={4}", Domain, Keyapi, page, id, agencyId);
            return GetObjJson<List<RewardHistoryItem>>(urlJson);
        }
        public JsonMessage AddRewardLocal(string json)
        {
            var urlJson = string.Format("{0}RewardHistory/AddRewardLocal?key={1}&json={2}", DomainSv, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        } 
    }
}
