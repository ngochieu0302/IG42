using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public class DNRoomDA : BaseDA
    {
        public DNRoomDA()
        {
        }
        public DNRoomDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public DNRoomDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        public List<DNRoomItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.DN_Room
                        where o.AgencyID == agencyId && (!o.IsDeleted.HasValue || !o.IsDeleted.Value) && (!o.DN_Level.IsDeleted.HasValue || !o.DN_Level.IsDeleted.Value)
                        orderby o.DN_Level.Sort, o.Sort
                        select new DNRoomItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            Sort = o.Sort,
                            Row = o.Row,
                            Column = o.Column,
                            DN_Level = new DNLevelRoomItem
                            {
                                Name = o.DN_Level.Name,
                            }
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public DNRoomItem GetItemById(int id)
        {
            var query = from c in FDIDB.DN_Room
                        where c.ID == id
                        select new DNRoomItem
                            {
                                ID = c.ID,
                                Name = c.Name.Trim(),
                                Sort = c.Sort,
                                Row = c.Row,
                                Column = c.Column,
                                LevelID = c.LevelID,
                                DN_Level = new DNLevelRoomItem
                                {
                                    Name = c.DN_Level.Name,
                                }
                            };
            return query.FirstOrDefault();
        }

        public DNRoomItem GetAll(int id)
        {
            var query = from c in FDIDB.DN_Room
                        where c.ID == id && c.IsDeleted == false
                        orderby c.Sort
                        select new DNRoomItem
                        {
                            ID = c.ID,
                            Name = c.Name.Trim(),
                            Sort = c.Sort,
                            Row = c.Row,
                            Column = c.Column,
                            LevelID = c.LevelID,
                            DN_Level = new DNLevelRoomItem
                            {
                                Name = c.DN_Level.Name,
                            }
                        };
            return query.FirstOrDefault();
        }

        public DN_Room GetById(int id)
        {
            var query = from c in FDIDB.DN_Room where c.ID == id select c;
            return query.FirstOrDefault();
        }
        public List<DN_Room> GetListByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.DN_Room where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }
        public void Add(DN_Room item)
        {
            FDIDB.DN_Room.Add(item);
        }

        public void AddDesk(int? row, int? column, int id, int agencyid)
        {
            FDIDB.InsertBedByRowColumn(row, column, id, agencyid);
        }
        public void Delete(DN_Room item)
        {
            FDIDB.DN_Room.Remove(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
