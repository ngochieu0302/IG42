using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Simple;

namespace FDI.GetAPI.StorageWarehouse
{
    public class StorageFreightWarehouseAPI:BaseAPI
    {
        public ModelStorageFreightWarehouseItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}StorageFreightWarehouse/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyid);
            return GetObjJson<ModelStorageFreightWarehouseItem>(urlJson);
        }
        public ModelStorageFreightWarehouseItem ListItemsAll(string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}StorageFreightWarehouse/ListItemsAll{1}&key={2}", Domain, url, Keyapi);
            return GetObjJson<ModelStorageFreightWarehouseItem>(urlJson);
        }
        public ModelStorageFreightWarehouseItem ListItemsRecive(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}StorageFreightWarehouse/ListItemsRecive{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyid);
            return GetObjJson<ModelStorageFreightWarehouseItem>(urlJson);
        }
        public List<StorageFreightWarehouseItemNew> ListItemsNotActive(bool active)
        {
            var urlJson = string.Format("{0}StorageFreightWarehouse/ListItemsNotActive?&key={1}&active={2}", Domain, Keyapi, active);
            return GetObjJson<List<StorageFreightWarehouseItemNew>>(urlJson);
        }
        public List<StorageFreightWarehouseItem> GetListExcel(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}StorageFreightWarehouse/GetListExcel{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyid);
            return GetObjJson<List<StorageFreightWarehouseItem>>(urlJson);
        }
        public List<SuggestionsProduct> GetListAuto(string keword, int showLimit, int agencyId, int type = 0)
        {
            var urlJson = string.Format("{0}StorageFreightWarehouse/GetListAuto?key={1}&keword={2}&showLimit={3}&agencyId={4}&type={5}", Domain, Keyapi, keword, showLimit, agencyId, type);
            return GetObjJson<List<SuggestionsProduct>>(urlJson);
        }
        public StorageFreightWarehouseItem GetStorageFreightWarehousesItem(int id)
        {
            var urlJson = string.Format("{0}StorageFreightWarehouse/GetStorageFreightWarehousesItem?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<StorageFreightWarehouseItem>(urlJson);
        }
        public int Add(string url, int agencyid, string code,string port)
        {
            var urlJson = string.Format("{0}StorageFreightWarehouse/Add?key={1}&agencyId={2}&codeLogin={3}&port={4}&{5}", Domain, Keyapi, agencyid, code,port, url);
            return GetObjJson<int>(urlJson);
        }
        public int Imported(string url, int agencyid, string code)
        {
            var urlJson = string.Format("{0}StorageFreightWarehouse/Imported?key={1}&agencyId={2}&codeLogin={3}&{4}", Domain, Keyapi, agencyid, code, url);
            return GetObjJson<int>(urlJson);
        }
        public int Update(string url, int agencyid, string code)
        {
            var urlJson = string.Format("{0}StorageFreightWarehouse/Update?key={1}&agencyId={2}&codeLogin={3}&{4}", Domain, Keyapi, agencyid, code, url);
            return GetObjJson<int>(urlJson);
        }
        public List<SuggestionsProduct> GetListAutoOne(string keword, int showLimit, int agencyId, int type = 0)
        {
            var urlJson = string.Format("{0}Product/GetListAutoOne?key={1}&keword={2}&showLimit={3}&agencyId={4}&type={5}", Domain, Keyapi, keword, showLimit, agencyId, type);
            return GetObjJson<List<SuggestionsProduct>>(urlJson);
        }
        public int UpdateActive(string url,string code,Guid userId)
        {
            var urlJson = string.Format("{0}StorageFreightWarehouse/UpdateActive?key={1}&codeLogin={2}&userIdActive={3}&{4}", Domain, Keyapi,  code, userId, url);
            return GetObjJson<int>(urlJson);
        }
        public int ActiveFrei(string lstArrId, Guid userId)
        {
            var urlJson = string.Format("{0}StorageFreightWarehouse/ActiveFrei?key={1}&userId={2}&lstArrId={3}", Domain, Keyapi, userId, lstArrId);
            return GetObjJson<int>(urlJson);
        }
        public int Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}StorageFreightWarehouse/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }
    }
}
