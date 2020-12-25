using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;


namespace FDI.MvcAPI.Controllers
{
    public class DNCategoryController : BaseApiController
    {
        //
        // GET: /DNActive/

        private readonly CategoryDA _da = new CategoryDA("#");
        private readonly ShopProductDetailDA _detailDa = new ShopProductDetailDA("#");

        public ActionResult GetListAttr(string key, string lstId)
        {
            var obj = key != Keyapi ? new List<DN_AttributeDynamic>() : _da.GetListAttr(lstId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListItems(int type)
        {
            var obj = Request["key"] != Keyapi
                ? new ModelCategoryItem()
                : new ModelCategoryItem { ListItem = _da.GetListSimpleByRequest(Request, type), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetById(string key, int id)
        {
            var obj = key != Keyapi ? new Base.Category() : _da.GetById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new CategoryItem() : _da.GetItemById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetChildByParentId(string key, bool setTitle)
        {
            var obj = key != Keyapi ? new List<CategoryItem>() : _da.GetChildByParentId(setTitle);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListTree(string key, int agencyId)
        {
            var obj = key != Keyapi ? new List<TreeViewItem>() : _da.GetListTree(agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListTreeByType(string key, int type, int agencyId, string lstId)
        {
            var obj = key != Keyapi ? new List<TreeViewItem>() : _da.GetListTreeByType(type, lstId, agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetListTreeByTypeListId(string key, int type, string lstId)
        {
            var obj = key != Keyapi ? new List<TreeViewItem>() : _da.GetListTreeByTypeListId(type, lstId, Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllListSimpleByParentId(string key, int id, int agencyId)
        {
            var obj = key != Keyapi ? new List<CategoryItem>() : _da.GetAllListSimpleByParentId(id, agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListSimpleByAutoComplete(string key, string keyword, int showLimit, bool isShow, int agencyId)
        {
            var obj = key != Keyapi ? new List<CategoryItem>() : _da.GetListSimpleByAutoComplete(keyword, showLimit, isShow, agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckTitleAsciiExits(string key, string slug, int id)
        {
            var obj = key == Keyapi && _da.CheckTitleAsciiExits(slug, id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetCategoryById(string key, int id)
        {
            var obj = key != Keyapi ? new CategoryItem() : _da.GetCategoryById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCategoryParentId(string key, int id)
        {
            var obj = key != Keyapi ? new CategoryItem() : _da.GetCategoryParentId(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAll(string key)
        {
            var obj = key != Keyapi ? new List<CategoryItem>() : _da.GetAll();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListByArrId(string key, string lstId)
        {
            var obj = key != Keyapi ? new List<Base.Category>() : _da.GetListByArrId(lstId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetByName(string key, string name, int agencyId)
        {
            var obj = key != Keyapi ? new Base.Category() : _da.GetByName(name, agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add(string key, string codelogin, string json)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            var model = new Base.Category();
            var pictureId = Request["Value_DefaultImages"];
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                model.Name = HttpUtility.UrlDecode(model.Name);
                model.Slug = FomatString.Slug(model.Name);
                model.Description = HttpUtility.UrlDecode(model.Description);
                var parent = _da.GetById(model.ParentId ?? 0);
                model.IsLevel = parent.IsLevel + 1;
                model.IsDeleted = false;
                model.LanguageId = "vi";
                model.Price = model.PriceCost + (model.Profit * model.WeightDefault) * 1000 + model.Incurred;
                model.PriceFinal = model.Price + (model.Percent * model.WeightDefault) * 1000 + model.TotalIncurredFinal;
                _da.Add(model);
                _da.Save();

            }
            catch (Exception ex)
            {
                Log2File.LogExceptionToFile(ex);
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được thêm mới";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(string key, string codelogin, string json)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
            var pictureId = Request["Value_DefaultImages"];
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetById(ItemId);
                UpdateModel(model);
                model.Description = HttpUtility.UrlDecode(model.Description);
                model.Name = HttpUtility.UrlDecode(model.Name);
                model.Slug = FomatString.Slug(model.Name);
                var parent = _da.GetById(model.ParentId ?? 0);
                model.IsLevel = parent.IsLevel + 1;

                model.Price = model.PriceCost + (model.Profit * model.WeightDefault) * 1000 + model.Incurred;
                model.PriceFinal = model.Price + (model.Percent * model.WeightDefault) * 1000 + model.TotalIncurredFinal;

                _da.Save();
            }
            catch (Exception ex)
            {
                Log2File.LogExceptionToFile(ex);
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được cập nhật";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowHide(string key, string json, bool showhide)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetById(ItemId);
                model.IsShow = showhide;
                _da.Save();
            }
            catch (Exception ex)
            {
                Log2File.LogExceptionToFile(ex);
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được cập nhật";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(string key, string json)
        {
            var msg = new JsonMessage(false, "Xóa dữ liệu thành công.");
            var pictureId = Request["Value_DefaultImages"];
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetById(ItemId);
                model.IsDeleted = true;
                _da.Save();
            }
            catch (Exception ex)
            {
                Log2File.LogExceptionToFile(ex);
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được xóa";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        //public List<Category_Product_Recipe> GetListCategoryRecipeItem(string code)
        //{
        //    const string url = "Utility/GetListCategoryRecipeItem?key=";
        //    var urlJson = string.Format("{0}{1}", UrlG + url, code);
        //    var list = Utility.GetObjJson<List<RecipeCateItem>>(urlJson);
        //    var date = DateTime.Now.TotalSeconds();
        //    return list.Select(item => new Category_Product_Recipe()
        //    {
        //        Quantity = item.Quantity,
        //        ProductId = item.ProductId,
        //        Price = item.Price,
        //        IsDeleted = false,
        //        DateCreate = date,
        //        Percent = item.Percent,
        //        IsCheck = item.IsCheck == 1,
        //    }).ToList();
        //}
    }
}
