using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.DA.DA.AppCustomer;
using FDI.Simple;

namespace FDI.MvcAPI.Controllers
{
    public class AgencyAppController : BaseApiAuthController
    {
        //
        // GET: /AgencyApp/
        private readonly AgencyAppDA _dl = new AgencyAppDA("#");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ListAgencyByKm(string key, double lat, double lng, int id, int aid, int km = 8)
        {
            var obj = _dl.ListAgencyByKm(lat, lng, id, aid, km);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAll(string key)
        {
            var list = key != Keyapi ? new List<ProductAppItem>() : _dl.GetAll();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAgencyItemByPhone(string phone)
        {
            var obj = _dl.GetAgencyItemByPhone(phone);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetById(string key, int id)
        {
            return key == Keyapi ? Json(_dl.GetById(id), JsonRequestBehavior.AllowGet) : Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}
