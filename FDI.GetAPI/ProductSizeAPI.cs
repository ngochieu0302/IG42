using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class ProductSizeAPI : BaseAPI
    {
        private readonly string _url = Domain;
        public ModelProductSizeItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}DNProductSize/ListItems{1}&key={2}&agencyId={3}", _url, url, Keyapi,agencyid);
            return GetObjJson<ModelProductSizeItem>(urlJson);
        }

        public List<ProductSizeItem> GetAll(int agencyid)
        {
            var urlJson = string.Format("{0}DNProductSize/GetAll?key={1}&agencyId={2}", _url, Keyapi,agencyid);
            return GetObjJson<List<ProductSizeItem>>(urlJson);
        }
        public List<ProductSizeItem> GetAllByUnitID(int agencyid, int? unitID)
        {
            var urlJson = string.Format("{0}DNProductSize/GetAllByUnitID?key={1}&agencyId={2}&unitID={3}", _url, Keyapi,agencyid,unitID);
            return GetObjJson<List<ProductSizeItem>>(urlJson);
        }

        public ProductSizeItem GetItemById(int agencyid, int id = 0)
        {
            var urlJson = string.Format("{0}DNProductSize/GetItemById?key={1}&agencyId={2}&id={3}", _url, Keyapi,agencyid, id);
            return GetObjJson<ProductSizeItem>(urlJson);
        }

        public List<ProductSizeItem> GetListByArrId(int agencyid, string lstId)
        {
            var urlJson = string.Format("{0}DNProductSize/GetListByArrId?key={1}&agencyId={2}&lstId={3}", _url, Keyapi,agencyid, lstId);
            return GetObjJson<List<ProductSizeItem>>(urlJson);
        }

        public int Add(int agencyid, string json)
        {
            var urlJson = string.Format("{0}DNProductSize/Add?key={1}&agencyId={2}&json={3}", _url, Keyapi,agencyid, json);
            return GetObjJson<int>(urlJson);
        }

        public int Update(int agencyid, string json, int id = 0)
        {
            var urlJson = string.Format("{0}DNProductSize/Update?key={1}&agencyId={2}&json={3}&id={4}", _url, Keyapi,agencyid, json, id);
            return GetObjJson<int>(urlJson);
        }

		public int Delete(string lstArrId)
		{
            var urlJson = string.Format("{0}DNProductSize/Delete?key={1}&listint={2}", _url, Keyapi, lstArrId);
			return GetObjJson<int>(urlJson);
		}
	}
}
