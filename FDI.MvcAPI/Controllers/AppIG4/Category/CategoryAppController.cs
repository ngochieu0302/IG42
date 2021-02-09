using FDI.DA;
using FDI.MvcAPI.Common;
using FDI.Simple;
using FDI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FDI.MvcAPI.Controllers
{
    [CustomerAuthorize]
    public class CategoryAppController : BaseAppApiController
    {
        readonly CategoryAppIG4DA _categoryDa = new CategoryAppIG4DA();
        [AllowAnonymous]
        public ActionResult GetAllProducts()
        {
            var lst = _categoryDa.GetAllByType((int)ModuleType.Product);
            return Json(new BaseResponse<List<CategoryAppIG4Item>> { Code = 200, Erros = false, Data = lst }, JsonRequestBehavior.AllowGet);
        }

    }
}
