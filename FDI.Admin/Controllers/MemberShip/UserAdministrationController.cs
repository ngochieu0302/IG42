using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using FDI.Admin.Models;
using FDI.DA;
using FDI.MvcMembership;
using FDI.MvcMembership.Settings;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Admin.Controllers
{
    public class UserAdministrationController : BaseController
    {
        //
        // GET: /Admin/UserAdministration/        

        private const int PageSize = 100;
        private const string ResetPasswordBody = "Your new password is: ";
        private const string ResetPasswordSubject = "Your New Password";
        private readonly IRolesService _rolesService;
        private readonly IMembershipSettings _membershipSettings;
        private readonly IUserService _userService;
        private readonly IPasswordService _passwordService;
        private readonly RoleDA _rolerDa = new RoleDA("#");
        public UserAdministrationController()
            : this(new AspNetMembershipProviderWrapper(), new AspNetRoleProviderWrapper())
        {
        }

        public UserAdministrationController(AspNetMembershipProviderWrapper membership, IRolesService roles)
            : this(membership.Settings, membership, membership, roles)
        {
        }

        public UserAdministrationController(
            IMembershipSettings membershipSettings,
            IUserService userService,
            IPasswordService passwordService,
            IRolesService rolesService)
        {
            _membershipSettings = membershipSettings;
            _userService = userService;
            _passwordService = passwordService;
            _rolesService = rolesService;
        }

        public ActionResult Roles(int? page, string search)
        {
            var users = string.IsNullOrWhiteSpace(search) ? _userService.FindAll(page ?? 1, PageSize)
                : search.Contains("@") ? _userService.FindByEmail(search, page ?? 1, PageSize)
                : _userService.FindByUserName(search, page ?? 1, PageSize);

            if (!string.IsNullOrWhiteSpace(search) && users.Count == 1)
            {
                var providerUserKey = users[0].ProviderUserKey;
                if (providerUserKey != null)
                    return RedirectToAction("Details", new { id = providerUserKey.ToString() });
            }

            return View(new IndexViewModel
            {
                Search = search,
                Users = users,
                Roles = _rolesService.Enabled ? _rolesService.FindAll() : Enumerable.Empty<string>(),
                IsRolesEnabled = _rolesService.Enabled
            });
        }

        public ActionResult Index(int? page, string search)
        {
            var users = string.IsNullOrWhiteSpace(search) ? _userService.FindAll(page ?? 1, PageSize) : search.Contains("@") ? _userService.FindByEmail(search, page ?? 1, PageSize) : _userService.FindByUserName(search, page ?? 1, PageSize);

            if (!string.IsNullOrWhiteSpace(search) && users.Count == 1)
            {
                var providerUserKey = users[0].ProviderUserKey;
                if (providerUserKey != null)
                    return RedirectToAction("Details", new { id = providerUserKey.ToString() });
            }

            return View(new IndexViewModel
            {
                Search = search,
                Users = users,
                Roles = _rolesService.Enabled ? _rolesService.FindAll() : Enumerable.Empty<string>(),
                IsRolesEnabled = _rolesService.Enabled
            });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public RedirectToRouteResult CreateRole(string id)
        {
            if (_rolesService.Enabled)
                _rolesService.Create(id);
            return RedirectToAction("Roles");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public RedirectToRouteResult DeleteRole(string id)
        {
            _rolesService.Delete(id);
            return RedirectToAction("Roles");
        }

        public ViewResult Role(string id)
        {
            return View(new RoleViewModel
            {
                Role = id,
                Users = _rolesService.FindUserNamesByRole(id).ToDictionary(k => k, v => _userService.Get(v))
            });
        }

        public ViewResult Details(Guid id)
        {
            var user = _userService.Get(id);
            var userRoles = _rolesService.Enabled ? _rolesService.FindByUser(user) : Enumerable.Empty<string>();
            ViewBag.IsLockedOut = user.IsLockedOut;
            var dictionary = _rolesService.FindAll().ToDictionary(s => s, userRoles.Contains);
            return View(new DetailsViewModel
            {
                CanResetPassword = _membershipSettings.Password.ResetOrRetrieval.CanReset,
                RequirePasswordQuestionAnswerToResetPassword = _membershipSettings.Password.ResetOrRetrieval.RequiresQuestionAndAnswer,
                DisplayName = user.UserName,
                User = user,
                Roles = _rolesService.Enabled ? dictionary : new Dictionary<string, bool>(0),
                IsRolesEnabled = _rolesService.Enabled,
                Status = user.IsOnline ? DetailsViewModel.StatusEnum.Online : !user.IsApproved
                    ? DetailsViewModel.StatusEnum.Unapproved
                    : user.IsLockedOut
                    ? DetailsViewModel.StatusEnum.LockedOut
                    : DetailsViewModel.StatusEnum.Offline
            });
        }

        public ViewResult Password(Guid id)
        {
            var user = _userService.Get(id);
            var userRoles = _rolesService.Enabled ? _rolesService.FindByUser(user) : Enumerable.Empty<string>();
            return View(new DetailsViewModel
            {
                CanResetPassword = _membershipSettings.Password.ResetOrRetrieval.CanReset,
                RequirePasswordQuestionAnswerToResetPassword = _membershipSettings.Password.ResetOrRetrieval.RequiresQuestionAndAnswer,
                DisplayName = user.UserName,
                User = user,
                Roles = _rolesService.Enabled ? _rolesService.FindAll().ToDictionary(role => role, userRoles.Contains) : new Dictionary<string, bool>(0),
                IsRolesEnabled = _rolesService.Enabled,
                Status = user.IsOnline
                            ? DetailsViewModel.StatusEnum.Online
                            : !user.IsApproved
                                ? DetailsViewModel.StatusEnum.Unapproved
                                : user.IsLockedOut
                                    ? DetailsViewModel.StatusEnum.LockedOut
                                    : DetailsViewModel.StatusEnum.Offline
            });
        }

        public ViewResult CreateUser()
        {
            var model = new CreateUserViewModel
            {
                InitialRoles = _rolesService.FindAll().ToDictionary(k => k, v => false)
            };
            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateUser(CreateUserViewModel createUserViewModel)
        {
            if (!ModelState.IsValid)
                return View(createUserViewModel);

            try
            {
                if (createUserViewModel.Password != createUserViewModel.ConfirmPassword)
                    throw new MembershipCreateUserException("Passwords do not match.");

                var user = _userService.Create(
                    createUserViewModel.Username,
                    createUserViewModel.Password,
                    createUserViewModel.Email,
                    createUserViewModel.PasswordQuestion,
                    createUserViewModel.PasswordAnswer,
                    true);

                if (createUserViewModel.InitialRoles != null)
                {
                    var rolesToAddUserTo = createUserViewModel.InitialRoles.Where(x => x.Value).Select(x => x.Key);
                    foreach (var usersInRoles in rolesToAddUserTo)
                    {
                        _rolesService.AddToRole(user, usersInRoles);
                    }

                }

                return RedirectToAction("Details", new { id = user.ProviderUserKey });
            }
            catch (MembershipCreateUserException e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return View(createUserViewModel);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public RedirectToRouteResult Details(Guid id, string email, string comments)
        {
            var user = _userService.Get(id);
            user.Email = email;
            user.Comment = comments;
            _userService.Update(user);
            return RedirectToAction("Details", new { id });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public RedirectToRouteResult DeleteUser(Guid id)
        {
            var user = _userService.Get(id);
            _userService.Delete(user);
            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public RedirectToRouteResult ChangeApproval(Guid id, bool isApproved)
        {
            var user = _userService.Get(id);
            user.IsApproved = isApproved;
            _userService.Update(user);
            return RedirectToAction("Details", new { id });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public RedirectToRouteResult Unlock(Guid id)
        {
            var rolerDa = new RoleDA("#");
            var membership = rolerDa.GetListByMembership(id);
            membership.IsLockedOut = membership.IsLockedOut != true;
            rolerDa.Save();
            return RedirectToAction("Details", new { id });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public RedirectToRouteResult ResetPassword(Guid id)
        {
            var user = _userService.Get(id);
            var pass = FDIUtils.RandomString(6).ToLower();
            _passwordService.ChangePassword(user, pass);
            return RedirectToAction("Password", new { id });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public RedirectToRouteResult ResetPasswordWithAnswer(Guid id, string answer)
        {
            var user = _userService.Get(id);
            _passwordService.ResetPassword(user, answer);
            return RedirectToAction("Password", new { id });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public RedirectToRouteResult SetPassword(Guid id, string password)
        {
            var user = _userService.Get(id);
            _passwordService.ChangePassword(user, password);
            var body = ResetPasswordBody + password;
            var msg = new MailMessage();
            msg.To.Add(user.Email);
            msg.Subject = ResetPasswordSubject;
            msg.Body = body;
            return RedirectToAction("Password", new { id });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public RedirectToRouteResult AddToRole(Guid id, string role)
        {
            var user = _userService.Get(id);
            _rolesService.AddToRole(user, role);
            return RedirectToAction("Details", new { id });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public RedirectToRouteResult RemoveFromRole(Guid id, string role)
        {
            var user = _userService.Get(id);
            _rolesService.RemoveFromRole(user, role);
            return RedirectToAction("Details", new { id });
        }

        public ActionResult AjaxViewModule(string id)
        {

            var model = new ModelAspnetRolesItem
            {
                Roles = _rolerDa.GetByName(id),
                ListActiveRoleItem = _rolerDa.GetActiveRoleAll()
            };
            return View(model);
        }

        public ActionResult AjaxViewRoleActive(string id)
        {
            var model = new ModelAspnetRolesItem
            {
                Roles = _rolerDa.GetByName(id),
                ListActiveRoleItem = _rolerDa.GetActiveRoleAll()
            };
            return View(model);
        }

        public ActionResult EditRoleAction()
        {
            var roleModuleActiveDa = new RoleModuleActiveDA("#");
            JsonMessage msg;
            var moduleid = Convert.ToInt16(Request["moduleid"]);
            var roleId = Guid.Parse(Request["ItemID"]);

            var module = moduleid != 0 ? roleModuleActiveDa.GetListRoleModuleActivekt(roleId, moduleid) : roleModuleActiveDa.GetListRoleModuleActivekt(roleId);
            if (module.Count > 0)
            {
                foreach (var user in module.Select(t => roleModuleActiveDa.GetByRoleModuleActiveId(t.ID)))
                {
                    var check = Request[user.ID.ToString()];
                    user.Active = check != null;
                    roleModuleActiveDa.Save();
                }
                msg = new JsonMessage
                {
                    Erros = false,
                    ID = moduleid.ToString(),
                    Message = string.Format("Đã cập nhật chuyên mục: <b>{0}</b>.<br />", Server.HtmlEncode("Thành công!"))
                };
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            msg = new JsonMessage
            {
                Erros = true,
                Message = "Không có hành động nào được thực hiện."
            };
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RoleAction()
        {
            var roleId = GuiId.FirstOrDefault();
            var role = _rolerDa.GetById(roleId);
            var roleName = Request["ItemNameRole"];
            var listActiveRole = _rolerDa.GetlistActiveRole();
            foreach (var activeRole in listActiveRole)
            {
                if (Request[activeRole.NameActive] != null)
                    role.ActiveRoles.Add(activeRole);
                else
                    role.ActiveRoles.Remove(activeRole);
                _rolerDa.Save();
            }
            var msg = new JsonMessage
            {
                Erros = false,
                ID = roleId.ToString(),
                Message =
                    string.Format("Đã cập nhật chuyên mục: <b>{0}</b>.<br />",
                                  Server.HtmlEncode(roleName))
            };
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteRoleAction()
        {
            JsonMessage msg;
            var roleModuleActiveDa = new RoleModuleActiveDA("#");
            try
            {
                int moduleid = Convert.ToInt16(Request["moduleid"]);
                var roleId = GuiId.FirstOrDefault();
                var role = _rolerDa.GetById(roleId);
                var module = role.Modules.FirstOrDefault(m => m.ID == moduleid);
                if (module != null)
                {
                    var namemodule = module.NameModule;
                    role.Modules.Remove(module);
                    _rolerDa.Save();
                    var roleModuleActive = roleModuleActiveDa.GetListRoleModuleActivekt(roleId, moduleid);
                    foreach (var moduleActive in roleModuleActive)
                    {
                        roleModuleActiveDa.Delete(moduleActive);
                        roleModuleActiveDa.Save();
                    }
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = moduleid.ToString(),
                        Message = string.Format("Đã xóa chuyên mục <b>{0}</b>.<br />", Server.HtmlEncode(namemodule))
                    };
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                msg = new JsonMessage
                {
                    Erros = true,
                    Message = "Không có hành động nào được thực hiện."
                };
            }
            catch (Exception)
            {

                msg = new JsonMessage
                {
                    Erros = true,
                    Message = "Không có hành động nào được thực hiện."
                };
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
