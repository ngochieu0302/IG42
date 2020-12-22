using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;

namespace FDI.Customer.Controllers
{
    public class BaseController : Controller
    {
        protected static CustomerItem UserItem { get; set; }
        public static int UserId { get; set; }
        readonly DNLoginAPI _dnLoginApi = new DNLoginAPI();
        protected string CodeLogin()
        {
            var codeCookie = HttpContext.Request.Cookies["CusLogin"];
            return codeCookie == null ? null : codeCookie.Value;
        }
        public CustomerItem GetUser(string code)
        {
            return _dnLoginApi.GetCustomerByCode(code);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (!string.IsNullOrEmpty(CodeLogin()) && Request.Url != null)
            {
                UserItem = GetUser(CodeLogin());
                UserId = UserItem.ID;
                if (UserItem == null)
                {
                    filterContext.Result = new RedirectResult("/Account/Logon?url=" + Request["url"]);
                }
            }
            else
            {
                filterContext.Result = new RedirectResult("/Account/Logon?url=" + Request["url"]);
            }
        }
    }
}
