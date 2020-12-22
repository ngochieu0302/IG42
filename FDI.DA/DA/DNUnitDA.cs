using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public class DNUnitDA: BaseDA
    {
        public DNUnitDA()
        {
        }
        public DNUnitDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public DNUnitDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        public List<DNUnitItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyid)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.DN_Unit
                        orderby o.ID descending
                        select new DNUnitItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<DNUnitItem> GetList(int agencyId)
        {
            var query = from o in FDIDB.DN_Unit
                        orderby o.ID descending
                        select new DNUnitItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                        };
            return query.ToList();
        }
        public List<DNUnitItem> GetList()
        {
            var query = from o in FDIDB.DN_Unit
                orderby o.ID descending
                select new DNUnitItem
                {
                    ID = o.ID,
                    Name = o.Name,
                };
            return query.ToList();
        }
        public DNUnitItem GetUnitItem(int id)
        {
            var query = from o in FDIDB.DN_Unit
                        where o.ID == id
                        orderby o.ID descending
                        select new DNUnitItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                        };
            return query.FirstOrDefault();
        }
        public DN_Unit GetById(int id)
        {
            var query = from o in FDIDB.DN_Unit where o.ID == id select o;
            return query.FirstOrDefault();
        }
        public List<DN_Unit> GetListArrId(List<int> lst)
        {
            var query = from o in FDIDB.DN_Unit where lst.Contains(o.ID) select o;
            return query.ToList();
        }

        public bool CheckByName(string name, int id, int agencyId)
        {
            return FDIDB.DN_Unit.Any(c => c.ID != id && c.Name.ToLower() == name.ToLower());

        }
        public void Add(DN_Unit item)
        {
            FDIDB.DN_Unit.Add(item);
        }
        public void Delete(DN_Unit item)
        {
            FDIDB.DN_Unit.Remove(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
