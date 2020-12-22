using FDI.Simple;
using FDI.Utils;
using System.Collections.Generic;

namespace FDI.DA
{
    public class DocumentBL : BaseBL
    {
        private readonly DocumentDl _dl = new DocumentDl();
        public List<DocumentItem> GetList(int page, int cateId,  ref int total)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetList(page, cateId,  ref total);
            var key = string.Format("DocumentDl_GetList_{0}_{1}", page, cateId);
            var keytotal = key + "_total";
            if (Cache.KeyExistsCache(key) && Cache.KeyExistsCache(keytotal))
            {
                var lst = (List<DocumentItem>)Cache.GetCache(key);
                var number = Cache.GetCache(keytotal);
                if (lst == null || number == null)
                {
                    Cache.DeleteCache(key);
                    lst = _dl.GetList(page, cateId, ref total);
                    Cache.Set(key, lst, ConfigCache.TimeExpire);
                    Cache.Set(keytotal, total, ConfigCache.TimeExpire);
                    return lst;
                }
                total = (int)number;
                return lst;
            }
            var data = _dl.GetList(page, cateId, ref total);
            Cache.Set(key, data, ConfigCache.TimeExpire);
            Cache.Set(keytotal, total, ConfigCache.TimeExpire);
            return data;
        }

        public DocumentItem GetById(int id)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetById(id);
            var key = string.Format("DocumentDl_GetById_{0}", id);
            if (Cache.KeyExistsCache(key))
            {
                var lst = (DocumentItem)Cache.GetCache(key);
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
        public List<DocumentItem> GetListByCateId(int id)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetListByCateId(id);
            var key = string.Format("DocumentDl_GetListByCateId_{0}", id);
            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<DocumentItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetListByCateId(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetListByCateId(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }

        public List<DocumentItem> GetListNew()
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetListNew();
            const string key = "DocumentDl_GetListNew";
            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<DocumentItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetListNew();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetListNew();
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
        public DocumentItem GetDocumentItem(int id)
        {
            var key = "DocumentDl_GetDocumentItem" + id;
            if (ConfigCache.EnableCache != 1)
                return _dl.GetDocumentItem(id);
            if (Cache.KeyExistsCache(key))
            {
                var lst = (DocumentItem)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetDocumentItem(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetDocumentItem(id);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }

        public List<DocumentItem> GetListOther(int cateId, int ortherId)
        {
            if (ConfigCache.EnableCache != 1)
                return _dl.GetListOther(cateId, ortherId);
            var key = "NewsBLGetListOther_" + cateId + "_" + ortherId;
            if (Cache.KeyExistsCache(key))
            {
                var lst = (List<DocumentItem>)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetListOther(cateId, ortherId);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetListOther(cateId, ortherId);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
    }
}