using FDI.Simple;
using System.Collections.Generic;
using System.Threading.Tasks;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class SupplieAPI : BaseAPI
    {
        public ModelDNSupplierItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Supplie/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyid);
            return GetObjJson<ModelDNSupplierItem>(urlJson);
        }
        public ModelDNSupplierItem ListItemsStatic(int areaId, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Supplie/ListItemsStatic{1}&key={2}&areaId={3}", Domain, url, Keyapi, areaId);
            return GetObjJson<ModelDNSupplierItem>(urlJson);
        }
        public ModelDNSupplierItem ListItemsGeneralDebt(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Supplie/ListItemsGeneralDebt{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyid);
            return GetObjJson<ModelDNSupplierItem>(urlJson);
        }
        public List<SupplieItem> GetList(int agencyid)
        {
            var urlJson = string.Format("{0}Supplie/GetList?key={1}&agencyId={2}", Domain, Keyapi, agencyid);
            return GetObjJson<List<SupplieItem>>(urlJson);
        }
        public List<SupplieItem> GetList()
        {
            var urlJson = $"{Domain}Supplie/GetList";
            return GetObjJson<List<SupplieItem>>(urlJson);
        }
        public DNSupplierItem GetItemById(int agencyid, int id)
        {
            var urlJson = string.Format("{0}Supplie/GetItemById?key={1}&agencyId={2}&id={3}", Domain, Keyapi, agencyid, id);
            return GetObjJson<DNSupplierItem>(urlJson);
        }
        public SupplieItem GetItemById(int id)
        {
            var urlJson = $"{Domain}Supplie/GetItemById?key={Keyapi}&id={id}";
            return GetObjJson<SupplieItem>(urlJson);
        }

        public int Add(int agencyId, string json)
        {
            var urlJson = string.Format("{0}Supplie/Add?key={1}&agencyId={2}&{3}", Domain, Keyapi, agencyId, json);
            return GetObjJson<int>(urlJson);
        }
        public int Update(string json)
        {
            var urlJson = string.Format("{0}Supplie/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<int>(urlJson);
        }

        public int Show(string lstArrId)
        {
            var urlJson = string.Format("{0}Supplie/Show?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }

        public int Hide(string lstArrId)
        {
            var urlJson = string.Format("{0}Supplie/Hide?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }
        public int Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}Supplie/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }
        public ModelDNSupplierItem GetListExport(int agencyId, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Supplie/GetListExport{1}&key={2}&agencyId={3}", Domain, url, Keyapi, agencyId);
            return GetObjJson<ModelDNSupplierItem>(urlJson);
        }

        public async Task<List<SupplieItem>> GetByName(string name)
        {
            var urlJson = $"{Domain}Supplie/GetByName?&name={name}";
            return await PostDataAsync<List<SupplieItem>>(urlJson, null);
        }

        public async Task<BaseResponse<bool>> AddProduct(SupplieProductItem request)
        {
            var urlJson = $"{Domain}Supplie/AddProduct";
            return await PostDataAsync<BaseResponse<bool>>(urlJson, request);
        }
  
        public SupplieProductResponse GetListSupplierProductById(int id, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = $"{Domain}Supplie/GetListSupplierProductById{url}&id={id}";
            return GetObjJson<SupplieProductResponse>(urlJson);
        }
        public SupplieProductItem GetSupplierProductById(int id)
        {
            var urlJson = $"{Domain}Supplie/GetSupplierProductById?id={id}";
            return GetObjJson<SupplieProductItem>(urlJson);
        }

    }
}