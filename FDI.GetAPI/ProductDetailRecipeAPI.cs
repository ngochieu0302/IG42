using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;
using FDI.Simple;
using FDI.Simple.Supplier;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class ProductDetailRecipeAPI:BaseAPI
    {
        public ModelProductDetailRecipeItem ListItems(string url)
        {
            var urlJson = string.Format("{0}ProductDetailRecipe/ListItems?key={1}&{2}", Domain, Keyapi, url);
            return GetObjJson<ModelProductDetailRecipeItem>(urlJson);
        }
        public ProductDetailRecipeItem GetItembyId(int id)
        {
            var urlJson = string.Format("{0}ProductDetailRecipe/GetItembyId?key={1}&id={2}", Domain, Keyapi,id);
            return GetObjJson<ProductDetailRecipeItem>(urlJson);
        }
        public List<ProductDetailRecipeItem> GetAll()
        {
            var urlJson = string.Format("{0}ProductDetailRecipe/GetAll?key={1}", Domain, Keyapi);
            return GetObjJson<List<ProductDetailRecipeItem>>(urlJson);
        }
        public JsonMessage Add(string json,string codelogin,Guid userId)
        {
            var urlJson = string.Format("{0}ProductDetailRecipe/Add?key={1}&codelogin={2}&userId={3}&{4}", Domain, Keyapi, codelogin, userId, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Update(string json,string codelogin,Guid userId)
        {
            var urlJson = string.Format("{0}ProductDetailRecipe/Update?key={1}&codelogin={2}&userId={3}&{4}", Domain, Keyapi, codelogin, userId, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Active(string id,Guid userId)
        {
            var urlJson = string.Format("{0}ProductDetailRecipe/Active?key={1}&itemID={2}&userId={3}", Domain, Keyapi, id, userId);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}ProductDetailRecipe/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
        //public async Task<JsonMessage> Add(ProductDetail_Recipe request)
        //{
        //    var urlJson = $"{Domain}ProductDetailRecipe/Add";
        //    return await PostDataAsync<JsonMessage>(urlJson, request);
        //}
        //public async Task<JsonMessage> Update(ProductDetail_Recipe request)
        //{
        //    var urlJson = $"{Domain}ProductDetailRecipe/Update";
        //    return await PostDataAsync<JsonMessage>(urlJson, request);
        //}
        //public async Task<JsonMessage> Delete(ProductDetail_Recipe request)
        //{
        //    var urlJson = $"{Domain}ProductDetailRecipe/Delete";
        //    return await PostDataAsync<JsonMessage>(urlJson, request);
        //}
    }
}
