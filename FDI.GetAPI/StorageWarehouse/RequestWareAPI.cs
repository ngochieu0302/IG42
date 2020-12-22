
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Simple;
using FDI.Simple.StorageWarehouse;

namespace FDI.GetAPI.StorageWarehouse
{
    public class RequestWareAPI : BaseAPI
    {
        public async Task<decimal?> GetTotalOrder(decimal todayCode)
        {
            var urlJson = $"{Domain}RequestWare/GetTotalOrder?todayCode={todayCode}";
            return await PostDataAsync<decimal?>(urlJson);
        }

        public async Task<List<DNRequestWareItem>> GetSummaryProductsByToDayCode(decimal todayCode)
        {
            var urlJson = $"{Domain}RequestWare/GetSummaryProductsByToDayCode?todayCode={todayCode}";
            return await PostDataAsync<List<DNRequestWareItem>>(urlJson);
        }

        public async Task<List<DNRequestWareItem>> GetTotalProductNotConfirm(decimal todayCode)
        {
            var urlJson = $"{Domain}RequestWare/GetTotalProductNotConfirm?todayCode={todayCode}";
            return await PostDataAsync<List<DNRequestWareItem>>(urlJson);
        }
        public async Task<List<TotalProductToDayConfirmItem>> GetTotalProductConfirm(decimal todayCode)
        {
            var urlJson = $"{Domain}RequestWare/GetTotalProductConfirm?todayCode={todayCode}";
            return await PostDataAsync<List<TotalProductToDayConfirmItem>>(urlJson);
        }
        public async Task<List<DNRequestWareTotalItem>> ListItems(decimal todayCode)
        {
            var urlJson = $"{Domain}RequestWare/ListItems?todayCode={todayCode}";
            return await PostDataAsync<List<DNRequestWareTotalItem>>(urlJson);
        }

        public async Task<List<OrderDetailProductItem>> GetSummary(decimal todayCode)
        {
            var urlJson = $"{Domain}RequestWare/GetSummary?todayCode={todayCode}";
            return await PostDataAsync<List<OrderDetailProductItem>>(urlJson);
        }

        public async Task<List<DNRequestWareItem>> GetDetails(Guid[] ids)
        {
            var urlJson = $"{Domain}RequestWare/GetDetails";
            return await PostDataAsync<List<DNRequestWareItem>>(urlJson, ids);
        }
    }
}
