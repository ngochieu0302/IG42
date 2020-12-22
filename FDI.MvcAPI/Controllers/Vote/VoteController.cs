using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers
{
	public class VoteController : BaseApiController
	{
		readonly VoteDA _da = new VoteDA();
		public ActionResult GetListSimple(string key, int agencyId)
		{
			var obj = Request["key"] != Keyapi ? new List<VoteItem>() : _da.GetListSimple(agencyId);
			return Json(obj, JsonRequestBehavior.AllowGet);
		}
		public ActionResult ListItems()
		{
			var obj = Request["key"] != Keyapi
				? new ModelVoteItem()
                : new ModelVoteItem { ListItems = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
			return Json(obj, JsonRequestBehavior.AllowGet);
		}
        public ActionResult GetList(string key, int agencyId, int treeid, string code, string date, Guid UserId)		
		{
			var obj = Request["key"] != Keyapi ? new List<VoteItem>() : _da.GetList(agencyId, treeid, UserId, date);
			return Json(obj, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetVoteItem(string key, int id)
		{
			var obj = Request["key"] != Keyapi ? new VoteItem() : _da.GetVoteItem(id);
			return Json(obj, JsonRequestBehavior.AllowGet);
		}

		//
		public ActionResult GetListSumUser(string key, string code, Guid userid, decimal dates, decimal datee)
		{
            var obj = Request["key"] != Keyapi ? new List<VoteItem>() : _da.GetListSumUser(Agencyid(), userid, dates, datee);
			return Json(obj, JsonRequestBehavior.AllowGet);
		}

		//
		public ActionResult GetSumListUser(string key, string code, decimal dates, decimal datee)
		{
            var obj = Request["key"] != Keyapi ? new List<DNUserVoteItem>() : _da.GetSumListUser(Agencyid(), dates, datee);
			return Json(obj, JsonRequestBehavior.AllowGet);
		}



		public ActionResult Add(string key, string json)
		{
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            var model = new DN_Vote();
			try
			{
				if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
				UpdateModel(model);
				model.Name = HttpUtility.UrlDecode(model.Name);
				_da.Add(model);
				_da.Save();				
			}
			catch (Exception ex)
			{
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được thêm mới.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
		public ActionResult Update(string key, string json)
		{
            var msg = new JsonMessage(false,"Cập nhật dữ liệu thành công.");
			try
			{
				if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
				var model = _da.GetById(ItemId);
				UpdateModel(model);
				model.Name = HttpUtility.UrlDecode(model.Name);
				_da.Save();				
			}
			catch (Exception ex)
			{
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được cập nhật.";
                Log2File.LogExceptionToFile(ex);
			}
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
		public ActionResult Delete(string key, string lstArrId)
		{
			if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
			var lstInt = FDIUtils.StringToListInt(lstArrId);
			var lst = _da.GetListArrId(lstInt);
			//foreach (var item in lst)
			//{
			//    item.IsDelete = true;
			//}
			_da.Save();
			return Json(1, JsonRequestBehavior.AllowGet);
		}
	}
}
