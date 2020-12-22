using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class ProductAPI : BaseAPI
    {
        private readonly string _url = Domain;
        public List<ProductItem> GetListSimple(int agencyId)
        {
            var urlJson = string.Format("{0}Product/GetListSimple?key={1}&agencyId={2}", Domain, Keyapi, agencyId);
            return GetObjJson<List<ProductItem>>(urlJson);
        }
        public List<CategoryItem> GetList(int agencyid)
        {
            var urlJson = string.Format("{0}Product/GetList?key={1}&agencyId={2}", Domain, Keyapi, agencyid);
            return GetObjJson<List<CategoryItem>>(urlJson);
        }
        public ModelHomeSupplierItem GetModelHomeSupplierItem(int agencyid)
        {
            var urlJson = string.Format("{0}Product/GetModelHomeSupplierItem?key={1}&agencyId={2}", Domain, Keyapi, agencyid);
            return GetObjJson<ModelHomeSupplierItem>(urlJson);
        }
        public ModelProductItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Product/ListItems{1}&key={2}&agencyId={3}", _url, url, Keyapi, agencyid);
            return GetObjJson<ModelProductItem>(urlJson);
        }
        public ModelSimItem ListSimItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Product/ListSimItems{1}&key={2}&agencyId={3}", _url, url, Keyapi, agencyid);
            return GetObjJson<ModelSimItem>(urlJson);
        }
        public int CountProductSearch(string keyword, int agencyId, int hid, int sid)
        {
            var urlJson = string.Format("{0}Product/CountProductSearch?key={1}&keyword={2}&agencyId={3}&hid={4}&sid={5}", _url, Keyapi, keyword, agencyId, hid, sid);
            var key = string.Format("ProductCountProductSearch_{0}_{1}_{2}_{3}", keyword, agencyId, hid, sid);
            return GetCacheNow<int>(key, urlJson, ConfigCache.TimeExpire);
        }
        public List<SuggestionsProduct> GetListCommentAuto(int agencyid, string keword, int showLimit)
        {
            var urlJson = string.Format("{0}Product/GetListCommentAuto?key={1}&keword={2}&showLimit={3}&agencyId={4}", _url, Keyapi, keword, showLimit, agencyid);
            return GetObjJson<List<SuggestionsProduct>>(urlJson);
        }

        public List<SuggestionsProduct> GetListAutoFull(string keword, int showLimit, int agencyId, int type = 0)
        {
            var urlJson = string.Format("{0}Product/GetListAutoFull?key={1}&keword={2}&showLimit={3}&agencyId={4}&type={5}", _url, Keyapi, keword, showLimit, agencyId, type);
            return GetObjJson<List<SuggestionsProduct>>(urlJson);
        }
        
        public List<SuggestionsProduct> GetListAuto(string keword, int showLimit, int agencyId, int type = 0)
        {
            var urlJson = string.Format("{0}Product/GetListAuto?key={1}&keword={2}&showLimit={3}&agencyId={4}&type={5}", _url, Keyapi, keword, showLimit, agencyId, type);
            return GetObjJson<List<SuggestionsProduct>>(urlJson);
        }
        public List<SuggestionsProduct> GetListAutoOne(string keword, int showLimit, int agencyId, int type = 0)
        {
            var urlJson = string.Format("{0}Product/GetListAutoOne?key={1}&keword={2}&showLimit={3}&agencyId={4}&type={5}", _url, Keyapi, keword, showLimit, agencyId, type);
            return GetObjJson < List<SuggestionsProduct>> (urlJson);
        }
        public List<SuggestionsProduct> GetListAutoSim(string keword, int showLimit, int agencyId, int type = 0)
        {
            var urlJson = string.Format("{0}Product/GetListAutoSim?key={1}&keword={2}&showLimit={3}&agencyId={4}&type={5}", _url, Keyapi, keword, showLimit, agencyId, type);
            return GetObjJson<List<SuggestionsProduct>>(urlJson);
        }
        public List<SuggestionsProduct> GetListAutoComplete(string keword, int showLimit, int agencyId, int type = 0)
        {
            var urlJson = string.Format("{0}Product/GetListAutoComplete?key={1}&keword={2}&showLimit={3}&agencyId={4}&type={5}", _url, Keyapi, keword, showLimit, agencyId, type);
            return GetObjJson<List<SuggestionsProduct>>(urlJson);
        }

        public List<ShopGroupItem> GetListGroupId(int id)
        {
            var urlJson = string.Format("{0}Product/GetListGroupId?key={1}&id={2}", _url, Keyapi, id);
            return GetObjJson<List<ShopGroupItem>>(urlJson);
        }
        public List<ProductItem> GetListByAgency(int id)
        {
            var urlJson = string.Format("{0}Product/GetListByAgency?key={1}&id={2}", _url, Keyapi, id);
            return GetObjJson<List<ProductItem>>(urlJson);
        }
        public List<PacketItem> GetListByPacket(int id, int beddeskId)
        {
            var urlJson = string.Format("{0}Product/GetListByPacket?key={1}&id={2}&beddeskId={3}", _url, Keyapi, id, beddeskId);
            return GetObjJson<List<PacketItem>>(urlJson);
        }
        public ModelOrderGetItem ListProductByDeddeskId(int agencyid, int beddeskId)
        {
            var urlJson = string.Format("{0}Product/ListProductByDeddeskId?key={1}&agencyId={2}&beddeskId={3}", _url, Keyapi, agencyid, beddeskId);
            return GetObjJson<ModelOrderGetItem>(urlJson);
        }
        public ModelOrderGetItem ListProductByDeddeskIdSpa(int agencyid, int beddeskId)
        {
            var urlJson = string.Format("{0}Product/ListProductByDeddeskIdSpa?key={1}&agencyId={2}&beddeskId={3}", _url, Keyapi, agencyid, beddeskId);
            return GetObjJson<ModelOrderGetItem>(urlJson);
        }
        public OrderGetItem GetListByProductDetailsId(int agencyid, int productDetailId)
        {
            var urlJson = string.Format("{0}Product/GetListByProductDetailsId?key={1}&agencyId={2}&productDetailId={3}", _url, Keyapi, agencyid, productDetailId);
            return GetObjJson<OrderGetItem>(urlJson);
        }

        public List<ShopStatusItem> GetStatus(int agencyId)
        {
            var urlJson = string.Format("{0}Product/GetStatus?key={1}&agencyId={2}", _url, Keyapi, agencyId);
            return GetObjJson<List<ShopStatusItem>>(urlJson);
        }
        public ProductItem GetProductItem(int id)
        {
            var urlJson = string.Format("{0}Product/GetProductItem?key={1}&id={2}", _url, Keyapi, id);
            return GetObjJson<ProductItem>(urlJson);
        }
        public ProductItem GetCostProduceItem(int id)
        {
            var urlJson = string.Format("{0}Product/GetCostProduceItem?key={1}&id={2}", _url, Keyapi, id);
            return GetObjJson<ProductItem>(urlJson);
        }
        public List<AttributeDynamicItem> GetAttribute(string lstInts, int id)
        {
            var urlJson = string.Format("{0}Product/GetAttribute?key={1}&lstInts={2}&id={}", _url, Keyapi, lstInts, id);
            return GetObjJson<List<AttributeDynamicItem>>(urlJson);
        }
        public int CheckExitCode(string code, int id, int agencyId)
        {
            var urlJson = string.Format("{0}Product/CheckExitCode?key={1}&code={2}&id={3}&agencyId={4}", Domain, Keyapi, code, id, agencyId);
            return GetObjJson<int>(urlJson);
        }
        public JsonMessage Add(string url, string code)
        {
            var urlJson = string.Format("{0}Product/Add?key={1}&code={2}&{3}", Domain, Keyapi, code, url);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Coppy(int agencyId, int ItemID)
        {
            var urlJson = string.Format("{0}Product/Coppy?key={1}&agencyId={2}&ItemID={3}", _url, Keyapi, agencyId, ItemID);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage AddRecipe(int id, string code)
        {
            var urlJson = string.Format("{0}Product/AddRecipe?key={1}&code={2}&ItemID={3}", Domain, Keyapi, code, id);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public int AddAttribute(string json)
        {
            var urlJson = string.Format("{0}Product/AddAttribute?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
        public JsonMessage Update(string url, string code)
        {
            var urlJson = string.Format("{0}Product/Update?key={1}&code={2}&{3}", Domain, Keyapi, code, url);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public int UpdateCost(string json, int agencyid)
        {
            var urlJson = string.Format("{0}Product/UpdateCost?key={1}&agencyId={2}&{3}", Domain, Keyapi, agencyid, json);
            return GetObjJson<int>(urlJson);
        }
        public JsonMessage Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}Product/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Show(string lstArrId)
        {
            var urlJson = string.Format("{0}Product/Show?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Hide(string lstArrId)
        {
            var urlJson = string.Format("{0}Product/Hide?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
    }
}