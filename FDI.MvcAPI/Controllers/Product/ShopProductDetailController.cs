using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;
using Newtonsoft.Json;
using System.Web;
using DotNetOpenAuth.Messaging;
using FDI.CORE;

namespace FDI.MvcAPI.Controllers
{
    public class ShopProductDetailController : BaseApiAuthController
    {
        //
        // GET: /DNActive/

        private readonly ShopProductDetailDA _da = new ShopProductDetailDA();
        readonly ProductDA _productDa = new ProductDA();

        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelShopProductDetailItem()
                : new ModelShopProductDetailItem { ListItem = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult GetListOrderDetail(string date)
        //{
        //    var obj = Request["key"] != Keyapi
        //        ? new ModelProductExportItem()
        //        : _da.GetListOrderDetail(Agencyid(), date);
        //    return Json(obj, JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult GetOrderDetailExport(string date)
        //{
        //    var obj = Request["key"] != Keyapi
        //        ? new ModelProductExportItem()
        //        : _da.GetOrderDetailExport(Agencyid(), date);
        //    return Json(obj, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult GetListValueDetail(string date)
        //{
        //    var obj = Request["key"] != Keyapi
        //        ? new ModuleShopProductValueItem()
        //        : _da.GetListValueDetail(Agencyid(), date);
        //    return Json(obj, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult GetValueDetailExport(int agencyId, string date)
        //{
        //    var obj = Request["key"] != Keyapi
        //        ? new ModuleShopProductValueItem()
        //        : _da.GetValueDetailExport(agencyId, date);
        //    return Json(obj, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult GetAll(string key)
        {
            var obj = key != Keyapi ? new List<ShopProductDetailItem>() : _da.GetAll(Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetById(string key, int id)
        {
            var obj = key != Keyapi ? new Shop_Product_Detail() : _da.GetById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new ShopProductDetailItem() : _da.GetItemById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult GetRecipeItemByDetailId(string key, int id)
        //{
        //    var obj = key != Keyapi ? new ProductDetailRecipeItem() : _da.GetRecipeItemByDetailId(id); ;
        //    return Json(obj, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult GetListByArrId(string key, string lstId)
        {
            var obj = key != Keyapi ? new List<ShopProductDetailItem>() : _da.GetListByArrId(lstId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListAuto(string key, string keword, int showLimit, int agencyId)
        {
            var obj = key != Keyapi ? new List<SuggestionsProduct>() : _da.GetListAuto(keword, showLimit, agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListAutoCate(string key, string keword, int showLimit, int agencyId, int cateId)
        {
            var obj = key != Keyapi ? new List<SuggestionsProduct>() : _da.GetListAutoCate(keword, showLimit, agencyId, cateId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListCateAuto(string key, string keword, int showLimit, int agencyId, int cateId)
        {
            var obj = key != Keyapi ? new List<SuggestionsProduct>() : _da.GetListCateAuto(keword, showLimit, agencyId, cateId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckExitCode(string key, string code, int id, int agencyId)
        {
            if (key == Keyapi)
            {
                var b = _da.CheckExitCode(code, id, agencyId);
                return Json(b ? 1 : 0, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add(string key, string codelogin)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            try
            {
                
                    var model = new Shop_Product_Detail();
                    UpdateModel(model);
                    var lstPicture = Request["Value_ImagesProducts"];
                    if (!string.IsNullOrEmpty(lstPicture))
                    {
                        model.Gallery_Picture2 = _da.GetListPictureByArrId(lstPicture);
                    }

                    model.Name = HttpUtility.UrlDecode(model.Name);
                    model.NameAscii = FomatString.Slug(model.Name);
                    model.IsDelete = false;
                    if (IsAdmin)
                    {
                        model.IsShow = true;
                    }
                    else
                    {
                        model.IsShow = false;
                    }
                    var objProduct = new Shop_Product
                    {
                        QuantityDay = model.QuantityDay,
                        Quantity = 0,
                        CreateDate = model.StartDate,
                        CodeSku = model.Code,
                        IsDelete = false,
                        IsShow = model.IsShow
                    };
                    model.Shop_Product.Add(objProduct);
                    _da.Add(model);
                    _da.Save();
                
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được thêm mới.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Active(string key, string codelogin, Guid? userId)
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
                    model.ProductValue_Recipe.AddRange(result2);
                    var restrictiveD = GetListRecipeDetailItem(codelogin);
                    var lst1 = model.Mapping_ProductDetail_Recipe.Where(c => c.IsDeleted == false).ToList();
                    var result1 = restrictiveD.Where(p => lst1.All(p2 => p2.DetailID != p.DetailID)).ToList();
                    model.Mapping_ProductDetail_Recipe.AddRange(result1);
                    _da.AddRecipe(model);
                    _da.Save();
                    var temp = _da.GetRecipeItemById(model.ID);
                    var detail = _da.GetById(model.ProductDetailId ?? 0);
                    //update gia san pham
                    var productPrice = temp.LstRecipeProductDetails.Sum(a => a.Quantity * a.ProductPrice);
                    var vaulePrice = temp.LstRecipeProductValues.Sum(a => a.Quantity * a.ValuePrice);
                    model.ProductPrice = productPrice;
                    model.ValuePrice = vaulePrice;

                    var pricecost = vaulePrice + productPrice;
                    detail.PriceCost = pricecost;
                    detail.PriceOld = pricecost + detail.Percent * 1000 + (detail.Incurred ?? 0);
                    
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
        public ActionResult NotActive(string key, string codelogin, Guid? userId)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var model = _da.GetbyIdRecipe(ItemId);
                    UpdateModel(model);
                    model.DateUpdate = DateTime.Now.TotalSeconds();
                    model.UserID = userId;
                    var lst = model.ProductValue_Recipe.Where(c => c.IsDeleted == false).ToList();
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
                    model.ProductValue_Recipe.AddRange(result2);

                    var vaulePrice = model.ProductValue_Recipe.Sum(a => a.Quantity * a.Shop_Product_Value.Price);
                    //
                    var lstDetail = model.Mapping_ProductDetail_Recipe.Where(c => c.IsDeleted == false).ToList();
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
                    model.Mapping_ProductDetail_Recipe.AddRange(result2Detail);
                    var productPrice = model.Mapping_ProductDetail_Recipe.Sum(a => a.Quantity * a.Shop_Product_Detail.PriceCost);
                    _da.Save();

                    var detail = _da.GetById(model.ProductDetailId ?? 0);
                    var pricecost = vaulePrice + productPrice;

                    model.ProductPrice = productPrice;
                    model.ValuePrice = vaulePrice;
                    detail.PriceCost = pricecost;

                    detail.PriceOld = pricecost + detail.Percent * 1000 + (detail.Incurred ?? 0);
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
        public ActionResult Coppy(string key)
        {
            var msg = new JsonMessage(false, "Coppy dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var model = _da.GetById(ItemId);
                    var modelNew = new Shop_Product_Detail
                    {
                        Name = model.Name + " coppy",
                        NameAscii = model.NameAscii + "-coppy",
                        Code = model.Code + "-coppy",
                        Price = model.Price,
                        QuantityDay = model.QuantityDay,
                        IsShow = model.IsShow,
                        IsDelete = false,
                        CateID = model.CateID,
                        PictureID = model.PictureID,
                        UnitID = model.UnitID,
                        StartDate = DateTime.Now.TotalSeconds(),
                        Description = model.Description,
                    };
                    foreach (var item in model.Shop_Product)
                    {
                        var objProduct = new Shop_Product
                        {
                            SizeID = item.SizeID,
                            PriceNew = item.PriceNew,
                            PriceOld = item.PriceOld,
                            QuantityDay = model.QuantityDay,
                            Quantity = 0,
                            CreateDate = model.StartDate,
                            CodeSku = model.Code,
                            IsDelete = false,
                            IsShow = model.IsShow
                        };

                        modelNew.Shop_Product.Add(objProduct);
                    }
                    
                    _da.Add(modelNew);
                    _da.Save();
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được Coppy.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update(string key, string json, string codelogin,bool isadmin)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var model = _da.GetById(ItemId);
                    UpdateModel(model);
                    if (!isadmin)
                    {
                        model.IsShow = false;
                    }
                    
                    model.Name = HttpUtility.UrlDecode(model.Name);
                    model.NameAscii = FomatString.Slug(model.Name);
                    _da.Save();
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được Cập nhật.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Addproduct(string key, string json)
        {
            var msg = new JsonMessage(false, "Thêm mới sản phẩm thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var model = _da.GetProductById(ItemId);
                    var product = _da.GetById(ItemId);
                    var list = new List<Shop_Product>();
                    foreach (var item in model)
                    {
                        var size = Request["Size_old" + item.ID];
                        var Type = Request["Type_old" + item.ID];
                        var price = Request["Price_old" + item.ID];
                        if (string.IsNullOrEmpty(size))
                        {
                            list.Add(item);
                        }
                        else
                        {
                            item.SizeID = int.Parse(size);
                            item.TypeID = int.Parse(Type);
                            item.PriceNew = decimal.Parse(price);
                            _da.Save();
                        }
                    }

                    foreach (var item in list)
                    {
                        _da.DeleteProduct(item);
                    }
                    var stt = ConvertUtil.ToInt32(Request["do_stt"]);
                    for (int i = 1; i <= stt; i++)
                    {
                        var name = Request["Size_add_" + i];
                        var type = Request["Type_add_" + i];
                        var price = Request["Price_add_" + i];
                        if (!string.IsNullOrEmpty(name))
                        {
                            var obj = new Shop_Product()
                            {
                                ProductDetailID = product.ID,
                                SizeID = int.Parse(name),
                                TypeID = int.Parse(type),
                                IsShow = true,
                                IsDelete = false,
                            };
                            obj.PriceNew = decimal.Parse(price);
                            _da.AddProduct(obj);
                        }
                    }
                    _da.Save();
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được Cập nhật.";
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
                    var lstProduct = _da.GetListProductByArrId(lstArrId);
                    foreach (var item in lstProduct)
                    {
                        item.IsDelete = true;
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
        public ActionResult ShowHide(string key, string lstArrId, bool showhide)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetListProductByArrId(lstArrId);
                foreach (var item in model)
                {
                    item.IsShow = showhide;
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
