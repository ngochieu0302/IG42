using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.Base;
using FDI.DA;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class ProductDetailController : BaseController
    {
        readonly ShopProductDetailAPI _api;
        readonly SystemColorAPI _systemColorApi;
        readonly DNUnitAPI _dnUnitApi;
        readonly System_ColorDA _colorDa = new System_ColorDA();
        readonly ProductTypeDA _productTypeDa = new ProductTypeDA();
        readonly ProductAPI _productApi = new ProductAPI();
        readonly ProductSizeAPI _productSizeApi = new ProductSizeAPI();
        readonly ProductTypeAPI _productTypeApi = new ProductTypeAPI();
        private readonly ProductValueAPI _valueApi = new ProductValueAPI();
        readonly CategoryAPI _categoryApi = new CategoryAPI();
        public ProductDetailController()
        {
            _api = new ShopProductDetailAPI();
            _dnUnitApi = new DNUnitAPI();
            _systemColorApi = new SystemColorAPI();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            return View(_api.ListItems(UserItem.AgencyID, Request.Url.Query));
        }

        public ActionResult AjaxView()
        {
            var shopProductDetail = _api.GetItemById(UserItem.AgencyID, ArrId.FirstOrDefault());
            ViewData.Model = shopProductDetail;
            return View();
        }

        public ActionResult AjaxForm()
        {
            var model = new ShopProductDetailItem();
            if (DoAction == ActionType.Edit)
            {
                model = _api.GetItemById(UserItem.AgencyID, ArrId.FirstOrDefault());
            }
            ViewBag.UnitID = _dnUnitApi.GetListUnit(UserItem.AgencyID).OrderBy(x => x.Name);
            ViewBag.category = _categoryApi.GetChildByParentId(true);
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View(model);
        }
        public ActionResult AjaxFormAdd()
        {
            var productId = Convert.ToInt32(Request.QueryString["productId"]);
            var model = _api.GetItemById(UserItem.AgencyID, productId);
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            ViewBag.listcolor = _colorDa.GetAll(2010);
            ViewBag.listSize = _productSizeApi.GetAllByUnitID(UserItem.AgencyID, model.UnitID);
            ViewBag.listType = _productTypeDa.GetAll();
            return View(model);
        }
        public ActionResult GetListSize()
        {
            var model = _productSizeApi.GetAll(UserItem.AgencyID);
            return View(model);
        }
        public ActionResult GetListType()
        {
            var model = _productTypeApi.GetAll();
            return View(model);
        }
        public ActionResult GetListColor()
        {
            var model = _systemColorApi.GetAll(UserItem.AgencyID);
            return View(model);
        }
        public ActionResult ListProduct()
        {
            var shopProductDetail = _productApi.GetListByProductDetailsId(UserItem.AgencyID, ArrId.FirstOrDefault());
            ViewData.Model = shopProductDetail;
            return View();
        }
        public ActionResult AjaxRecipe()
        {

            var model = new ProductDetailRecipeItem();
            if (DoAction == ActionType.NotActive)
            {
                model = _api.GetRecipeItemByDetailId(ArrId.FirstOrDefault());
            }
            ViewBag.id = ArrId.FirstOrDefault();
            ViewBag.Action = DoAction;
            return View(model);
        }
        public ActionResult Auto()
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

        public string CheckByCode(string Code, int pId)
        {
            var result = _productApi.CheckExitCode(Code, pId, UserItem.AgencyID);
            return result == 1 ? "false" : "true";
        }
        public string CheckByCodeDetail(string Code, int pId)
        {
            var result = _api.CheckExitCode(pId, UserItem.AgencyID);
            return result == 1 ? "false" : "true";
        }
        private readonly ShopProductDetailDA _da = new ShopProductDetailDA();
        [HttpPost]
        [ValidateInput(false)]
        public async  Task<ActionResult> Actions()
        {
            var msg = new JsonMessage();
            var url = Request.Form.ToString();
            url = HttpUtility.UrlDecode(url);
            var model = new Shop_Product_Detail();
            var lstArrId = "";
            var lstPicture = Request["Value_ImagesProducts"];

            switch (DoAction)
            {
                case ActionType.Add:
                    UpdateModel(model);
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
                    if (!string.IsNullOrEmpty(lstPicture))
                    {
                        model.Gallery_Picture2 = _da.GetListPictureByArrId(lstPicture);
                    }
                    _da.Add(model);
                    _da.Save();
                    break;
                case ActionType.View:
                    msg = _api.Coppy(UserItem.AgencyID, ArrId.FirstOrDefault());
                    break;
                case ActionType.Edit:
                    model = _da.GetById(ArrId.FirstOrDefault());
                    UpdateModel(model);
                    if (!IsAdmin)
                    {
                        model.IsShow = false;
                    }
                    model.Gallery_Picture2.Clear();
                    if (!string.IsNullOrEmpty(lstPicture))
                    {
                        model.Gallery_Picture2 = _da.GetListPictureByArrId(lstPicture);
                    }
                    _da.Save();
                    break;
                case ActionType.Active:
                    msg = _api.Active(url, CodeLogin(), UserItem.UserId);
                    break;
                case ActionType.NotActive:
                    msg = _api.NotActive(url, CodeLogin(), UserItem.UserId);
                    break;
                case ActionType.UserModule:
                    msg = _api.Addproduct(UserItem.AgencyID, url);
                    break;
                case ActionType.Show:
                    lstArrId = string.Join(",", ArrId);
                    msg = _api.ShowHide(lstArrId, true);
                    break;

                case ActionType.Hide:
                    lstArrId = string.Join(",", ArrId);
                    msg = _api.ShowHide(lstArrId, false);
                    break;
                case ActionType.Delete:
                    lstArrId = string.Join(",", ArrId);
                    msg = _api.Delete(lstArrId);
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
