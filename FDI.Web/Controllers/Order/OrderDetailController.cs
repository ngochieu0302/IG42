using System.IO;
using System.Web.Mvc;
using FDI.GetAPI;
using System;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class OrderDetailController : BaseController
    {
        // GET: /Admin/Order/
        private readonly ShopProductDetailAPI _api = new ShopProductDetailAPI();
        private readonly DNUserAPI _userApi = new DNUserAPI();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            var date = Request["fromDate"] ?? DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.date = date;
            return View(_api.GetListOrderDetail(UserItem.AgencyID, date));
        }

        public ActionResult AjaxForm()
        {
            var date = Request["fromDate"] ?? DateTime.Now.ToString("yyyy-MM-dd");
            var model = _api.GetOrderDetailExport(UserItem.AgencyID, date);
            model.AgencyID = UserItem.AgencyID;
            model.UserID = UserItem.UserId;
            model.UserName = UserItem.UserName;
            model.DateTime = date;
            ViewBag.LstUser = _userApi.GetAll(UserItem.AgencyID);
            return View(model);
        }
        public ActionResult ExportExcel()
        {
            var date = Request["fromDate"] ?? DateTime.Now.ToString("yyyy-MM-dd");
            var model = _api.GetListOrderDetail(UserItem.AgencyID, date);
            const string fileName = "Danh_sach_san_pham.xlsx";
            var filePath = Path.Combine(Request.PhysicalApplicationPath, "File\\ExportImport", fileName);
            var folder = Request.PhysicalApplicationPath + "File\\ExportImport";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            var di = new DirectoryInfo(folder);
            foreach (var file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (var dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
            Excels.ExportListOrderDetail(filePath, model, date);
            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, "text/xls", fileName);
        }

    }
}