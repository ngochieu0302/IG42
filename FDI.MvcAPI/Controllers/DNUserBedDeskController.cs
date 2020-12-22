using System;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;
using Newtonsoft.Json;

namespace FDI.MvcAPI.Controllers
{
    public class DNUserBedDeskController : BaseApiController
    {
        //
        // GET: /DNBedDesk/
        readonly DNUserBedDeskDA _da = new DNUserBedDeskDA();
        
        public ActionResult GetById(string key, int id)
        {
            var obj = Request["key"] != Keyapi ? new DNUserBedDeskItem() : _da.GetItemById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateBedId(string key, int bedid, int id)
        {
            if (key == Keyapi)
            {
                var item = _da.GetById(id);
                item.BedDeskID = bedid;
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add(string key, string json)
        {
            if (key == Keyapi)
            {
                var item = JsonConvert.DeserializeObject<DNUserBedDeskItem>(json);
                var obj = new DN_User_BedDesk { BedDeskID = item.BedDeskID, UserID = item.UserID, MWSID = item.MWSID, DateCreated = ConvertDate.TotalSeconds(DateTime.Now), AgencyID = Agencyid() };
                _da.Add(obj);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(string key, int id)
        {
            if (key == Keyapi)
            {
                var item = _da.GetById(id);
                _da.Delete(item);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}
