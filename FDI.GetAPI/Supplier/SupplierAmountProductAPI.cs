using FDI.Simple;
using FDI.Simple.Supplier;
using FDI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.GetAPI.Supplier
{
    public class SupplierAmountProductApi : BaseAPI
    {
        public SupplierAmountProductResponse ListItems(string url)
        {
            var urlJson = $"{Domain}SupplierAmountProduct/ListItems{url}";
            return GetObjJson<SupplierAmountProductResponse>(urlJson);
        }

        public async Task<BaseResponse<SupplieProductResponse>> Add(SupplierAmountProductItem request)
        {
            var urlJson = $"{Domain}SupplierAmountProduct/Add";
            return await PostDataAsync<BaseResponse<SupplieProductResponse>>(urlJson, request);
        }
        public async Task<BaseResponse<SupplieProductResponse>> Update(SupplierAmountProductItem request)
        {
            var urlJson = $"{Domain}SupplierAmountProduct/Update";
            return await PostDataAsync<BaseResponse<SupplieProductResponse>>(urlJson, request);
        }

        public async Task<SupplierAmountProductItem> GetById(int id)
        {
            var urlJson = $"{Domain}SupplierAmountProduct/GetById?id={id}";
            return await PostDataAsync<SupplierAmountProductItem>(urlJson);
        }
        public async Task<BaseResponse<bool>> Delete(int id)
        {
            var urlJson = $"{Domain}SupplierAmountProduct/Delete?id={id}";
            return await PostDataAsync<BaseResponse<bool>>(urlJson);
        }
        public async Task<List<SupplierAmountProductItem>> GetSupplierByCategoryId(int id)
        {
            var urlJson = $"{Domain}SupplierAmountProduct/GetSupplierByCategoryId?id={id}";
            return await PostDataAsync<List<SupplierAmountProductItem>>(urlJson);
        }
        public async Task<List<SupplierAmountProductItem>> GetAmount(int productId, decimal todayCode)
        {
            var urlJson = $"{Domain}SupplierAmountProduct/GetAmount?productId={productId}&todayCode={todayCode}";
            return await PostDataAsync<List<SupplierAmountProductItem>>(urlJson);
        }
    }
}
