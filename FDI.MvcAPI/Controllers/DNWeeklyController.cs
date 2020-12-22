using System.Collections.Generic;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using Newtonsoft.Json;


namespace FDI.MvcAPI.Controllers
{
    public class DNWeeklyController : BaseApiController
    {
        //
        // GET: /DNActive/

        private readonly DNWeeklyDA _dl = new DNWeeklyDA();

        public ActionResult GetListSimpleByRequest()
        {
            var obj = Request["key"] != Keyapi ? new List<WeeklyItem>() : _dl.GetListSimpleByRequest(Request);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAll(string key)
        {
            var obj = key != Keyapi ? new List<WeeklyItem>() : _dl.GetAll(); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetById(string key, int id)
        {
            var obj = key != Keyapi ? new DN_Weekly() : _dl.GetById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new WeeklyItem() : _dl.GetItemById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListByArrId(string key, string lstId)
        {
            var obj = key != Keyapi ? new List<WeeklyItem>() : _dl.GetListByArrId(lstId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add(string key, string json)
        {
            if (key == Keyapi)
            {
                var weeklyItem = JsonConvert.DeserializeObject<WeeklyItem>(json);
                var obj = new DN_Weekly();
                weeklyItem.AgencyID = Agencyid();
                UpdateBase(obj, weeklyItem);
                _dl.Add(obj);
                _dl.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(string key, string json, int id)
        {
            if (key == Keyapi)
            {
                var weeklyItem = JsonConvert.DeserializeObject<WeeklyItem>(json);
                var obj = _dl.GetById(id);
                weeklyItem.AgencyID = Agencyid();
                UpdateBase(obj, weeklyItem);
                _dl.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }

            return Json(0, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult Delete(string key, string lstInt)
        //{
        //    if (key == Keyapi)
        //    {
        //        var list = _dl.GetListByArrId(lstInt);
        //        foreach (var item in list)
        //        {
        //            _dl.Delete(item);
        //        }
        //        _dl.Save();
        //        return Json(1, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(0, JsonRequestBehavior.AllowGet);
        //}

        public DN_Weekly UpdateBase(DN_Weekly weekly, WeeklyItem weeklyItem)
        {
            weekly.Name = weeklyItem.Name;
            weekly.AgencyID = weeklyItem.AgencyID;
            weekly.IsShow = weeklyItem.IsShow;
            return weekly;
        }
    }
}
