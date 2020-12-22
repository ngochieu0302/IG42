using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.DA.DA.StorageWarehouse;
using FDI.MvcAPI.Models;
using FDI.Simple;
//using ConvertDate = FDI.Utils.ConvertDate;

namespace FDI.MvcAPI.Controllers.APPSales
{
    public class OrderAppController : BaseApiAuthAppSaleController
    {
        private int rowPerPage = 4;
        //
        // GET: /OrderApp/
        readonly OrderAppDA _da = new OrderAppDA();
        readonly CategoryDA _categoryDa = new CategoryDA();
        private readonly ProductDetailRecipeDA _productDetailRecipeDa = new ProductDetailRecipeDA();
        private readonly ContactOrderDA _contactOrderDa = new ContactOrderDA();
        public ActionResult ListByParentID(string key)
        {
            var obj = key != Keyapi ? new List<CategoryAppItem>() : _da.ListByParentID();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        #region UserLogin
        public ActionResult GetStorageByUser(int pageIndex, int orderStatus, decimal startDate, decimal endDate, bool? orderbyPrice)
        {
            var total = 0;
            var obj = _da.GetStorageByUser(AgencyId, pageIndex, rowPerPage, orderStatus, startDate, endDate, orderbyPrice, ref total);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetStorageById(string key, int agencyid, int id)
        {
            var obj = key != Keyapi ? new SWItem() : _da.GetStorageByID(agencyid, id);

            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListStorageByUser(int rowPerPage, int page, int agencyId, int status)
        {
            int total = 0;
            var obj = Request["key"] != Keyapi ? new ModelSWItem() : new ModelSWItem { LstItem = _da.ListStorageByUser(rowPerPage, page, agencyId, status, ref total), t = total };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        #endregion
        public ActionResult Add(string key, int agencyid, string j)
        {
            try
            {
                var date = ConvertDate.TotalSeconds(DateTime.Now.AddHours(12));
                var today = DateTime.Today.AddDays(1);
                var todayse = ConvertDate.TotalSeconds(today);
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var obj = new JavaScriptSerializer().Deserialize<SWItem>(j);
                var listc = _da.ListByListID(obj.LstItem.Select(m => m.c).ToList());
                var objagency = _da.GetbyID(agencyid);
                if (objagency != null)
                {
                    var list = obj.LstItem.Where(m => listc.Any(c => c.ID == m.c) && todayse + m.h * 3600 >= date).Select(item => new DN_RequestWare
                    {
                        GID = Guid.NewGuid(),
                        Quantity = item.q,
                        CateID = item.c,
                        IsDelete = false,
                        Date = ConvertDate.TotalSeconds(DateTime.Now),
                        Hour = item.h,
                        DateEnd = todayse + item.h * 3600,
                        Day = 0,
                        Price = listc.Where(c => c.ID == item.c).Select(m => m.Price).FirstOrDefault(),
                        TotalPrice = listc.Where(c => c.ID == item.c).Select(m => m.Price).FirstOrDefault() * item.q,
                        QuantityActive = item.q,
                        Today = todayse,
                        AgencyID = agencyid,
                        MarketID = objagency.MarketID,
                        AreaID = objagency.AreaID,
                        QuantityUsed = 0,
                        Type = item.t
                    }).ToList();
                    var total = list.Sum(m => m.TotalPrice);
                    if (total * 3 / 10 <= objagency.TotalDeposit)
                    {
                        var model = new StorageWarehousing
                        {
                            DN_RequestWare = list,
                            Status = (int)StatusWarehouse.Pending,
                            Name = "ĐH: " + today.ToString("dd/MM/yyyy"),
                            DateCreated = ConvertDate.TotalSeconds(DateTime.Now),
                            Code = today.ToString("yyMMdd"),
                            DateRecive = todayse,
                            AgencyId = agencyid,
                            IsDelete = false,
                            TotalPrice = total
                        };
                        if (obj.id > 0)
                        {
                            model = _da.GetObjByID(agencyid, obj.id);
                            if (model != null)
                            {
                                model.DN_RequestWare.Clear();
                                model.DN_RequestWare = list;
                            }
                            else return Json(0, JsonRequestBehavior.AllowGet);
                        }
                        else _da.Add(model);
                        _da.Save();
                        return Json(1, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpPost]
        public ActionResult GetProductToDay()
        {
            //TODO: get category chinh sach dai ly
            var todayCode = DateTime.Today.AddDays(1).TotalSeconds();
            var result = GetProducts(todayCode);

            var cate = result;
            decimal quantity = 0;

            if (cate != null)
            {
                // lay so luong dat nhieu nhat
                var maxProductWeight = cate.Products.OrderByDescending(m => m.Weight / m.WeightRecipe).FirstOrDefault();
                if (maxProductWeight != null)
                {
                    quantity = Math.Round(Math.Ceiling(maxProductWeight.Weight * 10 / maxProductWeight.WeightRecipe) / 10, 2);
                }
            }
            var storageWareHouseDA = new StorageWareHouseDA();
            var order = storageWareHouseDA.GetStorageWarehousingItem(todayCode, AgencyId);
            return Json(new { Quantity = quantity, Data = result, Status = order?.Status });
        }

        private OrderGabModel GetProducts(decimal todayCode)
        {
            var categoriesId = new int[] { 3 };

            var _cateRecipeDa = new CateRecipeDA();

            //get product customer order

            var products = _contactOrderDa.GetProductDetail(AgencyId, todayCode);
            var listTmp = new List<OrderDetailItem>();

            foreach (var orderDetailItem in products)
            {
                var productRecipe = _productDetailRecipeDa.GetProductRecipe(orderDetailItem.ProductID ?? 0);
                if (productRecipe != null)
                {
                    var productsRecipe = _productDetailRecipeDa.GetRecipeDetails(productRecipe.ID);
                    foreach (var recipeProductDetail in productsRecipe)
                    {
                        if (recipeProductDetail.DetailID != null)
                            listTmp.Add(new OrderDetailItem()
                            {
                                ProductID = recipeProductDetail.DetailID.Value,
                                Weight = recipeProductDetail.Quantity * orderDetailItem.Weight,
                            });
                        else
                        {
                            throw new Exception("ProductDetail_Recipe");
                        }
                    }
                }
                else
                {
                    listTmp.Add(orderDetailItem);
                }
            }

            var productWeight = listTmp.GroupBy(m => m.ProductID).Select(m => new OrderDetailProductItem()
            {
                ProductId = m.Key ?? 0,
                Weight = m.Sum(n => n.Weight ?? 0)
            });


            var result = new OrderGabModel();

            foreach (var categoryId in categoriesId)
            {
                var category = _categoryDa.GetItemById(categoryId);
                var categoryIds = _cateRecipeDa.GetCategoryChild(categoryId);
                var list = _cateRecipeDa.GetProduct(categoryIds);
                foreach (var orderDetailProductItem in list)
                {
                    var product = productWeight.FirstOrDefault(m => m.ProductId == orderDetailProductItem.ProductId);
                    orderDetailProductItem.Weight = product?.Weight ?? 0;
                }

                result = new OrderGabModel()
                {
                    ID = categoryId,
                    Name = category.Name,
                    Products = list.Where(m => m.PriceUnit > 0).ToList(),
                };
            }

            return result;
        }
        private OrderGabModel GetProducts2(decimal todayCode)
        {
            var categoriesId = new int[] { 3 };

            var _cateRecipeDa = new CateRecipeDA();

            //get product customer order

            var listTmp = OrderDetailItems(todayCode, 3);

            var productWeight = listTmp.GroupBy(m => m.ProductID).Select(m => new OrderDetailProductItem()
            {
                ProductId = m.Key ?? 0,
                Weight = m.Sum(n => n.Weight ?? 0)
            });


            var result = new OrderGabModel();

            foreach (var categoryId in categoriesId)
            {
                var category = _categoryDa.GetItemById(categoryId);
                var categoryIds = _cateRecipeDa.GetCategoryChild(categoryId);
                var list = _cateRecipeDa.GetProduct(categoryIds);
                foreach (var orderDetailProductItem in list)
                {
                    var product = productWeight.FirstOrDefault(m => m.ProductId == orderDetailProductItem.ProductId);
                    orderDetailProductItem.Weight = product?.Weight ?? 0;
                }

                result = new OrderGabModel()
                {
                    ID = categoryId,
                    Name = category.Name,
                    Products = list.Where(m => m.PriceUnit > 0).ToList(),
                };
            }

          

            return result;
        }

        private List<OrderDetailItem> OrderDetailItems(decimal todayCode, int categoryId)
        {
            var products = _contactOrderDa.GetProductDetail(AgencyId, todayCode, categoryId);
            var listTmp = new List<OrderDetailItem>();

            foreach (var orderDetailItem in products)
            {
                var productRecipe = _productDetailRecipeDa.GetProductRecipe(orderDetailItem.ProductID ?? 0);
                if (productRecipe != null)
                {
                    var productsRecipe = _productDetailRecipeDa.GetRecipeDetails(productRecipe.ID);
                    foreach (var recipeProductDetail in productsRecipe)
                    {
                        if (recipeProductDetail.DetailID != null)
                            listTmp.Add(new OrderDetailItem()
                            {
                                ProductID = recipeProductDetail.DetailID.Value,
                                Weight = orderDetailItem.Weight,
                                Quantity = recipeProductDetail.Quantity
                            });
                        else
                        {
                            throw new Exception("ProductDetail_Recipe");
                        }
                    }
                }
                else
                {
                    listTmp.Add(orderDetailItem);
                }
            }

            return listTmp;
        }

        private List<OrderDetailProductItem> GetProductRecipe(decimal todayCode, int categoryId)
        {
            var _cateRecipeDa = new CateRecipeDA();
            var categoryIds = _cateRecipeDa.GetCategoryChild(categoryId);
            var list = _cateRecipeDa.GetProduct(categoryIds);
            return list;
        }

        [HttpPost]
        public ActionResult DoOrder(decimal quantityExpect)
        {
            var todayCode = DateTime.Today.AddDays(1).TotalSeconds();
            var orderdetail = OrderDetailItems(todayCode, 3);

            var result = GetProducts2(todayCode);

            var model = new StorageWarehousing
            {
                Status = (int)StatusWarehouse.New,

                DateCreated = DateTime.Now.TotalSeconds(),
                IsDelete = false,
                DateRecive = todayCode,
                DN_RequestWare = new List<DN_RequestWare>(),
                AgencyId = AgencyId
            };
            var category = _categoryDa.GetItemById(3);

            var requestWare = new DN_RequestWare()
            {
                GID = Guid.NewGuid(),
                CateID = 3,
                Quantity = quantityExpect,
                QuantityActive = quantityExpect,
                Price = category.Price,
                CostPrice = category.CostPrice,
                //TotalPrice = item.TotalPrice,
                Today = todayCode,
                IsDelete = false
            };

            //if (result.Products != null)
            foreach (var detail in orderdetail)
            {
                requestWare.DN_RequestWareDetail.Add(new DN_RequestWareDetail()
                {
                    RequestWareId = Guid.NewGuid(),
                    ProductId = detail.ProductID ?? 0,
                    Quantity = 1,
                    Weight = detail.Weight
                });
            }
            model.DN_RequestWare.Add(requestWare);

            //product chua ban
            var productWeight = orderdetail.GroupBy(m => m.ProductID).Select(m => new OrderDetailProductItem()
            {
                ProductId = m.Key ?? 0,
                Weight = m.Sum(n => n.Weight ?? 0)
            });


            var productRecipe = GetProductRecipe(todayCode, 3);
            foreach (var orderDetailProductItem in productRecipe)
            {
                var orderproduct = productWeight.FirstOrDefault(m => m.ProductId == orderDetailProductItem.ProductId);
                if (orderproduct == null || orderproduct.Weight <= orderDetailProductItem.WeightRecipe * quantityExpect)
                {
                    requestWare.DN_RequestWareDetail.Add(new DN_RequestWareDetail()
                    {
                        RequestWareId = Guid.NewGuid(),
                        ProductId = orderDetailProductItem.ProductId,
                        Quantity = 1,
                        Weight = orderproduct == null ? orderDetailProductItem.WeightRecipe : orderDetailProductItem.WeightRecipe * quantityExpect - orderproduct.Weight
                    });
                }
            }
            //  return Json(new { orderdetail, productRecipe });
            _da.Add(model);
            _da.Save();
            return Json(model.ID);
        }
    }
}
