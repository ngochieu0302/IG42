using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.DA.DL;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class ProductDetailBL : BaseBL
    {
        private readonly ProductDetailDL _dl = new ProductDetailDL();
        public List<ShopProductDetailItem> GetListProductbylstId(string id)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetListProductbylstId(id);
            var key = string.Format("ProductDetailBL-GetListProductbylstId-{0}", id);
            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<ShopProductDetailItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetListProductbylstId(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetListProductbylstId(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public List<ProductDetailsItem> ListAll()
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.ListAll();
            var key = string.Format("ProductDetailBL-ListAll");
            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<ProductDetailsItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.ListAll();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.ListAll();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public ProductDetailsItem GetById(int id)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetById(id);
            var key = string.Format("ProductDetailBL-GetById{0}", id);
            if (Cache.KeyExistsCache(key))
            {
                var lst = (ProductDetailsItem)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetById(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetById(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public List<ProducComingsoonItem> ListProducComingsoonAll(decimal date)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.ListProducComingsoonAll(date);
            var key = string.Format("ProductDetailBL-ListProducComingsoonAll{0}", date);
            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<ProducComingsoonItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.ListProducComingsoonAll(date);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.ListProducComingsoonAll(date);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public List<ShopProductDetailItem> GetListProductbylstCateId(int id)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetListProductbylstCateId(id);
            var key = string.Format("ProductDetailBL-GetListProductbylstCateId-{0}", id);
            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<ShopProductDetailItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetListProductbylstCateId(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetListProductbylstCateId(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public ShopProductDetailItem GetProductbySlug(string slug)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetProductbySlug(slug);
            var key = string.Format("ProductDetailBL-GetProductbySlug-{0}", slug);
            if (Cache.KeyExistsCache(key))
            {
                var lst = (ShopProductDetailItem)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetProductbySlug(slug);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetProductbySlug(slug);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public List<ShopProductDetailItem> GetListHot()
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetListHot();
            var key = string.Format("ProductDetailBL-GetListHot");
            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<ShopProductDetailItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetListHot();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetListHot();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public List<ShopProductDetailItem> GetList(string slug, int page, int rowPage, ref int total, string color, string size, string sort)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetList(slug, page, rowPage, ref total, color, size, sort);
            var key = string.Format("ProductDetail_GetList_{0}-{1}-{2}-{3}-{4}", page, slug, color, size, sort);
            var keytotal = key + "_total";
            if (Cache.KeyExistsCache(key) && Cache.KeyExistsCache(keytotal))
            {
                var lst = (List<ShopProductDetailItem>)Cache.GetCache(key);
                var number = Cache.GetCache(keytotal);
                if (lst == null || number == null)
                {
                    Cache.DeleteCache(key);
                    lst = _dl.GetList(slug, page, rowPage, ref total, color, size, sort);
                    Cache.Set(key, lst, ConfigCache.TimeExpire);
                    Cache.Set(keytotal, total, ConfigCache.TimeExpire);
                    return lst;
                }
                total = (int)number;
                return lst;
            }
            var data = _dl.GetList(slug, page, rowPage, ref total, color, size, sort);
            Cache.Set(key, data, ConfigCache.TimeExpire);
            Cache.Set(keytotal, total, ConfigCache.TimeExpire);
            return data;
        }
        public List<ShopProductDetailItem> GetListByKeyword(string keyword, int page, int rowPage, ref int total, string color, string size, string sort)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetListBykeyword(keyword, page, rowPage, ref total, color, size, sort);
            var key = string.Format("ProductDetail_GetListByKeyword_{0}-{1}-{2}-{3}-{4}", page, keyword, color, size, sort);
            var keytotal = key + "_total";
            if (Cache.KeyExistsCache(key) && Cache.KeyExistsCache(keytotal))
            {
                var lst = (List<ShopProductDetailItem>)Cache.GetCache(key);
                var number = Cache.GetCache(keytotal);
                if (lst == null || number == null)
                {
                    Cache.DeleteCache(key);
                    lst = _dl.GetListBykeyword(keyword, page, rowPage, ref total, color, size, sort);
                    Cache.Set(key, lst, ConfigCache.TimeExpire);
                    Cache.Set(keytotal, total, ConfigCache.TimeExpire);
                    return lst;
                }
                total = (int)number;
                return lst;
            }
            var data = _dl.GetListBykeyword(keyword, page, rowPage, ref total, color, size, sort);
            Cache.Set(key, data, ConfigCache.TimeExpire);
            Cache.Set(keytotal, total, ConfigCache.TimeExpire);
            return data;
        }
    }
}
