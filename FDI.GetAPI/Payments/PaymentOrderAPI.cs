using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Simple;

namespace FDI.GetAPI.Payments
{
    public class PaymentOrderAPI:BaseAPI
    {
        private readonly string _url = Domain;
        public ModelPaymentOrderItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}PaymentOrder/ListItems{1}&key={2}", _url, url, Keyapi);
            return GetObjJson<ModelPaymentOrderItem>(urlJson);
        }
        public PaymentMethodItem GetItemById(int agencyid, int id = 0)
        {
            var urlJson = string.Format("{0}PaymentOrder/GetItemById?key={1}&id={2}", _url, Keyapi, id);
            return GetObjJson<PaymentMethodItem>(urlJson);
        }

    }
}
