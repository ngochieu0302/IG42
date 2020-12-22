using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FDI.Web;
using FDI.Web.Models;
using FDI.Utils;
using FDI.GetAPI;
using FDI.CORE;

namespace FDI.Enterprises.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /AccountAdmin/      
        readonly DNEnterprisesAPI _dnLoginApi = new DNEnterprisesAPI();
        public ViewResult Index()
        {

            return View();
        }

        [AllowAnonymous]
        public ActionResult LogOn()
        {
            var returnUrl = Request["url"];
            ViewBag.url = returnUrl;
            return ContextDependentView();
        }

        //
        // POST: /Account/JsonLogOn

        [AllowAnonymous]
        [HttpPost]
        public JsonResult JsonLogOn(LogOnModel model, string returnUrl)
        {

            if (Membership.ValidateUser(model.UserName, model.Password))
            {
                FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                return Json(new { success = true, redirect = returnUrl });
            }
            ModelState.AddModelError("", string.Format("The user {0}name or password provided is incorrect.", ""));

            // If we got this far, something failed
            return Json(new { errors = GetErrorsFromModelState() });
        }
        private ActionResult ContextDependentView()
        {
            var actionName = ControllerContext.RouteData.GetRequiredString("action");
            if (Request.QueryString["content"] != null)
            {
                ViewBag.FormAction = "Json" + actionName;
                return PartialView();
            }
            ViewBag.FormAction = actionName;
            return View();
        }

        private IEnumerable<string> GetErrorsFromModelState()
        {
            return ModelState.SelectMany(x => x.Value.Errors
                .Select(error => error.ErrorMessage));
        }

        public ActionResult Logo()
        {
            if (Request.Url != null) Utility._d = Request.Url.Host;
            var model = _dnLoginApi.GetContent(Utility._d);
            return View(model);
        }

        public ActionResult Content()
        {
            if (Request.Url != null) Utility._d = Request.Url.Host;
            var model = _dnLoginApi.GetContent(Utility._d);
            return View(model);
        }
        //
        // POST: /Account/LogOn
        [AllowAnonymous]
        [HttpPost]
        public ActionResult LogOn(LogOnModel model)
        {
            var returnUrl = Request["itemUrl"].Replace("/Account/LogOn", "");
            var code = DateTime.Now.TotalSeconds().ToString();
            if (Request.Url != null) Utility._d = Request.Url.Host;
            var obj = _dnLoginApi.Login(code, model.UserName, Utility._d, model.Password, model.RememberMe);
            //return Json(obj, JsonRequestBehavior.AllowGet);
            if (obj > 0)
            {
                //Gọi stored báo login thành công
                #region Cookie CodeLogin
                var expires = model.RememberMe ? DateTime.Now.AddDays(2) : DateTime.Now.AddHours(1);
                var codeCookie = HttpContext.Request.Cookies["CodeE"];
                if (codeCookie == null)
                {
                    codeCookie = new HttpCookie("CodeE") { Value = code, Expires = expires };
                    Response.Cookies.Add(codeCookie);
                }
                else
                {
                    codeCookie.Value = code;
                    codeCookie.Expires = expires;
                    Response.Cookies.Add(codeCookie);
                }
                #endregion
                return Redirect("/");
            }
            ModelState.AddModelError("", string.Format("{0}Tên tài khoản hoặc mật khẩu không chính xác.", ""));
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            var codeCookie = HttpContext.Request.Cookies["CodeE"];
            if (codeCookie != null && _dnLoginApi.LogOut(codeCookie.ToString()) > 0)
            {
                codeCookie.Value = null;
                codeCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(codeCookie);
            }
            return RedirectToAction("LogOn", "Account");
        }
        //
        // GET: /Account/ChangePassword

        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword
        [HttpPost]
        public ActionResult ActionChangePassword(ChangePasswordModel model)
        {
            JsonMessage msg;
            var changePasswordSucceeded = false;
            try
            {
                var codeCookie = HttpContext.Request.Cookies["CodeE"];
                if (codeCookie != null)
                {
                    changePasswordSucceeded = _dnLoginApi.UpdatePass(codeCookie.Value, model.OldPassword, model.NewPassword) == 1;
                }
            }
            catch (Exception)
            {
                changePasswordSucceeded = false;
            }

            if (changePasswordSucceeded)
            {
                msg = new JsonMessage
                {
                    Erros = false,
                    Message = "Đổi mật khẩu thành công !"
                };
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            msg = new JsonMessage
            {
                Erros = true,
                Message = "Sai mật khẩu hoặc mật khẩu mới không đúng định dạng.</br> Vui lòng kiểm tra lại!"
            };
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Account/ChangePasswordSuccess
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }
    }
}
