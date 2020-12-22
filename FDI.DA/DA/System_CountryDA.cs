using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public partial class System_CountryDA : BaseDA
    {
        #region Constructer
        public System_CountryDA()
        {
        }

        public System_CountryDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public System_CountryDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<CountryItem> GetListSimpleByAutoComplete(string keyword, int showLimit, bool isShow)
        {
            var query = from c in FDIDB.System_Country
                        orderby c.Name
                        where c.Show == isShow
                        && c.Name.StartsWith(keyword)
                        select new CountryItem
                                   {
                                       ID = c.ID,
                                       Code = c.Code,
                                       Name = c.Name
                                   };
            return query.Take(showLimit).ToList();
        }

        public List<CountryItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.System_Country
                        orderby  o.Name
                        where o.IsDelete == false
                        select new CountryItem
                                   {
                                       ID = o.ID,
                                       Code = o.Code,
                                       Name = o.Name,
                                       Show = o.Show,
                                       Description = o.Description
                                   };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public System_Country GetById(int id)
        {
            var query = from c in FDIDB.System_Country where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public List<System_Country> GetListByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.System_Country where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }

        public void Add(System_Country systemCountry)
        {
            FDIDB.System_Country.Add(systemCountry);
        }

        public void Delete(System_Country systemCountry)
        {
            FDIDB.System_Country.Remove(systemCountry);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
       
    }
}
