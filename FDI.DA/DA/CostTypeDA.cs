using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public class CostTypeDA : BaseDA
    {
        public CostTypeDA()
        {
        }
        public CostTypeDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public CostTypeDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        public List<CostTypeItem> GetListSimple(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.CostTypes
                        where o.AgencyId == agencyId && o.IsDelete == false
                        orderby o.ID descending
                        select new CostTypeItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Description = o.Description,
                            Type = o.Type
                        };
            var type = httpRequest["Type"];
            if (!string.IsNullOrEmpty(type))
            {
                var t = int.Parse(type);
                query = query.Where(c => c.Type == t);
            }
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<CostTypeItem> GetList(int agencyId,int type)
        {
            var query = from o in FDIDB.CostTypes
                        where o.AgencyId == agencyId && o.IsDelete == false && o.Type == type
                        orderby o.ID descending
                        select new CostTypeItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Description = o.Description
                        };
            return query.ToList();
        }
        public CostTypeItem GetCostTypeItem(int id)
        {
            var query = from o in FDIDB.CostTypes
                        where o.ID == id
                        orderby o.ID descending
                        select new CostTypeItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Type = o.Type,
                            Description = o.Description
                        };
            return query.FirstOrDefault();
        }

        public List<SuggestionsProduct> GetListAuto(string keword, int showLimit, int agencyId,int type)
        {
            var query = from c in FDIDB.CostTypes
                        where c.IsDelete == false &&  c.Name.Contains(keword) && c.AgencyId == agencyId && c.Type == type
                        select new SuggestionsProduct
                        {
                            ID = c.ID,
                            value = c.Name,
                            title = c.Name,
                            data = c.Name,
                        };
            return query.Take(showLimit).ToList();
        }
        public CostType GetById(int id)
        {
            var query = from o in FDIDB.CostTypes where o.ID == id select o;
            return query.FirstOrDefault();
        }
        public List<CostType> GetListArrId(List<int> lst)
        {
            var query = from o in FDIDB.CostTypes where lst.Contains(o.ID) select o;
            return query.ToList();
        }
        public void Add(CostType item)
        {
            FDIDB.CostTypes.Add(item);
        }
        public void Delete(CostType item)
        {
            FDIDB.CostTypes.Remove(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
