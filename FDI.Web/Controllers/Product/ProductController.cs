using System;
using System.Linq;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class ProductController : BaseController
    {
        readonly ProductAPI _api = new ProductAPI();
        readonly ProductSizeAPI _productSizeApi = new ProductSizeAPI();
        readonly SystemColorAPI _systemColorApi = new SystemColorAPI();
        readonly DNUnitAPI _dnUnit = new DNUnitAPI();
        readonly ShopProductDetailAPI _productDetailApi = new ShopProductDetailAPI();
        private readonly ProductValueAPI _valueApi = new ProductValueAPI();
        readonly ProductTypeAPI _productTypeApi = new ProductTypeAPI();

        public ActionResult Index()
        {
            var model = new ModelProductItem { PageId = Convert.ToInt32(Request.QueryString["productDetailId"]) };
            return View(model);
        }

        public ActionResult ListItems()
        {
            return View(_api.ListItems(UserItem.AgencyID, Request.Url.Query));
        }

        public ActionResult AjaxView()
        {
            var model = _api.GetProductItem(ArrId.FirstOrDefault());
            return View(model);
        }
        public ActionResult ProductAnalysis()
        {
            return View();
        }
        public ActionResult AjaxForm()
        {
            var productId = Convert.ToInt32(Request.QueryString["productId"]);
            var objProduct = _productDetailApi.GetItemById(UserItem.AgencyID, productId);
            var model = new ProductItem();
            if (DoAction == ActionType.Edit)
                model = _api.GetProductItem(ArrId.FirstOrDefault());
            model.ColorID = model.ColorID.HasValue ? model.ColorID : 0;
            if (objProduct != null)
            {
                model.Name = objProduct.Name;
                model.PriceNew = objProduct.Price ?? 0;
                model.ProductDetailID = objProduct.ID;
            }
            ViewBag.ColorID = _systemColorApi.GetAll(UserItem.AgencyID);
            if (model.UnitID != null)
            {
                ViewBag.SizeID = _productSizeApi.GetAllByUnitID(UserItem.AgencyID, model.UnitID ?? 0);
            }
            else
            {
                ViewBag.SizeID = _productSizeApi.GetAll(UserItem.AgencyID);
            }
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            ViewBag.Unit = _dnUnit.GetListUnit(UserItem.AgencyID);
            ViewBag.type = _productTypeApi.GetAll();
            ViewBag.Color = _productTypeApi.GetAll();
            ViewBag.CreateBy = UserItem.UserName;
            ViewBag.AgencyID = UserItem.AgencyID;
            return View(model);
        }
        public ActionResult AjaxCost()
        {
            var model = _api.GetCostProduceItem(ArrId.FirstOrDefault());
            return View(model);
        }

        public ActionResult AjaxRecipe()
        {
            var model = _api.GetProductItem(ArrId.FirstOrDefault());
            return View(model);
        }

        //[HttpPost]
        public ActionResult AddAttribute()
        {
            var msg = new JsonMessage { Erros = false, Message = "Cập nhật thuộc tính thành công" };
            var url = Request.Form.ToString();
            try
            {
                _api.AddAttribute(url);
            }
            catch (Exception)
            {
                msg.Erros = true;
                msg.Message = "Cập nhật thất bại vui lòng kiểm tra lại.";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var url = Request.Form.ToString();
            var lst1 = string.Join(",", ArrId);
            switch (DoAction)
            {
                case ActionType.Add:
                    msg = _api.Add(url, CodeLogin());
                    break;
                case ActionType.Order:
                    msg = _api.Coppy(UserItem.AgencyID, ArrId.FirstOrDefault());
                    break;
                case ActionType.Edit:
                    msg = _api.Update(url, CodeLogin());
                    break;
                case ActionType.View:
                    msg = _api.AddRecipe(ArrId.FirstOrDefault(),CodeLogin());
                    break;
                case ActionType.Delete:
                    msg = _api.Delete(lst1);
                    break;
                case ActionType.Show:
                    msg = _api.Show(lst1);
                    break;
                case ActionType.Hide:
                    msg = _api.Hide(lst1);
                    break;
                default:
                    msg.Message = "Bạn không được phần quyền cho chức năng này.";
                    msg.Erros = true;
                    break;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public string CheckByCode(string CodeSku, int pId)
        {
            var result = _api.CheckExitCode(CodeSku, pId, UserItem.AgencyID);
            return result == 1 ? "false" : "true";
        }

        public ActionResult Auto()
        {
            var query = Request["query"];
            var type = Request["type"] ?? "3";
            var ltsResults = _api.GetListAuto(query, 10, UserItem.AgencyID, int.Parse(type));
            var resulValues = new AutoCompleteProduct
            {
                query = query,
                suggestions = ltsResults
            };
            return Json(resulValues, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutoValue()
        {
            var query = Request["query"];
            var ltsResults = _valueApi.GetListAuto(query, 10, UserItem.AgencyID);
            var resulValues = new AutoCompleteProduct
            {
                query = query,
                suggestions = ltsResults,
            };
            return Json(resulValues, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AutoProduct()
        {
            var query = Request["query"];
            var ltsResults = _productDetailApi.GetListAuto(query, 10, UserItem.AgencyID);
            var resulValues = new AutoCompleteProduct
            {
                query = query,
                suggestions = ltsResults,
            };
            return Json(resulValues, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AutoProductCate(int cateId)
        {
            var query = Request["query"];
            var ltsResults = _productDetailApi.GetListAutoCate(query, 10, UserItem.AgencyID,cateId);
            var resulValues = new AutoCompleteProduct
            {
                query = query,
                suggestions = ltsResults,
            };
            return Json(resulValues, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AutoCate(int cateId)
        {
            var query = Request["query"];
            var ltsResults = _productDetailApi.GetListCateAuto(query, 10, UserItem.AgencyID,cateId);
            var resulValues = new AutoCompleteProduct
            {
                query = query,
                suggestions = ltsResults,
            };
            return Json(resulValues, JsonRequestBehavior.AllowGet);
        }
    }
}
