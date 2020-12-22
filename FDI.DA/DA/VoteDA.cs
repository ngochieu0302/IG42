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
	public class VoteDA : BaseDA
	{
		public VoteDA()
		{
		}
		public VoteDA(string pathPaging)
		{
			PathPaging = pathPaging;
		}
		public VoteDA(string pathPaging, string pathPagingExt)
		{
			PathPaging = pathPaging;
			PathPagingext = pathPagingExt;
		}

		public List<VoteItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyid)
		{
			Request = new ParramRequest(httpRequest);
			var query = from o in FDIDB.DN_Vote
						where o.AgencyID == agencyid
						orderby o.ID descending
						select new VoteItem
						{
							ID = o.ID,
							Name = o.Name,
							AgencyID = o.AgencyID,
							Value = o.Value,
							Soft = o.Soft,
							IsVote = o.IsVote
						};
			query = query.SelectByRequest(Request, ref TotalRecord);
			return query.ToList();
		}

		public List<VoteItem> GetListSimple(int agencyId)
		{
			var query = from o in FDIDB.DN_Vote
						where o.AgencyID == agencyId
						orderby o.ID descending
						select new VoteItem
						{
							ID = o.ID,
							Name = o.Name,
							AgencyID = o.AgencyID,
							Value = o.Value,
							Soft = o.Soft,
							IsVote = o.IsVote
						};
			return query.ToList();
		}
		public List<VoteItem> GetList(int agencyId, int treeid, Guid? userid, string dates)
		{
            var date = dates.StringToDate();
            //var datee = dates + 86400;
            //var now = DateTime.Now;
            //var date = new DateTime(now.Year, now.Month, now.Day);
            var startDate = date.TotalSeconds();
            var endDate = date.AddDays(1).TotalSeconds();
            var query = from o in FDIDB.DN_Vote
						where o.AgencyID == agencyId
						orderby o.Soft
						select new VoteItem
						{
							ID = o.ID,
							Name = o.Name,
							Value = o.Value,
							IsVote = o.IsVote,
							ContentVoteItems = o.DN_ContentVote.Where(m => m.UserID == userid && m.TreeID == treeid && m.DateEvaluation > startDate && m.DateEvaluation < endDate).Select(m => new ContentVoteItem
							{
								ID = m.ID,
								LevelVoteID = m.LevelVoteID,
								Value = m.Value,
								UserID = userid,
								Content = m.Content,
								//IsEdit = m.DateCreated < date
								//DNTreeItem = new DNTreeItem
								//{
								//	ID = treeid,
								//	Name = m.DN_Tree.Name,
								//	UserName = m.DN_Tree.DN_UsersInRoles.DN_Users.LoweredUserName
								//},
							})
						};
			return query.ToList();
		}
		/// <summary>
		/// Thống kê đánh giá theo tiêu chí đánh giá
		/// Ducda 8/7/2017
		/// </summary>
		/// <param name="agencyId">id đại lý</param>
		/// <param name="userid">id user</param>
		/// <param name="dates">Ngày bắt đầu</param>
		/// <param name="datee">ngày kết thúcs</param>
		/// <returns></returns>
		public List<VoteItem> GetListSumUser(int agencyId, Guid userid, decimal dates, decimal datee)
		{
			var query = from o in FDIDB.DN_Vote
						where o.AgencyID == agencyId && o.DN_ContentVote.Any(m => m.DN_Tree.DN_UsersInRoles.UserId == userid && m.DateCreated > dates && m.DateCreated < datee)
						orderby o.Soft
						select new VoteItem
						{
							ID = o.ID,
							Name = o.Name,
                            DNUserVoteItem = FDIDB.DN_Users.Where(m=>m.UserId == userid).Select(m=>new DNUserVoteItem
                            {
                                UserId = userid,
                                LoweredUserName = m.LoweredUserName,
                                UserName = m.UserName
                            }).FirstOrDefault(),
							Value = o.Value,
							TotalValue = o.DN_ContentVote.Where(m => m.DN_Tree.DN_UsersInRoles.UserId == userid && m.DateCreated > dates && m.DateCreated < datee).Sum(m => m.Value)
						};
			return query.ToList();
		}
		/// <summary>
		/// Thống kê đáng giá theo user
		/// Ducda 8/7/2017
		/// </summary>
		/// <param name="agencyId"></param>
		/// <param name="dates"></param>
		/// <param name="datee"></param>
		/// <returns></returns>
		public List<DNUserVoteItem> GetSumListUser(int agencyId, decimal dates, decimal datee)
		{
			var query = from o in FDIDB.DN_Users
						where o.AgencyID == agencyId && o.DN_UsersInRoles.Any(m =>m.IsDelete == false && m.DN_Tree.Any(n => n.DN_ContentVote.Any(c => c.DateCreated > dates && c.DateCreated < datee)))
						select new DNUserVoteItem
						{
							UserId = o.UserId,
							UserName = o.UserName,
                            LoweredUserName = o.LoweredUserName,
                            ListTree = FDIDB.DN_Tree.Where(m=>m.DN_UsersInRoles.UserId == o.UserId).Select(n=>n.Name),
                            TotalValue = o.DN_UsersInRoles.Where(v => v.IsDelete == false).Sum(m => m.DN_Tree.Sum(n => n.DN_ContentVote.Where(c => c.DateCreated > dates && c.DateCreated < datee).Sum(c => c.Value)))
						};
			return query.ToList();
		}

		public VoteItem GetVoteItem(int id)
		{
			var query = from o in FDIDB.DN_Vote
						where o.ID == id
						orderby o.ID descending
						select new VoteItem
						{
							ID = o.ID,
							Name = o.Name,
							AgencyID = o.AgencyID,
							Value = o.Value,
							Soft = o.Soft,
							IsVote = o.IsVote
						};
			return query.FirstOrDefault();
		}

		public DN_Vote GetById(int id)
		{
			var query = from o in FDIDB.DN_Vote where o.ID == id select o;
			return query.FirstOrDefault();
		}

		public List<DN_Vote> GetListArrId(List<int> lst)
		{
			var query = from o in FDIDB.DN_Vote where lst.Contains(o.ID) select o;
			return query.ToList();
		}
		public void Add(DN_Vote item)
		{
			FDIDB.DN_Vote.Add(item);
		}
		public void Delete(DN_Vote item)
		{
			FDIDB.DN_Vote.Remove(item);
		}
		public void Save()
		{
			FDIDB.SaveChanges();
		}
	}
}
