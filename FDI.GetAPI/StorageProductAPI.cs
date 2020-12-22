using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class StorageProductAPI : BaseAPI
    {
        public List<StorageProductItem> GetListSimple()
        {
            var urlJson = string.Format("{0}StorageProduct/GetListSimple?key={1}", Domain, Keyapi);
            return GetObjJson<List<StorageProductItem>>(urlJson);
        }

        public ModelStorageProductItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}StorageProduct/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi,agencyid);
            return GetObjJson<ModelStorageProductItem>(urlJson);
        }

        public ModelProductItem GetListProductByRequest(int agencyId, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}StorageProduct/GetListProductByRequest{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyId);
            return GetObjJson<ModelProductItem>(urlJson);
        }
        public ModelProductItem GetListProductValueByRequest(int agencyId, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}StorageProduct/GetListProductValueByRequest{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyId);
            return GetObjJson<ModelProductItem>(urlJson);
        }
        public ModelCateValueItem GetListCateValueByRequest(int agencyId, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}StorageProduct/GetListCateValueByRequest{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyId);
            return GetObjJson<ModelCateValueItem>(urlJson);
        }
        public ModelProductItem GetListProductLater(int agencyId, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}StorageProduct/GetListProductLater{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyId);
            return GetObjJson<ModelProductItem>(urlJson);
        }

        public List<StorageProductItem> GetListExcel(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}StorageProduct/GetListExcel{1}&key={2}&agencyId={3}", Domain, url, Keyapi,agencyid);
            return GetObjJson<List<StorageProductItem>>(urlJson);
        }
        
        public StorageProductItem GetStorageProductItem(int id)
        {
            var urlJson = string.Format("{0}StorageProduct/GetStorageProductItem?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<StorageProductItem>(urlJson);
        }
        public List<CateValueItem> GetCateProductValueItem(double quantity,int skip)
        {
            var urlJson = string.Format("{0}StorageProduct/GetCateProductValueItem?key={1}&quantity={2}&skip={3}", Domain, Keyapi, quantity,skip);
            return GetObjJson<List<CateValueItem>>(urlJson);
        }
        public ProductValueItem GetStorageProductValueItem(int id)
        {
            var urlJson = string.Format("{0}StorageProduct/GetStorageProductValueItem?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<ProductValueItem>(urlJson);
        }
        public CateValueItem GetCateValueItem(int id)
        {
            var urlJson = string.Format("{0}StorageProduct/GetCateValueItem?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<CateValueItem>(urlJson);
        }
        public int Add(string url, int agencyid, string code)
        {
            var urlJson = string.Format("{0}StorageProduct/Add?key={1}&agencyId={2}&codeLogin={3}&{4}", Domain, Keyapi, agencyid, code, url);
            return GetObjJson<int>(urlJson);
        }
        public int Update(string url, int agencyid, string code)
        {
            var urlJson = string.Format("{0}StorageProduct/Update?key={1}&agencyId={2}&codeLogin={3}&{4}", Domain, Keyapi, agencyid, code, url);
            return GetObjJson<int>(urlJson);
        }

        public int Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}StorageProduct/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }
    }
}
