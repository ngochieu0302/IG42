using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class PartnersAPI : BaseAPI
    {

        public List<PartnerItem>  GetList(string url,int cateId, int page, ref int total)
        {
            var urlJson = string.Format("{0}Partners/GetList?key={1}&cateId={2}&page={3}&total={4}", url, Keyapi, cateId,page,total);
            return GetObjJson<List<PartnerItem>>(urlJson);
        }

        public PartnerItem GetById(string url, int id)
        {
            var urlJson = string.Format("{0}Partners/GetById?key={1}&id={2}", url, Keyapi, id);
            return GetObjJson<PartnerItem>(urlJson);
        }

        public List<ProductItem> GetProduct(string url, int id)
        {
            var urlJson = string.Format("{0}Partners/GetProduct?key={1}&id={2}", url, Keyapi, id);
            return GetObjJson<List<ProductItem>>(urlJson);
        }
    }
}
