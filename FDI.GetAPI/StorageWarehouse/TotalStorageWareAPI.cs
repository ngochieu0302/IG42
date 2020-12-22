using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI.StorageWarehouse
{
    public class TotalStorageWareAPI : BaseAPI
    {
        public ModelTotalStorageWareItem ListItems(string url, int area)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}TotalStorageWare/ListItems{1}&key={2}&areaID={3}", Domain, url, Keyapi,  area);
            return GetObjJson<ModelTotalStorageWareItem>(urlJson);
        }
        public TotalStorageWareItem GetTotalStorageWareItem(int id)
        {
            var urlJson = string.Format("{0}TotalStorageWare/GetTotalStorageWareItem?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<TotalStorageWareItem>(urlJson);
        }
        public JsonMessage AddStorage(string url,int agencyId,Guid userId)
        {
            var urlJson = string.Format("{0}TotalStorageWare/AddStorage?key={1}&agencyId={2}&{3}&userId={4}", Domain, Keyapi, agencyId, url, userId);
            return GetObjJson<JsonMessage>(urlJson);
        }
        
    }
}
