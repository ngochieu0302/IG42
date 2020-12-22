using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FDI.Web.Models;
using FDI.Utils;
using FDI.GetAPI;
namespace FDI.Web.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /AccountAdmin/
        readonly DNLoginAPI _dnLoginApi = new DNLoginAPI();
        readonly DNUserAPI _dnUser = new DNUserAPI();
        readonly DNEnterprisesAPI _enterprisesApi =new DNEnterprisesAPI();
        public ViewResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult LogOn()
        {
            var returnUrl = Request["url"].Replace("/Account/LogOn", "");
            ViewBag.url = returnUrl;
            return ContextDependentView();
        }
        public ActionResult Logo()
        {
            if (Request.Url != null) Utility._d = Request.Url.Host;
            var model = _enterprisesApi.GetContent(Utility._d);
            return PartialView(model);
        }
        public ActionResult Content()
        {
            if (Request.Url != null) Utility._d = Request.Url.Host;
            var model = _enterprisesApi.GetContent(Utility._d);
            return PartialView(model);
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
            return ModelState.SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage));
        }
        //
        // POST: /Account/LogOn
        [AllowAnonymous]
        [HttpPost]
        public ActionResult LogOn(LogOnModel model)
        {
            var date = DateTime.Now;
            //var lg = new Ultils();
            var code = Ultils.CodeLogin(date);
            if (Request.Url != null) Utility._d = Request.Url.Host;
            var obj = _dnLoginApi.Login(code, model.UserName, model.Password, model.RememberMe, Utility._d);
            if (obj != null && obj.UserId != Guid.Empty)
            {
                //Gọi stored báo login thành công
                #region Cookie CodeLogin
                var expires = model.RememberMe ? date.AddDays(5) : date.AddMinutes(20);
                var codeCookie = HttpContext.Request.Cookies["CodeLogin"];
                //if (codeCookie == null)
                //{
                    codeCookie = new HttpCookie("CodeLogin") { Value = code, Expires = expires };
                    Response.Cookies.Add(codeCookie);
                    
                //}
                //else
                //{
                //    codeCookie.Value = code;
                //    codeCookie.Expires = expires;
                //    Response.Cookies.Add(codeCookie);
                //}
                return Redirect("/");
                #endregion
            }
            ModelState.AddModelError("", string.Format("{0}Tên người dùng hoặc mật khẩu được cung cấp là không chính xác.", ""));
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            string[] myCookies = Request.Cookies.AllKeys;
            foreach (string cookie in myCookies)
            {
                Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
            }
            //HttpRuntime.Close();
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
                var codeCookie = HttpContext.Request.Cookies["CodeLogin"];
                if (codeCookie != null)
                {
                    changePasswordSucceeded = _dnUser.UpdatePass(codeCookie.Value, model.OldPassword, model.NewPassword) == 1;
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
