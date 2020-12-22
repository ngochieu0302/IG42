using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.DA.DA.Logistics;
using FDI.DA.DA.StorageWarehouse;
using FDI.Simple;
using FDI.Simple.Logistics;
using FDI.Simple.StorageWarehouse;
using FDI.Utils;
using ConvertDate = FDI.CORE.ConvertDate;

namespace FDI.MvcAPI.Controllers.StorageWarehouse
{
    public class ProduceController : BaseApiAuthController
    {
        private readonly ProduceDa _produceDa = new ProduceDa();
        private readonly OrderCarProductDetailDA _carProductDetailDa = new OrderCarProductDetailDA();

        private readonly CateRecipeDA _cateRecipeDa = new CateRecipeDA();
        private readonly ProduceProduceCatogoryDA _produceProduceCatogoryDa = new ProduceProduceCatogoryDA();
        private readonly ShopProductValueDA _shopProductValueDa = new ShopProductValueDA();
        private readonly CateValueDA _cateValueDa = new CateValueDA();
        private readonly ShopProductDetailDA _shopProductDetailDa = new ShopProductDetailDA();
        private readonly ImportProductDA _importProductDa = new ImportProductDA();
        private readonly ProductValueDA _productValueDa = new ProductValueDA();

        //
        // GET: /Produce/

        [HttpPost]
        public ActionResult GetById(int id)
        {
            return Json(_produceDa.GetItemById(id));
        }
        [HttpPost]
        public ActionResult GetProduceDetail(string code)
        {

            return Json(_produceDa.GetProduceDetail(code));
        }

        [HttpPost]
        public ActionResult GetProductDetail(string code)
        {
            var produce = _produceDa.GetProduceDetailByIdLog(code);
            if (produce == null)
            {
                return Json(new JsonMessage(true, "Mã không tồn tại"));
            }

            var result = new BaseResponse<List<OrderDetailProductItem>> { Data = _produceDa.GetDetail(produce.ID).ToList() };

            return Json(result);
        }


        //nhập con lợn
        [HttpPost]
        public ActionResult Insert(int produceId, string[] codes)
        {
            var produce = _produceDa.GetById(produceId);
            //produce.ProduceProductPrepares.Clear();
            foreach (var code in codes)
            {
                if (!_carProductDetailDa.CheckEsixt(code))
                {
                    return Json(new JsonMessage()
                    {
                        Erros = true,
                        Message = $"Mã {code} không tồn tại"
                    });
                }

                var item = new ProduceProductPrepare()
                {
                    ProductId = produce.ProductId,
                    Code = code,
                    UserId = Userid.Value,
                    DateCreate = ConvertDate.TotalSeconds(DateTime.Now),
                    ProduceId = produceId,
                    Isdelete = false
                };
                produce.ProduceProductPrepares.Add(item);
            }

            produce.Status = (int) ProduceStatus.GetProduct;

            _produceDa.Save();
            return Json(new JsonMessage());
        }

        //cân tảng sản phẩm
        [HttpPost]
        public ActionResult InsertProductCategory(List<ProduceCatogoryItem> model)
        {
            var groups = model.GroupBy(m => new { m.IdLog, m.ProductId });

            if (groups.Count() > 1)
            {
                return Json(new JsonMessage() { Erros = true, Message = "Chỉ nhập 1 mã sản phẩm" });
            }

            if (model.Count == 0)
            {
                return Json(new JsonMessage() { Erros = true });
            }

            var productvalue = _productValueDa.GetByCode(model[0].IdLog);


            if (productvalue != null && productvalue.ProductId != model[0].ProductId)
            {
                var temp = _shopProductDetailDa.GetItemById(productvalue.ProductId);
                return Json(new JsonMessage() { Erros = true, Message = $"Id thùng đã được nhập cho sản phẩm: {temp.Name}" });
            }

            var product = _produceDa.GetProduceDetail(model[0].ProductOriginalCode);

            var catevalue = _cateValueDa.GetByCode(model[0].ProductOriginalCode);


            foreach (var produceCatogory in model)
            {
                var productdetial = _shopProductDetailDa.GetItemById(produceCatogory.ProductId);

                var item = new Product_Value()
                {
                    Barcode = produceCatogory.Code,
                    ProductID = produceCatogory.ProductId,
                    UnitID = (int)UnitID.KG,
                    IsDelete = false,
                    Value = produceCatogory.Weight,
                    Quantity = 1,
                    DateCreated = DateTime.Now.TotalSeconds(),
                    DateImport = DateTime.Now.TotalSeconds(),
                    PriceCost = productdetial.Price,
                    PriceNew = productdetial.Price * produceCatogory.Weight,
                    CateValueID = catevalue.ID,
                    IdLog = produceCatogory.IdLog,
                    QuantityOut = 0,
                    ProduceId = product.ID
                };
                _productValueDa.Add(item);
            }

            _productValueDa.Save();

            return Json(new JsonMessage());
        }

        //cân chi tiet san pham
        [HttpPost]
        public ActionResult InsertProductDetail(List<ImportProductItem> requests)
        {
            if (requests.Count == 0)
            {
                return Json(new JsonMessage(true, ""));
            }

            var productvalue = _productValueDa.GetByCode(requests[0].Code);

            foreach (var data in requests.Select(importProductItem => new DN_ImportProduct()
            {
                GID = Guid.NewGuid(),
                Quantity = importProductItem.Quantity,
                IsDelete = false,
                Price = importProductItem.Price,
                DateEnd = importProductItem.DateEnd,
                Value = importProductItem.Value,
                PriceNew = importProductItem.PriceNew,
                BarCode = importProductItem.BarCode,
                ProductValueID = productvalue.ID,
                UserCreated = Userid.Value,
                CreateDate = DateTime.Now.TotalSeconds(),
                QuantityOut = 0,
            }))
            {
                _importProductDa.Add(data);
            }

            _importProductDa.Save();
            return Json(new JsonMessage());
        }
    }
}
