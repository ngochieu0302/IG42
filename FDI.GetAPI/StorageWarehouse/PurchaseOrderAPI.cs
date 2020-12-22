using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Simple;
using FDI.Simple.Logistics;
using FDI.Simple.StorageWarehouse;
using FDI.Utils;

namespace FDI.GetAPI.StorageWarehouse
{
    public class PurchaseOrderAPI : BaseAPI
    {
        public async Task<BaseResponse<bool>> Add(PurchaseOrderItem request)
        {
            var urlJson = $"{Domain}PurchaseOrder/Add";
            return await PostDataAsync<BaseResponse<bool>>(urlJson, request);
        }
        public async Task<BaseResponse<string>> GetExport(int[] ids, int orderCarId)
        {
            var urlJson = $"{Domain}PurchaseOrder/GetExport";
            return await PostDataAsync<BaseResponse<string>>(urlJson, new { ids, orderCarId });
        }

        public PurchaseOrderModel ListItems(string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = $"{Domain}PurchaseOrder/ListItems{url}&key={Keyapi}";
            return GetObjJson<PurchaseOrderModel>(urlJson);
        }

        public async Task<PurchaseOrderItem> GetById(int id)
        {
            var urlJson = $"{Domain}PurchaseOrder/GetById?id={id}";
            return await PostDataAsync<PurchaseOrderItem>(urlJson);
        }
        public async Task<List<OrderCarProductDetailItem>> GetByOrderCarId(int id)
        {
            var urlJson = $"{Domain}PurchaseOrder/GetByOrderCarId?ordercarId={id}";
            return await PostDataAsync<List<OrderCarProductDetailItem>>(urlJson);
        }
        public async Task<decimal> CountRecevied(int ordercarId)
        {
            var urlJson = $"{Domain}PurchaseOrder/CountRecevied?ordercarId={ordercarId}";
            return await PostDataAsync<decimal>(urlJson);
        }

        
    }
}
