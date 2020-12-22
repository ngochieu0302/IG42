using FDI.Simple;
using FDI.Simple.Supplier;
using FDI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Simple.Logistics;
using FDI.CORE;

namespace FDI.GetAPI.Supplier
{
    public class OrderCarAPI : BaseAPI
    {
        public OrderCarResponse ListItems(string url)
        {
            var urlJson = $"{Domain}OrderCar/ListItems{url}";
            return GetObjJson<OrderCarResponse>(urlJson);
        }

        public async Task<OrderCarResponse> ListItemsByStatus(string url, OrderCarStatus[] status)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = $"{Domain}OrderCar/ListItemsByStatus{url}&key={Keyapi}";
            var obj = status.Select(m => (int)m).ToArray();
            return await PostDataAsync<OrderCarResponse>(urlJson, obj);
        }

        public async Task<BaseResponse<bool>> Add(OrderCarItem request)
        {
            var urlJson = $"{Domain}OrderCar/Add";
            return await PostDataAsync<BaseResponse<bool>>(urlJson, request);
        }
        public async Task<BaseResponse<bool>> Update(OrderCarItem request)
        {
            var urlJson = $"{Domain}OrderCar/Update";
            return await PostDataAsync<BaseResponse<bool>>(urlJson, request);
        }

        public async Task<OrderCarItem> GetById(int id)
        {
            var urlJson = $"{Domain}OrderCar/GetById?id={id}";
            return await PostDataAsync<OrderCarItem>(urlJson);
        }
        public async Task<BaseResponse<bool>> Delete(int id)
        {
            var urlJson = $"{Domain}OrderCar/Delete?id={id}";
            return await PostDataAsync<BaseResponse<bool>>(urlJson);
        }
    }
}
