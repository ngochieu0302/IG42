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
    public class DNComboController : BaseApiController
    {
        //
        // GET: /DNCombo/
        readonly DNComboDA _da = new DNComboDA();
        public ActionResult GetListSimple(string key, int agencyId)
        {
            var obj = Request["key"] != Keyapi ? new List<DNComboItem>() : _da.GetListSimple(agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelDNComboItem()
                : new ModelDNComboItem { ListItem = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetComboItem(string key, int id)
        {
            var obj = key != Keyapi ? new DNComboItem() : _da.GetComboItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key)
        {
            var model = new DN_Combo();
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                model.Name = HttpUtility.UrlDecode(model.Name);
                var dateStart = Request["DateStart_"];
                var pictureId = Convert.ToInt32(Request["Value_DefaultImages"]);
                if (pictureId > 0)
                    model.PictureID = pictureId;
                var dateEnd = Request["DateEnd_"];
                var lstRet = Request["lstRet"];
                model.DateStart = dateStart.StringToDecimal();
                model.DateEnd = dateEnd.StringToDecimal();
                var lstInt = FDIUtils.StringToListInt(lstRet);
                model.Shop_Product = _da.GetListProduct(lstInt);
                model.IsDeleted = false;
                _da.Add(model);
                _da.Save();
                return Json(model.ID, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Update(string key)
        {
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetById(ItemId);
                UpdateModel(model);
                var pictureId = Convert.ToInt32(Request["Value_DefaultImages"]);
                if (pictureId > 0)
                    model.PictureID = pictureId;
                var dateStart = Request["DateStart_"];
                var dateEnd = Request["DateEnd_"];
                var lstRet = Request["lstRet"];
                model.Name = HttpUtility.UrlDecode(model.Name);
                model.DateStart = dateStart.StringToDecimal();
                model.DateEnd = dateEnd.StringToDecimal();
                model.Shop_Product.Clear();
                var lstInt = FDIUtils.StringToListInt(lstRet);
                model.Shop_Product = _da.GetListProduct(lstInt);
               
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
    }
}
