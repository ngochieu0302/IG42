using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using FDI.Admin.Models;
using FDI.MvcMembership;
using FDI.MvcMembership.Settings;
using FDI.Utils;

namespace FDI.Admin.Controllers
{
    public class AccountAdminController : Controller
    {
        //
        // GET: /Admin/AccountAdmin/
        //
        // GET: /Account/LogOn
        private readonly IUserService _userService;
        private readonly IRolesService _rolesService;
        private readonly IMembershipSettings _membershipSettings;

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
            //model.UserName = "Forum" + model.UserName;
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    return Json(new { success = true, redirect = returnUrl });
                }
                ModelState.AddModelError("", string.Format("The user {0}name or password provided is incorrect.", ""));
            }

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
            //model.UserName = "Forum" + model.UserName;
            var returnUrl = Request["itemUrl"];
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    var user = Membership.GetUser(model.UserName);
                    if (user != null && user.IsLockedOut)
                    {

                        ModelState.AddModelError("", string.Format("{0}Tài khoản của bạn bị khóa. Hãy liên hệ với quản trị!", ""));
                    }
                    else
                    {
                        //Gọi stored báo login thành công
                        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                        Session["ProviderUserKey"] = user.ProviderUserKey;
                        return Redirect(Url.IsLocalUrl(returnUrl) ? returnUrl : "~/Admin");
                    }
                }
                else
                {
                    ModelState.AddModelError("", string.Format("{0}Tên người dùng hoặc mật khẩu được cung cấp là không chính xác.", ""));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Admin");
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
                var currentUser = Membership.GetUser(User.Identity.Name, true);
                if (currentUser != null)
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
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
