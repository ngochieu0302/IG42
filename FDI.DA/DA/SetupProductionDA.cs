using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public class SetupProductionDA : BaseDA
    {
        public SetupProductionDA()
        {
        }
        public SetupProductionDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public SetupProductionDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        public List<SetupProductionItem> GetListSimple(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.SetupProductions
                        where o.AgencyId == agencyId && o.IsDelete == false
                        orderby o.ID descending
                        select new SetupProductionItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Description = o.Description,
                            Percent = o.Percent
                        };            
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<SetupProductionItem> GetList(int agencyId)
        {
            var query = from o in FDIDB.SetupProductions
                        where o.AgencyId == agencyId && o.IsDelete == false
                        orderby o.ID descending
                        select new SetupProductionItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Description = o.Description,
                            Percent = o.Percent
                        };
            return query.ToList();
        }
        public SetupProductionItem GetSetupProductionItem(int id)
        {
            var query = from o in FDIDB.SetupProductions
                        where o.ID == id
                        orderby o.ID descending
                        select new SetupProductionItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Description = o.Description,
                            Percent = o.Percent
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
                            Quantity = c.Percent
                        };
            return query.Take(showLimit).ToList();
        }
        public SetupProduction GetById(int id)
        {
            var query = from o in FDIDB.SetupProductions where o.ID == id select o;
            return query.FirstOrDefault();
        }
        public List<SetupProduction> GetListArrId(List<int> lst)
        {
            var query = from o in FDIDB.SetupProductions where lst.Contains(o.ID) select o;
            return query.ToList();
        }
        public void Add(SetupProduction item)
        {
            FDIDB.SetupProductions.Add(item);
        }
        public void Delete(SetupProduction item)
        {
            FDIDB.SetupProductions.Remove(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
