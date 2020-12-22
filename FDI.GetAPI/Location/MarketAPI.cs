using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class MarketAPI : BaseAPI
    {
        private readonly string _url = Domain;
        public ModelMarketItem ListItems(string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Market/ListItems{1}&key={2}", _url, url, Keyapi);
            return GetObjJson<ModelMarketItem>(urlJson);
        }
        public ModelMarketItem ListItemsStatic(string url,int areaId)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Market/ListItemsStatic{1}&key={2}&areaId={3}", _url, url, Keyapi, areaId);
            return GetObjJson<ModelMarketItem>(urlJson);
        }
        public MarketItem GetMarketItem(int id)
        {
            var urlJson = string.Format("{0}Market/GetMarketItem?key={1}&id={2}", _url, Keyapi, id);
            return GetObjJson<MarketItem>(urlJson);
        }
        public List<MarketItem> GetAll()
        {
            var urlJson = string.Format("{0}Market/GetAll?key={1}", _url, Keyapi);
            return GetObjJson<List<MarketItem>>(urlJson);
        }
        public JsonMessage Add(string json)
        {
            var urlJson = string.Format("{0}Market/Add?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Update(string json)
        {
            var urlJson = string.Format("{0}Market/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}Market/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
    }

}
