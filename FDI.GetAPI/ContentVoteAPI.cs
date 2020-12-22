using System;
using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;
namespace FDI.GetAPI
{
    public class ContentVoteAPI : BaseAPI
    {
        private readonly string _url = Domain;

        public ModelContentVoteItem ListItems(int agencyId, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}ContentVote/ListItems{1}&key={2}&agencyId={3}", _url, url, Keyapi, agencyId);
            return GetObjJson<ModelContentVoteItem>(urlJson);
        }
        public ModelContentVoteItem GetListExport(int agencyId, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}ContentVote/GetListExport{1}&key={2}&agencyId={3}", _url, url, Keyapi, agencyId);
            return GetObjJson<ModelContentVoteItem>(urlJson);
        }

        public ModelContentVoteItem ListItemsUser(int agencyId, Guid userId, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}ContentVote/ListItemsUser{1}&key={2}&agencyId={3}&userId={4}", _url, url, Keyapi, agencyId, userId);
            return GetObjJson<ModelContentVoteItem>(urlJson);
        }

        public ModelContentVoteItem ListItemsUserNew(int agencyId, string userId, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}ContentVote/ListItemsUserNew{1}&key={2}&agencyId={3}&userId={4}", _url, url, Keyapi, agencyId, userId);
            return GetObjJson<ModelContentVoteItem>(urlJson);
        }

        public List<GeneralVoteItem> GeneralListTotal(int year, int agencyId)
        {
            var urlJson = string.Format("{0}ContentVote/GeneralListTotal?key={1}&agencyId={2}&year={3}", _url, Keyapi, agencyId, year);
            return GetObjJson<List<GeneralVoteItem>>(urlJson);
        }

        public SumContentVoteItem SumContentVoteItem(Guid? userid)
        {
            var urlJson = string.Format("{0}ContentVote/SumContentVoteItem?key={1}&userid={2}", _url, Keyapi, userid);
            return GetObjJson<SumContentVoteItem>(urlJson);
        }

        public List<ContentVoteItem> ListItemByUserId(int agencyId, string date, Guid? userid)
        {
            var urlJson = string.Format("{0}ContentVote/ListItemByUserId?key={1}&agencyId={2}&userid={3}&date={4}", _url, Keyapi, agencyId, userid, date);
            return GetObjJson<List<ContentVoteItem>>(urlJson);
        }

        public JsonMessage AddUpdate(string json, int agencyId, Guid userId)
        {
            var urlJson = string.Format("{0}ContentVote/AddUpdate?key={1}&{2}&agencyId={3}&userId={4}", Domain, Keyapi, json, agencyId, userId);
            return GetObjJson<JsonMessage>(urlJson);
        }

    }
}
