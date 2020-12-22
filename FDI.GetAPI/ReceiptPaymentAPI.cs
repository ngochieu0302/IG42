using System.Collections.Generic;
using System.Data;
using FDI.Simple;
using System;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class ReceiptPaymentAPI : BaseAPI
    {
        public ModelReceiptPaymentItem GetListByRequest(int agencyId, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}ReceiptPayment/GetListByRequest{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyId);
            return GetObjJson<ModelReceiptPaymentItem>(urlJson);
        }
        public ModelReceiptPaymentItem GetListByUserRequest(int agencyId, Guid user, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}ReceiptPayment/GetListByUserRequest{1}&key={2}&agencyId={3}&user={4}", Domain, url, Keyapi, agencyId, user);
            return GetObjJson<ModelReceiptPaymentItem>(urlJson);
        }
        public ModelReceiptPaymentItem GetListReceipt(int agencyId, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}ReceiptPayment/GetListReceipt{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyId);
            return GetObjJson<ModelReceiptPaymentItem>(urlJson);
        }

        public ModelReceiptPaymentItem GetListPayment(int agencyId, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}ReceiptPayment/GetListPayment{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyId);
            return GetObjJson<ModelReceiptPaymentItem>(urlJson);
        }

        public List<ReceiptPaymentItem> GetListUserRp(int agencyId, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}ReceiptPayment/GetListUserRp{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyId);
            return GetObjJson<List<ReceiptPaymentItem>>(urlJson);
        }

        public List<ReceiptPaymentItem> GetListGeneralTrip(int agencyId, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}ReceiptPayment/GetListGeneralTrip{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyId);
            return GetObjJson<List<ReceiptPaymentItem>>(urlJson);
        }

        public List<ReceiptPaymentItem> GetListGeneralOrder(int agencyId, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}ReceiptPayment/GetListGeneralOrder{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyId);
            return GetObjJson<List<ReceiptPaymentItem>>(urlJson);
        }

        public List<GeneralTotalItem> GeneralListTotal(int year, int agencyId)
        {
            var urlJson = string.Format("{0}ReceiptPayment/GeneralListTotal?key={1}&year={2}&agencyId={3}", Domain, Keyapi, year, agencyId);
            return GetObjJson<List<GeneralTotalItem>>(urlJson);
        }

        public ModelReceiptPaymentItem GetListTransferSlip(int agencyId, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}ReceiptPayment/GetListTransferSlip{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyId);
            return GetObjJson<ModelReceiptPaymentItem>(urlJson);
        }
        public List<ReceiptPaymentItem> GetListSimple(int agencyId)
        {
            var urlJson = string.Format("{0}ReceiptPayment/GetListSimple?key={1}&agencyId={2}", Domain, Keyapi, agencyId);
            return GetObjJson<List<ReceiptPaymentItem>>(urlJson);
        }
        public ReceiptPaymentItem GetReceiptPaymentItem(int id)
        {
            var urlJson = string.Format("{0}ReceiptPayment/GetReceiptPaymentItem?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<ReceiptPaymentItem>(urlJson);
        }
        public ReceiptPaymentItem GetPaymentItem(int id)
        {
            var urlJson = string.Format("{0}ReceiptPayment/GetPaymentItem?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<ReceiptPaymentItem>(urlJson);
        }
        public ReceiptPaymentItem GetReceiptItem(int id)
        {
            var urlJson = string.Format("{0}ReceiptPayment/GetReceiptItem?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<ReceiptPaymentItem>(urlJson);
        }

        public JsonMessage AddPayment(string json)
        {
            var urlJson = string.Format("{0}ReceiptPayment/AddPayment?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage AddReceipt(string json)
        {
            var urlJson = string.Format("{0}ReceiptPayment/AddReceipt?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Add(string json)
        {
            var urlJson = string.Format("{0}ReceiptPayment/Add?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }

        public JsonMessage UpdatePayment(string json)
        {
            var urlJson = string.Format("{0}ReceiptPayment/UpdatePayment?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage UpdateReceipt(string json)
        {
            var urlJson = string.Format("{0}ReceiptPayment/UpdateReceipt?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Update(string json)
        {
            var urlJson = string.Format("{0}ReceiptPayment/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }

        public JsonMessage Active(string lstArrId, Guid userid)
        {
            var urlJson = string.Format("{0}ReceiptPayment/Active?key={1}&lstArrId={2}&userid={3}", Domain, Keyapi, lstArrId, userid);
            return GetObjJson<JsonMessage>(urlJson);
        }

        public JsonMessage ActivePayment(string lstArrId, Guid userid)
        {
            var urlJson = string.Format("{0}ReceiptPayment/ActivePayment?key={1}&lstArrId={2}&userid={3}", Domain, Keyapi, lstArrId, userid);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage ActiveReceipt(string lstArrId)
        {
            var urlJson = string.Format("{0}ReceiptPayment/ActiveReceipt?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}ReceiptPayment/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }

        public JsonMessage DeletePayment(string lstArrId)
        {
            var urlJson = string.Format("{0}ReceiptPayment/DeletePayment?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage DeleteReceipt(string lstArrId)
        {
            var urlJson = string.Format("{0}ReceiptPayment/DeleteReceipt?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }

    }
}
