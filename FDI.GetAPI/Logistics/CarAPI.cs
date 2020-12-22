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
    public class CarApi : BaseAPI
    {
        public CarResponse ListItems(string url)
        {
            var urlJson = $"{Domain}Car/ListItems{url}";
            return GetObjJson<CarResponse>(urlJson);
        }

        public async Task<BaseResponse<bool>> Add(CarItem request)
        {
            var urlJson = $"{Domain}Car/Add";
            return await PostDataAsync<BaseResponse<bool>>(urlJson, request);
        }
        public async Task<BaseResponse<bool>> Update(CarItem request)
        {
            var urlJson = $"{Domain}Car/Update";
            return await PostDataAsync<BaseResponse<bool>>(urlJson, request);
        }

        public async Task<CarItem> GetById(int id)
        {
            var urlJson = $"{Domain}Car/GetById?id={id}";
            return await PostDataAsync<CarItem>(urlJson);
        }
        public async Task<BaseResponse<bool>> Delete(int id)
        {
            var urlJson = $"{Domain}Car/Delete?id={id}";
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
    }
}
