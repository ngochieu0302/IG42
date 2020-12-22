using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public class DNDrawerDA : BaseDA
    {
        public DNDrawerDA()
        {
        }
        public DNDrawerDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public DNDrawerDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        public List<DNDrawerItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.DN_Drawer
                        where  c.IsDelete == false
                        orderby c.ID descending
                        select new DNDrawerItem
                        {
                            ID = c.ID,
                            Name = c.Name.Trim(),
                            Description = c.Description,
                            CabinetDocumentID = c.CabinetDocumentID,
                            IsShow = c.IsShow,
                            Sort = c.Sort
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<DNDrawerItem> GetListSimple()
        {
            var query = from o in FDIDB.DN_Drawer
                        where  o.IsShow == true && o.IsDelete == false
                        orderby o.ID descending
                        select new DNDrawerItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            Sort = o.Sort
                        };
            return query.ToList();
        }

        public DNDrawerItem GetItemsByID(int id)
        {
            var query = from o in FDIDB.DN_Drawer
                        where o.ID == id 
                        orderby o.ID descending
                        select new DNDrawerItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Description = o.Description,
                            CabinetDocumentID = o.CabinetDocumentID,
                            Sort = o.Sort,
                            IsShow = o.IsShow,
                        };
            return query.FirstOrDefault();
        }

        public List<DNDrawerItem> GetItemsCabinetId(int cabinetId)
        {
            var query = from o in FDIDB.DN_Drawer
                        where o.CabinetDocumentID == cabinetId
                        orderby o.ID descending
                        select new DNDrawerItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Description = o.Description,
                            CabinetDocumentID = o.CabinetDocumentID,
                            Sort = o.Sort,
                            IsShow = o.IsShow,
                        };
            return query.ToList();
        }
     
        public List<DN_Drawer> GetListByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.DN_Drawer where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }
        public DN_Drawer GetById(int id)
        {
            var query = from c in FDIDB.DN_Drawer where c.ID == id select c;
            return query.FirstOrDefault();
        }
        public void Add(DN_Drawer item)
        {
            FDIDB.DN_Drawer.Add(item);
        }
        public void Delete(DN_Drawer item)
        {
            FDIDB.DN_Drawer.Remove(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
