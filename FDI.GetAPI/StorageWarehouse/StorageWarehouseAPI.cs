using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;
using FDI.Simple;
using FDI.Simple.Order;
using FDI.Simple.Supplier;
using FDI.Utils;

namespace FDI.GetAPI.StorageWarehouse
{
    public class StorageWarehouseAPI : BaseAPI
    {
        public List<StorageWarehousingItem> GetListSimple(int agencyId)
        {
            var urlJson = string.Format("{0}StorageWarehouse/GetListSimple?key={1}&agencyId={2}", Domain, Keyapi, agencyId);
            return GetObjJson<List<StorageWarehousingItem>>(urlJson);
        }

        public ModelStorageWarehousingItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}StorageWarehouse/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyid);
            return GetObjJson<ModelStorageWarehousingItem>(urlJson, agencyid);
        }

        public ModelDNRequestWareHouseItem ListItemsByOrderId(int agencyid, int orderId)
        {
            var urlJson = $"{Domain}StorageWarehouse/ListItemsByOrderId?orderId={orderId}";
            return GetObjJson<ModelDNRequestWareHouseItem>(urlJson, agencyid);
        }
        public ModelDNRequestWareHouseItem ListItemsAll(string url, int area)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}StorageWarehouse/ListItemsAll{1}&key={2}&area={3}", Domain, url, Keyapi, area);
            return GetObjJson<ModelDNRequestWareHouseItem>(urlJson, area);
        }
        public ModelDNRequestWareHouseItem ListItemsStatic(string url, int area)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}StorageWarehouse/ListItemsStatic{1}&key={2}&area={3}", Domain, url, Keyapi, area);
            return GetObjJson<ModelDNRequestWareHouseItem>(urlJson);
        }
        public ModelProductItem GetListProductByRequest(int agencyId, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}StorageWarehouse/GetListProductByRequest{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyId);
            return GetObjJson<ModelProductItem>(urlJson);
        }
        public ModelProductItem GetListProductLater(int agencyId, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}StorageWarehouse/GetListProductLater{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyId);
            return GetObjJson<ModelProductItem>(urlJson);
        }

        public List<StorageWarehousingItem> GetListExcel(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}StorageWarehouse/GetListExcel{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyid);
            return GetObjJson<List<StorageWarehousingItem>>(urlJson);
        }

        public StorageWarehousingItem GetStorageWarehousingItem(int id, int agencyid)
        {
            var urlJson = $"{Domain}StorageWarehouse/GetStorageWarehousingItem?key={Keyapi}&id={id}";
            return GetObjJson<StorageWarehousingItem>(urlJson, agencyid);
        }
        public StorageWarehousingItem GetStorageWarehousingItem(int id)
        {
            var urlJson = string.Format("{0}StorageWarehouse/GetStorageWarehousingItem?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<StorageWarehousingItem>(urlJson);
        }
        public bool ConfirmAmount(int id, int agencyId)
        {
            var urlJson = $"{Domain}StorageWarehouse/ConfirmAmount?orderId={id}";
            return GetObjJson<bool>(urlJson, agencyId);
        }
        public async Task<BaseResponse<bool>> MoveProduce(int id)
        {
            var urlJson = $"{Domain}StorageWarehouse/MoveProduce?orderId={id}";
            return await PostDataAsync<BaseResponse<bool>>(urlJson);
        }
        //public List<DN_ImportProduct> GetListDNImportItem(int id)
        //{
        //    var urlJson = string.Format("{0}StorageWarehouse/GetListDNImportItem?key={1}&id={2}", Domain, Keyapi, id);
        //    return GetObjJson<List<DN_ImportProduct>>(urlJson);
        //}
        public List<SuggestionsProduct> GetListAuto(string keword, int showLimit, int agencyId, int type = 0)
        {
            var urlJson = string.Format("{0}StorageWarehouse/GetListAuto?key={1}&keword={2}&showLimit={3}&agencyId={4}&type={5}", Domain, Keyapi, keword, showLimit, agencyId, type);
            return GetObjJson<List<SuggestionsProduct>>(urlJson);
        }
        public List<SuggestionsProduct> GetListAutoProductValue(string keword, int showLimit, int agencyId, int type = 0)
        {
            var urlJson = string.Format("{0}StorageWarehouse/GetListAutoProductValue?key={1}&keword={2}&showLimit={3}&agencyId={4}&type={5}", Domain, Keyapi, keword, showLimit, agencyId, type);
            return GetObjJson<List<SuggestionsProduct>>(urlJson);
        }
        public int Add(string url, int agencyid, string code)
        {
            var urlJson = string.Format("{0}StorageWarehouse/Add?key={1}&agencyId={2}&codeLogin={3}&{4}", Domain, Keyapi, agencyid, code, url);
            return GetObjJson<int>(urlJson);
        }
        public JsonMessage Active(string lstArrId, Guid userId)
        {
            var urlJson = string.Format("{0}StorageWarehouse/Active?key={1}&userId={2}&lstArrId={3}", Domain, Keyapi, userId, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public int AddApp(string url, int agencyid, string code, string jsonlist, Guid userId, string date, string hours, string money,int marketId,int areaId)
        {
            var urlJson = string.Format("{0}StorageWarehouse/AddApp?key={1}&agencyId={2}&codeLogin={3}&userId={4}&jsonlist={5}&date={6}&hours={7}&money={8}&marketId={9}&areaId={10}", Domain, Keyapi, agencyid, code, userId, jsonlist, date, hours, money, marketId, areaId);
            return GetObjJson<int>(urlJson);
        }
        public int Imported(string url, int agencyid, string code)
        {
            var urlJson = string.Format("{0}StorageWarehouse/Imported?key={1}&agencyId={2}&codeLogin={3}&{4}", Domain, Keyapi, agencyid, code, url);
            return GetObjJson<int>(urlJson);
        }
        public int Update(string url, int agencyid, string code)
        {
            var urlJson = string.Format("{0}StorageWarehouse/Update?key={1}&agencyId={2}&codeLogin={3}&{4}", Domain, Keyapi, agencyid, code, url);
            return GetObjJson<int>(urlJson);
        }
        public int UpdateActive(string url, int agencyid, string code)
        {
            var urlJson = string.Format("{0}StorageWarehouse/UpdateActive?key={1}&agencyId={2}&codeLogin={3}&{4}", Domain, Keyapi, agencyid, code, url);
            return GetObjJson<int>(urlJson);
        }
        public int Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}StorageWarehouse/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }

        public async Task<BaseResponse<int>> AssignUser(UserStorageWarehousingRequest data)
        {
            var urlJson = $"{Domain}StorageWarehouse/AssignUser";
            return await PostDataAsync<BaseResponse<int>>(urlJson, data);
        }
        public async Task<BaseResponse<int>> AddSupplier(IList<RequestWareSupplierRequest> data)
        {
            var urlJson = $"{Domain}StorageWarehouse/AddSupplier";
            return await PostDataAsync<BaseResponse<int>>(urlJson, data);
        }

        public async Task<BaseResponse<int>> DeleteSupplier(int Id)
        {
            var urlJson = $"{Domain}StorageWarehouse/DeleteSupplier?id={Id}";
            return await PostDataAsync<BaseResponse<int>>(urlJson, null);
        }
        public async Task<BaseResponse<int>> ChangeStatus(int Id, int status)
        {
            var urlJson = $"{Domain}StorageWarehouse/ChangeStatus?orderId={Id}&status={status}";
            return await PostDataAsync<BaseResponse<int>>(urlJson, null);
        }
        public async Task<ModelDNRequestWareHouseItem> GetRequestWareSummary(string url)
        {
            var urlJson = $"{Domain}StorageWarehouse/GetRequestWareSummary{url}";
            return await PostDataAsync<ModelDNRequestWareHouseItem>(urlJson);
        }
        public async Task<DNRequestWareHouseItem> GetRequestWareById(Guid id)
        {
            var urlJson = $"{Domain}StorageWarehouse/GetRequestWareById?id={id}";
            return await PostDataAsync<DNRequestWareHouseItem>(urlJson);
        }
        public async Task<IList<SupplierAmountProductItem>> GetRequestWareSummaryByProduct(decimal today, int productId)
        {
            var urlJson = $"{Domain}StorageWarehouse/GetRequestWareSummaryByProduct?today={today}&productId={productId}";
            return await PostDataAsync<IList<SupplierAmountProductItem>>(urlJson);
        }
        public async Task<IList<DNRequestWareItem>> GetSummaryDetailConfirmed(decimal today)
        {
            var urlJson = $"{Domain}StorageWarehouse/GetSummaryDetailConfirmed?today={today}";
            return await PostDataAsync<IList<DNRequestWareItem>>(urlJson);
        }
    }
}
