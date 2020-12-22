using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;
using Resources;
namespace FDI.Enterprises.Controllers
{
    public class BaseController : Controller
    {
        protected static SystemActionItem SystemActionItem { get; set; }
        protected static List<ActionActiveItem> LtsModuleActive { get; set; }
        protected static string Checkdomain { get; set; }
        public static string UserName { get; set; }
        protected EnterprisesItem EnterprisesItem { get; set; }
        protected static DateTime Daycheck = DateTime.Now;
        readonly DNEnterprisesAPI _dnLoginApi = new DNEnterprisesAPI();

        public string CodeLogin()
        {
            var codeCookie = HttpContext.Request.Cookies["CodeE"];
            return codeCookie == null ? null : codeCookie.Value;
        }
        public EnterprisesItem GetUser(string code)
        {
            return _dnLoginApi.GetItemByCodeLogin(code);
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
            if (!string.IsNullOrEmpty(CodeLogin()) && Request.Url != null)
            {
                var obj = GetUser(CodeLogin());
                if (obj != null && !string.IsNullOrEmpty(obj.UserName))
                {
                    EnterprisesItem = obj;
                    UserName = obj.UserName;
                    SystemActionItem = new SystemActionItem { IsAdmin = true };
                }
                else
                {
                    filterContext.Result = new RedirectResult("/Account/Logon?url=" + Request["url"]);
                }
            }
            else
            {
                filterContext.Result = new RedirectResult("/Account/Logon?url=" + Request["url"]);
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

        // Dongdt 13/03/2017
        protected List<Guid> LsGuiId
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["GuiID"]))
                {
                    if (Request["GuiID"].Contains(","))
                    {
                        return Request["GuiID"].Trim().Split(',').Select(Guid.Parse).ToList();
                    }
                    var ltsTemp = new List<Guid> { Guid.Parse(Request["GuiID"]) };
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

    //public 

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
    }
}
