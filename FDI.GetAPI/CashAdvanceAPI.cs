using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class CashAdvanceAPI : BaseAPI
    {
        public ModelCashAdvanceItem GetListByRequest(int agencyId, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}CashAdvance/GetListByRequest{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyId);
            return GetObjJson<ModelCashAdvanceItem>(urlJson);
        }
        
        public CashAdvanceItem GetCashAdvanceItem(int id)
        {
            var urlJson = string.Format("{0}CashAdvance/GetCashAdvanceItem?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<CashAdvanceItem>(urlJson);
        }
        
        public JsonMessage Add(string json)
        {
            var urlJson = string.Format("{0}CashAdvance/Add?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }        
        public JsonMessage Update(string json)
        {
            var urlJson = string.Format("{0}CashAdvance/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Active(string lstArrId)
        {
            var urlJson = string.Format("{0}CashAdvance/Active?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}CashAdvance/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }

        //Trả ứng lương

        public ModelCashAdvanceItem GetListRepayByRequest(int agencyId, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}CashAdvance/GetListRepayByRequest{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyId);
            return GetObjJson<ModelCashAdvanceItem>(urlJson);
        }

        public CashAdvanceItem GetRepayItem(int id)
        {
            var urlJson = string.Format("{0}CashAdvance/GetRepayItem?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<CashAdvanceItem>(urlJson);
        }

        public JsonMessage AddRepay(string json)
        {
            var urlJson = string.Format("{0}CashAdvance/AddRepay?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }

        public JsonMessage UpdateRepay(string json)
        {
            var urlJson = string.Format("{0}CashAdvance/UpdateRepay?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage ActiveRepay(string lstArrId)
        {
            var urlJson = string.Format("{0}CashAdvance/ActiveRepay?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage DeleteRepay(string lstArrId)
        {
            var urlJson = string.Format("{0}CashAdvance/DeleteRepay?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }

        //Tổng quát
        public List<CashAdvanceItem> GetListGeneralCash(int agencyId, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}CashAdvance/GetListGeneralCash{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyId);
            return GetObjJson<List<CashAdvanceItem>>(urlJson);
        }
    }
}
