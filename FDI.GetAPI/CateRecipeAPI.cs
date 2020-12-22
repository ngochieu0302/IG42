using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class CateRecipeAPI:BaseAPI
    {
        public ModelCateRecipeItem ListItems(string url)
        {
            var urlJson = string.Format("{0}CateRecipe/ListItems?key={1}&{2}", Domain, Keyapi, url);
            return GetObjJson<ModelCateRecipeItem>(urlJson);
        }
        public CateRecipeItem GetItembyId(int id)
        {
            var urlJson = string.Format("{0}CateRecipe/GetItembyId?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<CateRecipeItem>(urlJson);
        }
        public List<CateRecipeItem> GetAll()
        {
            var urlJson = string.Format("{0}CateRecipe/GetAll?key={1}", Domain, Keyapi);
            return GetObjJson<List<CateRecipeItem>>(urlJson);
        }
        public JsonMessage Add(string json, string codelogin, Guid userId)
        {
            var urlJson = string.Format("{0}CateRecipe/Add?key={1}&codelogin={2}&userId={3}&{4}", Domain, Keyapi, codelogin, userId, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Update(string json, string codelogin, Guid userId)
        {
            var urlJson = string.Format("{0}CateRecipe/Update?key={1}&codelogin={2}&userId={3}&{4}", Domain, Keyapi, codelogin, userId, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Active(string id,Guid userId)
        {
            var urlJson = string.Format("{0}CateRecipe/Active?key={1}&itemID={2}&userId={3}", Domain, Keyapi, id, userId);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}CateRecipe/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }

        public async Task<List<CategoryRecipeItem>> GetProductCate(int categoryId)
        {
            var urlJson = $"{Domain}CateRecipe/GetProductCate?categoryId={categoryId}";
            return await PostDataAsync<List<CategoryRecipeItem>>(urlJson);
        }
    }
}
