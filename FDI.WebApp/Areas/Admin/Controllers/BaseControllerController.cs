using FDI.DA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using FDI.Simple;
using FDI.Utils;
using Resources;

namespace FDI.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        ModuleDA moduleDa = new ModuleDA("#");
        protected static SystemActionItem SystemActionItem { get; set; }
        protected static List<ActionActiveItem> LtsModuleActive { get; set; }
        protected static string Checkdomain { get; set; }
        protected static DateTime Daycheck = DateTime.Now;
        public static int AgencyId { get; set; }
        protected string Module()
        {
            var path = RawUrl();
            var module = path[1];
            return module;
        }
        private string[] RawUrl()
        {
            var path = Request.Path;
            var url = Request.UrlReferrer;
            return path.Split('/');
        }
        protected string ParentId()
        {
            var codeCookie = HttpContext.Request.Cookies["ParentId"];
            return codeCookie == null ? "0" : codeCookie.Value;
        }
        protected string ModuleId()
        {
            var codeCookie = HttpContext.Request.Cookies["ModuleId"];
            return codeCookie == null ? null : codeCookie.Value;
        }
        public static string Mid { get; set; }
        public static string Title { get; set; }
        public bool CheckAdmin()
        {
            var lstAdmin = WebConfig.ListAdmin;
            var lstAdminArr = lstAdmin.Split(',');
            return lstAdminArr.Any(role => User.IsInRole(role));
        }

        /// <summary>
        /// 1.  kiem tra phan quyen khi hien len view - object ltsModuleActive
        /// 2.  kiem tra phan quyen khi thuc hien action - object systemActionItem
        /// </summary>
        /// <author> linhtx </author>
        /// <datemodified> 15-Jan-2014 </datemodified>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (Request.Url != null)
            {
                if (User.Identity.IsAuthenticated)
                {

                    var membershipUser = Membership.GetUser();
                    if (membershipUser != null)
                    {
                        SystemActionItem = new SystemActionItem();
                        var providerUserKey = membershipUser.ProviderUserKey;
                        if (providerUserKey != null)
                        {
                            var userId = (Guid)providerUserKey;
                            var path = Request.Url.AbsolutePath.ToLower() + "/";
                            if (path.Contains(WebConfig.AdminUrl))
                            {
                                path = path.Replace(WebConfig.AdminUrl, "");
                                string[] moduleArr = path.Split('/');
                                if (Mid != moduleArr[0])
                                {
                                    Mid = moduleArr[0];
                                    Title = moduleDa.GetNameByTag(Mid.ToLower());
                                }
                                var keyCache = "ltsPermissionrole" + userId; // ltsPermissionProductAttribute
                                if (HttpRuntime.Cache[keyCache] == null)
                                {
                                    HttpRuntime.Cache[keyCache] = CheckAdmin();
                                }
                                SystemActionItem.IsAdmin = (bool)HttpRuntime.Cache[keyCache];
                                if (!SystemActionItem.IsAdmin)
                                {
                                    var module = moduleArr[0]; // ProductAttribute
                                    var keyCacheModule = "ltsPermission" + userId + module; // ltsPermissionProductAttribute
                                    #region user module active

                                    if (HttpRuntime.Cache[keyCacheModule] == null)
                                    {
                                        LtsModuleActive = UserRoleModule(userId, module);
                                        if (LtsModuleActive.Any())
                                            HttpRuntime.Cache[keyCacheModule] = LtsModuleActive;
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(module))
                                                filterContext.Result = new RedirectResult("/AccountAdmin");
                                        }
                                    }
                                    else
                                    {
                                        LtsModuleActive = HttpRuntime.Cache[keyCacheModule] as List<ActionActiveItem>;
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                }
            }
        }

        protected string ActionText
        {
            get
            {
                string action = null;
                var doaction = Request["do"];
                if (!string.IsNullOrEmpty(doaction))
                {
                    ListActionText().TryGetValue(doaction.Trim().ToLower(), out action);
                }
                return action;
            }
        }

        protected ActionType DoAction
        {
            get
            {
                var action = new ActionType();
                var doaction = Request["do"];
                if (!string.IsNullOrEmpty(doaction))
                {
                    if (SystemActionItem.IsAdmin)
                    {
                        ListActionType().TryGetValue(doaction.Trim().ToLower(), out action);
                    }
                    else
                    {
                        var obj = LtsModuleActive.FirstOrDefault(m => m.NameActive.Trim().ToLower() == doaction.Trim().ToLower());
                        if (obj != null)
                        {
                            ListActionType().TryGetValue(doaction.Trim().ToLower(), out action);
                        }
                    }
                }
                return action;
            }
        }

        protected List<int> ArrId
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["itemId"]))
                {
                    if (Request["itemId"].Contains(","))
                    {
                        return Request["itemId"].Trim().Split(',').Select(o => Convert.ToInt32(o)).ToList();
                    }
                    var ltsTemp = new List<int> { Convert.ToInt32(Request["itemId"]) };
                    return ltsTemp;
                }
                return new List<int>();
            }
        }

        // Dongdt 22/11/2013
        protected List<Guid> GuiId
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["itemID"]))
                {
                    if (Request["ItemID"].Contains(","))
                    {
                        return Request["ItemID"].Trim().Split(',').Select(Guid.Parse).ToList();
                    }
                    var ltsTemp = new List<Guid> { Guid.Parse(Request["ItemID"]) };
                    return ltsTemp;
                }
                return new List<Guid>();
            }
        }

        protected List<long> ArrLongId
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["itemID"]))
                {
                    if (Request["ItemID"].Contains(","))
                    {
                        return Request["ItemID"].Trim().Split(',').Select(o => Convert.ToInt64(o)).ToList();
                    }
                    var ltsTemp = new List<long> { Convert.ToInt64(Request["ItemID"]) };
                    return ltsTemp;
                }
                return new List<long>();
            }
        }

        protected List<FileItem> ListFileUpload
        {
            get
            {
                var ltsFileItem = new List<FileItem>();
                if (!string.IsNullOrEmpty(Request["listValueFileAttach"]))
                {
                    var strListFileAttach = Request["listValueFileAttach"];
                    var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var ltsFileForm = oSerializer.Deserialize<List<FileAttachForm>>(strListFileAttach);
                    const string filePath = "/Uploads/ajaxUpload/";
                    ltsFileItem.AddRange(ltsFileForm.Select(fileForm => new FileItem
                    {
                        Name = fileForm.FileName,
                        Data = FDIUtils.ReadFile(Server.MapPath(filePath + fileForm.FileServer)),
                        Url = Server.MapPath(filePath + fileForm.FileServer)
                    }));
                }
                return ltsFileItem;
            }
        }

        protected List<int> ListFileRemove
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["listValueFileAttachRemove"]))
                {
                    if (Request["listValueFileAttachRemove"].Contains(","))
                    {
                        return Request["listValueFileAttachRemove"].Trim().Split(',').Select(o => Convert.ToInt32(o)).ToList();
                    }
                    var ltsTemp = new List<int> { Convert.ToInt32(Request["listValueFileAttachRemove"]) };
                    return ltsTemp;
                }
                return new List<int>();
            }
        }

        protected List<ActionActiveItem> UserRoleModule(Guid userId, string module)
        {
            if (!string.IsNullOrEmpty(module))
            {
                return moduleDa.GetlistByTagUserId(module, userId, Roles.GetRolesForUser().ToList());
            }
            return new List<ActionActiveItem>();
        }

        /// <summary>
        /// add by DuongNT
        /// 06-05-2014
        /// use render partial view to string
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        public static Dictionary<string, string> ListActionText()
        {
            var list = new Dictionary<string, string>
                   {
                       {"add", CSResourceString.Add},
                       {"edit", CSResourceString.Edit},
                       {"view", CSResourceString.View},
                       {"delete", CSResourceString.Delete},
                       {"show", CSResourceString.Show},
                       {"hide", CSResourceString.Hide},
                       {"complete", CSResourceString.Complete},
                       {"order", CSResourceString.Order},
                       {"active", CSResourceString.Active},
                       {"usermodule", CSResourceString.UserModule},
                       {"rolemodule", CSResourceString.RoleModule}
                   };
            return list;
        }

        public static Dictionary<string, ActionType> ListActionType()
        {
            var list = new Dictionary<string, ActionType>
                   {
                       {"add", ActionType.Add},
                       {"edit", ActionType.Edit},
                       {"view", ActionType.View},
                       {"delete", ActionType.Delete},
                       {"show", ActionType.Show},
                       {"hide", ActionType.Hide},
                       {"complete", ActionType.Complete},
                       {"order", ActionType.Order},
                       {"active", ActionType.Active},
                       {"usermodule", ActionType.UserModule},
                       {"rolemodule", ActionType.RoleModule},
                       {"excel", ActionType.Excel}
                   };
            return list;
        }

        public static bool CheckAction(string name)
        {
            if (SystemActionItem.IsAdmin)
            {
                return true;
            }
            var obj = LtsModuleActive.FirstOrDefault(m => m.NameActive.Trim().ToLower() == name);
            if (obj != null)
            {
                return true;
            }
            return false;
        }

        public static string LanguageId
        {
            get
            {
                var cookie = Utility.Getcookie("LanguageId");
                return cookie ?? "vi";
            }
        }

    }

    public enum ActionType
    {
        NoRole = 0,
        Add = 1,
        Edit = 2,
        Delete = 3,
        Active = 4,
        View = 5,
        Show = 6,
        Hide = 7,
        Order = 8,
        Public = 9,
        Complete = 10,
        UserModule = 11,
        RoleModule = 12,
        Excel = 13,
        RoleActive = 11
    }
}

