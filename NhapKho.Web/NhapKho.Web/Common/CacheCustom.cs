using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NhapKho.Web.Common
{
    public class CacheCustomObject
    {
        private static CacheCustomObject instance = null;
        private Dictionary<string, List<object>> cache = new Dictionary<string, List<object>>();
        public static CacheCustomObject Instance
        {
            get { return instance ?? (instance = new CacheCustomObject()); }
        }

        public List<object> GetOrAdd(string key, object obj)
        {
            if (cache.ContainsKey(key))
            {
                var item = cache[key];
                item.Add(obj);
            }
            else
            {
                cache.Add(key, new List<object>() { obj });
            }

            return cache[key];
        }
        public bool Remove(string key, object value)
        {
            if (!cache.ContainsKey(key)) return false;
            var item = cache[key];
            item.Remove(value);
            return true;
        }
        public bool Remove(string key)
        {
            return cache.Remove(key);
        }
        public bool Remove(int key)
        {
            return cache.Remove(key.ToString());
        }
        public List<object> Get(string key)
        {
            if (cache.ContainsKey(key))
            {
                return cache[key];
            }
            return new List<object>();
        }
        public List<object> Get(int key)
        {
            if (cache.ContainsKey(key.ToString()))
            {
                return cache[key.ToString()];
            }
            return new List<object>();
        }

        public bool CheckExist(string key, string value)
        {
            return cache.ContainsKey(key) && cache[key].Any(m => m == value);
        }

    }
}