using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;
namespace FDI.GetAPI
{
    public class ShopProductDetailAPI : BaseAPI
    {
        private readonly string _url = Domain;
        public ModelShopProductDetailItem ListItems(int agencyId, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}ShopProductDetail/ListItems{1}&key={2}&agencyId={3}", _url, url, Keyapi, agencyId);
            return GetObjJson<ModelShopProductDetailItem>(urlJson);
        }

        public ModelProductExportItem GetListOrderDetail(int agencyId, string date)
        {
            var urlJson = string.Format("{0}ShopProductDetail/GetListOrderDetail?key={1}&agencyId={2}&date={3}", _url, Keyapi, agencyId, date);
            return GetObjJson<ModelProductExportItem>(urlJson);
        }
        public ModelProductExportItem GetOrderDetailExport(int agencyId, string date)
        {
            var urlJson = string.Format("{0}ShopProductDetail/GetOrderDetailExport?key={1}&agencyId={2}&date={3}", _url, Keyapi, agencyId, date);
            return GetObjJson<ModelProductExportItem>(urlJson);
        }

        public ModuleShopProductValueItem GetListValueDetail(int agencyId, string date)
        {
            var urlJson = string.Format("{0}ShopProductDetail/GetListValueDetail?key={1}&agencyId={2}&date={3}", _url, Keyapi, agencyId, date);
            return GetObjJson<ModuleShopProductValueItem>(urlJson);
        }

        public ModuleShopProductValueItem GetValueDetailExport(int agencyId, string date)
        {
            var urlJson = string.Format("{0}ShopProductDetail/GetValueDetailExport?key={1}&agencyId={2}&date={3}", _url, Keyapi, agencyId, date);
            return GetObjJson<ModuleShopProductValueItem>(urlJson);
        }

        public List<ShopProductDetailItem> GetAll(int agencyId)
        {
            var urlJson = string.Format("{0}ShopProductDetail/GetAll?key={1}&agencyId={2}", _url, Keyapi, agencyId);
            return GetObjJson<List<ShopProductDetailItem>>(urlJson);
        }

        public ShopProductDetailItem GetItemById(int agencyId, int id = 0)
        {
            var urlJson = string.Format("{0}ShopProductDetail/GetItemById?key={1}&agencyId={2}&id={3}", _url, Keyapi, agencyId, id);
            return GetObjJson<ShopProductDetailItem>(urlJson);
        }
        public ProductDetailRecipeItem GetRecipeItemByDetailId(int id)
        {
            var urlJson = string.Format("{0}ShopProductDetail/GetRecipeItemByDetailId?key={1}&id={2}", _url, Keyapi, id);
            return GetObjJson<ProductDetailRecipeItem>(urlJson);
        }
        public List<ShopProductDetailItem> GetListByArrId(int agencyId, string lstId)
        {
            var urlJson = string.Format("{0}ShopProductDetail/GetListByArrId?key={1}&agencyId={2}&lstId={3}", _url, Keyapi, agencyId, lstId);
            return GetObjJson<List<ShopProductDetailItem>>(urlJson);
        }

        public int CheckExitCode(int id, int agencyId)
        {
            var urlJson = string.Format("{0}ShopProductDetail/CheckExitCode?key={1}&agencyId={2}&id={3}", Domain, Keyapi, agencyId, id);
            return GetObjJson<int>(urlJson);
        }

        public JsonMessage Add(int agencyId, string url, string code,bool isadmin)
        {
            var urlJson = string.Format("{0}ShopProductDetail/Add?key={1}&agencyId={2}&codelogin={3}&isadmin={4}&{5}", _url, Keyapi, agencyId, code, isadmin, url);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public async Task<BaseResponse<bool>> AddApi(Shop_Product_Detail obj)
        {
            var urlJson = string.Format("{0}ShopProductDetail/Add",_url);
            return await PostDataAsync<BaseResponse<bool>>(urlJson,obj);
        }
        public async Task<BaseResponse<bool>> UpdateApi(Shop_Product_Detail obj,int id)
        {
            var urlJson = string.Format("{0}ShopProductDetail/Update", _url);
            return await PostDataAsync<BaseResponse<bool>>(urlJson, obj,id);
        }
        public JsonMessage Update(int agencyId, string url, string code,bool isadmin)
        {
            var urlJson = string.Format("{0}ShopProductDetail/Update?key={1}&agencyId={2}&codelogin={3}&isadmin={4}&{5}", _url, Keyapi, agencyId, code, isadmin, url);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Active(string url, string code,Guid? userId)
        {
            var urlJson = string.Format("{0}ShopProductDetail/Active?key={1}&codelogin={2}&userId={3}&{4}", _url, Keyapi, code, userId, url);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage NotActive(string url, string code, Guid? userId)
        {
            var urlJson = string.Format("{0}ShopProductDetail/NotActive?key={1}&codelogin={2}&userId={3}&{4}", _url, Keyapi, code, userId, url);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Addproduct(int agencyId, string url)
        {
            var urlJson = string.Format("{0}ShopProductDetail/Addproduct?key={1}&agencyId={2}&{3}", _url, Keyapi, agencyId, url);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Coppy(int agencyId, int ItemID)
        {
            var urlJson = string.Format("{0}ShopProductDetail/Coppy?key={1}&agencyId={2}&ItemID={3}", _url, Keyapi, agencyId, ItemID);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public List<SuggestionsProduct> GetListAuto(string keword, int showLimit, int agencyId)
        {
            var urlJson = string.Format("{0}ShopProductDetail/GetListAuto?key={1}&keword={2}&showLimit={3}&agencyId={4}", _url, Keyapi, keword, showLimit, agencyId);
            return GetObjJson<List<SuggestionsProduct>>(urlJson);
        }
        public List<SuggestionsProduct> GetListAutoCate(string keword, int showLimit, int agencyId,int CateId)
        {
            var urlJson = string.Format("{0}ShopProductDetail/GetListAutoCate?key={1}&keword={2}&showLimit={3}&agencyId={4}&cateId={5}", _url, Keyapi, keword, showLimit, agencyId,CateId);
            return GetObjJson<List<SuggestionsProduct>>(urlJson);
        }
        public List<SuggestionsProduct> GetListCateAuto(string keword, int showLimit, int agencyId, int CateId)
        {
            var urlJson = string.Format("{0}ShopProductDetail/GetListCateAuto?key={1}&keword={2}&showLimit={3}&agencyId={4}&cateId={5}", _url, Keyapi, keword, showLimit, agencyId, CateId);
            return GetObjJson<List<SuggestionsProduct>>(urlJson);
        }
        public JsonMessage Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}ShopProductDetail/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }

        public JsonMessage ShowHide(string lstArrId, bool showhide)
        {
            var urlJson = string.Format("{0}ShopProductDetail/ShowHide?key={1}&lstArrId={2}&showhide={3}", Domain, Keyapi, lstArrId, showhide);
            return GetObjJson<JsonMessage>(urlJson);
        }
    }
}
