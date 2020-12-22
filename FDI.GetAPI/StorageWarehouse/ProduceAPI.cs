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
    public class ProduceAPI : BaseAPI
    {
        public ProduceAPI()
        {

        }

        public ProduceAPI(DNUserItem userItem) : base(userItem)
        {

        }
        public async Task<ProduceItem> GetById(int id)
        {
            var urlJson = $"{Domain}Produce/GetById?id={id}";
            return await PostDataAsync<ProduceItem>(urlJson);
        }
        public async Task<ProduceItem> GetProduceDetail(string id)
        {
            var urlJson = $"{Domain}Produce/GetProduceDetail?code={id}";
            return await PostDataAsync<ProduceItem>(urlJson);
        }
    
        public async Task<BaseResponse<bool>> Insert(int produceId, string[] codes)
        {
            var urlJson = $"{Domain}Produce/Insert";
            var obj = new { produceId, codes };
            return await PostDataAsync<BaseResponse<bool>>(urlJson, obj);
        }
        public async Task<BaseResponse<bool>> InsertProductCategory(List<ProduceCatogoryItem> model)
        {
            var urlJson = $"{Domain}Produce/InsertProductCategory";
            return await PostDataAsync<BaseResponse<bool>>(urlJson, model);
        }

        public async Task<BaseResponse<List<OrderDetailProductItem>>> GetProductDetail(string code)
        {
            var urlJson = $"{Domain}Produce/GetProductDetail?code={code}";
            return await PostDataAsync<BaseResponse<List<OrderDetailProductItem>>>(urlJson);
        }

        public async Task<BaseResponse<bool>> InsertProductDetail(List<ImportProductItem> model)
        {
            var urlJson = $"{Domain}Produce/InsertProductDetail";
            return await PostDataAsync<BaseResponse<bool>>(urlJson, model);
        }
    }
}
