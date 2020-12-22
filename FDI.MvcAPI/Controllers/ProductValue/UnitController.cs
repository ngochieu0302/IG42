using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.Simple;
using FDI.DA;

namespace FDI.MvcAPI.Controllers
{
    public class UnitController : BaseApiController
    {
        private readonly DNUnitDA _da = new DNUnitDA("#");
        public ActionResult GetList(string key, string code)
        {
            var obj = key != Keyapi ? new List<DNUnitItem>() : _da.GetList(Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllList(string key)
        {
            var obj = key != Keyapi ? new List<DNUnitItem>() : _da.GetList();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelDNUnitItem()
                : new ModelDNUnitItem { ListItems = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckByName(string key, string name, int id, int agencyId)
        {
            name = HttpUtility.UrlDecode(name);
            var b = key == Keyapi && _da.CheckByName(name, id, agencyId);
            return Json(b ? 1 : 0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUnitItem(string key,int id)
        {
            var obj = key != Keyapi ? new DNUnitItem() : _da.GetUnitItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key, string json)
        {
            var model = new DN_Unit();
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                model.Name = HttpUtility.UrlDecode(model.Name);
                _da.Add(model);
                _da.Save();
                return Json(model.ID, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Update(string key, string json)
        {
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetById(ItemId);
                UpdateModel(model);
                model.Name = HttpUtility.UrlDecode(model.Name);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

	    public ActionResult Delete(string key)
	    {
		    try
		    {
			    if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
			    var model = _da.GetById(ItemId);
			    _da.Delete(model);
			    _da.Save();
			    return Json(1, JsonRequestBehavior.AllowGet);
		    }
		    catch (Exception)
		    {
			    return Json(0, JsonRequestBehavior.AllowGet);
		    }
	    }
	}
}
