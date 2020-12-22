using System.Collections.Generic;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using Newtonsoft.Json;


namespace FDI.MvcAPI.Controllers
{
    public class DNFileMailController : BaseApiController
    {
        //
        // GET: /DNActive/

        private readonly DNFileMailDA _dl = new DNFileMailDA();

        public ActionResult GetListSimpleByRequest()
        {
            var obj = Request["key"] != Keyapi ? new List<DNFileMailItem>() : _dl.GetListSimpleByRequest(Request, Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAll(string key, string code)
        {
            var obj = key != Keyapi ? new List<DNFileMailItem>() : _dl.GetAll(Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetById(string key, int id)
        {
            var obj = key != Keyapi ? new DN_File_Mail() : _dl.GetById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new DNFileMailItem() : _dl.GetItemById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListByArrId(string key, string lstId)
        {
            var obj = key != Keyapi ? new List<DNFileMailItem>() : _dl.GetListByArrId(lstId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Add(string key, string json)
        {
            if (key == Keyapi)
            {
                var dayOff = JsonConvert.DeserializeObject<DNFileMailItem>(json);
                var obj = new DN_File_Mail();
                dayOff.AgencyId = Agencyid();
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
                var dayOff = JsonConvert.DeserializeObject<DNFileMailItem>(json);
                var obj = _dl.GetById(id);
                dayOff.AgencyId = Agencyid();
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

        public DN_File_Mail UpdateBase(DN_File_Mail dnFileMail, DNFileMailItem DNFileMailItem)
        {
            DNFileMailItem.AgencyId = DNFileMailItem.AgencyId;
            dnFileMail.Name = DNFileMailItem.Name;
            dnFileMail.Url = DNFileMailItem.Url;
            dnFileMail.Folder = DNFileMailItem.Folder;
            dnFileMail.DateCreated = DNFileMailItem.DateCreated;
            dnFileMail.IsShow = DNFileMailItem.IsShow;
            dnFileMail.IsDeleted = DNFileMailItem.IsDeleted;
            return dnFileMail;
        }
    }
}
