using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public class DNCabinetDocumentDA : BaseDA
    {
        public DNCabinetDocumentDA()
        {
        }
        public DNCabinetDocumentDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public DNCabinetDocumentDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        public List<DNCabinetDocumentItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyid)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.DN_CabinetDocument
                        where c.AgencyID == agencyid && c.IsDeleted == false
                        orderby c.ID descending
                        select new DNCabinetDocumentItem
                        {
                            ID = c.ID,
                            Name = c.Name.Trim(),
                            RoomID = c.RoomID,
                            Description = c.Description,
                            IsShow = c.IsShow,
                            RoomName = c.DN_DocumentRoom.Name,
                            Sort = c.Sort
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<DNCabinetDocumentItem> GetListSimple(int agencyId)
        {
            var query = from o in FDIDB.DN_CabinetDocument
                        where o.AgencyID == agencyId && o.IsDeleted == false
                        orderby o.ID descending
                        select new DNCabinetDocumentItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            Sort = o.Sort
                        };
            return query.ToList();
        }

        public List<DNCabinetDocumentItem> GetItemsByRoomID(int roomId)
        {
            var query = from o in FDIDB.DN_CabinetDocument
                        where o.RoomID == roomId
                        orderby o.ID descending
                        select new DNCabinetDocumentItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Sort = o.Sort,
                            Description = o.Description,
                            RoomID = o.RoomID,
                            IsShow = o.IsShow
                        };
            return query.ToList();
        }

        public DNCabinetDocumentItem GetItemsByID(int id)
        {
            var query = from o in FDIDB.DN_CabinetDocument
                        where o.ID == id 
                        orderby o.ID descending
                        select new DNCabinetDocumentItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Sort = o.Sort,
                            Description = o.Description,
                            RoomID = o.RoomID,
                            IsShow = o.IsShow
                        };
            return query.FirstOrDefault();
        }
     
        public List<DN_CabinetDocument> GetListByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.DN_CabinetDocument where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }
        public DN_CabinetDocument GetById(int id)
        {
            var query = from c in FDIDB.DN_CabinetDocument where c.ID == id select c;
            return query.FirstOrDefault();
        }
        public void Add(DN_CabinetDocument item)
        {
            FDIDB.DN_CabinetDocument.Add(item);
        }
        public void Delete(DN_CabinetDocument item)
        {
            FDIDB.DN_CabinetDocument.Remove(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
