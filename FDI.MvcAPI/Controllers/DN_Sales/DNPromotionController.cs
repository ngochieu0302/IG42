//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using DotNetOpenAuth.Messaging;
//using FDI.Base;
//using FDI.CORE;
//using FDI.DA.DA.DN_Sales;
//using FDI.Simple;
//using FDI.Utils;

//namespace FDI.MvcAPI.Controllers.DN_Sales
//{
//    public class DNPromotionController : BaseApiController
//    {
//        //
//        // GET: /DNPromotion/
//        readonly DNPromotionDA _da = new DNPromotionDA("#");
//        public ActionResult ListItems(int agencyId)
//        {
//            var obj = Request["key"] != Keyapi
//                ? new ModelDNPromotionItem()
//                : new ModelDNPromotionItem { ListItems = _da.GetListSimpleByRequest(Request, agencyId), PageHtml = _da.GridHtmlPage };
//            return Json(obj, JsonRequestBehavior.AllowGet);
//        }
//        public ActionResult GetDNPromotionItem(string key, int id)
//        {
//            var obj = key != Keyapi ? new DNPromotionItem() : _da.GetDNPromotionItem(id);
//            return Json(obj, JsonRequestBehavior.AllowGet);
//        }
//        public ActionResult Add(string key, int agencyId, Guid userId, string codeLogin)
//        {
//            var model = new DN_Promotion();
//            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
//            try
//            {
//                //var lstproduct = Request["values-arr-product"];
//                var dateE = Request["DateEnd_"];
//                var dateS = Request["DateStart_"];
//                var lstCateIds = Request["ListCateId"];
//                UpdateModel(model);
//                model.IsDeleted = false;
//                model.IsShow = true;
//                model.AgencyId = agencyId;
//                model.IsAgency = false;
//                model.IsEnd = false;
//                model.UserCreate = userId;
//                model.QuantityUse = 0;
//                model.DateEnd = !string.IsNullOrEmpty(dateE)
//                    ? ConvertUtil.ToDateTime(dateE).TotalSeconds()
//                    : DateTime.Now.TotalSeconds();
//                model.DateStart = !string.IsNullOrEmpty(dateS)
//                    ? ConvertUtil.ToDateTime(dateS).TotalSeconds()
//                    : DateTime.Now.TotalSeconds();
//                if (model.TotalOrder == 0 || model.TotalOrder == null)
//                {
//                    if (model.IsAll != true)
//                    {
//                        if (!string.IsNullOrEmpty(lstCateIds))
//                        {
//                            model.Categories = _da.GetListCateByArrId(lstCateIds);
//                            model.IsAll = false;
//                        }
//                        else
//                        {
//                            var lst = GetListImportItem(codeLogin);
//                            if (lst.Any())
//                            {
//                                model.Product_Promotion = lst;
//                                model.IsAll = false;
//                            }
//                        }
//                    }
//                }
//                else
//                {
//                    model.IsAll = false;
//                }
//                if (model.IsAll == true)
//                {
//                    model.TotalOrder = 0;
//                }
//                model.Promotion_Product = GetListImportmItem(codeLogin);
//                _da.Add(model);
//                _da.Save();
//            }
//            catch (Exception ex)
//            {
//                msg.Erros = true;
//                msg.Message = "Dữ liệu chưa được thêm mới";
//                Log2File.LogExceptionToFile(ex);
//            }
//            return Json(msg, JsonRequestBehavior.AllowGet);
//        }
//        public List<Product_Promotion> GetListImportItem(string code)
//        {
//            const string url = "Utility/GetListImportPp?key=";
//            var urlJson = string.Format("{0}{1}", UrlG + url, code);
//            var list = Utility.GetObjJson<List<DNImportProductPromotionItem>>(urlJson);
//            return list.Select(item => new Product_Promotion()
//            {
//                Quantity = item.Quantity,
//                ProductID = item.ProductID,
//            }).ToList();
//        }
//        public List<Promotion_Product> GetListImportmItem(string code)
//        {
//            const string url = "Utility/GetListImportPm?key=";
//            var urlJson = string.Format("{0}{1}", UrlG + url, code);
//            var list = Utility.GetObjJson<List<DNPromotionProductItem>>(urlJson);
//            return list.Select(item => new Promotion_Product()
//            {
//                 Quantity = item.Quantity,
//                 ProductID = item.ProductID,
//                 Price = item.Price,
//                 Note = item.Note,
//                 IsEnd = item.IsEnd,
//                 Percent = item.Percent,
//            }).ToList();
//        }
//        public ActionResult Update(string key, Guid userId, string codeLogin)
//        {
//            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
//            try
//            {
//                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
//                var dateE = Request["DateEnd_"];
//                var dateS = Request["DateStart_"];
//                var model = _da.GetById(ItemId);
//                UpdateModel(model);
//                model.UserUpdate = userId;
//                model.DateEnd = !string.IsNullOrEmpty(dateE)
//                    ? ConvertUtil.ToDateTime(dateE).TotalSeconds()
//                    : DateTime.Now.TotalSeconds();
//                model.DateStart = !string.IsNullOrEmpty(dateS)
//                    ? ConvertUtil.ToDateTime(dateS).TotalSeconds()
//                    : DateTime.Now.TotalSeconds();
//                model.Categories.Clear();
//                var lstCateIds = Request["ListCateId"];
//                if (model.TotalOrder == 0 || model.TotalOrder == null)
//                {
//                    if (model.IsAll != true)
//                    {
//                        if (!string.IsNullOrEmpty(lstCateIds))
//                        {
//                            model.Categories = _da.GetListCateByArrId(lstCateIds);
//                            model.IsAll = false;
//                        }
//                        else
//                        {
//                           var lstNew = GetListImportItem(codeLogin);
//                            if (lstNew.Any())
//                            {
//                                //sản phẩm có khuyến mãi
//                                var lst = model.Product_Promotion;
//                                var result1 = lst.Where(p => lstNew.All(p2 => p2.ProductID != p.ProductID)).ToList();
//                                foreach (var i in result1)
//                                {
//                                    lst.Remove(i);
//                                }
//                                foreach (var i in lst)
//                                {
//                                    var j = lstNew.FirstOrDefault(c => c.ProductID == i.ProductID);
//                                    if (j == null) continue;
//                                    i.Quantity = j.Quantity;
//                                }
//                                var result2 = lstNew.Where(p => lst.All(p2 => p2.ProductID != p.ProductID)).ToList();
//                                model.Product_Promotion.AddRange(result2);
//                                model.IsAll = false;
//                            }
//                        }
//                    }
//                }
//                else
//                {
//                    model.IsAll = false;
//                }
//                //sản phẩm đính kèm sp khuyến mãi
//                var lstm = model.Promotion_Product;
//                var lstNewm = GetListImportmItem(codeLogin);
//                var result1M = lstm.Where(p => lstNewm.All(p2 => p2.ProductID != p.ProductID)).ToList();
//                foreach (var i in result1M)
//                {
//                    lstm.Remove(i);
//                }
//                foreach (var i in lstm)
//                {
//                    var j = lstNewm.FirstOrDefault(c => c.ProductID == i.ProductID);
//                    if (j == null) continue;
//                    i.Quantity = j.Quantity;
//                    i.Price = j.Price;
//                    i.Note = j.Note;
//                    i.IsEnd = j.IsEnd;
//                }
//                var result2M = lstNewm.Where(p => lstm.All(p2 => p2.ProductID != p.ProductID)).ToList();
//                model.Promotion_Product.AddRange(result2M);
//                _da.Save();
//            }
//            catch (Exception ex)
//            {
//                msg.Erros = true;
//                msg.Message = "Dữ liệu chưa được cập nhật";
//                Log2File.LogExceptionToFile(ex);
//            }
//            return Json(msg, JsonRequestBehavior.AllowGet);
//        }
//        public ActionResult Delete(string key, string lstArrId)
//        {
//            var msg = new JsonMessage(false, "Xóa dữ liệu thành công.");
//            try
//            {
//                if (key == Keyapi)
//                {
//                    var lstInt = FDIUtils.StringToListInt(lstArrId);
//                    var lst = _da.GetListByArrId(lstInt);
//                    foreach (var item in lst)
//                    {
//                        item.IsDeleted = true;
//                    }
//                    _da.Save();
//                }
//                else
//                {
//                    msg.Erros = true;
//                    msg.Message = "Truy cập thất bại.";
//                }
//            }
//            catch (Exception ex)
//            {
//                msg.Erros = true;
//                msg.Message = "Dữ liệu chưa được xóa";
//                Log2File.LogExceptionToFile(ex);
//            }
//            return Json(msg, JsonRequestBehavior.AllowGet);
//        }

//    }
//}
