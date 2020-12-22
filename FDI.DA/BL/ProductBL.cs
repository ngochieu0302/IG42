using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class ProductBL : BaseBL
    {
        private readonly ProductDL _dl = new ProductDL();
        public List<ProductItem> GetList()
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetList();
            var key = string.Format("ProductBLGetList");
            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<ProductItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetList();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetList();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public List<ShopProductDetailItem> GetListProductDetails(int cate, int page, int rowPage, ref int total)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetListProductDetails(cate, page, rowPage, ref total);
            var key = string.Format("ProductBL_GetListProductDetails_{0}_{1}", page, cate);
            var keytotal = key + "_total";
            if (Cache.KeyExistsCache(key) && Cache.KeyExistsCache(keytotal))
            {
                var lst = (List<ShopProductDetailItem>)Cache.GetCache(key);
                var number = Cache.GetCache(keytotal);
                if (lst == null || number == null)
                {
                    Cache.DeleteCache(key);
                    lst = _dl.GetListProductDetails(cate, page, rowPage, ref total);
                    Cache.Set(key, lst, ConfigCache.TimeExpire);
                    Cache.Set(keytotal, total, ConfigCache.TimeExpire);
                    return lst;
                }
                total = (int)number;
                return lst;
            }
            var data = _dl.GetListProductDetails(cate, page, rowPage, ref total);
            Cache.Set(key, data, ConfigCache.TimeExpire);
            Cache.Set(keytotal, total, ConfigCache.TimeExpire);
            return data;
        }
        public List<ProductItem> GetListHome()
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetListHome();
            var key = string.Format("ProductBLGetListHome");
            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<ProductItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetListHome();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetListHome();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
       
        public List<OrderItem> GetListOrder()
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetListOrder();
            var key = string.Format("ProductBLGetListOrder");
            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<OrderItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetListOrder();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetListOrder();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }

        public ShopProductDetailItem GetProductId(int id)
        {
            var key = string.Format("ProductBLGetProductId_{0}", id);
            if (ConfigCache.EnableCache != 1)
                return _dl.GetProductId(id);

            if (Cache.KeyExistsCache(key))
            {
                var lst = (ShopProductDetailItem)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetProductId(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetProductId(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public CategoryItem CategoryItem(int id)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.CategoryItem(id);
            var key = string.Format("ProductBL_CategoryItem_{0}", id);
            if (Cache.KeyExistsCache(key))
            {
                var lst = (CategoryItem)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.CategoryItem(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.CategoryItem(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public CategoryItem GetCateId(int id)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetCateId(id);
            var key = string.Format("ProductBL_GetCateId_{0}", id);
            if (Cache.KeyExistsCache(key))
            {
                var lst = (CategoryItem)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetCateId(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetCateId(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public List<ShopProductDetailItem> GetListCateId(List<int> lstId,int id)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetListCateId(lstId,id);
            var key = string.Format("ProductBL_GetListCateId_{0}", lstId);
            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<ShopProductDetailItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetListCateId(lstId,id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetListCateId(lstId,id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public List<ProductItem> GetListOther(int id, int ortherId)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetListOther(id, ortherId);
            var key = string.Format("NewsBLGetListOther_{0}_{1}", id,ortherId);
            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<ProductItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetListOther(id, ortherId);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetListOther(id, ortherId);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }

       
    }
}