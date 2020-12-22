using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
	public class LevelVoteDa : BaseDA
	{
		public LevelVoteDa()
		{
		}
		public LevelVoteDa(string pathPaging)
		{
			PathPaging = pathPaging;
		}
		public LevelVoteDa(string pathPaging, string pathPagingExt)
		{
			PathPaging = pathPaging;
			PathPagingext = pathPagingExt;
		}
		public List<LevelVoteItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
		{
			Request = new ParramRequest(httpRequest);
			var query = from o in FDIDB.LevelVotes
						orderby o.ID descending
						select new LevelVoteItem
						{
							ID = o.ID,
							Name = o.Name,
							Soft = o.Soft,
							Value = o.Value,
							AgencyID = o.AgencyID
						};
			query = query.SelectByRequest(Request, ref TotalRecord);
			return query.ToList();
		}
		public List<LevelVoteItem> GetList(int agencyId)
		{
			var query = from o in FDIDB.LevelVotes
                //where o.AgencyID == agencyId
				orderby o.Soft
				select new LevelVoteItem
				{
					ID = o.ID,
					Name = o.Name,
					Value = o.Value,
				};
			return query.ToList();
		}
		public List<LevelVoteItem> GetListSimple(int agencyId)
		{
			var query = from o in FDIDB.LevelVotes
                        //where o.AgencyID == agencyId
						orderby o.Soft
						select new LevelVoteItem
						{
							ID = o.ID,
							Name = o.Name,
							Value = o.Value,
							AgencyID = o.AgencyID
						};
			return query.ToList();
		}

		public LevelVoteItem GetLevelVoteItem(int id)
		{
			var query = from o in FDIDB.LevelVotes
						where o.ID == id
						orderby o.ID descending
						select new LevelVoteItem
						{
                            ID = o.ID,
							Name = o.Name,
							Soft = o.Soft,
							Value = o.Value,
							AgencyID = o.AgencyID
						};
			return query.FirstOrDefault();
		}

		public List<LevelVote> GetListArrId(List<int> lst)
		{
			var query = from o in FDIDB.LevelVotes where lst.Contains(o.ID) select o;
			return query.ToList();
		}

		public LevelVote GetById(int id)
		{
			var query = from o in FDIDB.LevelVotes
						where o.ID == id
						select o;
			return query.FirstOrDefault();
		}
		public void Add(LevelVote item)
		{
			FDIDB.LevelVotes.Add(item);
		}
		public void Delete(LevelVote item)
		{
			FDIDB.LevelVotes.Remove(item);
		}
		public void Save()
		{
			FDIDB.SaveChanges();
		}
	}
}
