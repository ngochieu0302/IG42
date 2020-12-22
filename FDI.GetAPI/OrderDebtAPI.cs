using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class OrderDebtAPI : BaseAPI
    {
        public ModelOrderDebtItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}OrderDebt/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyid);
            return GetObjJson<ModelOrderDebtItem>(urlJson);
        }
        public OrderDebtItem GetItemById(int agencyid, int id)
        {
            var urlJson = string.Format("{0}OrderDebt/GetItemById?key={1}&agencyId={2}&id={3}", Domain, Keyapi, agencyid, id);
            return GetObjJson<OrderDebtItem>(urlJson);
        }
        public int Add(string json)
        {
            var urlJson = string.Format("{0}OrderDebt/Add?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
        public int Update(string json)
        {
            var urlJson = string.Format("{0}OrderDebt/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }

        public int Show(string lstArrId)
        {
            var urlJson = string.Format("{0}OrderDebt/Show?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }

        public int Hide(string lstArrId)
        {
            var urlJson = string.Format("{0}OrderDebt/Hide?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }
        public int Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}OrderDebt/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }
    }
}
