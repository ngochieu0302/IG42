using FDI.DA.DA.Supplier;
using FDI.Simple.Supplier;
using FDI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA;

namespace FDI.MvcAPI.Controllers.Supplier
{
    public class SupplierAmountProductController : BaseApiAuthController
    {
        //
        // GET: /SupplierAmountProduct/
        private readonly SupplierAmountProductDA _da = new SupplierAmountProductDA();
        private readonly CateRecipeDA _cateRecipeDa = new CateRecipeDA("#");


        public ActionResult ListItems()
        {
            var obj = new SupplierAmountProductResponse() { ListItem = _da.GetListSimpleByRequest(Request), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add(SupplierAmountProductItem request)
        {
            var model = new Base.SupplierAmountProduct()
            {
                SupplierId = request.SupplierId,
                ProductID = request.ProductID,
                PublicationDate = request.PublicationDate,
                ExpireDate = request.ExpireDate,
                IsAlwayExist = request.IsAlwayExist,
                AmountEstimate = request.AmountEstimate,
                AmountPayed = request.AmountPayed,
                CallDate = request.CallDate,
                Note = request.Note,
                IsDelete = false,
                CreatedDate = DateTime.Now.TotalSeconds()
            };
            
            var temp = _cateRecipeDa.GetItemByCateIdUser(request.ProductID ?? 0);
            if (temp != null)
            {
                var lstProduct = temp.LstCategoryRecipeItems.Select(item => new Shop_Product_Comingsoon
                    {
                        ProductID = item.ProductId,
                        DateEx = request.PublicationDate,
                        Quantity = request.AmountEstimate * item.Quantity,
                        Price = item.PriceProduct,
                        TotalPrice = request.AmountEstimate * item.Quantity * item.PriceProduct,
                        QuantityOut = 0,
                    })
                    .ToList();

                foreach (var itema in temp.LstMappingCategoryRecipeItems)
                {
                    temp = _cateRecipeDa.GetItemByCateIdUser(itema.CategoryID ?? 0);
                    if (temp != null)
                    {
                        lstProduct.AddRange(temp.LstCategoryRecipeItems.Select(item1 => new Shop_Product_Comingsoon
                        {
                            ProductID = item1.ProductId,
                            DateEx = request.PublicationDate,
                            Quantity = request.AmountEstimate * item1.Quantity,
                            Price = item1.PriceProduct,
                            TotalPrice = request.AmountEstimate * item1.Quantity * item1.PriceProduct,
                            QuantityOut = 0,
                        }));
                    }
                }
                model.Shop_Product_Comingsoon = lstProduct;
                _da.Add(model);
                _da.Save();
            }
            return Json(new JsonMessage() { Erros = false }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update(SupplierAmountProductItem request)
        {
            var model = _da.GetById(request.ID);
            model.AmountEstimate = request.AmountEstimate;
            model.AmountPayed = request.AmountPayed;
            model.CallDate = request.CallDate;
            model.IsAlwayExist = request.IsAlwayExist;
            model.ExpireDate = request.ExpireDate;
            model.Note = request.Note;
            model.ProductID = request.ProductID;
            model.SupplierId = request.SupplierId;
            model.UserActiveId = request.UserActiveId;
            model.PublicationDate = request.PublicationDate;
            model.Shop_Product_Comingsoon.Clear();
            var temp = _cateRecipeDa.GetItemByCateIdUser(request.ProductID ?? 0);
            if (temp != null)
            {
                var lstProduct = temp.LstCategoryRecipeItems.Select(item => new Shop_Product_Comingsoon
                    {
                        ProductID = item.ProductId,
                        DateEx = request.PublicationDate,
                        Quantity = request.AmountEstimate * item.Quantity,
                        Price = item.PriceProduct,
                        TotalPrice = request.AmountEstimate * item.Quantity * item.PriceProduct,
                        QuantityOut = 0,
                    })
                    .ToList();

                foreach (var itema in temp.LstMappingCategoryRecipeItems)
                {
                    temp = _cateRecipeDa.GetItemByCateIdUser(itema.CategoryID ?? 0);
                    if (temp != null)
                    {
                        lstProduct.AddRange(temp.LstCategoryRecipeItems.Select(item1 => new Shop_Product_Comingsoon
                        {
                            ProductID = item1.ProductId,
                            DateEx = request.PublicationDate,
                            Quantity = request.AmountEstimate * item1.Quantity,
                            Price = item1.PriceProduct,
                            TotalPrice = request.AmountEstimate * item1.Quantity * item1.PriceProduct,
                            QuantityOut = 0,
                        }));
                    }
                }
                model.Shop_Product_Comingsoon = lstProduct;
                _da.Save();
            }

            return Json(new JsonMessage() { Erros = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            var model = _da.GetById(id);
            model.IsDelete = true;
            _da.Save();
            return Json(new JsonMessage() { Erros = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetById(int id)
        {
            return Json(_da.GetItemById(id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSupplierByCategoryId(int id)
        {
            return Json(_da.GetSupplierByCategoryId(id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAmount(int productId, decimal todayCode)
        {
            return Json(_da.GetAmount(productId, todayCode));
        }
    }
}
