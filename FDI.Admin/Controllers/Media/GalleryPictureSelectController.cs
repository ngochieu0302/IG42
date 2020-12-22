using System;
using System.Web.Mvc;
using FDI.DA;
using FDI.Simple;

namespace FDI.Admin.Controllers
{
    public class GalleryPictureSelectController : BaseController
    {
        //
        // GET: /Admin/GalleryPictureSelect/

        readonly Gallery_PictureDA _pictureDa = new Gallery_PictureDA("#");
        /// <summary>
        /// Hiển thị select ảnh
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new ModelAlbumItem
            {
                Container = Request["Container"],
                ValuesSelected = Request["ValuesSelected"],
                SelectMutil = Convert.ToBoolean(Request["MutilFile"]),
                Type = Convert.ToInt32(Request["ModuleType"])
            };
            return View(model);
        }

        /// <summary>
        /// Load danh sách bản ghi dưới dạng bảng
        /// </summary>
        /// <returns></returns>
        public ActionResult ListItems(int type)
        {
            var listFaqQuestionItem = _pictureDa.GetListSimpleByRequest(Request);
            var model = new ModelPictureItem
            {
                Container = Request["Container"],
                ListItem = listFaqQuestionItem,
                PageHtml = _pictureDa.GridHtmlPage
            };
            return View(model);
        }
    }
}
