using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public class DNDocumentLevelDA : BaseDA
    {
        public DNDocumentLevelDA()
        {
        }
        public DNDocumentLevelDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public DNDocumentLevelDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        public List<DNDocumentLevelItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyid)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.DN_DocumentLevel
                        where c.AgencyId == agencyid && c.IsDeleted == false
                        orderby c.ID descending
                        select new DNDocumentLevelItem
                        {
                            ID = c.ID,
                            Name =  c.Name,
                            NameLevel = c.NameLevel + c.Name,
                            Sort = c.Sort,
                            IsShow = c.IsShow
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public List<DNDocumentLevelItem> GetListItemByParentID(int agencyid)
        {
            var query = from c in FDIDB.DN_DocumentLevel
                        where c.IsShow == true && c.IsDeleted == false && c.AgencyId == agencyid
                        orderby c.Level, c.Sort
                        select new DNDocumentLevelItem
                        {
                            ID = c.ID,
                            ParentId = c.ParentID,
                            Name = c.NameLevel + c.Name,
                            Level = c.Level
                        };
            return query.ToList();
        }

        public List<DNDocumentLevelItem> GetListSimple(int agencyId)
        {
            var query = from o in FDIDB.DN_DocumentLevel
                        where o.AgencyId == agencyId && o.IsDeleted == false && o.IsShow == true
                        orderby o.ID descending
                        select new DNDocumentLevelItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            Sort = o.Sort
                        };
            return query.ToList();
        }

        public DNDocumentLevelItem GetItemsByID(int id)
        {
            var query = from o in FDIDB.DN_DocumentLevel
                        where o.ID == id
                        orderby o.ID descending
                        select new DNDocumentLevelItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Sort = o.Sort,
                            IsShow = o.IsShow,
                          ParentId = o.ParentID
                        };
            return query.FirstOrDefault();
        }
        public List<DN_DocumentLevel> GetListBeDesk(List<int> ltsArrId)
        {
            var query = from c in FDIDB.DN_DocumentLevel where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }
 
        public List<DN_DocumentLevel> GetListByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.DN_DocumentLevel where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }
        public DN_DocumentLevel GetById(int id)
        {
            var query = from c in FDIDB.DN_DocumentLevel where c.ID == id select c;
            return query.FirstOrDefault();
        }
        public void Add(DN_DocumentLevel item)
        {
            FDIDB.DN_DocumentLevel.Add(item);
        }
        public void Delete(DN_DocumentLevel item)
        {
            FDIDB.DN_DocumentLevel.Remove(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
