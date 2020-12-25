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
    public class DNBedDeskDA : BaseDA
    {
        public DNBedDeskDA()
        {
        }
        public DNBedDeskDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public DNBedDeskDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        public List<BedDeskItem> GetListSimple(int agencyId)
        {
            var query = from o in FDIDB.DN_Bed_Desk
                        where o.AgencyId == agencyId && (!o.IsDeleted.HasValue || o.IsDeleted == false) && (!o.DN_Room.IsDeleted.HasValue || o.DN_Room.IsDeleted == false)
                        orderby o.Sort descending
                        select new BedDeskItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            IsShow = o.IsShow,
                            Sort = o.Sort,
                            LevelRoomId = o.RoomId,
                            RoomName = o.DN_Room.DN_Level.Name,
                        };
            return query.ToList();
        }
        public List<BedDeskItem> GetListInPacket(int agencyId, int packetid)
        {
            var query = from o in FDIDB.DN_Bed_Desk
                        where o.AgencyId == agencyId && (!o.IsDeleted.HasValue || o.IsDeleted == false) && (!o.DN_Room.IsDeleted.HasValue || o.DN_Room.IsDeleted == false) && (!o.PacketID.HasValue || o.PacketID == 0 || o.PacketID == packetid)
                        orderby o.Sort descending
                        select new BedDeskItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim()
                        };
            return query.ToList();
        }
        public List<BedDeskItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyid)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.DN_Bed_Desk
                        where o.AgencyId == agencyid && o.IsShow == true && (!o.IsDeleted.HasValue || o.IsDeleted == false) && (!o.DN_Room.IsDeleted.HasValue || o.DN_Room.IsDeleted == false)
                        && (!o.DN_Room.DN_Level.IsDeleted.HasValue || o.DN_Room.DN_Level.IsDeleted == false)
                        orderby o.Sort descending
                        select new BedDeskItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            Quantity = o.Quantity,
                            //LevelRoomId = o.RoomId,
                            LevelName = o.DN_Room.DN_Level.Name,
                            RoomName = o.DN_Room.Name,
                            PacketName = o.DN_Packet.FirstOrDefault().Name
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public List<BedDeskItem> GetList(int agencyId)
        {
            var query = from o in FDIDB.DN_Bed_Desk
                        where o.AgencyId == agencyId && (!o.IsDeleted.HasValue || o.IsDeleted == false)
                        orderby o.Sort
                        select new BedDeskItem
                        {
                            ID = o.ID,
                            Name = o.Name
                        };
            return query.ToList();
        }

        public List<BedDeskItem> GetListItemByMWSID(int agencyId, int mwsid, int id)
        {
            var query = from o in FDIDB.DN_Bed_Desk
                        where o.AgencyId == agencyId && (!o.IsDeleted.HasValue || o.IsDeleted == false) && o.IsShow == true && (!o.DN_User_BedDesk.Any() || o.DN_User_BedDesk.All(m => m.MWSID != mwsid) || o.DN_User_BedDesk.Any(m => m.ID == id))
                        orderby o.Sort
                        select new BedDeskItem
                        {
                            ID = o.ID,
                            Name = o.Name
                        };
            return query.ToList();
        }

        public List<BedDeskItem> GetListItemByDateNow(int agencyId)
        {
            var date = DateTime.Now.TotalSeconds();
            //var datetoday = DateTime.Today.TotalSeconds();
            //var thu = DateTime.Now.FdiDayOfWeek();
            //const int before = (int)Order.Before;
            var query = from v in FDIDB.DN_Bed_Desk
                        where v.RoomId.HasValue && v.AgencyId == agencyId && (!v.IsDeleted.HasValue || !v.IsDeleted.Value) && v.IsShow == true && (!v.DN_Room.IsDeleted.HasValue || !v.DN_Room.IsDeleted.Value) && (!v.DN_Room.DN_Level.IsDeleted.HasValue || !v.DN_Room.DN_Level.IsDeleted.Value)
                        select new BedDeskItem
                        {
                            ID = v.ID,
                            Name = v.Name,
                            RoomName = v.DN_Room.DN_Level.Name,
                            LevelRoomId = v.DN_Room.LevelID,
                            Sort = v.DN_Room.DN_Level.Sort,
                            PacketId = v.PacketID,
                            PacketName = v.DN_Packet.FirstOrDefault().Name,
                            IsEarly = v.DN_Packet.FirstOrDefault().IsEarly.HasValue && v.DN_Packet.FirstOrDefault().IsEarly.Value,
                            PacketSort = v.DN_Packet.FirstOrDefault().Sort,
                            Value = v.DN_Packet.FirstOrDefault().Time,
                            IsShow = v.IsShow,
                            LstPacketItems = v.DN_Packet.Select(c=> new PacketItem
                            {
                                ID = c.ID,
                                Name = c.Name,
                                IsEarly = c.IsEarly,
                                Time = c.Time,
                                Sort = c.Sort,
                                IsDefault = c.IsDefault,
                            })
                            //CountOrder = v.Shop_Orders.Count(m => m.StartDate > date && (!m.IsDelete.HasValue || !m.IsDelete.Value)),
                            //CountContactOrder = v.Shop_ContactOrder.Count(m => m.StartDate > date),
                            //DN_User_BedDesk = v.DN_User_BedDesk.Where(m => m.DN_Weekly_Schedule.WeeklyID == thu && ((m.DN_Weekly_Schedule.DN_Schedule.HoursStart * 3600 + m.DN_Weekly_Schedule.DN_Schedule.MinuteStart * 60 + before + datetoday) < date && (m.DN_Weekly_Schedule.DN_Schedule.HoursEnd * 3600 + m.DN_Weekly_Schedule.DN_Schedule.MinuteEnd * 60 + before + datetoday) > date))
                            //        .Select(m => new DNUserBedDeskItem
                            //        {
                            //            UserID = FDIDB.DN_EDIT_Schedule.Any(e => (e.Date + e.DN_Schedule.HoursStart * 3600 + e.DN_Schedule.MinuteStart * 60) >= date && (e.Date + e.DN_Schedule.HoursEnd * 3600 + e.DN_Schedule.MinuteEnd * 60) <= date && e.UserId == m.UserID) ? FDIDB.DN_EDIT_Schedule.Where(e => (e.Date + e.DN_Schedule.HoursStart * 3600 + e.DN_Schedule.MinuteStart * 60) >= date && (e.Date + e.DN_Schedule.HoursEnd * 3600 + e.DN_Schedule.MinuteEnd * 60) <= date && e.UserId == m.UserID).Select(e => e.UserChangeId).FirstOrDefault() : m.UserID,
                            //            DateCreated = FDIDB.DN_EDIT_Schedule.Any(e => (e.Date + e.DN_Schedule.HoursStart * 3600 + e.DN_Schedule.MinuteStart * 60) >= date && (e.Date + e.DN_Schedule.HoursEnd * 3600 + e.DN_Schedule.MinuteEnd * 60) <= date && e.UserId == m.UserID) ?
                            //            FDIDB.DN_Time_Job.Where(d => d.DateCreated > datetoday && !d.ScheduleEndID.HasValue && d.UserId == FDIDB.DN_EDIT_Schedule.Where(e => (e.Date + e.DN_Schedule.HoursStart * 3600 + e.DN_Schedule.MinuteStart * 60) >= date && (e.Date + e.DN_Schedule.HoursEnd * 3600 + e.DN_Schedule.MinuteEnd * 60) <= date && e.UserId == m.UserID).Select(e => e.UserChangeId).FirstOrDefault()).Select(d => d.DateCreated).FirstOrDefault() :
                            //            FDIDB.DN_Time_Job.Where(d => d.DateCreated > datetoday && !d.ScheduleEndID.HasValue && d.UserId == m.UserID).Select(d => d.DateCreated).FirstOrDefault(),
                            //            FullName = FDIDB.DN_EDIT_Schedule.Any(e => (e.Date + e.DN_Schedule.HoursStart * 3600 + e.DN_Schedule.MinuteStart * 60) >= date && (e.Date + e.DN_Schedule.HoursEnd * 3600 + e.DN_Schedule.MinuteEnd * 60) <= date && e.UserId == m.UserID) ? FDIDB.DN_EDIT_Schedule.Where(e => (e.Date + e.DN_Schedule.HoursStart * 3600 + e.DN_Schedule.MinuteStart * 60) >= date && (e.Date + e.DN_Schedule.HoursEnd * 3600 + e.DN_Schedule.MinuteEnd * 60) <= date && e.UserId == m.UserID).Select(e => e.DN_Users1.LoweredUserName).FirstOrDefault() : m.DN_Users.LoweredUserName,
                            //            UserName = FDIDB.DN_EDIT_Schedule.Any(e => (e.Date + e.DN_Schedule.HoursStart * 3600 + e.DN_Schedule.MinuteStart * 60) >= date && (e.Date + e.DN_Schedule.HoursEnd * 3600 + e.DN_Schedule.MinuteEnd * 60) <= date && e.UserId == m.UserID) ? FDIDB.DN_EDIT_Schedule.Where(e => (e.Date + e.DN_Schedule.HoursStart * 3600 + e.DN_Schedule.MinuteStart * 60) >= date && (e.Date + e.DN_Schedule.HoursEnd * 3600 + e.DN_Schedule.MinuteEnd * 60) <= date && e.UserId == m.UserID).Select(e => e.DN_Users1.UserName).FirstOrDefault() : m.DN_Users.UserName,
                            //            countorder = FDIDB.DN_EDIT_Schedule.Any(e => (e.Date + e.DN_Schedule.HoursStart * 3600 + e.DN_Schedule.MinuteStart * 60) >= date && (e.Date + e.DN_Schedule.HoursEnd * 3600 + e.DN_Schedule.MinuteEnd * 60) <= date && e.UserId == m.UserID) ?
                            //            FDIDB.Shop_Orders.Count(s => s.Status == 3 && s.StartDate > datetoday && (!s.IsDelete.HasValue || s.IsDelete == false) &&  s.UserIdBedDeskID == FDIDB.DN_EDIT_Schedule.Where(e => (e.Date + e.DN_Schedule.HoursStart * 3600 + e.DN_Schedule.MinuteStart * 60) >= date && (e.Date + e.DN_Schedule.HoursEnd * 3600 + e.DN_Schedule.MinuteEnd * 60) <= date && e.UserId == m.UserID).Select(e => e.UserChangeId).FirstOrDefault()) :
                            //            FDIDB.Shop_Orders.Count(s => s.Status == 3 && s.StartDate > datetoday && (!s.IsDelete.HasValue || s.IsDelete == false) && s.UserIdBedDeskID == m.UserID),
                            //            MWSID = m.MWSID,
                            //            DN_Weekly_Schedule = new WeeklyScheduleItem
                            //            {
                            //                ScheduleTimeEnd = m.DN_Weekly_Schedule.DN_Schedule.HoursEnd * 3600 + m.DN_Weekly_Schedule.DN_Schedule.MinuteEnd * 60 + datetoday,
                            //            }
                            //        }).FirstOrDefault(),

                        };
            var lst = query.ToList();
            return lst;
        }
        public List<BedDeskItem> GetListBedItemByDateNow(int agencyId)
        {
            var query = from v in FDIDB.DN_Bed_Desk
                        where v.AgencyId == agencyId && (!v.IsDeleted.HasValue || v.IsDeleted == false) && (!v.DN_Room.IsDeleted.HasValue || v.DN_Room.IsDeleted == false)
                        && (!v.DN_Room.DN_Level.IsDeleted.HasValue || v.DN_Room.DN_Level.IsDeleted == false)
                        && (!v.DN_Room.DN_Level.IsDeleted.HasValue || v.DN_Room.DN_Level.IsDeleted == false)
                        select new BedDeskItem
                        {
                            ID = v.ID,
                            Name = v.Name,
                            LevelName = v.DN_Room.DN_Level.Name,
                            LevelRoomId = v.DN_Room.LevelID,
                            Sort = v.DN_Room.Sort,
                            RoomName = v.DN_Room.Name,
                            RoomId = v.RoomId,
                            IsShow = v.IsShow,
                            Quantity = v.Quantity,
                            Row = v.DN_Room.Row,
                            Column = v.DN_Room.Column,
                            PacketId = v.PacketID,
                            PacketName = v.DN_Packet.FirstOrDefault().Name,
                            PacketSort = v.DN_Packet.FirstOrDefault().Sort,
                            Value = v.DN_Packet.FirstOrDefault().Time
                        };
            return query.ToList();
        }
        public Guid? UserIdByBedDate(decimal? start, int? bedid)
        {
            var date = start.DecimalToDate();
            var dates = date.Hour * 3600 - date.Minute * 60;
            var thu = date.FdiDayOfWeek();
            var query = (from v in FDIDB.DN_User_BedDesk
                         where v.DN_Weekly_Schedule.WeeklyID == thu && (v.DN_Weekly_Schedule.DN_Schedule.HoursStart * 3600 + v.DN_Weekly_Schedule.DN_Schedule.MinuteStart * 60) < dates && (v.DN_Weekly_Schedule.DN_Schedule.HoursEnd * 3600 + v.DN_Weekly_Schedule.DN_Schedule.MinuteEnd * 60) > dates && v.BedDeskID == bedid
                         select FDIDB.DN_EDIT_Schedule.Any(e => e.Date == start - dates && e.ScheduleID == v.DN_Weekly_Schedule.WeeklyID) ? FDIDB.DN_EDIT_Schedule.Where(e => e.Date == start - dates && e.ScheduleID == v.DN_Weekly_Schedule.WeeklyID).Select(e => e.UserChangeId).FirstOrDefault() : v.UserID

                ).FirstOrDefault();
            return query;
        }
        public BedDeskItem GetBedDeskItem(int id)
        {
            var query = from c in FDIDB.DN_Bed_Desk
                        where c.ID == id
                        select new BedDeskItem
                        {
                            ID = c.ID,
                            Sort = c.Sort,
                            Name = c.Name,
                            Quantity = c.Quantity,
                            RoomName = c.DN_Room.Name,
                            LevelName = c.DN_Room.DN_Level.Name,
                            LevelRoomId = c.DN_Room.LevelID,
                            PacketId = c.PacketID,
                            IsShow = c.IsShow,
                        };
            return query.FirstOrDefault();
        }

        public List<BedDeskItem> GetListNow(int agencyid)
        {
            var datenow = DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.DN_Bed_Desk
                        where c.IsShow == true && (!c.IsDeleted.HasValue || !c.IsDeleted.Value) && c.AgencyId == agencyid
                        select new BedDeskItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            IsShow = c.Shop_Orders1.Any(m => m.StartDate < datenow && m.EndDate > datenow && m.Status < 3) || c.Shop_ContactOrder1.Any(m => m.StartDate < datenow && m.EndDate > datenow && m.Status < 3)
                        };
            return query.ToList();
        }

        public void SortNameBed(int agencyid)
        {
            FDIDB.SortNameBed(agencyid);
        }

        public List<SuggestionsProduct> GetListAuto(string keword, int showLimit, int agencyId)
        {
            var query = from c in FDIDB.DN_Bed_Desk
                        where c.IsDeleted == false && c.Name.Contains(keword.ToLower())
                        select new SuggestionsProduct
                        {
                            ID = c.ID,
                            value = c.Name
                        };
            return query.Take(showLimit).ToList();
        }

        public List<BedDeskItem> GetListByRoomId(int id)
        {
            var query = from c in FDIDB.DN_Bed_Desk
                        where c.RoomId == id && (!c.IsDeleted.HasValue || c.IsDeleted == false)
                        orderby c.Row, c.Column
                        select new BedDeskItem
                        {
                            ID = c.ID,
                            IsShow = c.IsShow,
                            Name = c.Name,
                            Column = c.Column,
                            Row = c.Row,
                            Quantity = c.Quantity,
                            RoomId = id
                        };
            return query.ToList();
        }

        public BedDeskItem GetOrderOrContactByBedId(int id)
        {
            var date = DateTime.Now.TotalSeconds();
            const int before = (int)Utils.Order.Order;
            var query = from v in FDIDB.DN_Bed_Desk
                        where v.ID == id
                        select new BedDeskItem
                        {
                            ID = v.ID,
                            Shop_Orders = v.Shop_Orders.Where(m => m.StartDate - before < date && m.EndDate > date && m.Status <= 3).Select(m => m.ID),
                            ContactOrders = v.Shop_ContactOrder.Where(m => m.StartDate - before < date && m.EndDate > date && m.Status <= 3).Select(m => m.ID)
                        };
            return query.FirstOrDefault();
        }

        public DN_Bed_Desk GetById(int id)
        {
            var query = from c in FDIDB.DN_Bed_Desk where c.ID == id select c;
            return query.FirstOrDefault();
        }
        public List<DN_Bed_Desk> GetListArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.DN_Bed_Desk where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }
        public List<DN_Bed_Desk> ListItemArrId(List<int> ltsArrId, int id)
        {
            var query = from c in FDIDB.DN_Bed_Desk where c.RoomId == id && (ltsArrId.Contains(c.ID) || c.IsShow == false) select c;
            return query.ToList();
        }
        public void Add(DN_Bed_Desk item)
        {
            FDIDB.DN_Bed_Desk.Add(item);
        }
        public void Delete(DN_Bed_Desk item)
        {
            FDIDB.DN_Bed_Desk.Remove(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}