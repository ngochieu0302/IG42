using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public class DNDocumentRoomDA : BaseDA
    {
        public DNDocumentRoomDA()
        {
        }
        public DNDocumentRoomDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public DNDocumentRoomDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        public List<DNDocumentRoomItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyid)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.DN_DocumentRoom
                        where c.AgencyID == agencyid
                        orderby c.ID descending
                        select new DNDocumentRoomItem
                        {
                            ID = c.ID,
                            Name = c.Name.Trim(),
                            Sort = c.Sort,
                            DocumentLevelID = c.DocumentLevelID,
                            DocumentLevelName = c.DN_DocumentLevel.Name
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<DNDocumentRoomItem> GetListSimple(int agencyId)
        {
            var query = from o in FDIDB.DN_DocumentRoom
                        where o.AgencyID == agencyId && o.IsDeleted == false 
                        orderby o.ID descending
                        select new DNDocumentRoomItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            Sort = o.Sort
                        };
            return query.ToList();
        }

        public DNDocumentRoomItem GetItemsByID(int id)
        {
            var query = from o in FDIDB.DN_DocumentRoom
                        where o.ID == id
                        orderby o.ID descending
                        select new DNDocumentRoomItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Sort = o.Sort,
                            DocumentLevelID = o.DocumentLevelID
                        };
            return query.FirstOrDefault();
        }

        public List<DNDocumentRoomItem> GetRoomByLevelId(int levelId)
        {
            var query = from o in FDIDB.DN_DocumentRoom
                        where o.DocumentLevelID == levelId
                        orderby o.ID descending
                        select new DNDocumentRoomItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Sort = o.Sort,
                        };
            return query.ToList();
        }
     
        public List<DN_DocumentRoom> GetListByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.DN_DocumentRoom where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }
        public DN_DocumentRoom GetById(int id)
        {
            var query = from c in FDIDB.DN_DocumentRoom where c.ID == id select c;
            return query.FirstOrDefault();
        }
        public void Add(DN_DocumentRoom item)
        {
            FDIDB.DN_DocumentRoom.Add(item);
        }
        public void Delete(DN_DocumentRoom item)
        {
            FDIDB.DN_DocumentRoom.Remove(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
