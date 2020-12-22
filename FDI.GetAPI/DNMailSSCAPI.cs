using System;
using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class DNMailSSCAPI : BaseAPI
    {
        private readonly string _url = Domain;
        public List<DNMailSSCItem> GetListSimpleByRequest(int agencyid, int type, Guid userId)
        {
            var urlJson = string.Format("{0}DNMailSSC/GetListSimpleByRequest?key={1}&agencyId={2}&type={3}&userId={4}", _url, Keyapi,agencyid, type, userId);
            return GetObjJson<List<DNMailSSCItem>>(urlJson);
        }

        public List<DNMailSSCItem> GetAll(int agencyid)
        {
            var urlJson = string.Format("{0}DNMailSSC/GetAll?key={1}&agencyId={2}", _url, Keyapi,agencyid);
            return GetObjJson<List<DNMailSSCItem>>(urlJson);
        }

        public DNMailSSCItem GetItemById(string code, int id = 0)
        {
            var urlJson = string.Format("{0}DNMailSSC/GetItemById?key={1}&code={2}&id={3}", _url, Keyapi, code, id);
            return GetObjJson<DNMailSSCItem>(urlJson);
        }


        public List<DNMailSSCItem> CountInboxNew(Guid userId, int type = 0)
        {
            var urlJson = string.Format("{0}DNMailSSC/CountInboxNew?key={1}&type={2}&userId={3}", _url, Keyapi, type, userId);
            return GetObjJson<List<DNMailSSCItem>>(urlJson);
        }

        public List<DNMailSSCItem> CountInbox(Guid userId, int type = 0)
        {
            var urlJson = string.Format("{0}DNMailSSC/CountInbox?key={1}&type={2}&userId={3}", _url, Keyapi, type, userId);
            return GetObjJson<List<DNMailSSCItem>>(urlJson);
        }

        public List<DNMailSSCItem> SentMail(Guid userId)
        {
            var urlJson = string.Format("{0}DNMailSSC/SentMail?key={1}&userId={2}", _url, Keyapi, userId);
            return GetObjJson<List<DNMailSSCItem>>(urlJson);
        }

        public List<DNMailSSCItem> CountDrafts(Guid userId)
        {
            var urlJson = string.Format("{0}DNMailSSC/CountDrafts?key={1}&userId={2}", _url, Keyapi, userId);
            return GetObjJson<List<DNMailSSCItem>>(urlJson);
        }

        public List<DNMailSSCItem> CountRecycleBin(Guid userId)
        {
            var urlJson = string.Format("{0}DNMailSSC/CountRecycleBin?key={1}&userId={2}", _url, Keyapi, userId);
            return GetObjJson<List<DNMailSSCItem>>(urlJson);
        }

        public List<DNMailSSCItem> CountSpam(Guid userId, int type = 0)
        {
            var urlJson = string.Format("{0}DNMailSSC/CountSpam?key={1}&type={2}&userId={3}", _url, Keyapi, type, userId);
            return GetObjJson<List<DNMailSSCItem>>(urlJson);
        }

        #region Customer
        public List<DNMailSSCItem> CustomerCountInboxNew(int customerId, int type = 0)
        {
            var urlJson = string.Format("{0}DNMailSSC/CustomerCountInboxNew?key={1}&type={2}&customerId={3}", _url, Keyapi, type, customerId);
            return GetObjJson<List<DNMailSSCItem>>(urlJson);
        }

        public List<DNMailSSCItem> CustomerCountInbox(int customerId, int type = 0)
        {
            var urlJson = string.Format("{0}DNMailSSC/CustomerCountInbox?key={1}&type={2}&customerId={3}", _url, Keyapi, type, customerId);
            return GetObjJson<List<DNMailSSCItem>>(urlJson);
        }

        public List<DNMailSSCItem> CustomerSentMail(int customerId)
        {
            var urlJson = string.Format("{0}DNMailSSC/CustomerSentMail?key={1}&customerId={2}", _url, Keyapi, customerId);
            return GetObjJson<List<DNMailSSCItem>>(urlJson);
        }

        public List<DNMailSSCItem> CustomerCountDrafts(int customerId)
        {
            var urlJson = string.Format("{0}DNMailSSC/CustomerCountDrafts?key={1}&customerId={2}", _url, Keyapi, customerId);
            return GetObjJson<List<DNMailSSCItem>>(urlJson);
        }

        public List<DNMailSSCItem> CustomerCountRecycleBin(int customerId)
        {
            var urlJson = string.Format("{0}DNMailSSC/CustomerCountRecycleBin?key={1}&customerId={2}", _url, Keyapi, customerId);
            return GetObjJson<List<DNMailSSCItem>>(urlJson);
        }

        public List<DNMailSSCItem> CustomerCountRecycleBinSend(int customerId)
        {
            var urlJson = string.Format("{0}DNMailSSC/CustomerCountRecycleBinSend?key={1}&customerId={2}", _url, Keyapi, customerId);
            return GetObjJson<List<DNMailSSCItem>>(urlJson);
        }

        public List<DNMailSSCItem> CustomerCountRecycleBinReceive(int customerId)
        {
            var urlJson = string.Format("{0}DNMailSSC/CustomerCountRecycleBinReceive?key={1}&customerId={2}", _url, Keyapi, customerId);
            return GetObjJson<List<DNMailSSCItem>>(urlJson);
        }

        public List<DNMailSSCItem> CustomerCountSpam(int customerId, int type = 0)
        {
            var urlJson = string.Format("{0}DNMailSSC/CustomerCountSpam?key={1}&type={2}&customerId={3}", _url, Keyapi, type, customerId);
            return GetObjJson<List<DNMailSSCItem>>(urlJson);
        }


        public List<DNMailSSCItem> CustomerCountSpamSend(int customerId, int type = 0)
        {
            var urlJson = string.Format("{0}DNMailSSC/CustomerCountSpamSend?key={1}&type={2}&customerId={3}", _url, Keyapi, type, customerId);
            return GetObjJson<List<DNMailSSCItem>>(urlJson);
        }

        public List<DNMailSSCItem> CustomerCountSpamReceive(int customerId, int type = 0)
        {
            var urlJson = string.Format("{0}DNMailSSC/CustomerCountSpamReceive?key={1}&type={2}&customerId={3}", _url, Keyapi, type, customerId);
            return GetObjJson<List<DNMailSSCItem>>(urlJson);
        }
        #endregion

        public List<DNMailSSCItem> GetListDelete(Guid userId, int type = 0)
        {
            var urlJson = string.Format("{0}DNMailSSC/GetListDelete?key={1}&userId={2}&type={3}", _url, Keyapi, userId, type);
            return GetObjJson<List<DNMailSSCItem>>(urlJson);
        }

        public List<DNUserItem> GetListBirthDay()
        {
            var urlJson = string.Format("{0}DNMailSSC/GetListBirthDay?key={1}", _url, Keyapi);
            return GetObjJson<List<DNUserItem>>(urlJson);
        }

        public List<DNMailSSCItem> GetListByArrId(int agencyid, string lstId)
        {
            var urlJson = string.Format("{0}DNMailSSC/GetListByArrId?key={1}&agencyId={2}&lstId={3}", _url, Keyapi,agencyid, lstId);
            return GetObjJson<List<DNMailSSCItem>>(urlJson);
        }

        public void SetCacheMail(int agencyid, DNMailSSCItem obj)
        {
            var key = string.Format("SetCacheMailsend={0}",agencyid);
            if (Cache.KeyExistsCache(key))
            {
                Cache.DeleteCache(key);
            }
            Cache.Set(key, obj, 10);
        }

        public DNMailSSCItem GetCacheMail(string code)
        {
            var key = string.Format("SetCacheMailsend={0}",code);
            return Cache.KeyExistsCache(key) ? (DNMailSSCItem)Cache.GetCache(key) : new DNMailSSCItem();
        }

        public void SetCacheMailCustomer(string code, DNMailSSCItem obj)
        {
            var key = string.Format("SetCacheMailCusSend={0}", code);
            if (Cache.KeyExistsCache(key))
            {
                Cache.DeleteCache(key);
            }
            Cache.Set(key, obj, 10);
        }

        public DNMailSSCItem GetCacheMailCustomer(int agencyid)
        {
            var key = string.Format("SetCacheMailCusSend={0}",agencyid);
            return Cache.KeyExistsCache(key) ? (DNMailSSCItem)Cache.GetCache(key) : new DNMailSSCItem();
        }

        public int Add(int agencyid, string code)
        {
            var urlJson = string.Format("{0}DNMailSSC/Add?key={1}&agencyId={2}&code={3}", _url, Keyapi, agencyid, code);
            return GetObjJson<int>(urlJson);
        }

        public int AddCustomer(string code)
        {
            var urlJson = string.Format("{0}DNMailSSC/AddCustomer?key={1}&code={2}", _url, Keyapi, code);
            return GetObjJson<int>(urlJson);
        }

        public int Update(int agencyid, string json, int id = 0)
        {
            var urlJson = string.Format("{0}DNMailSSC/Update?key={1}&agencyId={2}&json={3}&id={4}", _url, Keyapi,agencyid, json, id);
            return GetObjJson<int>(urlJson);
        }

        public int UpdateStatus(string code, int id = 0)
        {
            var urlJson = string.Format("{0}DNMailSSC/UpdateStatus?key={1}&code={2}&id={3}", _url, Keyapi, code, id);
            return GetObjJson<int>(urlJson);
        }

        public int UpdateDraftMailbox(string code, int id = 0)
        {
            var urlJson = string.Format("{0}DNMailSSC/UpdateDraftMailbox?key={1}&code={2}&id={3}", _url, Keyapi, code, id);
            return GetObjJson<int>(urlJson);
        }

        public int UpdateType(string code, string json)
        {
            var urlJson = string.Format("{0}DNMailSSC/UpdateType?key={1}&code={2}&json={3}", _url, Keyapi, code, json);
            return GetObjJson<int>(urlJson);
        }

        public int Delete(string code, string json)
        {
            var urlJson = string.Format("{0}DNMailSSC/Delete?key={1}&code={2}&json={3}", _url, Keyapi, code, json);
            return GetObjJson<int>(urlJson);
        }

        public int ReportDelete(string code, string json)
        {
            var urlJson = string.Format("{0}DNMailSSC/ReportDelete?key={1}&code={2}&json={3}", _url, Keyapi, code, json);
            return GetObjJson<int>(urlJson);
        }
    }
}
