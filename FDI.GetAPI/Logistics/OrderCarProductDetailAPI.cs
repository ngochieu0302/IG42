using FDI.Simple;
using FDI.Simple.Supplier;
using FDI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Simple.Logistics;

namespace FDI.GetAPI.Supplier
{
    public class OrderCarProductDetailAPI : BaseAPI
    {
        public OrderCarProductDetailResponse ListItems(string url)
        {
            var urlJson = $"{Domain}OrderCarProductDetail/ListItems{url}";
            return GetObjJson<OrderCarProductDetailResponse>(urlJson);
        }

        public async Task<BaseResponse<bool>> Add(OrderCarProductDetailItem request)
        {
            var urlJson = $"{Domain}OrderCarProductDetail/Add";
            return await PostDataAsync<BaseResponse<bool>>(urlJson, request);
        }

        public async Task<BaseResponse<bool>> Update(OrderCarProductDetailItem request)
        {
            var urlJson = $"{Domain}OrderCarProductDetail/Update";
            return await PostDataAsync<BaseResponse<bool>>(urlJson, request);
        }

        public async Task<OrderCarProductDetailItem> GetById(int id)
        {
            var urlJson = $"{Domain}OrderCarProductDetail/GetById?id={id}";
            return await PostDataAsync<OrderCarProductDetailItem>(urlJson);
        }
        public async Task<BaseResponse<bool>> Delete(int id)
        {
            var urlJson = $"{Domain}OrderCarProductDetail/Delete?id={id}";
            return await PostDataAsync<BaseResponse<bool>>(urlJson);
        }
        public async Task<List<CarItem>> GetAll()
        {
            var urlJson = $"{Domain}Car/GetAll";
            return await PostDataAsync<List<CarItem>>(urlJson);
        }
        public async Task<List<CarItem>> GetListAssign(int unitId)
        {
            var urlJson = $"{Domain}Car/GetListAssign?unitId=" + unitId;
            return await PostDataAsync<List<CarItem>>(urlJson);
        }

        public async Task<decimal> GetAmountRecevied(int ordercarId)
        {
            var urlJson = $"{Domain}OrderCarProductDetail/GetAmountRecevied?ordercarId=" + ordercarId;
            return await PostDataAsync<decimal>(urlJson);
        }

        public async Task<int> CountRecevied(int ordercarId)
        {
            var urlJson = $"{Domain}OrderCarProductDetail/CountRecevied?ordercarId=" + ordercarId;
            return await PostDataAsync<int>(urlJson);
        }
    }
}
