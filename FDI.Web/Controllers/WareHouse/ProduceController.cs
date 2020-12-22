using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.DA.DA.StorageWarehouse;
using FDI.GetAPI.StorageWarehouse;
using FDI.Simple;
using FDI.Simple.StorageWarehouse;
using FDI.Utils;
using ConvertDate = FDI.CORE.ConvertDate;

namespace FDI.Web.Controllers.WareHouse
{
    public class ProduceController : BaseController
    {
        private readonly RequestWareAPI _requestWareApi = new RequestWareAPI();
        private readonly StorageWarehouseAPI _storageWarehouseApi = new StorageWarehouseAPI();
        private readonly ProduceDa _produceDa = new ProduceDa();
        private readonly RequestWareDA _requestWareDa = new RequestWareDA();
        private readonly ProductDetailRecipeDA _productDetailRecipeDa = new ProductDetailRecipeDA();
        private readonly StorageWareHouseDA _storageWareHouseDa = new StorageWareHouseDA();

        //
        // GET: /Produce/

        public ActionResult Index(decimal? todayCode)
        {
            if (todayCode == null)
            {
                return RedirectToAction("Index", new { todayCode = ConvertDate.TotalSeconds(DateTime.Now.AddDays(1).Date) });
            }
            return View(todayCode.Value);
        }

        public async Task<ActionResult> ListItems(decimal todayCode)
        {
            var mode = new ProduceModel()
            {
                //get product order confirmed
                RequestWareItems = _requestWareDa.GetSummary(todayCode),
                // get product_detail from order
                CategorysDetail = _storageWareHouseDa.GetSummaryDetailConfirmed(todayCode).ToList()
            };
            //get detail

            return PartialView(mode);
        }

        public ActionResult AjaxForm(decimal todayCode, decimal Quantity)
        {
            ViewBag.Action = DoAction;
            var productId = ArrId.FirstOrDefault();
            ViewBag.ProductId = productId;
            ViewBag.Quantity = Quantity;
            ViewBag.todayCode = todayCode;

            var lst = GetProducts(todayCode, productId);

            return View(lst);
        }

        List<ProduceProductDetailItem> GetProducts(decimal todayCode, int productId)
        {
            var detailitems = _requestWareDa.GetSummary(todayCode).Where(m => m.CateID == productId);
            var lst = new List<ProduceProductDetailItem>();

            foreach (var item in detailitems)
            {
                var productRecipe = _productDetailRecipeDa.GetProductRecipe(item.ProductId);

                // co cong thuc
                if (productRecipe != null)
                {
                    var productsRecipe = _productDetailRecipeDa.GetRecipeDetails(productRecipe.ID);
                    foreach (var recipeProductDetail in productsRecipe)
                    {
                        if (recipeProductDetail.DetailID != null)
                            lst.Add(new ProduceProductDetailItem()
                            {
                                Quantity = recipeProductDetail.Quantity ?? 0,
                                ProductId = recipeProductDetail.DetailID.Value,
                                ProductName = recipeProductDetail.ProductName,
                                Weight = recipeProductDetail.Quantity ?? 0 * item.Quantity,
                                ProductParentId = productRecipe.ID
                            });
                        else
                        {
                            throw new Exception("ProductDetail_Recipe");
                        }
                    }
                }
                else
                {
                    lst.Add(new ProduceProductDetailItem()
                    {
                        Quantity = item.Quantity,
                        ProductId = item.CateID,
                        ProductName = item.ProductName,
                        SizeId = item.SizeId,
                        UnitName = item.UnitName,
                        Weight = item.Quantity * item.UnitValue
                    });
                }
            }
            return lst;
        }
        public ActionResult Actions()
        {
            var msg = new JsonMessage { Erros = false };
            var model = new ProduceItem();
            switch (DoAction)
            {
                case ActionType.Add:
                    UpdateModel(model);
                    var produce = new Produce()
                    {
                        Quantity = model.Quantity,
                        ProductId = model.ProductId,
                        ProduceProductDetails = new List<ProduceProductDetail>(),
                        UserId = UserItem.UserId,
                        DateCreate = DateTime.Now.TotalSeconds(),
                        Isdelete = false,
                        Status = (int)ProduceStatus.New,
                        DateProduce = model.DateProduce,
                        MapProduceRequestWares = new List<MapProduceRequestWare>()
                    };

                    var lst = GetProducts(model.ToDayCode, model.ProductId);

                    foreach (var produceProductDetailItem in lst)
                    {
                        produce.ProduceProductDetails.Add(new ProduceProductDetail()
                        {
                            ProductId = produceProductDetailItem.ProductId,
                            Quantity = produceProductDetailItem.Quantity,
                            Weight = produceProductDetailItem.Weight,
                            SizeId = produceProductDetailItem.SizeId,
                            ProductParentId = produceProductDetailItem.ProductParentId
                        });
                    }

                    _produceDa.Add(produce);

                    var dnRequests = _requestWareDa.GetWaittingProduceByTodayCode(model.ToDayCode, model.ProductId);
                    foreach (var dnRequestWare in dnRequests)
                    {
                        dnRequestWare.Status = (int)CORE.DNRequestStatus.Processing;
                        produce.MapProduceRequestWares.Add(new MapProduceRequestWare()
                        {
                            ID = Guid.NewGuid(),
                            RequestWareID = dnRequestWare.GID
                        });
                    }
                    _produceDa.Save();

                    // update status order detail is produce
                    _requestWareDa.Save();

                    break;
                default:
                    msg.Message = "Bạn chưa được phân quyền cho chức năng này.";
                    msg.Erros = true;
                    break;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
