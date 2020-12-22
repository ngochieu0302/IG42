using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using Newtonsoft.Json;


namespace FDI.MvcAPI.Controllers
{
    public class DNGroupMailSSCController : BaseApiController
    {
        //
        // GET: /DNActive/

        private readonly DNGroupMailSSCDA _dl = new DNGroupMailSSCDA();

        public ActionResult GetListSimpleByRequest()
        {
            var obj = Request["key"] != Keyapi ? new List<DNGroupMailSSCItem>() : _dl.GetListSimpleByRequest(Request, Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAll(string key, string code)
        {
            var obj = key != Keyapi ? new List<DNGroupMailSSCItem>() : _dl.GetAll(Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllByUserId(string key, string code, Guid userId)
        {
            var obj = key != Keyapi ? new List<DNGroupMailSSCItem>() : _dl.GetAllByUserId(Agencyid(), userId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetById(string key, int id)
        {
            var obj = key != Keyapi ? new DN_GroupEmail() : _dl.GetById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new DNGroupMailSSCItem() : _dl.GetItemById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListByArrId(string key, string lstId)
        {
            var obj = key != Keyapi ? new List<DNGroupMailSSCItem>() : _dl.GetListByArrId(lstId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Add(string key, string json)
        {
            if (key == Keyapi)
            {
                var dayOff = JsonConvert.DeserializeObject<DNGroupMailSSCItem>(json);
                var obj = new DN_GroupEmail();
                dayOff.AgencyID = Agencyid();
                UpdateBase(obj, dayOff);
                _dl.Add(obj);

                // add user
                var lstUser = _dl.GetUserArrId(dayOff.LstUserIds);
                foreach (var item in lstUser)
                    obj.DN_Users.Add(item);

                _dl.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(string key, string json, int id)
        {
            if (key == Keyapi)
            {
                var dnMailSsc = JsonConvert.DeserializeObject<DNGroupMailSSCItem>(json);
                var obj = _dl.GetById(id);
                dnMailSsc.AgencyID = Agencyid();
                UpdateBase(obj, dnMailSsc);

                // add user
                obj.DN_Users.Clear();
                var lstUser = _dl.GetUserArrId(dnMailSsc.LstUserIds);
                foreach (var item in lstUser)
                    obj.DN_Users.Add(item);

                _dl.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }

            return Json(0, JsonRequestBehavior.AllowGet);
        }
        
        public DN_GroupEmail UpdateBase(DN_GroupEmail groupEmail, DNGroupMailSSCItem mailSscItem)
        {
            groupEmail.AgencyID = mailSscItem.AgencyID;
            groupEmail.Name = mailSscItem.Name;
            groupEmail.Slug = mailSscItem.Slug;
            groupEmail.DateCreated = mailSscItem.DateCreated;
            groupEmail.IsShow = mailSscItem.IsShow;
            return groupEmail;
        }
    }
}
