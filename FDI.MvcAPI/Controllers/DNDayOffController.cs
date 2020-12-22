using System.Collections.Generic;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using Newtonsoft.Json;


namespace FDI.MvcAPI.Controllers
{
    public class DNDayOffController : BaseApiController
    {
        //
        // GET: /DNActive/
                
        private readonly DayOffDA _da = new DayOffDA();
        
        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelDayOffItem()
                : new ModelDayOffItem { ListItem = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAll(string key, string code)
        {
            var obj = key != Keyapi ? new List<DayOffItem>() : _da.GetAll(Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetById(string key, int id)
        {
            var obj = key != Keyapi ? new DN_DayOff() : _da.GetById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new DayOffItem() : _da.GetItemById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListByArrId(string key, string lstId)
        {
            var obj = key != Keyapi ? new List<DayOffItem>() : _da.GetListByArrId(lstId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Add(string key, string json)
        {
            if (key == Keyapi)
            {
                var dayOff = JsonConvert.DeserializeObject<DayOffItem>(json);
                var obj = new DN_DayOff();
                dayOff.AgencyID = Agencyid();
                UpdateBase(obj, dayOff);
                _da.Add(obj);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(string key, string json, int id)
        {
            if (key == Keyapi)
            {
                var dayOff = JsonConvert.DeserializeObject<DayOffItem>(json);
                var obj = _da.GetById(id);
                dayOff.AgencyID = Agencyid();
                UpdateBase(obj, dayOff);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }

            return Json(0, JsonRequestBehavior.AllowGet);
        }        

        public DN_DayOff UpdateBase(DN_DayOff dnDayOff, DayOffItem dayOffItem)
        {
            dayOffItem.AgencyID = dayOffItem.AgencyID;
            dnDayOff.Name = dayOffItem.Name;
            dnDayOff.Quantity = dayOffItem.Quantity;
            dnDayOff.Date = dayOffItem.Date;
            dnDayOff.AgencyID = dayOffItem.AgencyID;
            dnDayOff.IsYear = dayOffItem.IsYear;
            dnDayOff.IsShow = dayOffItem.IsShow;
            dnDayOff.IsDelete = dayOffItem.IsDelete;
            return dnDayOff;
        }
    }
}
