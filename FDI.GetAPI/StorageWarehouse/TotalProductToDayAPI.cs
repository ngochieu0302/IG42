using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using FDI.Simple;
using FDI.Simple.Order;
using FDI.Simple.StorageWarehouse;
using FDI.Utils;

namespace FDI.GetAPI.StorageWarehouse
{
    public class TotalProductToDayAPI : BaseAPI
    {
        public ModelTotalProductToDayItem ListItems(string url, decimal todayCode, int productId)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = $"{Domain}TotalProductToDay/ListItems{url}&todayCode={todayCode}&productId={productId}";
            return GetObjJson<ModelTotalProductToDayItem>(urlJson);
        }
        public async Task<BaseResponse<bool>> AddSupplier(IList<RequestWareSupplierRequest> request)
        {
            var urlJson = $"{Domain}TotalProductToDay/AddSupplier";
            return await PostDataAsync<BaseResponse<bool>>(urlJson, request);
        }
        public async Task<BaseResponse<bool>> SaveSupplier(RequestWareSupplierRequest request)
        {
            var urlJson = $"{Domain}TotalProductToDay/";
            return await PostDataAsync<BaseResponse<bool>>(urlJson, request);
        }
        public async Task<decimal?> GetTotalOrder(decimal totalCode)
        {
            var urlJson =
                $"{Domain}TotalProductToDay/GetTotalOrder?totalCode={totalCode}";
            return await PostDataAsync<decimal?>(urlJson);
        }

        public async Task<List<TotalProductToDayItem>> GetSummaryTotalByToDay(decimal todayCode)
        {
            var urlJson =
                $"{Domain}TotalProductToDay/GetSummaryTotalByToDay?todayCode={todayCode}";
            return await PostDataAsync<List<TotalProductToDayItem>>(urlJson);
        }
        public async Task<List<TotalProductToDayItem>> GetListByToDay(decimal todayCode)
        {
            var urlJson =
                $"{Domain}TotalProductToDay/GetListByToDay?todayCode={todayCode}";
            return await PostDataAsync<List<TotalProductToDayItem>>(urlJson);
        }
        public async Task<TotalProductToDayItem> GetItem(decimal todayCode, int productId, int supplierId)
        {
            var urlJson =
                $"{Domain}TotalProductToDay/GetItem?todayCode={todayCode}&productId={productId}&supplierId={supplierId}";
            return await PostDataAsync<TotalProductToDayItem>(urlJson);
        }

    }
}
