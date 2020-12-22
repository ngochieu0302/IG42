using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NhapKho.Web.Common
{
    public class CacheCustom
    {
        private static CacheCustom instance = null;
        private Dictionary<string, List<string>> cache = new Dictionary<string, List<string>>();
        public static CacheCustom Instance
        {
            get { return instance ?? (instance = new CacheCustom()); }
        }

        public List<string> GetOrAdd(string key, string value)
        {
            if (cache.ContainsKey(key))
            {
                var item = cache[key];
                item.Add(value);
            }
            else
            {
                cache.Add(key, new List<string>() { value });
            }

            return cache[key];
        }
        public bool Remove(string key, string value)
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
        public List<string> Get(string key)
        {
            if (cache.ContainsKey(key))
            {
                return cache[key];
            }
            return new List<string>();
        }
        public List<string> Get(int key)
        {
            if (cache.ContainsKey(key.ToString()))
            {
                return cache[key.ToString()];
            }
            return new List<string>();
        }

        public bool CheckExist(string key, string value)
        {
            return cache.ContainsKey(key) && cache[key].Any(m => m == value);
        }

    }
}