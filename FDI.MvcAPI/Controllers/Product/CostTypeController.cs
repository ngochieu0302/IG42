using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers
{
    public class CostTypeController : BaseApiController
    {
        //
        // GET: /CostType/
        //readonly CostTypeBL _bl = new CostTypeBL();
        readonly CostTypeDA _da = new CostTypeDA();
        public ActionResult GetListSimple(string key, int agencyId)
        {
            var obj = Request["key"] != Keyapi
                ? new ModelCostType()
                : new ModelCostType { ListItem = _da.GetListSimple(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetList(string key, int agencyId,int type)
        {
            var obj = Request["key"] != Keyapi ? new List<CostTypeItem>() : _da.GetList(agencyId, type);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCostTypeItem(string key, int id)
        {
            var obj = Request["key"] != Keyapi ? new CostTypeItem() : _da.GetCostTypeItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListAuto(string key, string keword, int showLimit, int agencyId, int type)
        {
            var obj = key != Keyapi ? new List<SuggestionsProduct>() : _da.GetListAuto(keword, showLimit, agencyId, type);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key, string json)
        {
            var model = new CostType();
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                model.Name = HttpUtility.UrlDecode(model.Name);
                model.Description = HttpUtility.UrlDecode(model.Description);
                model.DateCreate = DateTime.Now.TotalSeconds();
                model.IsDelete = false;
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
            if (key == Keyapi)
            {
                var model = _da.GetById(ItemId);
                UpdateModel(model);
                model.Name = HttpUtility.UrlDecode(model.Name);
                model.Description = HttpUtility.UrlDecode(model.Description);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(string key, string lstArrId)
        {
            if (key == Keyapi)
            {
                var lstInt = FDIUtils.StringToListInt(lstArrId);
                var lst = _da.GetListArrId(lstInt);
                foreach (var item in lst)
                {
                    item.IsDelete = true;
                }
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}
