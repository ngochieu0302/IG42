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
    public class DNLevelRoomController : BaseApiController
    {
        private readonly DNLevelRoomDA _da = new DNLevelRoomDA();
       
        public ActionResult GetList(string key, int agencyId)
        {
            var obj = Request["key"] != Keyapi ? new List<DNLevelRoomItem>() : _da.GetList(agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAll(string key, string code)
        {
            var obj = key != Keyapi ? new List<DNLevelRoomShowItem>() : _da.GetAll(Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLevelRoomItem(string key, int id)
        {
            var obj = Request["key"] != Keyapi ? new DNLevelRoomItem() : _da.GetLevelRoomItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelDNLevelRoomItem()
                : new ModelDNLevelRoomItem { ListItems = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListBed(string key, int agencyId)
        {
            var obj = Request["key"] != Keyapi ? new List<DNLevelRoomItem>() : _da.GetListBed(agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListTree(string key, int agencyId)
        {
            var obj = Request["key"] != Keyapi ? new List<TreeViewItem>() : _da.GetListTree(agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult GetListParent(string key, int agencyId)
        //{
        //    var obj = Request["key"] != Keyapi ? new List<TreeViewItem>() : _da.GetListParent(agencyId);
        //    return Json(obj, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult GetListParentID(string key)
        {
            var obj = Request["key"] != Keyapi ? new List<DNLevelRoomItem>() : _da.GetListParentID(Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key, string json, string code)
        {
            var model = new DN_Level();
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                model.Name = HttpUtility.UrlDecode(model.Name);
                model.IsDeleted = false;
                model.AgencyId = Agencyid();
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
        public ActionResult Delete(string key, string lstArrId)
        {
            if (key == Keyapi)
            {
                var lstInt = FDIUtils.StringToListInt(lstArrId);
                var lst = _da.GetListByArrId(lstInt);
                foreach (var item in lst)
                {
                    item.IsDeleted = true;
                }
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Show(string key, string lstArrId)
        {
            if (key == Keyapi)
            {
                var lstInt = FDIUtils.StringToListInt(lstArrId);
                var lst = _da.GetListByArrId(lstInt);
                foreach (var item in lst)
                {
                    item.IsShow = true;
                }
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Hide(string key, string lstArrId)
        {
            if (key == Keyapi)
            {
                var lstInt = FDIUtils.StringToListInt(lstArrId);
                var lst = _da.GetListByArrId(lstInt);
                foreach (var item in lst)
                {
                    item.IsShow = false;
                }
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}
