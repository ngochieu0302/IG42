using System;
using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class BiasProduceAPI : BaseAPI
    {
        private readonly string _url = Domain;

        public ModelBiasProduceItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}BiasProduce/ListItems{1}&key={2}&agencyId={3}", _url, url, Keyapi,agencyid);
            return GetObjJson<ModelBiasProduceItem>(urlJson);
        }
        public ModelProductCodeItem GetListProductCode(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}BiasProduce/GetListProductCode{1}&key={2}&agencyId={3}", _url, url, Keyapi,agencyid);
            return GetObjJson<ModelProductCodeItem>(urlJson);
        }
        public List<CostProductUserItem> GetListCostProductUser(int biasId)
        {
            var urlJson = string.Format("{0}BiasProduce/GetListCostProductUser?key={1}&biasId={2}", _url, Keyapi, biasId);
            return GetObjJson<List<CostProductUserItem>>(urlJson);
        }
        public List<CostProductCostUserItem> GetListEvaluate(int id)
        {
            var urlJson = string.Format("{0}BiasProduce/GetListEvaluate?key={1}&id={2}", _url, Keyapi, id);
            return GetObjJson<List<CostProductCostUserItem>>(urlJson);
        }

        public List<BiasProduceItem> GetListUnit(int agencyid)
        {
            var urlJson = string.Format("{0}BiasProduce/GetList?key={1}&agencyId={2}", _url, Keyapi,agencyid);
            return GetObjJson<List<BiasProduceItem>>(urlJson);
        }
        public BiasProduceItem GetBiasProduceItem(int id)
        {
            var urlJson = string.Format("{0}BiasProduce/GetBiasProduceItem?key={1}&id={2}", _url, Keyapi, id);
            return GetObjJson<BiasProduceItem>(urlJson);
        }
        public int AddEvaluate(int agencyid,string status,string itemId, string json, Guid UserId)
        {
            var urlJson = string.Format("{0}BiasProduce/AddEvaluate?key={1}&agencyId={2}&status={3}&itemId={4}&lstRet={5}&UserId={6}", Domain, Keyapi, agencyid, status, itemId, json, UserId);
            return GetObjJson<int>(urlJson);
        }
        public JsonMessage Add(string json,int agencyid, string code)
        {
            var urlJson = string.Format("{0}BiasProduce/Add?key={1}&agencyId={2}&code={3}&{4}", Domain, Keyapi,agencyid, code, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Update(string json, int agencyid, string code)
        {
            var urlJson = string.Format("{0}BiasProduce/Update?key={1}&agencyId={2}&code={3}&{4}", Domain, Keyapi, agencyid, code, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}BiasProduce/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
    }
}
