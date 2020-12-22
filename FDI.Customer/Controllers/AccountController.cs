using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FDI.Customer.Models;
using FDI.Utils;
using FDI.GetAPI;
namespace FDI.Customer.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /AccountAdmin/
        readonly DNLoginAPI _dnLoginApi = new DNLoginAPI();
        readonly DNUserAPI _dnUser = new DNUserAPI();
        private readonly CustomerAPI _api = new CustomerAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ChangePassword()
        {
            return PartialView();
        }

        [AllowAnonymous]
        public ActionResult LogOn()
        {
            var returnUrl = Request["url"];
            ViewBag.url = returnUrl;
            return ContextDependentView();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ActionRegister()
        {
            var url = Request.Form.ToString();
            var msg = _api.Add(url);
            return Json(msg, JsonRequestBehavior.AllowGet);
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

        //
        // POST: /Account/LogOn
        [AllowAnonymous]
        [HttpPost]
        public ActionResult LogOn(LogOnModel model)
        {
            var returnUrl = Request["itemUrl"].Replace("/Account/LogOn", "");
            var code = ConvertDate.TotalSeconds(DateTime.Now).ToString();
            var obj = _dnLoginApi.CustomerLogin(code, model.UserName, model.Password, model.RememberMe);
            if (obj != null && obj.ID != 0)
            {
                #region Cookie CodeLogin
                var expires = model.RememberMe ? DateTime.Now.AddDays(1) : DateTime.Now.AddMinutes(20);
                var codeCookie = HttpContext.Request.Cookies["CusLogin"];
                if (codeCookie == null)
                {
                    codeCookie = new HttpCookie("CusLogin") { Value = code, Expires = expires };
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
            ModelState.AddModelError("", string.Format("{0}Tên người dùng hoặc mật khẩu được cung cấp là không chính xác.", ""));
            return View(model);
        }

        //
        // GET: /Account/LogOff
        public ActionResult LogOff()
        {
            var codeCookie = HttpContext.Request.Cookies["CusLogin"];
            if (codeCookie != null && _dnLoginApi.Logout(codeCookie.Value))
            {
                codeCookie.Value = null;
                codeCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(codeCookie);
            }
            return RedirectToAction("LogOn", "Account");
        }

        [HttpPost]
        public ActionResult ActionChangePassword(ChangePasswordModel model)
        {
            var msg = new JsonMessage{Erros = false};
            try
            {
                var codeCookie = HttpContext.Request.Cookies["CusLogin"];
                if (codeCookie != null)
                {
                    msg.ID = _dnUser.UpdateCusPass(codeCookie.Value, model.OldPassword, model.NewPassword).ToString();
                    if (msg.ID == "1")
                    {
                        msg = new JsonMessage
                        {
                            Erros = false,
                            Message = "Đổi mật khẩu thành công !"
                        };
                        return Json(msg, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        msg = new JsonMessage
                        {
                            Erros = true,
                            Message = "Sai mật khẩu hoặc mật khẩu mới không đúng định dạng.</br> Vui lòng kiểm tra lại!"
                        };
                        return Json(msg, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception)
            {
                msg = new JsonMessage
                {
                    Erros = true,
                    Message = "Sai mật khẩu hoặc mật khẩu mới không đúng định dạng.</br> Vui lòng kiểm tra lại!"
                };
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public string CheckCardSerial(string CardSerial)
        {
            var result = _api.CheckCardSerial(CardSerial);
            return result == 1 ? "true" : "false";
        }
        public string CheckByParent(string Parent)
        {
            var result = _api.CheckParent(Parent);
            return result == 0 ? "false" : "true";
        }
        public string CheckByUserName(string UserName, int id)
        {
            var result = _api.CheckUserName(UserName.Trim(), id);
            return result == 1 ? "false" : "true";
        }
        public string CheckByEmail(string Email, int id)
        {
            var result = _api.CheckEmail(Email.Trim(), id);
            return result == 1 ? "false" : "true";
        }
        public string CheckByPhone(string Phone, int id)
        {
            var result = _api.CheckPhone(Phone.Trim(), id);
            return result == 1 ? "false" : "true";
        }
        public ActionResult AddParent(string serial)
        {
            var json = _api.GetCustomerBySerial(serial);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}
