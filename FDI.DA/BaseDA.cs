using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Utils;

namespace FDI.DA
{
    public class BaseDA : IDisposable
    {
        protected FDIEntities FdiEntities = new FDIEntities();
        private readonly Pager _pager = new Pager();
        public FDIEntities FDIDB
        {
            get
            {
                return FdiEntities;
            }
        }
        public string LanguageId = Utility.Getcookie("LanguageId") ?? "vi";
        public static int TotalRecord = 0;
        public string PathPaging { get; set; }
        public string PathPagingext { get; set; }
        public ParramRequest Request { get; set; }
        public string GridHtmlPage
        {
            get
            {
                if (!string.IsNullOrEmpty(PathPaging))
                    PathPaging = PathPaging.Replace("#", "");
                return _pager.GetPage("" + Request, Request.CurrentPage, Request.RowPerPage, TotalRecord);
            }
        }

        #region cơ chế dọn rác
        private bool _isDisposed;
        public void Free()
        {
            if (_isDisposed)
                throw new ObjectDisposedException("Object Name");
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~BaseDA()
        {
            //Pass false as param because no need to free managed resources when you call finalize it
            //by GC itself as its work of finalize to manage managed resources.
            Dispose(false);
        }
        //Implement dispose to free resources
        protected virtual void Dispose(bool disposedStatus)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                FdiEntities.Dispose(); // Released unmanaged Resources
                if (disposedStatus)
                {
                    // Released managed Resources
                }
            }
        }
        #endregion

        public int Save()
        {
            return FDIDB.SaveChanges();
        }
    }

    public class ParramRequest
    {
        public int CurrentPage { get; set; }
        public int RowPerPage { get; set; }
        public string Keyword { get; set; }
        public List<string> SearchInField { get; set; }
        public string FieldSort { get; set; }
        public bool TypeSort { get; set; }
        public int CategoryID { get; set; }
        public string ProductName { get; set; }
        public string OtherParram
        {
            get
            {
                var listSytemParram = new[] { "CategoryID", "Page", "RowPerPage", "Keyword", "SearchIn", "Field", "FieldOption", "message", "key", "code", "agencyId", "userId" };//"StartDate", "EndDate",
                return HttpContext.Current.Request.QueryString.AllKeys.Where(item => !listSytemParram.Contains(item)).Aggregate(string.Empty, (current, item) => current + (item + "=" + HttpContext.Current.Request[item] + "&"));
            }
        }
        public override string ToString()
        {
            return string.Format(OtherParram + "Keyword={0}&CategoryID={5}&SearchIn={4}&RowPerPage={1}&Field={2}&FieldOption={3}&Page=", Keyword, RowPerPage, FieldSort, (TypeSort) ? 1 : 0, string.Join(",", SearchInField), CategoryID);
        }
        public string GetCategoryString()
        {
            return string.Format(OtherParram + "Keyword={0}&SearchIn={4}&RowPerPage={1}&Field={2}&FieldOption={3}&Page=1&CategoryID=", Keyword, RowPerPage, FieldSort, (TypeSort) ? 1 : 0, string.Join(",", SearchInField));
        }
        public string ParramArr
        {
            get
            {
                return string.Format(OtherParram + "Keyword={0}&RowPerPage={1}&Field={2}&FieldOption={3}&Page=", Keyword, RowPerPage, FieldSort, (TypeSort) ? 1 : 0);
            }
        }
        public string SortUrl
        {
            get
            {
                return string.Format("FieldOption={2}&Keyword={0}&SearchIn={1}", Keyword, string.Join(",", SearchInField), (TypeSort) ? 1 : 0);
            }
        }
        public ParramRequest(HttpRequestBase request)
        {
            CategoryID = !string.IsNullOrEmpty(request["CategoryID"]) ? Convert.ToInt32(request["CategoryID"]) : 0;
            CurrentPage = !string.IsNullOrEmpty(request["Page"]) ? Convert.ToInt32(request["page"]) : 1;
            RowPerPage = !string.IsNullOrEmpty(request["RowPerPage"]) ? Convert.ToInt32(request["RowPerPage"]) : 50;
            ProductName = !string.IsNullOrEmpty(request["ProductName"]) ? request["ProductName"].Trim() : string.Empty;
            Keyword = !string.IsNullOrEmpty(request["Keyword"]) ? request["Keyword"].Trim() : string.Empty;
            FieldSort = !string.IsNullOrEmpty(request["Field"]) ? request["Field"].Trim() : string.Empty;
            TypeSort = string.IsNullOrEmpty(request["FieldOption"]) || (request["FieldOption"].Equals("1"));
            //if (!string.IsNullOrEmpty(request["StartDate"])) StartDate = DateTime.ParseExact(request["StartDate"], "dd/MM/yyyy", null);
            //if (!string.IsNullOrEmpty(request["EndDate"])) EndDate = DateTime.ParseExact(request["EndDate"], "dd/MM/yyyy", null);
            SearchInField = new List<string>();
            if (string.IsNullOrEmpty(request["SearchIn"])) return;
            var temp = request["SearchIn"];
            if (temp.IndexOf(',') > 0)
                SearchInField = temp.Split(',').ToList();
            else
                SearchInField.Add(temp);
        }
    }
}
