
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
    public class ProductDetailRecipeController : BaseApiAuthController
    {
        //
        // GET: /ProductDetailRecipe/

        private readonly ProductDetailRecipeDA _da = new ProductDetailRecipeDA("#");
        private readonly ShopProductDetailDA _detailDa = new ShopProductDetailDA("#");
        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelProductDetailRecipeItem()
                : new ModelProductDetailRecipeItem { ListItems = _da.GetListSimpleByRequest(Request), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new ProductDetailRecipeItem() : _da.GetItemById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAll(string key)
        {
            var obj = key != Keyapi ? new List<ProductDetailRecipeItem>() : _da.GetAll(); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key, string codelogin, Guid? userId)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var model = new ProductDetail_Recipe();
                    UpdateModel(model);
                    model.IsDeleted = false;
                    model.DateCreate = DateTime.Now.TotalSeconds();
                    model.UserID = userId;
                    var restrictiveV = GetListRecipeValueItem(codelogin);

                    var lst = model.ProductValue_Recipe.Where(c => c.IsDeleted == false).ToList();
                    var result2 = restrictiveV.Where(p => lst.All(p2 => p2.ProductValueId != p.ProductValueId)).ToList();
                    foreach (var item in result2)
                    {
                        model.ProductValue_Recipe.Add(item);
                    }
                    var restrictiveD = GetListRecipeDetailItem(codelogin);
                    var lst1 = model.Mapping_ProductDetail_Recipe.Where(c => c.IsDeleted == false).ToList();
                    var result1 = restrictiveD.Where(p => lst1.All(p2 => p2.DetailID != p.DetailID)).ToList();
                    foreach (var item in result1)
                    {
                        model.Mapping_ProductDetail_Recipe.Add(item);
                    }
                    _da.Add(model);
                    _da.Save();
                    //var temp = _da.GetItemById(model.ID);
                    //var detail = _detailDa.GetById(model.ProductDetailId ?? 0);
                    ////update gia san pham
                    //var productPrice = temp.LstRecipeProductDetails.Sum(a => a.Quantity * a.ProductPrice);
                    //var vaulePrice = temp.LstRecipeProductValues.Sum(a => a.Quantity * a.ValuePrice);
                    //model.ProductPrice = productPrice;
                    //model.ValuePrice = vaulePrice;

                    //var pricecost = vaulePrice + productPrice;
                    //detail.PriceCost = pricecost;
                    //detail.PriceOld = pricecost + detail.Percent * 1000 + (detail.Incurred ?? 0);
                    //_da.Save();
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được thêm mới.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update(string key, string codelogin, Guid? userId)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var model = _da.GetById(ItemId);
                    UpdateModel(model);
                    model.DateUpdate = DateTime.Now.TotalSeconds();
                    model.UserID = userId;
                    var lst = model.ProductValue_Recipe.Where(c => c.IsDeleted == false || !c.IsDeleted.HasValue).ToList();
                    var lstNew = GetListRecipeValueItem(codelogin);
                    //xóa
                    var result1 = lst.Where(p => lstNew.All(p2 => p2.ProductValueId != p.ProductValueId));
                    foreach (var i in result1)
                    {
                        i.IsDeleted = true;
                    }
                    //Sửa
                    foreach (var i in lst)
                    {
                        var j = lstNew.FirstOrDefault(c => c.ProductValueId == i.ProductValueId);
                        if (j != null)
                        {
                            i.Quantity = j.Quantity;

                        }
                    }
                    //thêm mới
                    var result2 = lstNew.Where(p => lst.All(p2 => p2.ProductValueId != p.ProductValueId)).ToList();
                    foreach (var item in result2)
                    {
                        model.ProductValue_Recipe.Add(item);
                    }


                    //var vaulePrice = model.ProductValue_Recipe.Sum(a => a.Quantity * a.Shop_Product_Value.Price);
                    //
                    var lstDetail = model.Mapping_ProductDetail_Recipe.Where(c => c.IsDeleted == false || !c.IsDeleted.HasValue).ToList();
                    var lstNewDetail = GetListRecipeDetailItem(codelogin);
                    //xóa
                    var resultDetail = lstDetail.Where(p => lstNewDetail.All(p2 => p2.DetailID != p.DetailID));
                    foreach (var i in resultDetail)
                    {
                        i.IsDeleted = true;
                    }
                    //Sửa
                    foreach (var i in lstDetail)
                    {
                        var j = lstNewDetail.FirstOrDefault(c => c.DetailID == i.DetailID);
                        if (j != null)
                        {
                            i.Quantity = j.Quantity;
                        }
                    }
                    //thêm mới
                    var result2Detail = lstNewDetail.Where(p => lstDetail.All(p2 => p2.DetailID != p.DetailID)).ToList();
                    foreach (var item in result2Detail)
                    {
                        model.Mapping_ProductDetail_Recipe.Add(item);
                    }
                    //var productPrice = model.Mapping_ProductDetail_Recipe.Sum(a => a.Quantity * a.Shop_Product_Detail.PriceCost);
                    _da.Save();

                    //var detail = _detailDa.GetById(model.ProductDetailId ?? 0);
                    //var pricecost = vaulePrice + productPrice;

                    //model.ProductPrice = productPrice;
                    //model.ValuePrice = vaulePrice;
                    //detail.PriceCost = pricecost;

                    //detail.PriceOld = pricecost + detail.Percent * 1000 + (detail.Incurred ?? 0);
                    _da.Save();
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được thêm mới.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Active(string key, Guid userId)
        {
            var msg = new JsonMessage(false, "Update dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {

                    var lst = Request["ItemID"].Split(',');
                    var id = int.Parse(lst[0] ?? "0");
                    var cateId = int.Parse(lst[1] ?? "0");
                    var model = _da.GetListByArrCateId(cateId);
                    foreach (var item1 in model)
                    {
                        item1.IsUse = false;
                        if (id == item1.ID)
                        {
                            item1.IsUse = true;
                            item1.DateUpdate = DateTime.Now.TotalSeconds();
                            item1.UserID = userId;

                            var detail = _detailDa.GetById(item1.ProductDetailId ?? 0);

                            var productPrice = item1.Mapping_ProductDetail_Recipe.Sum(a => a.Quantity * a.Shop_Product_Detail.PriceCost);
                            var vaulePrice = item1.ProductValue_Recipe.Sum(a => a.Quantity * a.Shop_Product_Value.Price);

                            var pricecost = vaulePrice + productPrice;

                            item1.ProductPrice = productPrice;
                            item1.ValuePrice = vaulePrice;

                            detail.PriceCost = pricecost;
                            detail.PriceOld = pricecost + detail.Percent * 1000 + (detail.Incurred ?? 0);

                            _detailDa.Save();
                        }
                    }
                    _da.Save();
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được xóa.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(string key, string lstArrId)
        {
            var msg = new JsonMessage(false, "Xóa dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var lstProduct = _da.ListByArrId(lstArrId);
                    foreach (var item in lstProduct)
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
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public List<Mapping_ProductDetail_Recipe> GetListRecipeDetailItem(string code)
        {
            const string url = "Utility/GetListDetailRecipeItem?key=";
            var urlJson = string.Format("{0}{1}", UrlG + url, code);
            var list = Utility.GetObjJson<List<ProductRecipeItem>>(urlJson);
            var date = DateTime.Now.TotalSeconds();
            return list.Where(c => c.ValueId == null).Select(item => new Mapping_ProductDetail_Recipe
            {
                Quantity = item.Quantity,
                DetailID = item.ProductDetailID,
                IsDeleted = false,
                DateCreate = date
            }).ToList();
        }
        public List<ProductValue_Recipe> GetListRecipeValueItem(string code)
        {
            const string url = "Utility/GetListDetailRecipeItem?key=";
            var urlJson = string.Format("{0}{1}", UrlG + url, code);
            var list = Utility.GetObjJson<List<ProductRecipeItem>>(urlJson);
            var date = DateTime.Now.TotalSeconds();
            return list.Where(c => c.ProductDetailID == null).Select(item => new ProductValue_Recipe()
            {
                Quantity = item.Quantity,
                ProductValueId = item.ValueId,
                IsDeleted = false,
                DateCreate = date
            }).ToList();
        }
    }
}
