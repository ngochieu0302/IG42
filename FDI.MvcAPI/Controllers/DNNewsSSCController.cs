using System.Collections.Generic;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using Newtonsoft.Json;


namespace FDI.MvcAPI.Controllers
{
    public class DNNewsSSCController : BaseApiController
    {
        //
        // GET: /DNNewsSSC/

        private readonly DNNewsSSCDA _dl = new DNNewsSSCDA();

        public ActionResult GetListSimpleByRequest()
        {
            var obj = Request["key"] != Keyapi ? new List<DNNewsSSCItem>() : _dl.GetListSimpleByRequest(Request);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAll(string key, string code)
        {
            var obj = key != Keyapi ? new List<DNNewsSSCItem>() : _dl.GetAll(Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetById(string key, int id)
        {
            var obj = key != Keyapi ? new DN_NewsSSC() : _dl.GetById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new DNNewsSSCItem() : _dl.GetItemById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListByArrId(string key, string lstId)
        {
            var obj = key != Keyapi ? new List<DNNewsSSCItem>() : _dl.GetListByArrId(lstId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Add(string key, string json)
        {
            if (key == Keyapi)
            {
                var dayOff = JsonConvert.DeserializeObject<DNNewsSSCItem>(json);
                var obj = new DN_NewsSSC();
                dayOff.AgencyID = Agencyid();
                UpdateBase(obj, dayOff);
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
                var dayOff = JsonConvert.DeserializeObject<DNNewsSSCItem>(json);
                var obj = _dl.GetById(id);
                dayOff.AgencyID = Agencyid();
                UpdateBase(obj, dayOff);
                _dl.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }

            return Json(0, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult Delete(string key, List<int> listint)
        //{
        //    if (key == Keyapi)
        //    {
        //        var list = _dl.GetListByArrId(listint);
        //        foreach (var item in list)
        //        {
        //            _dl.Delete(item);
        //        }
        //        _dl.Save();
        //        return Json(1, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(0, JsonRequestBehavior.AllowGet);
        //}

        public DN_NewsSSC UpdateBase(DN_NewsSSC newsSsc, DNNewsSSCItem newsSscItem)
        {
            newsSsc.AgencyID = newsSscItem.AgencyID;
            newsSsc.Title = newsSscItem.Title;
            newsSsc.Content = newsSscItem.Content;
            newsSsc.DateCreated = newsSscItem.DateCreated;
            newsSsc.IsShow = newsSscItem.IsShow;
            return newsSsc;
        }
    }
}
