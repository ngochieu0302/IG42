using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FDI.CORE;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers
{
    public class DNEnterprisesController : BaseApiController
    {
        readonly EnterprisesDA _da = new EnterprisesDA();
        public ActionResult Login(string key, string codee, string username, string domain, string pass, bool isonline)
        {
            if (key == Keyapi)
            {
                var obj = _da.GetPassByUserName(username, domain);
                if (obj != null)
                {
                    var sha1PasswordHash = FDIUtils.CreatePasswordHash(obj.PasswordSalt, pass);
                    if (sha1PasswordHash == obj.Password)
                    {
                        var datenow = DateTime.Now;
                        var datelogin = datenow.TotalSeconds();
                        var dateout = isonline ? datenow.AddDays(2).TotalSeconds() : datenow.AddHours(1).TotalSeconds();
                        obj.IsOnline = true;
                        obj.DateLogin = datelogin;
                        obj.DateOut = dateout;
                        obj.CodeLogin = codee;
                        _da.Save();
                        return Json(1, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdatePass(string key, string codee, string pass, string passnew)
        {
            if (key == Keyapi)
            {
                var obj = _da.GetPassByUserName(codee);
                if (obj != null && obj.IsOnline == true)
                {
                    var sha1PasswordHash = FDIUtils.CreatePasswordHash(obj.PasswordSalt, pass);
                    if (sha1PasswordHash == obj.PasswordSalt)
                    {
                        obj.DateUpdatePass = DateTime.Now.TotalSeconds();
                        obj.Password = FDIUtils.CreatePasswordHash(obj.PasswordSalt, passnew);
                        _da.Save();
                        return Json(1, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LogOut(string key, string codee)
        {
            if (key == Keyapi)
            {
                var obj = _da.GetPassByUserName(codee);
                if (obj != null && obj.IsOnline == true)
                {
                    obj.IsOnline = false;
                    _da.Save();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemByCodeLogin(string key, string codee)
        {
            var obj = Request["key"] != Keyapi ? new EnterprisesItem() : _da.GetItemByCodeLogin(codee);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListStGroupById(string key)
        {
            var obj = Request["key"] != Keyapi ? new List<STGroupItem>() : _da.GetListStGroupById(EnterprisesId());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetContent(string key, string domain)
        {
            //var time = DateTime.Now;
            var obj = Request["key"] != Keyapi ? new EnterprisesItem() : _da.GetContent(domain);
            //var time1 = DateTime.Now - time;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetStaticEnterprise(string key)
        {
            var obj = Request["key"] != Keyapi ? new ModelTotalItem() : _da.GetStaticEnterprise(EnterprisesId());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }
}