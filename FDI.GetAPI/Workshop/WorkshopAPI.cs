using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class WorkshopAPI:BaseAPI
    {
        public ModelWorkShopItem ListItems(string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Workshop/ListItems{1}&key={2}", Domain, url, Keyapi);
            return GetObjJson<ModelWorkShopItem>(urlJson);
        }
        public WorkShopItem GetItemById(int id)
        {
            var urlJson = string.Format("{0}Workshop/GetItemById?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<WorkShopItem>(urlJson);
        }
        public JsonMessage Add(string json, Guid userId)
        {
            var urlJson = string.Format("{0}Workshop/Add?key={1}&userId={2}&{3}", Domain, Keyapi, userId, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Update(string json, Guid userId)
        {
            var urlJson = string.Format("{0}Workshop/Update?key={1}&userId={2}&{3}", Domain, Keyapi, userId, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}Workshop/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public async Task<List<WorkShopItem>> GetAll()
        {
            var urlJson = $"{Domain}Workshop/GetAll";
            return await PostDataAsync<List<WorkShopItem>>(urlJson);
        }
    }
}
