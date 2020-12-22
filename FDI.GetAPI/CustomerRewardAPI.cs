using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;
using System;

namespace FDI.GetAPI
{
    public class CustomerRewardAPI : BaseAPI
    {
        public ModelCustomerRewardItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}CustomerReward/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyid);
            return GetObjJson<ModelCustomerRewardItem>(urlJson);
        }
        public ModelDNUserItem GetListRewardRequest(int agencyId, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}CustomerReward/GetListRewardRequest{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyId);
            return GetObjJson<ModelDNUserItem>(urlJson);
        }
        public ModelCustomerRewardItem GetListCustomerByRequest(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}CustomerReward/GetListCustomerByRequest{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyid);
            return GetObjJson<ModelCustomerRewardItem>(urlJson);
        }
        public List<CustomerRewardItem> GetListByCustomer(int page, int id, int agencyId)
        {
            var urlJson = string.Format("{0}CustomerReward/GetListByCustomer?key={1}&page={2}&id={3}&agencyId={4}", Domain, Keyapi, page, id, agencyId);
            return GetObjJson<List<CustomerRewardItem>>(urlJson);
        }
        public ModelCustomerRewardItem GetListUserRequest(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}CustomerReward/GetListUserRequest{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyid);
            return GetObjJson<ModelCustomerRewardItem>(urlJson);
        }
        public CustomerRewardItem GetCustomerRewardItem(int agencyId, int cusId, int month, int year, Guid Userid)
        {
            var urlJson = string.Format("{0}CustomerReward/GetCustomerRewardItem?key={1}&agencyId={2}&cusId={3}&month={4}&year={5}&UserId={6}", Domain, Keyapi, agencyId, cusId, month, year, Userid);
            return GetObjJson<CustomerRewardItem>(urlJson);
        }

        public JsonMessage Add(string url, int agencyId)
        {
            var urlJson = string.Format("{0}CustomerReward/Add?key={1}&{2}&agencyId={3}", Domain, Keyapi, url, agencyId);
            return GetObjJson<JsonMessage>(urlJson);
        }
    }
}
