using System;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers
{
    public class DNRoomController : BaseApiController
    {
        //private readonly DNLevelRoomBL _bl = new DNLevelRoomBL();
        private readonly DNRoomDA _da = new DNRoomDA();


        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelDNRoomItem()
                : new ModelDNRoomItem { ListItems = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemById(string key, int id)
        {
            var obj = Request["key"] != Keyapi ? new DNRoomItem() : _da.GetItemById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAll(string key, int id)
        {
            var obj = Request["key"] != Keyapi ? new DNRoomItem() : _da.GetItemById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add(string key, string json, string code, int agencyid)
        {
            if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
            var model = new DN_Room();
            try
            {
                UpdateModel(model);
                model.Name = HttpUtility.UrlDecode(model.Name);
                model.AgencyID = Agencyid();
                model.IsDeleted = false;
                _da.Add(model);
                _da.Save();
                if (model.Row > 0 && model.Column > 0)
                {
                    _da.AddDesk(model.Row, model.Column, model.ID, agencyid);
                }
                return Json(model.ID, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Update(string key, string json, int agencyid)
        {
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetById(ItemId);
                UpdateModel(model);
                model.Name = HttpUtility.UrlDecode(model.Name);
                _da.Save();
                if (model.Row > 0 && model.Column > 0)
                {
                    _da.AddDesk(model.Row, model.Column, model.ID, agencyid);
                }
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
    }
}
