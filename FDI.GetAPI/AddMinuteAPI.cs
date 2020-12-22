using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class AddMinuteAPI : BaseAPI
    {
        public ModelAddMinuteItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}AddMinute/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi,agencyid);
            return GetObjJson<ModelAddMinuteItem>(urlJson);
        }
        public List<AddMinuteItem> ListAllItems()
        {
            var urlJson = string.Format("{0}AddMinute/ListAllItems?key={1}", Domain, Keyapi);
            return GetObjJson<List<AddMinuteItem>>(urlJson);
        }
        public AddMinuteItem GetItemById(int id)
        {
            var urlJson = string.Format("{0}AddMinute/GetItemById?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<AddMinuteItem>(urlJson);
        }

        public int Add(string json)
        {
            var urlJson = string.Format("{0}AddMinute/Add?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
        public int Update(string json)
        {
            var urlJson = string.Format("{0}AddMinute/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }

        public int Show(string lstArrId)
        {
            var urlJson = string.Format("{0}AddMinute/Show?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }

        public int Hide(string lstArrId)
        {
            var urlJson = string.Format("{0}AddMinute/Hide?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }
        public int Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}AddMinute/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }
    }
}