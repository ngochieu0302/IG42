using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
	public class DNImportAPI : BaseAPI
	{
		public List<StorageItem> GetListSimple(int agencyId)
		{
			var urlJson = string.Format("{0}DNImport/GetListSimple?key={1}&agencyId={2}", Domain, Keyapi, agencyId);
			return GetObjJson<List<StorageItem>>(urlJson);
		}

		public ModelStorageItem ListItems(int agencyid, string url)
		{
			url = string.IsNullOrEmpty(url) ? "?" : url;
			var urlJson = string.Format("{0}DNImport/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi,agencyid);
			return GetObjJson<ModelStorageItem>(urlJson);
		}

        public ModuleShopProductValueItem GetListValueByRequest(int agencyId, string url)
		{
			url = string.IsNullOrEmpty(url) ? "?" : url;
			var urlJson = string.Format("{0}DNImport/GetListValueByRequest{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyId);
            return GetObjJson<ModuleShopProductValueItem>(urlJson);
		}
        public List<ShopProductValueItem> GetListValueView(int agencyId, int id)
        {
            var urlJson = string.Format("{0}DNImport/GetListValueView?key={1}&agencyId={2}&Id={3}", Domain, Keyapi, agencyId,id);
            return GetObjJson<List<ShopProductValueItem>>(urlJson);
        }
		public ModuleShopProductValueItem GetListValueLater(int agencyId, string url)
		{
			url = string.IsNullOrEmpty(url) ? "?" : url;
			var urlJson = string.Format("{0}DNImport/GetListValueLater{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyId);
			return GetObjJson<ModuleShopProductValueItem>(urlJson);
		}

		public StorageItem GetStorageItem(int id)
		{
			var urlJson = string.Format("{0}DNImport/GetStorageItem?key={1}&id={2}", Domain, Keyapi, id);
			return GetObjJson<StorageItem>(urlJson);
		}
        public int Add(string url, int agencyid, string code)
		{
            var urlJson = string.Format("{0}DNImport/Add?key={1}&agencyId={2}&codeLogin={3}&{4}", Domain, Keyapi, agencyid, code, url);
			return GetObjJson<int>(urlJson);
		}
        public int Update(string url, int agencyid, string code)
		{
            var urlJson = string.Format("{0}DNImport/Update?key={1}&agencyId={2}&codeLogin={3}&{4}", Domain, Keyapi, agencyid, code, url);
			return GetObjJson<int>(urlJson);
		}

		public int Delete(string lstArrId)
		{
			var urlJson = string.Format("{0}DNImport/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
			return GetObjJson<int>(urlJson);
		}

        public List<ImportProductItem> GetAll(int agencyId)
        {
            var urlJson = string.Format("{0}ImportProduct/GetAll?key={1}&agencyId={2}", Domain, Keyapi, agencyId);
            return GetObjJson<List<ImportProductItem>>(urlJson);
        }

	}
}
