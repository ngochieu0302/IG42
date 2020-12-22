using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers
{
    public class CateRecipeController : BaseApiAuthController
    {
        //
        // GET: /CateRecipe/
        private readonly CateRecipeDA _da = new CateRecipeDA("#");
        private readonly ShopProductDetailDA _detailDa = new ShopProductDetailDA("#");
        readonly CategoryDA _categoryDa = new CategoryDA("#");
        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new CateRecipeItem() : _da.GetItemById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelCateRecipeItem()
                : new ModelCateRecipeItem { ListItems = _da.GetListSimpleByRequest(Request), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAll(string key)
        {
            var obj = key != Keyapi ? new List<CateRecipeItem>() : _da.GetAll(); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key, string codelogin, string json, Guid userId)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            var model = new Category_Recipe();
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                model.IsDeleted = false;
                model.UserId = userId;
                model.DateCreate = DateTime.Now.TotalSeconds();
                var lst = GetListCategoryRecipeItem(codelogin);
                var caterecipe = model.Category_Product_Recipe.Where(c => c.IsDeleted == false);
                var result2 = lst.Where(p => caterecipe.All(p2 => p2.ProductId != p.ProductId)).ToList();
                foreach (var item in result2)
                {
                    model.Category_Product_Recipe.Add(item);
                }

                var lstmap = GetListMappingCategoryRecipeItem(codelogin);
                var catemap = model.Mapping_Category_Recipe.Where(c => c.IsDeleted == false);
                var result = lstmap.Where(a => catemap.All(c => c.CategoryID != a.CategoryID)).ToList();
                foreach (var item in result)
                {
                    model.Mapping_Category_Recipe.Add(item);
                }

                _da.Add(model);
                _da.Save();
                if (model.IsUse == true)
                {
                    decimal? cost = 0;
                    decimal? priceFinal = 0;
                    foreach (var items in model.Category_Product_Recipe.Where(c => c.IsCheck == true && (!c.IsDeleted.HasValue || c.IsDeleted == false)))
                    {
                        var temp = _detailDa.GetById(items.ProductId ?? 0);
                        cost += items.Price * items.Quantity;
                        priceFinal += items.Price == 0 ? 0 : (items.Price + temp.Category.Percent * 1000) * items.Quantity + items.Incurred;
                        temp.PriceCost = items.Price;
                        temp.Value = items.Price;
                        temp.Percent = temp.Category.Percent;
                        temp.PriceOld = items.Price + (temp.Incurred ?? 0) + (temp.Percent ?? 0) * 1000;
                        temp.Incurred = items.Incurred;
                        temp.Price = items.PriceProduct;
                        _detailDa.Save();
                    }

                    foreach (var items in model.Mapping_Category_Recipe.Where(c => c.IsCheck == true && (!c.IsDeleted.HasValue || c.IsDeleted == false)))
                    {
                        //var temp = _detailDa.GetById(model.CategoryID ?? 0);

                        //priceFinal += items.PriceProduct * items.Quantity;
                        //temp.Price = items.PriceProduct;
                        //temp.PriceCost = items.Price;
                        //temp.Incurred = items.Incurred;
                        //temp.Price = items.PriceProduct;
                        //temp.Value = items.Price;
                        //temp.PriceOld = items.Price + (temp.Incurred ?? 0) + (temp.Percent ?? 0) * 1000;
                        //_detailDa.Save();
                        var temp1 = _categoryDa.GetById(model.CategoryID ?? 0);
                        cost += items.Price;
                        priceFinal += items.Price + (temp1.Percent * 1000 * items.Quantity) + items.Incurred;
                        temp1.PriceRecipeFinal = items.Price + (items.Quantity * temp1.Percent * 1000) + items.Incurred;
                        _detailDa.Save();
                    }
                    var cate = _categoryDa.GetById(model.CategoryID ?? 0);
                    cate.PriceRecipe = cost;
                    cate.PriceRecipeFinal = priceFinal;
                    cate.TotalIncurredFinal = model.TotalIncurred;
                    _categoryDa.Save();
                }
            }
            catch (Exception ex)
            {
                Log2File.LogExceptionToFile(ex);
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được thêm mới";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update(string key, string codelogin, string json, Guid userId)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
            var pictureId = Request["Value_DefaultImages"];
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetById(ItemId);
                UpdateModel(model);
                model.DateUpdate = DateTime.Now.TotalSeconds();
                model.UserId = userId;
                var lst = model.Category_Product_Recipe.Where(c => c.IsDeleted == false).ToList();
                var lstNew = GetListCategoryRecipeItem(codelogin);
                //xóa
                var result1 = lst.Where(p => lstNew.All(p2 => p2.ProductId != p.ProductId));
                foreach (var i in result1)
                {
                    i.IsDeleted = true;
                }
                //Sửa
                foreach (var i in lst)
                {
                    var j = lstNew.FirstOrDefault(c => c.ProductId == i.ProductId);
                    if (j != null)
                    {
                        i.Quantity = j.Quantity;
                        i.Price = j.Price;
                        i.IsCheck = j.IsCheck;
                        i.Percent = j.Percent;
                        i.Incurred = j.Incurred;
                        i.PriceProduct = j.PriceProduct;
                    }
                }
                //thêm mới
                var result2 = lstNew.Where(p => lst.All(p2 => p2.ProductId != p.ProductId));
                foreach (var item in result2)
                {
                    model.Category_Product_Recipe.Add(item);
                }

                var lstmap = model.Mapping_Category_Recipe.Where(c => c.IsDeleted == false).ToList();
                var lstNewmap = GetListMappingCategoryRecipeItem(codelogin);

                var resultmap = lstmap.Where(p => lstNewmap.All(p2 => p2.CategoryID != p.CategoryID));
                //xóa
                foreach (var i in resultmap)
                {
                    i.IsDeleted = true;
                }
                //sửa
                foreach (var i in lstmap)
                {
                    var j = lstNewmap.FirstOrDefault(c => c.CategoryID == i.CategoryID);
                    if (j != null)
                    {
                        i.Quantity = j.Quantity;
                        i.Price = j.Price;
                        i.IsCheck = j.IsCheck;
                        i.Percent = j.Percent;
                        i.Incurred = j.Incurred;
                        i.PriceProduct = j.PriceProduct;
                    }
                }
                //thêm mới
                var result2map = lstNewmap.Where(p => lstmap.All(p2 => p2.CategoryID != p.CategoryID));
                foreach (var item in result2map)
                {
                    model.Mapping_Category_Recipe.Add(item);
                }
                //update price
                if (model.IsUse == true)
                {
                    decimal? cost = 0;
                    decimal? priceFinal = 0;
                    foreach (var items in model.Category_Product_Recipe.Where(c => c.IsCheck == true && (!c.IsDeleted.HasValue || c.IsDeleted == false)))
                    {
                        var temp = _detailDa.GetById(items.ProductId ?? 0);
                        cost += items.Price * items.Quantity;
                        priceFinal += items.Price == 0 ? 0 : (items.Price + temp.Category.Percent * 1000) * items.Quantity + items.Incurred;
                        temp.PriceCost = items.Price;
                        temp.Value = items.Price;
                        temp.Percent = temp.Category.Percent;
                        temp.PriceOld = items.Price + (temp.Incurred ?? 0) + (temp.Percent ?? 0) * 1000;
                        temp.Incurred = items.Incurred;
                        temp.Price = items.PriceProduct;
                        _detailDa.Save();
                    }

                    foreach (var items in model.Mapping_Category_Recipe.Where(c => c.IsCheck == true && (!c.IsDeleted.HasValue || c.IsDeleted == false)))
                    {
                        //var temp = _detailDa.GetById(model.CategoryID ?? 0);

                        //priceFinal += items.PriceProduct * items.Quantity;
                        //temp.Price = items.PriceProduct;
                        //temp.PriceCost = items.Price;
                        //temp.Incurred = items.Incurred;
                        //temp.Price = items.PriceProduct;
                        //temp.Value = items.Price;
                        //temp.PriceOld = items.Price + (temp.Incurred ?? 0) + (temp.Percent ?? 0) * 1000;
                        //_detailDa.Save();
                        var temp1 = _categoryDa.GetById(model.CategoryID ?? 0);
                        cost += items.Price;
                        priceFinal += items.Price + (temp1.Percent * 1000 * items.Quantity) + items.Incurred;
                        temp1.PriceRecipeFinal = items.Price + (items.Quantity * temp1.Percent * 1000) + items.Incurred;
                        _detailDa.Save();
                    }
                    var cate = _categoryDa.GetById(model.CategoryID ?? 0);
                    cate.PriceRecipe = cost;
                    cate.PriceRecipeFinal = priceFinal;
                    cate.TotalIncurredFinal = model.TotalIncurred;
                    _categoryDa.Save();
                }
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
        public ActionResult Active(string key, Guid userId)
        {
            var msg = new JsonMessage(false, "Update dữ liệu thành công !");
            try
            {
                if (key == Keyapi)
                {
                    var lst = Request["ItemID"].Split(',');
                    var id = int.Parse(lst[0] ?? "0");
                    var cateId = int.Parse(lst[1] ?? "0");
                    var model = _da.GetListByCateId(cateId);
                    foreach (var item in model)
                    {
                        item.IsUse = false;
                        if (id == item.ID)
                        {
                            item.IsUse = true;
                            item.DateUpdate = DateTime.Now.TotalSeconds();
                            item.UserId = userId;

                            decimal? cost = 0;
                            decimal? priceFinal = 0;
                            foreach (var items in item.Category_Product_Recipe.Where(c => c.IsCheck == true && (!c.IsDeleted.HasValue || c.IsDeleted == false)))
                            {
                                var temp = _detailDa.GetById(items.ProductId ?? 0);
                                cost += items.Price * items.Quantity;
                                priceFinal += items.Price == 0 ? 0 : (items.Price + temp.Category.Percent * 1000) * items.Quantity + items.Incurred;
                                temp.PriceCost = items.Price;
                                temp.Value = items.Price;
                                temp.Percent = temp.Category.Percent;
                                temp.PriceOld = items.Price + (temp.Incurred ?? 0) + (temp.Percent ?? 0) * 1000;
                                temp.Incurred = items.Incurred;
                                temp.Price = items.PriceProduct;
                                _detailDa.Save();
                            }

                            foreach (var items in item.Mapping_Category_Recipe.Where(c => c.IsCheck == true && (!c.IsDeleted.HasValue || c.IsDeleted == false)))
                            {
                                //var temp = _detailDa.GetById(model.CategoryID ?? 0);

                                //priceFinal += items.PriceProduct * items.Quantity;
                                //temp.Price = items.PriceProduct;
                                //temp.PriceCost = items.Price;
                                //temp.Incurred = items.Incurred;
                                //temp.Price = items.PriceProduct;
                                //temp.Value = items.Price;
                                //temp.PriceOld = items.Price + (temp.Incurred ?? 0) + (temp.Percent ?? 0) * 1000;
                                //_detailDa.Save();
                                var temp1 = _categoryDa.GetById(item.CategoryID ?? 0);
                                cost += items.Price;
                                priceFinal += items.Price + (temp1.Percent * 1000 * items.Quantity) + items.Incurred;
                                temp1.PriceRecipeFinal = items.Price + (items.Quantity * temp1.Percent * 1000) + items.Incurred;
                                _detailDa.Save();
                            }
                            var cate = _categoryDa.GetById(item.CategoryID ?? 0);
                            cate.PriceRecipe = cost;
                            cate.PriceRecipeFinal = priceFinal;
                            cate.TotalIncurredFinal = item.TotalIncurred;
                            _categoryDa.Save();
                        }
                    }
                    _da.Save();
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được Update.";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public List<Category_Product_Recipe> GetListCategoryRecipeItem(string code)
        {
            const string url = "Utility/GetListCategoryRecipeItem?key=";
            var urlJson = string.Format("{0}{1}", UrlG + url, code);
            var list = Utility.GetObjJson<List<RecipeCateItem>>(urlJson);
            var date = DateTime.Now.TotalSeconds();
            return list.Where(c => c.ProductId != null && c.CategoryID == null).Select(item => new Category_Product_Recipe()
            {
                Quantity = item.Quantity,
                ProductId = item.ProductId,
                Price = item.Price,
                IsDeleted = false,
                Incurred = item.Incurred,
                PriceProduct = item.PriceProduct,
                DateCreate = date,
                Percent = item.Percent,
                IsCheck = item.IsCheck == 1,
            }).ToList();
        }
        public List<Mapping_Category_Recipe> GetListMappingCategoryRecipeItem(string code)
        {
            const string url = "Utility/GetListCategoryRecipeItem?key=";
            var urlJson = string.Format("{0}{1}", UrlG + url, code);
            var list = Utility.GetObjJson<List<RecipeCateItem>>(urlJson);
            var date = DateTime.Now.TotalSeconds();
            return list.Where(c => c.ProductId == null && c.CategoryID != null).Select(item => new Mapping_Category_Recipe()
            {
                Quantity = item.Quantity,
                CategoryID = item.CategoryID,
                Price = item.Price,
                IsDeleted = false,
                Incurred = item.Incurred,
                PriceProduct = item.PriceProduct,
                DateCreate = date,
                Sl = item.Sl,
                Percent = item.Percent,
                IsCheck = item.IsCheck == 1,
            }).ToList();
        }
        public ActionResult Delete(string key, string lstArrId)
        {
            var msg = new JsonMessage(false, "Xóa dữ liệu thành công !");
            try
            {
                if (key == Keyapi)
                {
                    var lstInt = FDIUtils.StringToListInt(lstArrId);
                    var lst = _da.GetListByArrId(lstInt);
                    foreach (var item in lst)
                    {
                        item.IsDeleted = true;
                    }
                    _da.Save();
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được xóa.";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetProductCate(int categoryId)
        {
            var lst = _da.GetProduct(categoryId);
            return Json(lst);
        }

    }
}
