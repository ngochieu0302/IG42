using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class ProductTypeAPI : BaseAPI
    {
        private readonly string _url = Domain;
        public ModelProductTypeItem ListItems(string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}DNProductType/ListItems{1}&key={2}", _url, url, Keyapi);
            return GetObjJson<ModelProductTypeItem>(urlJson);
        }

        public List<ProductTypeItem> GetAll()
        {
            var urlJson = string.Format("{0}DNProductType/GetAll?key={1}", _url, Keyapi);
            return GetObjJson<List<ProductTypeItem>>(urlJson);
        }

        public ProductTypeItem GetItemById(int id = 0)
        {
            var urlJson = string.Format("{0}DNProductType/GetItemById?key={1}&id={2}", _url, Keyapi,id);
            return GetObjJson<ProductTypeItem>(urlJson);
        }

        public List<ProductTypeItem> GetListByArrId(int agencyid, string lstId)
        {
            var urlJson = string.Format("{0}DNProductType/GetListByArrId?key={1}&lstId={2}", _url, Keyapi, lstId);
            return GetObjJson<List<ProductTypeItem>>(urlJson);
        }

        public int Add( string json)
        {
            var urlJson = string.Format("{0}DNProductType/Add?key={1}&json={2}", _url, Keyapi,json);
            return GetObjJson<int>(urlJson);
        }

        public int Update(string json, int id = 0)
        {
            var urlJson = string.Format("{0}DNProductType/Update?key={1}&json={2}&id={3}", _url, Keyapi, json, id);
            return GetObjJson<int>(urlJson);
        }

		public int Delete(string lstArrId)
		{
            var urlJson = string.Format("{0}DNProductType/Delete?key={1}&listint={2}", _url, Keyapi, lstArrId);
			return GetObjJson<int>(urlJson);
		}
	}
}
