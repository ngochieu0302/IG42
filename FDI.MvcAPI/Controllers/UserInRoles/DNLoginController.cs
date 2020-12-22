using System;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers
{
    public class DNLoginController : BaseApiController
    {
        //
        // GET: /DNUser/

        private readonly DNLoginDA _dl = new DNLoginDA("#");
        private readonly DNUserDA _dluserda = new DNUserDA("#");
        private readonly CustomerDA _customerDa = new CustomerDA("#");
        public ActionResult GetUserItemByCode(string key, string code)
        {
            var obj = key != Keyapi ? new DNUserItem() : _dl.GetUserItemByCode(code);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout(string key, string code)
        {
            if (key == Keyapi)
            {
                _dl.Logout(code);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Login(string key, string code, string username, string pass, bool ischeck, string domain)
        {
            if (key.ToLower() != Keyapi.ToLower())
                return Json(0, JsonRequestBehavior.AllowGet);
            var obj = _dluserda.GetPassByUserName(username, domain);
            //var lg = new Ultils();
            if (obj != null)
            {
                var date = DateTime.Now;
                //var codenew = lg.CodeLogin(date);
                //if (code.Substring(0, 5) == codenew.Substring(0, 5))
                //{
                obj.CodeLogin = code;
                var dateend = date.AddMinutes(20);
                if (ischeck) dateend = date.AddDays(5);
                var timeend = dateend.TotalSeconds();
                var pas = FDIUtils.CreatePasswordHash(pass, obj.PasswordSalt);
                if (obj.Password == pas)
                {
                    var dNlogin = new DN_Login
                    {
                        UserId = obj.UserId,
                        DateCreated = date.TotalSeconds(),
                        DateEnd = timeend,
                        Code = code,
                        IsOut = false
                    };
                    _dl.Add(dNlogin);
                    _dl.Save();
                    return Json(obj, JsonRequestBehavior.AllowGet);
                }
                //}
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCustomerByCode(string key, string code)
        {
            var obj = key != Keyapi ? new CustomerItem() : _dl.GetCustomerByCode(code);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CustomerLogin(string key, string code, string username, string pass, bool ischeck)
        {
            if (key != Keyapi)
                return Json(0, JsonRequestBehavior.AllowGet);
            var obj = _customerDa.GetPass(username);
            if (obj != null)
            {
                var date = DateTime.Now;
                var dateend = date.AddMinutes(20);
                var time = date.TotalSeconds();
                if (ischeck)
                {
                    dateend = date.AddDays(1);
                }
                var timeend = dateend.TotalSeconds();
                var pas = FDIUtils.CreatePasswordHash(pass, obj.PasswordSalt);
                if (obj.Password == pas)
                {
                    var dNlogin = new DN_Login
                    {
                        CustomerID = obj.ID,
                        DateCreated = time,
                        DateEnd = timeend,
                        Code = code,
                        IsOut = false
                    };
                    _dl.Add(dNlogin);
                    _dl.Save();
                    return Json(obj, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}
