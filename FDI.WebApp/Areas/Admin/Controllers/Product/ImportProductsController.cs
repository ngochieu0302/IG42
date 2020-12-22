using System.Web.Mvc;
using FDI.Areas.Admin.Controllers;
using FDI.Controllers;
using FDI.DA;
using FDI.Utils;

namespace FDI.Areas.FDI.Areas.Admin.Controllers
{
    public class ImportProductsController : BaseController
    {
        //
        // GET: /Admin/ImportProducts/
        readonly Shop_ProductDA _productDA = new Shop_ProductDA("#");

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ImportExcel()
        {
            var msg = new JsonMessage();
            //var file = Request.Files[0];
            //var import = new ImportProduct(file, Product());
            //if (import.ResultValue != null)
            //{
            //    _productDA.InsertExcelShopProduct(WebConfig.ConnectString,  "@tbl", import.ResultValue);
            //}
            //msg.Erros = false;
            //msg.Message = string.Format("Dữ liệu đã được import thành công");
            return Json(msg, JsonRequestBehavior.AllowGet);
        }


    }
}
