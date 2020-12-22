using System;
using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class DNGroupMailSSCAPI : BaseAPI
    {
        private readonly string _url = Domain;
        public List<DNGroupMailSSCItem> GetListSimpleByRequest(int agencyid)
        {
            var urlJson = string.Format("{0}DNGroupMailSSC/GetListSimpleByRequest?key={1}&agencyId={2}", _url, Keyapi,agencyid);
            return GetObjJson<List<DNGroupMailSSCItem>>(urlJson);
        }

        public List<DNGroupMailSSCItem> GetAll(int agencyid)
        {
            var urlJson = string.Format("{0}DNGroupMailSSC/GetAll?key={1}&agencyId={2}", _url, Keyapi,agencyid);
            return GetObjJson<List<DNGroupMailSSCItem>>(urlJson);
        }

        public List<DNGroupMailSSCItem> GetAllByUserId(int agencyid, Guid userId)
        {
            var urlJson = string.Format("{0}DNGroupMailSSC/GetAllByUserId?key={1}&agencyId={2}&userId={3}", _url, Keyapi,agencyid, userId);
            return GetObjJson<List<DNGroupMailSSCItem>>(urlJson);
        }


        public DNGroupMailSSCItem GetItemById(int agencyid, int id = 0)
        {
            var urlJson = string.Format("{0}DNGroupMailSSC/GetItemById?key={1}&agencyId={2}&id={3}", _url, Keyapi,agencyid, id);
            return GetObjJson<DNGroupMailSSCItem>(urlJson);
        }

        public List<DNGroupMailSSCItem> GetListByArrId(int agencyid, string lstId)
        {
            var urlJson = string.Format("{0}DNGroupMailSSC/GetListByArrId?key={1}&agencyId={2}&lstId={3}", _url, Keyapi,agencyid, lstId);
            return GetObjJson<List<DNGroupMailSSCItem>>(urlJson);
        }

        public int Add(int agencyid, string json)
        {
            var urlJson = string.Format("{0}DNGroupMailSSC/Add?key={1}&agencyId={2}&json={3}", _url, Keyapi,agencyid, json);
            return GetObjJson<int>(urlJson);
        }

        public int Update(int agencyid, string json, int id = 0)
        {
            var urlJson = string.Format("{0}DNGroupMailSSC/Update?key={1}&agencyId={2}&json={3}&id={4}", _url, Keyapi,agencyid, json, id);
            return GetObjJson<int>(urlJson);
        }

        public int UpdateType(int agencyid, string json)
        {
            var urlJson = string.Format("{0}DNGroupMailSSC/UpdateType?key={1}&agencyId={2}&json={3}", _url, Keyapi,agencyid, json);
            return GetObjJson<int>(urlJson);
        }

        public int Delete(int agencyid, string json)
        {
            var urlJson = string.Format("{0}DNGroupMailSSC/Delete?key={1}&agencyId={2}&json={3}", _url, Keyapi,agencyid, json);
            return GetObjJson<int>(urlJson);
        }
    }
}
