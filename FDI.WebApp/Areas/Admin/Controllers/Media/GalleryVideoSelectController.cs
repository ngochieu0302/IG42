using System;
using System.Text;
using System.Web.Mvc;

using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Areas.Admin.Controllers
{
    public class GalleryVideoSelectController : BaseController
    {
        //
        // GET: /Admin/GalleryVideoSelect/

        readonly Gallery_VideoDA _videoDA = new Gallery_VideoDA("#");

        /// <summary>
        /// Hiển thị select ảnh
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var stbHtml = new StringBuilder();
            stbHtml.Append("<li title=\"Thư mục gốc\" class=\"select\" id=\"0\"><span class=\"file\">");
            stbHtml.Append("<a href=\"#\">Thư mục gốc</a>");
            stbHtml.Append("</span></li>");

            var model = new ModelAlbumItem
            {
                Container = Request["Container"],
                PageHtml = stbHtml.ToString(),
                SelectMutil = Convert.ToBoolean(Request["SelectMutil"]),
                ValuesSelected = Request["ValuesSelected"]
            }; 
            return View(model);
        }


        /// <summary>
        /// Load danh sách bản ghi dưới dạng bảng
        /// </summary>
        /// <returns></returns>
        public ActionResult ListItems()
        {
            var ltsValues = FDIUtils.StringToListInt(Request["ValuesSelected"]);
            var listVideoItem = _videoDA.GetListSimpleByRequest(Request, ltsValues);

            var model = new ModelVideoItem
            {
                Container = Request["Container"],
                ListItem = listVideoItem,
                PageHtml = _videoDA.GridHtmlPage
            };
            ViewData.Model = model;
            return View();
        }


    }
}
