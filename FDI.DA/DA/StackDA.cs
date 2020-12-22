using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class StackDA : BaseDA
    {
        #region Constructer
        public StackDA()
        {
        }

        public StackDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public StackDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<StackItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int Agencyid)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.DN_Stack
                        where o.AgencyID == Agencyid
                        orderby o.ID descending
                        select new StackItem
                        {
                            ID = o.ID,

                            Date = o.Date,
                            Json = o.Json,
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public DN_Stack GetById(int id)
        {
            var query = from c in FDIDB.DN_Stack where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public StackItem GetItemById(int id)
        {
            var query = from o in FDIDB.DN_Stack
                        where o.ID == id
                        select new StackItem
                        {
                            ID = o.ID,

                            Date = o.Date,
                            Json = o.Json,
                        };
            return query.FirstOrDefault();
        }
        public string GetJsonByDate(decimal date, int agencyid)
        {
            var query = from o in FDIDB.DN_Stack
                        where o.Date == date && o.AgencyID == agencyid
                        select o.Json;
            return query.FirstOrDefault();
        }
        public DN_Stack GetByDate(decimal date, int agencyid)
        {
            var query = from c in FDIDB.DN_Stack where c.Date == date && c.AgencyID == agencyid select c;
            return query.FirstOrDefault();
        }
        public void Add(DN_Stack item)
        {
            FDIDB.DN_Stack.Add(item);
        }

        public void Delete(DN_Stack item)
        {
            FDIDB.DN_Stack.Remove(item);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
