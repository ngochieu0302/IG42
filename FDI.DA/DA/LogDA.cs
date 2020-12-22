using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;
using FDI.DA;

namespace FDI.DA
{
	public class LogDA : BaseDA
	{
		#region Constructor
		public LogDA()
		{
		}

		public LogDA(string pathPaging)
		{
			PathPaging = pathPaging;
		}

		public LogDA(string pathPaging, string pathPagingExt)
		{
			PathPaging = pathPaging;
			PathPagingext = pathPagingExt;
		}
		#endregion

		public List<LogItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
		{
			Request = new ParramRequest(httpRequest);
			var query = from o in FDIDB.Logs
						orderby o.ID descending
						select new LogItem
						{
							ID = o.ID,
							User = o.User,
							ChucNang = o.ChucNang,
							NgayTao = o.NgayTao,
							CuaHang = o.CuaHang,
							NoiDung = o.NoiDung
						};
			query = query.SelectByRequest(Request, ref TotalRecord);
			return query.ToList();
		}

		public Log GetById(int id)
		{
			var query = from c in FDIDB.Logs where c.ID == id select c;
			return query.FirstOrDefault();
		}

		public void Add(Log log)
		{
			FDIDB.Logs.Add(log);
		}

		public void Delete(Log log)
		{
			FDIDB.Logs.Remove(log);
		}

		public void Save()
		{
			FDIDB.SaveChanges();
		}

		public string GetIdLog()
		{
			int countProduct = FDIDB.Logs.OrderByDescending(m => m.ID).Select(m => m.ID).FirstOrDefault();
			int nextNumber = countProduct + 1;
			return nextNumber.ToString();
		}

		public List<Log> GetListByArrId(string ltsArrID)
		{
			var arrId = FDIUtils.StringToListInt(ltsArrID);
			var query = from c in FDIDB.Logs where arrId.Contains(c.ID) select c;
			return query.ToList();
		}
	}
}
