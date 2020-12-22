using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.CORE;
using FDI.Simple;
using FDI.Utils;
using CORE = FDI.CORE;

namespace FDI.DA
{
    public class ContentVoteDa : BaseDA
    {
        public ContentVoteDa()
        {
        }
        public ContentVoteDa(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public ContentVoteDa(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        public List<ContentVoteItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyid, out int total)
        {
            Request = new ParramRequest(httpRequest);
            var now = DateTime.Now;
            var y = httpRequest["DateYear"];
            var m = httpRequest["DateMonth"];
            var view = httpRequest["view"] ?? "";
            var user = httpRequest["user"];
            var vote = httpRequest["vote"];
            var year = string.IsNullOrEmpty(y) ? now.Year : int.Parse(y);
            var month = string.IsNullOrEmpty(m) ? now.Month : int.Parse(m);
            var date = new DateTime(year, month, 1);
            var fromDate = date.TotalSeconds();
            var toDate = date.AddMonths(1).TotalSeconds();
            var query = from o in FDIDB.DN_ContentVote
                        where o.AgencyId == agencyid && o.DateCreated >= fromDate && o.DateCreated <= toDate
                        //&& o.DN_Users.UserName == user
                        orderby o.ID descending
                        select new ContentVoteItem
                        {
                            ID = o.ID,
                            //NVBDG = o.DN_Tree.DN_UsersInRoles.DN_Users.LoweredUserName,
                            US_NVBDG = o.DN_Tree.UserName,
                            //NVDG = o.DN_Users.LoweredUserName,
                            UserName = o.DN_Users.UserName,
                            US_NVDG = o.DN_Users.UserName,
                            Value = o.Value,
                            Content = o.Content,
                            VoteName = o.DN_Vote.Name,
                            VoteID = o.VoteID,
                            LevelName = o.LevelVote.Name,
                            DateCreated = o.DateCreated,
                            DateEvaluation = o.DateEvaluation
                        };
            if (!string.IsNullOrEmpty(user) && view == "")
            {
                query = query.Where(c => c.US_NVDG == user || c.US_NVBDG == user);
            }
            if (!string.IsNullOrEmpty(user) && view == "0")
            {
                query = query.Where(c => c.US_NVDG == user);
            }
            else if (!string.IsNullOrEmpty(user) && view == "1")
            {
                query = query.Where(c => c.US_NVBDG == user);
            }

            if (!string.IsNullOrEmpty(vote))
            {
                var t = int.Parse(vote);
                query = query.Where(c => c.VoteID == t);
            }
            total = query.Any() ? query.Sum(c => c.Value ?? 0) : 0;
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<ContentVoteItem> GetListSimpleByRequestExcel(HttpRequestBase httpRequest, int agencyid)
        {
            Request = new ParramRequest(httpRequest);
            var now = DateTime.Now;
            var y = httpRequest["DateYear"];
            var m = httpRequest["DateMonth"];
            var view = httpRequest["view"] ?? "";
            var user = httpRequest["user"];
            var vote = httpRequest["vote"];
            var year = string.IsNullOrEmpty(y) ? now.Year : int.Parse(y);
            var month = string.IsNullOrEmpty(m) ? now.Month : int.Parse(m);
            var date = new DateTime(year, month, 1);
            var fromDate = date.TotalSeconds();
            var toDate = date.AddMonths(1).TotalSeconds();
            var query = from o in FDIDB.DN_ContentVote
                        where o.AgencyId == agencyid && o.DateCreated >= fromDate && o.DateCreated <= toDate
                        //&& o.DN_Users.UserName == user
                        orderby o.ID descending
                        select new ContentVoteItem
                        {
                            ID = o.ID,
                            //NVBDG = o.DN_Tree.DN_UsersInRoles.DN_Users.LoweredUserName,
                            US_NVBDG = o.DN_Tree.UserName,
                            //NVDG = o.DN_Users.LoweredUserName,
                            UserName = o.DN_Users.UserName,
                            US_NVDG = o.DN_Users.UserName,
                            Value = o.Value,
                            Content = o.Content,
                            VoteName = o.DN_Vote.Name,
                            VoteID = o.VoteID,
                            LevelName = o.LevelVote.Name,
                            DateEvaluation = o.DateEvaluation
                        };
            if (!string.IsNullOrEmpty(user) && view == "")
            {
                query = query.Where(c => c.US_NVDG == user || c.US_NVBDG == user);
            }
            if (!string.IsNullOrEmpty(user) && view == "0")
            {
                query = query.Where(c => c.US_NVDG == user);
            }
            else if (!string.IsNullOrEmpty(user) && view == "1")
            {
                query = query.Where(c => c.US_NVBDG == user);
            }

            if (!string.IsNullOrEmpty(vote))
            {
                var t = int.Parse(vote);
                query = query.Where(c => c.VoteID == t);
            }
            return query.ToList();
        }
        public List<ContentVoteItem> ListItemsUser(HttpRequestBase httpRequest, int agencyid, Guid userId, out int total)
        {
            Request = new ParramRequest(httpRequest);
            var now = DateTime.Now;
            var y = httpRequest["DateYear"];
            var m = httpRequest["DateMonth"];
            var vote = httpRequest["vote"];
            var year = string.IsNullOrEmpty(y) ? now.Year : int.Parse(y);
            var month = string.IsNullOrEmpty(m) ? now.Month : int.Parse(m);
            var date = new DateTime(year, month, 1);
            var fromDate = date.TotalSeconds();
            var toDate = date.AddMonths(1).TotalSeconds();
            var query = from o in FDIDB.DN_ContentVote
                        where o.AgencyId == agencyid && o.DateCreated >= fromDate && o.DateCreated <= toDate && o.UserID == userId
                        orderby o.ID descending
                        select new ContentVoteItem
                        {
                            ID = o.ID,
                            US_NVBDG = o.DN_Tree.DN_UsersInRoles.DN_Users.UserName,
                            Value = o.Value,
                            Content = o.Content,
                            VoteName = o.DN_Vote.Name,
                            VoteID = o.VoteID,
                            LevelName = o.LevelVote.Name,
                            DateCreated = o.DateCreated,
                            DateEvaluation = o.DateEvaluation
                        };

            if (!string.IsNullOrEmpty(vote))
            {
                var t = int.Parse(vote);
                query = query.Where(c => c.VoteID == t);
            }
            query = query.SelectByRequest(Request);
            total = query.Sum(c => c.Value ?? 0);
            query = query.SelectPageByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public List<ContentVoteItem> ListItemsUser(HttpRequestBase httpRequest, int agencyid, string username, out int total)
        {
            Request = new ParramRequest(httpRequest);
            var now = DateTime.Now;
            var y = httpRequest["DateYear"];
            var m = httpRequest["DateMonth"];
            var vote = httpRequest["vote"];
            var year = string.IsNullOrEmpty(y) ? now.Year : int.Parse(y);
            var month = string.IsNullOrEmpty(m) ? now.Month : int.Parse(m);
            var date = new DateTime(year, month, 1);
            var fromDate = date.TotalSeconds();
            var toDate = date.AddMonths(1).TotalSeconds();
            var query = from o in FDIDB.DN_ContentVote
                        where o.AgencyId == agencyid && o.DateCreated >= fromDate && o.DateCreated <= toDate && o.DN_Tree.UserName == username
                        orderby o.ID descending
                        select new ContentVoteItem
                        {
                            ID = o.ID,
                            Value = o.Value,
                            Content = o.Content,
                            VoteName = o.DN_Vote.Name,
                            VoteID = o.VoteID,
                            LevelName = o.LevelVote.Name,
                            DateCreated = o.DateCreated,
                            DateEvaluation = o.DateEvaluation
                        };

            if (!string.IsNullOrEmpty(vote))
            {
                var t = int.Parse(vote);
                query = query.Where(c => c.VoteID == t);
            }
            query = query.SelectByRequest(Request);
            total = query.Sum(c => c.Value ?? 0);
            query = query.SelectPageByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<GeneralVoteItem> GeneralListTotal(int year, int agencyId)
        {
            var t = (int)CORE.OrderStatus.Complete;
            var date = new DateTime(year, 1, 1);
            var star = date.TotalSeconds();
            var end = date.AddYears(1).TotalSeconds();
            var query = from c in FDIDB.DN_ContentVote
                        where c.DateCreated >= star && c.DateCreated <= end
                        select new GeneralVoteItem
                        {
                            Date = c.DateCreated,
                            Value = c.Value,
                            GuiId = c.DN_Tree.DN_UsersInRoles.UserId
                        };
            return query.ToList();
        }

        public SumContentVoteItem SumWMY(Guid userid)
        {
            var date = DateTime.Today;
            decimal endweek, endMonth, endYear;
            var startweek = date.WeekStart(out endweek);
            var startMonth = date.MonthStart(out endMonth);
            var startYear = date.YearStart(out endYear);
            var obj = new SumContentVoteItem
            {
                Week = FDIDB.DN_ContentVote.Where(m => m.DateCreated > startweek && m.DateCreated < endweek && m.DN_Tree.DN_UsersInRoles.UserId == userid).Sum(m => m.Value),
                Month = FDIDB.DN_ContentVote.Where(m => m.DateCreated > startMonth && m.DateCreated < endMonth && m.DN_Tree.DN_UsersInRoles.UserId == userid).Sum(m => m.Value),
                Year = FDIDB.DN_ContentVote.Where(m => m.DateCreated > startYear && m.DateCreated < endYear && m.DN_Tree.DN_UsersInRoles.UserId == userid).Sum(m => m.Value),
            };
            return obj;
        }
        public List<ContentVoteItem> GetListByUserId(Guid UserId)
        {
            var query = from o in FDIDB.DN_ContentVote
                        where o.UserID == UserId
                        orderby o.ID descending
                        select new ContentVoteItem
                        {
                            ID = o.ID,
                            Content = o.Content,
                            AgencyId = o.AgencyId
                        };
            return query.ToList();
        }
        public List<ContentVoteItem> ListItemByUserId(Guid? userid, int agencyid, string date)
        {
            var now = date.StringToDate();
            var startDate = now.TotalSeconds();
            var endDate = now.AddDays(1).TotalSeconds();
            var query = from o in FDIDB.DN_ContentVote
                        where o.DN_Tree.DN_UsersInRoles.UserId == userid && o.AgencyId == agencyid && o.DateCreated > startDate && o.DateCreated < endDate
                        orderby o.DateCreated descending
                        select new ContentVoteItem
                        {
                            ID = o.ID,
                            DNTreeItem = new DNTreeItem
                            {
                                Name = o.DN_Tree.Name
                            },
                            UserItem = new DNUserItem
                            {
                                LoweredUserName = o.DN_Users.LoweredUserName
                            },
                            VoteName = o.DN_Vote.Name,
                            LevelName = o.LevelVote.Name,
                            DateCreated = o.DateCreated,
                            UserName = o.DN_Tree.DN_UsersInRoles.DN_Users.UserName,
                            FullName = o.DN_Tree.DN_UsersInRoles.DN_Users.LoweredUserName,
                            Value = o.Value,
                            Content = o.Content
                        };
            return query.ToList();
        }

        public DN_ContentVote GetByDate(int voteId, int treeId, decimal star, decimal end)
        {
            var query = from o in FDIDB.DN_ContentVote
                        where o.VoteID == voteId && o.TreeID == treeId && o.DateCreated >= star && o.DateCreated < end
                        select o;
            return query.FirstOrDefault();
        }
        public void Add(DN_ContentVote item)
        {
            FDIDB.DN_ContentVote.Add(item);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
