using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class NewsAPI : BaseAPI
    {
        public ModelNewsItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}News/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi,agencyid);
            return GetObjJson<ModelNewsItem>(urlJson);
        }

        public List<NewsItem> GetList(string url,int cateId, int page, ref int total)
        {
            var urlJson = string.Format("{0}News/GetList?key={1}&cateId={2}&page={3}&total={4}", url, Keyapi, cateId, page, total);
            return GetObjJson<List<NewsItem>>(urlJson);
        }

        public List<SuggestionsTMNews> GetListAuto(string keword, int showLimit)
        {
            var urlJson = string.Format("{0}News/GetListAuto?key={1}&keword={2}&showLimit={3}", Domain, Keyapi, keword, showLimit);
            return GetObjJson<List<SuggestionsTMNews>>(urlJson);
        }

        public List<NewsItem> GetNewByTag(string url, int cateId, int page, ref int total)
        {
            var urlJson = string.Format("{0}News/GetNewByTag?key={1}&cateId={2}&page={3}&total={4}", url, Keyapi, cateId, page, total);
            return GetObjJson<List<NewsItem>>(urlJson);
        }

        public List<NewsItem> GetNewKeyword(string url, string keyword, int page, ref int total)
        {
            var urlJson = string.Format("{0}News/GetNewKeyword?key={1}&keyword={2}&page={3}&total={4}", url, Keyapi, keyword, page, total);
            return GetObjJson<List<NewsItem>>(urlJson);
        }

        public CategoryItem GetListHot(string url, int id)
        {
            var urlJson = string.Format("{0}News/GetListHot?key={1}&id={2}", url, Keyapi, id);
            return GetObjJson<CategoryItem>(urlJson);
        }

        public List<NewsItem> GetListOther(string url, int id)
        {
            var urlJson = string.Format("{0}News/GetListOther?key={1}&id={2}", url, Keyapi, id);
            return GetObjJson<List<NewsItem>>(urlJson);
        }

        public List<NewsItem> GetListHome(string url, int cateId)
        {
            var urlJson = string.Format("{0}News/GetListHome?key={1}&cateId={2}", url, Keyapi, cateId);
            return GetObjJson<List<NewsItem>>(urlJson);
        }

        public NewsItem GetNewsId(string url, int id)
        {
            var urlJson = string.Format("{0}News/GetDistrictItemByCityId?key={1}&id={2}", url, Keyapi, id);
            return GetObjJson<NewsItem>(urlJson);
        }

        public NewsItem GetItemById(int id)
        {
            var urlJson = string.Format("{0}News/GetItemById?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<NewsItem>(urlJson);
        }

        public int Add(string json)
        {
            var urlJson = string.Format("{0}News/Add?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
        public int Update(string json)
        {
            var urlJson = string.Format("{0}News/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }

        public int Show(string lstArrId)
        {
            var urlJson = string.Format("{0}News/Show?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }

        public int Hide(string lstArrId)
        {
            var urlJson = string.Format("{0}News/Hide?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }
        public int Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}News/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }
    }
}
