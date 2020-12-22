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
using Newtonsoft.Json;

namespace FDI.MvcAPI.Controllers
{
    public class ProductController : BaseApiController
    {
        readonly ProductDA _da = new ProductDA();
        readonly ShopProductDetailDA _detailDa = new ShopProductDetailDA();
        readonly AttributesDA _attributesDa = new AttributesDA();
        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelProductItem()
                : new ModelProductItem { ListItem = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult GetListSimple(string key, int agencyId)
        {
            var obj = key != Keyapi ? new List<ProductItem>() : _da.GetListSimple(agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult GetList(string key)
        {
            var obj = key != Keyapi ? new List<CategoryItem>() : _da.GetList(Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }        
        public ActionResult GetListAuto(string key, string keword, int showLimit, int agencyId, int type = 0)
        {
            var obj = Request["key"] != Keyapi ? new List<SuggestionsProduct>() : _da.GetListAuto(keword, showLimit, agencyId, type);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListAutoOne(string key, string keword, int showLimit, int agencyId, int type = 0)
        {
            var obj = Request["key"] != Keyapi ? new List<SuggestionsProduct>() : _da.GetListAutoOne(keword, showLimit, agencyId, type);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult GetListAutoFull(string key, string keword, int showLimit, int agencyId, int type = 0)
        {
            var obj = Request["key"] != Keyapi ? new List<SuggestionsProduct>() : _da.GetListAutoFull(keword, showLimit, agencyId, type);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListCommentAuto(string key, string keword, int showLimit, int agencyId)
        {
            var obj = Request["key"] != Keyapi ? new List<SuggestionsProduct>() : _da.GetListCommentAuto(keword, showLimit, agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListAutoComplete(string key, string keword, int showLimit, int agencyId, int type = 0)
        {
            var obj = Request["key"] != Keyapi ? new List<SuggestionsProduct>() : _da.GetListAutoComplete(keword, showLimit, agencyId, type);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListByAgency(string key, int id)
        {
            var obj = Request["key"] != Keyapi ? new List<ProductItem>() : _da.GetListByAgency(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListByPacket(string key, int id, int beddeskId)
        {
            var obj = Request["key"] != Keyapi ? new List<PacketItem>() : _da.GetListByPacket(id, beddeskId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListProductByDeddeskId(string key, string code, int beddeskId)
        {
            var obj = key != Keyapi ? new ModelOrderGetItem() : _da.ListProductByDeddeskId(Agencyid(), beddeskId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListProductByDeddeskIdSpa(string key, string code, int beddeskId)
        {
            var obj = key != Keyapi ? new ModelOrderGetItem() : _da.ListProductByDeddeskIdSpa(Agencyid(), beddeskId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListByProductDetailsId(string key, int productDetailId)
        {
            var obj = Request["key"] != Keyapi ? new List<ProductItem>() : _da.GetListByProductDetailsId(Agencyid(), productDetailId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCostProduceItem(int id)
        {
            var obj = Request["key"] != Keyapi ? new ProductItem() : _da.GetCostProduceItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetStatus(string key, int agencyId)
        {
            var obj = Request["key"] != Keyapi ? new List<ShopStatusItem>() : _da.GetStatus(agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetProductItem(string key, int id)
        {
            var obj = key != Keyapi ? new ProductItem() : _da.GetProductItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult GetAttribute(string key, string lstInts, int id)
        //{
        //    var obj = key != Keyapi ? new List<AttributeDynamicItem>() : _attrDa.GetAttribute(lstInts, id);
        //    return Json(obj, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult CheckExitCode(string key, string code, int id, int agencyId)
        {
            var b = key == Keyapi && _da.CheckExitCode(code, id, agencyId);
            return Json(b ? 1 : 0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddAttribute(string key)
        {
            if (key == Keyapi)
            {
                var countAttr = Request["CountAttr"];
                var productId = Request["productId"];
                var lstAttr = new List<AttributeOption>();
                var lstInt = new List<int>();
                for (var i = 1; i <= int.Parse(countAttr); i++)
                {
                    var attrId = Request["AttrId_" + i];
                    var item = new AttributeOption
                    {
                        //ProductID = int.Parse(productId),
                        AttributeID = int.Parse(Request["AttrValue_" + i]),
                        Values = Request["Attribute_" + i],
                        CreatedDate = DateTime.Now
                    };
                    if (!string.IsNullOrEmpty(attrId))
                    {
                        item.ID = int.Parse(attrId);
                        lstInt.Add(int.Parse(attrId));
                        lstAttr.Add(item);
                    }
                    else _attributesDa.Add(item);
                    
                }
                var lst = _attributesDa.GetAttrValue(lstInt);
                foreach (var it in lst)
                {
                    var newitem = lstAttr.FirstOrDefault(c => c.ID == it.ID);
                    if (newitem != null) it.Values = newitem.Values;
                }
                _attributesDa.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key, string code)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            var model = new Shop_Product_Detail();
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                var CodeSku = Request["CodeSku"];
                model.IsDelete = false;

                
                var lstPicture = Request["Value_DefaultImages"];
                if (!string.IsNullOrEmpty(lstPicture))
                {
                    var lstInt = FDIUtils.StringToListInt(lstPicture);
                    model.PictureID = lstInt.FirstOrDefault();
                }
                model.Name = HttpUtility.UrlDecode(model.Name);
                model.NameAscii = FDIUtils.Slug(model.Name);
                model.Code = HttpUtility.UrlDecode(CodeSku);
                _detailDa.Add(model);
                _detailDa.Save();
                var CreateBy = Request["CreateBy"];
                //var Percent = Request["Percent"];
                var product = new Shop_Product
                {
                    ProductDetailID = model.ID,
                    CodeSku = model.Code,
                    Quantity = model.QuantityDay,
                    
                    CreateBy = CreateBy,
                    CreateDate = DateTime.Now.TotalSeconds(),
                    IsShow = true,
                    IsDelete = false,
                };
               
                _da.Add(product);
                _da.Save();
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu Chưa được thêm mới.";
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
                    var modelNew = new Shop_Product();
                    var model = _da.GetById(ItemId);
                    modelNew.IsDelete = false;
                    modelNew.IsShow = true;
                    modelNew.ColorID = model.ColorID;
                    modelNew.SizeID = model.SizeID;
                    modelNew.Quantity = 0;
                    modelNew.CodeSku = model.CodeSku + "Coppy";
                    modelNew.ProductDetailID = model.ProductDetailID;
                   
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
        public ActionResult Update(string key, string code)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetById(ItemId);
                UpdateModel(model);
                var lstPicture = Request["Value_DefaultImages"];
                var createBy = Request["CreateBy"];
                var codeSku = Request["CodeSku"];
                model.CodeSku = HttpUtility.UrlDecode(codeSku);
                model.UpdateBy = createBy;
                //Công thức

                //model.PriceCost = model.Product_Recipe.Where(c => c.IsDelete == false).Sum(c => c.Quantity * c.Price);

                model.Shop_Product_Picture.Clear();
                _da.Save();
                if (!string.IsNullOrEmpty(lstPicture))
                {
                    var lstInt = FDIUtils.StringToListInt(lstPicture);
                    foreach (var item in lstInt)
                    {
                        var pic = new Shop_Product_Picture
                        {
                            PictureID = item,
                            ProductID = model.ID,
                            Sort = 0
                        };
                        model.Shop_Product_Picture.Add(pic);
                    }
                    _da.Save();
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được cập nhật.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddRecipe(string key, string code)
        {
            var msg = new JsonMessage(false, "Công thức cập nhật thành công");
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetById(ItemId);
                _da.Save();
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Công thức chưa được cập nhật.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateCost(string key)
        {
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var lst1 = Request["lstRet1"];
                var lst2 = Request["lstRet2"];
                var model = _da.GetById(ItemId);
                while (model.Cost_Product.Count > 0)
                {
                    var item = model.Cost_Product.FirstOrDefault();
                    _da.Delete(item);
                }
                while (model.Cost_Product_User.Count > 0)
                {
                    var item = model.Cost_Product_User.FirstOrDefault();
                    _da.Delete(item);
                }
                model.Cost_Product.Clear();
                model.Cost_Product_User.Clear();
                model.Cost_Product = JsonConvert.DeserializeObject<List<Cost_Product>>(lst1);
                model.Cost_Product_User = JsonConvert.DeserializeObject<List<Cost_Product_User>>(lst2);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Delete(string key, string lstArrId)
        {
            var msg = new JsonMessage(false, "Xóa dữ liệu thành công !");
            try
            {
                if (key == Keyapi)
                {
                    var lstInt = FDIUtils.StringToListInt(lstArrId);
                    var lst = _da.GetListArrId(lstInt);
                    foreach (var item in lst)
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
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Show(string key, string lstArrId)
        {
            var msg = new JsonMessage(false, "Hiển thị dữ liệu thành công !");
            try
            {
                if (key == Keyapi)
                {
                    var lstInt = FDIUtils.StringToListInt(lstArrId);
                    var lst = _da.GetListArrId(lstInt);
                    foreach (var item in lst)
                    {
                        item.IsShow = true;
                    }
                    _da.Save();
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Hiển thị dữ liệu chưa thành công.";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Hide(string key, string lstArrId)
        {
            var msg = new JsonMessage(false, "Ẩn dữ liệu thành công !");
            try
            {
                if (key == Keyapi)
                {
                    var lstInt = FDIUtils.StringToListInt(lstArrId);
                    var lst = _da.GetListArrId(lstInt);
                    foreach (var item in lst)
                    {
                        item.IsShow = false;
                    }
                    _da.Save();
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Ẩn dữ liệu chưa thành công.";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        
    }
}