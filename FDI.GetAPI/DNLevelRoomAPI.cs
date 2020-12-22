using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class DNLevelRoomAPI : BaseAPI
    {
        public List<DNLevelRoomItem> GetList(int agencyId)
        {
            var urlJson = string.Format("{0}DNLevelRoom/GetList?key={1}&agencyId={2}", Domain, Keyapi, agencyId);
            return GetObjJson<List<DNLevelRoomItem>>(urlJson);
        }

        public ModelDNLevelRoomItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}DNLevelRoom/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi,agencyid);
            return GetObjJson<ModelDNLevelRoomItem>(urlJson);
        }

        public List<DNLevelRoomItem> GetAll(int agencyid)
        {
            var urlJson = string.Format("{0}DNLevelRoom/GetAll?key={1}&agencyId={2}", Domain, Keyapi,agencyid);
            return GetObjJson<List<DNLevelRoomItem>>(urlJson);
        }
        
        public DNLevelRoomItem GetLevelRoomItem(int id)
        {
            var urlJson = string.Format("{0}DNLevelRoom/GetLevelRoomItem?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<DNLevelRoomItem>(urlJson);
        }
        public List<DNLevelRoomItem> GetListBed(int agencyId)
        {
            var urlJson = string.Format("{0}DNLevelRoom/GetListBed?key={1}&agencyId={2}", Domain, Keyapi, agencyId);
            return GetObjJson<List<DNLevelRoomItem>>(urlJson);
        }
        public List<TreeViewItem> GetListTree(int agencyId)
        {
            var urlJson = string.Format("{0}DNLevelRoom/GetListTree?key={1}&agencyId={2}", Domain, Keyapi, agencyId);
            return GetObjJson<List<TreeViewItem>>(urlJson);
        }
        public List<TreeViewItem> GetListParent(int agencyId = 0)
        {
            var urlJson = string.Format("{0}DNLevelRoom/GetListParent?key={1}&agencyId={2}", Domain, Keyapi, agencyId);
            return GetObjJson<List<TreeViewItem>>(urlJson);
        }
        public List<DNLevelRoomItem> GetListParentID(int agencyId = 0)
        {
            var urlJson = string.Format("{0}DNLevelRoom/GetListParentID?key={1}&agencyId={2}", Domain, Keyapi, agencyId);
            return GetObjJson<List<DNLevelRoomItem>>(urlJson);
        }

        public int Add(int agencyid, string json)
        {
            var urlJson = string.Format("{0}DNLevelRoom/Add?key={1}&agencyId={2}&{3}", Domain, Keyapi,agencyid, json);
            return GetObjJson<int>(urlJson);
        }
        public int Update(string json)
        {
            var urlJson = string.Format("{0}DNLevelRoom/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }
        public int Show(string lstArrId)
        {
            var urlJson = string.Format("{0}DNLevelRoom/Show?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }
        public int Hide(string lstArrId)
        {
            var urlJson = string.Format("{0}DNLevelRoom/Hide?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }
        public int Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}DNLevelRoom/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }
    }
}
