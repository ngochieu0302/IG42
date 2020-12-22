using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class DNNewsCommentAPI : BaseAPI
    {
        private readonly string _url = Domain;
        public List<DNNewsCommentItem> GetListSimpleByRequest(int agencyid)
        {
            var urlJson = string.Format("{0}DNNewsComment/GetListSimpleByRequest?key={1}&agencyId={2}", _url, Keyapi,agencyid);
            return GetObjJson<List<DNNewsCommentItem>>(urlJson);
        }

        public List<DNNewsCommentItem> GetAll(int agencyid)
        {
            var urlJson = string.Format("{0}DNNewsComment/GetAll?key={1}&agencyId={2}", _url, Keyapi,agencyid);
            return GetObjJson<List<DNNewsCommentItem>>(urlJson);
        }

        public DNNewsCommentItem GetItemById(int agencyid, int id = 0)
        {
            var urlJson = string.Format("{0}DNNewsComment/GetItemById?key={1}&agencyId={2}&id={3}", _url, Keyapi,agencyid, id);
            return GetObjJson<DNNewsCommentItem>(urlJson);
        }

        public List<DNNewsCommentItem> GetListByArrId(int agencyid, string lstId)
        {
            var urlJson = string.Format("{0}DNNewsComment/GetListByArrId?key={1}&agencyId={2}&lstId={3}", _url, Keyapi,agencyid, lstId);
            return GetObjJson<List<DNNewsCommentItem>>(urlJson);
        }

        public List<DNNewsCommentItem> GetListByParentID(int agencyid, int parentId)
        {
            var urlJson = string.Format("{0}DNNewsComment/GetListByParentID?key={1}&agencyId={2}&parentId={3}", _url, Keyapi,agencyid, parentId);
            return GetObjJson<List<DNNewsCommentItem>>(urlJson);
        }

        public int Add(int agencyid, string json)
        {
            var urlJson = string.Format("{0}DNNewsComment/Add?key={1}&agencyId={2}&json={3}", _url, Keyapi,agencyid, json);
            return GetObjJson<int>(urlJson);
        }

        public int Update(int agencyid, string json, int id = 0)
        {
            var urlJson = string.Format("{0}DNNewsComment/Update?key={1}&agencyId={2}&json={3}&id={4}", _url, Keyapi,agencyid, json, id);
            return GetObjJson<int>(urlJson);
        }

        
    }
}
