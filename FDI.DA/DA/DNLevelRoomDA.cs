using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.CORE;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class DNLevelRoomDA : BaseDA
    {
        public DNLevelRoomDA()
        {
        }
        public DNLevelRoomDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public DNLevelRoomDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        public List<DNLevelRoomItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.DN_Level
                        where o.AgencyId == agencyId && o.IsDeleted == false
                        orderby o.ID descending 
                        select new DNLevelRoomItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            IsShow = o.IsShow,
                            Sort = o.Sort,
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<DNLevelRoomItem> GetListSimple()
        {
            var query = from o in FDIDB.DN_Level where o.IsDeleted == false
                        orderby o.ID descending
                        select new DNLevelRoomItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            AgencyId = o.AgencyId,
                            IsShow = o.IsShow,
                            Sort = o.Sort,
                        };
            return query.ToList();
        }
        public List<DNLevelRoomShowItem> GetAll(int agencyid)
        {
            var query = from o in FDIDB.DN_Room
                        where o.AgencyID == agencyid 
                        orderby o.Sort
                        select new DNLevelRoomShowItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            Count = o.DN_Bed_Desk.Count(m => m.IsShow == true && m.AgencyId == agencyid && (!m.IsDeleted.HasValue || m.IsDeleted == false)),
                            LiInts = o.DN_Bed_Desk.Where(m => m.IsShow == true && m.AgencyId == agencyid && (!m.IsDeleted.HasValue || m.IsDeleted == false)).Select(m => m.ID),
                        };
            return query.ToList();
        }
        public List<DNLevelRoomItem> GetList(int agencyId)
        {
            var query = from o in FDIDB.DN_Level
                        where o.AgencyId == agencyId && o.IsDeleted == false && o.IsShow == true
                        orderby o.Sort
                        select new DNLevelRoomItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                        };
            return query.ToList();
        }
        public List<DNLevelRoomItem> GetListBed(int agencyId)
        {
            var date = DateTime.Now.TotalSeconds();
            var datetoday = DateTime.Today.TotalSeconds();
            var dates = date - datetoday;
            var thu = DateTime.Now.FdiDayOfWeek();

            var query = from o in FDIDB.DN_Level
                        where o.AgencyId == agencyId
                        orderby o.Sort
                        select new DNLevelRoomItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            //LstBedDeskItems = (from v in o.DN_Bed_Desk
                            //                   where v.AgencyId == agencyId && v.IsDeleted == false && v.IsShow == true
                            //                   orderby v.Sort
                            //                   select new BedDeskItem
                            //                   {
                            //                       ID = v.ID,
                            //                       Name = v.Name,
                            //                       Status = v.StatusID,
                            //                       Color = v.DN_Status.Color,

                            //                       //DN_Packet = v.DN_Packet.Select(m => new DNPacketItem
                            //                       //{
                            //                       //    ID = m.ID,
                            //                       //    Name = m.Name
                            //                       //}).FirstOrDefault()
                            //                   })
                        };
            return query.ToList();
        }
        public List<DNLevelRoomItem> GetListBedByOrder(int agencyId)
        {
            var query = from o in FDIDB.DN_Level
                        where o.AgencyId == agencyId
                        orderby o.Sort
                        select new DNLevelRoomItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            //LstBedDeskItems = (from v in o.DN_Bed_Desk
                            //                   orderby v.Sort
                            //                   select new BedDeskItem
                            //                   {
                            //                       ID = v.ID,
                            //                       Name = v.Name,
                            //                       Status = v.StatusID,
                            //                       Color = v.DN_Status.Color


                            //                   })
                        };
            return query.ToList();
        }
        public List<TreeViewItem> GetListTree(int agencyId)
        {
            var query = from c in FDIDB.DN_Level
                        where c.ID > 1 && c.AgencyId == agencyId
                        orderby c.Sort
                        select new TreeViewItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            IsShow = c.IsShow,
                        };
            return query.ToList();
        }
        public List<int> GetListID(int agencyId)
        {
            var query = from c in FDIDB.DN_Level
                        where c.ID > 1 && c.AgencyId == agencyId
                        orderby c.Sort
                        select c.ID;
            return query.ToList();
        }
        public List<DNLevelRoomItem> GetListParentID(int agencyId)
        {
            var query = from c in FDIDB.DN_Level
                        where c.AgencyId == agencyId && c.IsShow == true && c.IsDeleted != true
                        orderby c.Sort
                        select new DNLevelRoomItem
                        {
                            ID = c.ID,
                            Name = c.Name
                        };
            return query.ToList();
        }
        public DNLevelRoomItem GetLevelRoomItem(int id)
        {
            var query = from c in FDIDB.DN_Level
                        where c.ID == id
                        select new DNLevelRoomItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            Sort = c.Sort,
                            IsShow = c.IsShow
                        };
            return query.FirstOrDefault();
        }
        public DN_Level GetById(int id)
        {
            var query = from c in FDIDB.DN_Level where c.ID == id select c;
            return query.FirstOrDefault();
        }
        public List<DN_Level> GetListByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.DN_Level where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }
        public void Add(DN_Level item)
        {
            FDIDB.DN_Level.Add(item);
        }
        public void Delete(DN_Level item)
        {
            FDIDB.DN_Level.Remove(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
