using FDI.DA;
using FDI.MvcAPI.Common;
using FDI.Simple;
using FDI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FDI.MvcAPI.Controllers.Category
{
    [CustomerAuthorize]
    public class CategoryController : BaseAppApiController
    {
        readonly CategoryDA _categoryDa = new CategoryDA();
        [AllowAnonymous]
        public ActionResult GetAllProducts()
        {
            //_categoryDa.updatenameasi();
            var lst = _categoryDa.GetAllByType((int)ModuleType.Product);
            return Json(new BaseResponse<List<CategoryItem>> { Code = 200, Erros = false, Data = lst }, JsonRequestBehavior.AllowGet);
        }

    }
}
