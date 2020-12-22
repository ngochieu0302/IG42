using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
	public class ProductValueAPI : BaseAPI
	{
		private readonly string _url = Domain;
		public ModuleShopProductValueItem ListItems(int agencyid, string url)
		{
			url = string.IsNullOrEmpty(url) ? "?" : url;
			var urlJson = string.Format("{0}ProductValue/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi,agencyid);
			return GetObjJson<ModuleShopProductValueItem>(urlJson);
		}
		public List<ShopProductValueItem> GetList(int agencyId)
		{
			var urlJson = string.Format("{0}ProductValue/GetList?key={1}&agencyId={2}", _url, Keyapi, agencyId);
			return GetObjJson<List<ShopProductValueItem>>(urlJson);
		}
		public ShopProductValueItem GetProductValueItem(int id)
		{
			var urlJson = string.Format("{0}ProductValue/GetProductValueItem?key={1}&id={2}", _url, Keyapi, id);
			return GetObjJson<ShopProductValueItem>(urlJson);
		}
		public List<SuggestionsProduct> GetListAuto(string keword, int showLimit, int agencyId)
		{
			var urlJson = string.Format("{0}ProductValue/GetListAuto?key={1}&keword={2}&showLimit={3}&agencyId={4}", _url, Keyapi, keword, showLimit, agencyId);
			return GetObjJson<List<SuggestionsProduct>>(urlJson);
		}

        public int CheckByName(string name, int id, int agencyId)
        {
            var urlJson = string.Format("{0}ProductValue/CheckByName?key={1}&name={2}&id={3}&agencyId={4}", Domain, Keyapi, name, id, agencyId);
            return GetObjJson<int>(urlJson);
        }
        public int Add(string json)
		{
			var urlJson = string.Format("{0}ProductValue/Add?key={1}&{2}", Domain, Keyapi, json);
			return GetObjJson<int>(urlJson);
		}
		public int Update(string json)
		{
			var urlJson = string.Format("{0}ProductValue/Update?key={1}&{2}", Domain, Keyapi, json);
			return GetObjJson<int>(urlJson);
		}
		public int Delete(string lstArrId)
		{
			var urlJson = string.Format("{0}ProductValue/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
			return GetObjJson<int>(urlJson);
		}

	}
}
