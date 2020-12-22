using System;
using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class VoteAPI : BaseAPI
    {
        public List<VoteItem> GetListSimple(int agencyId)
        {
            var urlJson = string.Format("{0}Vote/GetListSimple?key={1}&agencyId={2}", Domain, Keyapi, agencyId);
            return GetObjJson<List<VoteItem>>(urlJson);
        }
        public List<VoteItem> GetList(int treeid, int agencyId, int agencyid, string date, Guid userId)        
        {
            var urlJson = string.Format("{0}Vote/GetList?key={1}&agencyId={2}&treeid={3}&agencyId={4}&date={5}&userId={6}", Domain, Keyapi, agencyId, treeid, agencyid, date, userId);
            return GetObjJson<List<VoteItem>>(urlJson);
        }
        public VoteItem GetVoteItem(int id)
        {
            var urlJson = string.Format("{0}Vote/GetVoteItem?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<VoteItem>(urlJson);
        }
        public JsonMessage Add(string json)
        {
            var urlJson = string.Format("{0}Vote/Add?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Update(string json)
        {
            var urlJson = string.Format("{0}Vote/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public int Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}Vote/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }

        //
        public List<VoteItem> GetListSumUser(string userid, decimal dates, decimal datee)
        {
            var urlJson = string.Format("{0}Vote/GetListSumUser?key={1}&userid={2}&dates={3}&datee={4}", Domain, Keyapi, userid, dates, datee);
            return GetObjJson<List<VoteItem>>(urlJson);
        }

        //
        public List<DNUserVoteItem> GetSumListUser(int agencyid, decimal dates, decimal datee)
        {
            var urlJson = string.Format("{0}Vote/GetSumListUser?key={1}&agencyId={2}&dates={3}&datee={4}", Domain, Keyapi,agencyid, dates, datee);
            return GetObjJson<List<DNUserVoteItem>>(urlJson);
        }
    }
}
