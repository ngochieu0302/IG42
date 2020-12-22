using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class DNDrawerAPI : BaseAPI
    {
        public List<DNDrawerItem> GetListSimple()
        {
            var urlJson = string.Format("{0}DNDrawer/GetListSimple?key={1}", Domain, Keyapi);
            return GetObjJson<List<DNDrawerItem>>(urlJson);
        }

        public List<DNDrawerItem> GetListNotInBedDesk(int agencyid)
        {
            var urlJson = string.Format("{0}DNDrawer/GetListNotInBedDesk?key={1}&agencyId={2}", Domain, Keyapi,agencyid);
            return GetObjJson<List<DNDrawerItem>>(urlJson);
        }

        public ModelDNDrawerItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}DNDrawer/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi,agencyid);
            return GetObjJson<ModelDNDrawerItem>(urlJson);
        }

        public DNDrawerItem GetItemsByID(int id)
        {
            var urlJson = string.Format("{0}DNDrawer/GetItemsByID?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<DNDrawerItem>(urlJson);
        }

        public List<DNDrawerItem> GetItemsCabinetId(int cabinetId)
        {
            var urlJson = string.Format("{0}DNDrawer/GetItemsCabinetId?key={1}&cabinetId={2}", Domain, Keyapi, cabinetId);
            return GetObjJson<List<DNDrawerItem>>(urlJson);
        }

        public int Add(string json)
        {
            var urlJson = string.Format("{0}DNDrawer/Add?key={1}&{2}", Domain, Keyapi,json);
            return GetObjJson<int>(urlJson);
        }
        public int Update(string json)
        {
            var urlJson = string.Format("{0}DNDrawer/Update?key={1}&{2}", Domain, Keyapi,json);
            return GetObjJson<int>(urlJson);
        }
        public int Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}DNDrawer/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }
    }
}
