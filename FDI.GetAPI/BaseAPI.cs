using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using FDI.Utils;
using FDI.Memcached;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using FDI.Simple;
using System.Web;

namespace FDI.GetAPI
{
    public class BaseAPI
    {
        protected static string Keyapi = "Fdi@123";
        public static string Domain = Utility.DomaApi;
        public static string DomainSv = Utility.ApiSv;
        public static DNUserItem UserItem { get; set; }
        protected bool IsAdmin { get; set; }
        //public static string Domain = WebConfig.UrlJson;
        protected readonly CacheController Cache = CacheController.GetInstance();

        public BaseAPI()
        {
            var code = HttpContext.Current.Request.Cookies["CodeLogin"];
            if (code != null)
            {
                var key = $"DNLoginAPIGetUserItemByCode_{code.Value}";
                if (Cache.KeyExistsCache(key))
                {
                    UserItem = (DNUserItem)Cache.GetCache(key);
                    var keyCache = "ltsPermissionrole" + code; // ltsPermissionProductAttribute
                    if (HttpRuntime.Cache[keyCache] == null)
                        HttpRuntime.Cache[keyCache] = CheckAdmin(UserItem.listRole);
                    IsAdmin = (bool)HttpRuntime.Cache[keyCache];
                }
            }
        }

        public BaseAPI(DNUserItem user)
        {
            UserItem = user;
        }
        public bool CheckAdmin(IEnumerable<string> listRole)
        {
            var lstAdmin = WebConfig.ListAdmin;
            var lstAdminArr = lstAdmin.Split(',');
            return lstAdminArr.Any(role => listRole.Any(m => m.ToLower() == role.ToLower()));
        }
        protected static string GetUrlJson(string url)
        {
            var data = new WebClient();
            try
            {
                data.Encoding = Encoding.UTF8;
                var datas = data.DownloadString(url);
                return datas;
            }
            catch (Exception ex)
            {
                return " ";
                Log2File.LogExceptionToFile(ex);
            }
        }

        protected static T GetObjJson<T>(string url) where T : new()
        {
            var data = new WebClient();
            data.Headers.Add("x-access-key", Keyapi);
            try
            {
                data.Encoding = Encoding.UTF8;
                var datas = data.DownloadString(url);

                return JsonConvert.DeserializeObject<T>(datas);
            }
            catch (Exception ex)
            {
                return new T();
                Log2File.LogExceptionToFile(ex);
            }
        }

        public async Task<T> PostDataAsync<T>(string url, object data)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-access-key", Keyapi);
            if (UserItem != null)
            {
                client.DefaultRequestHeaders.Add("x-access-userId", UserItem.UserId.ToString());
            }


            client.DefaultRequestHeaders.Add("x-access-isadmin", IsAdmin.ToString());

            if (data == null)
            {
                data = new object();
            }
            var json = JsonConvert.SerializeObject(data);
            var postdata = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, postdata);
            if (response.IsSuccessStatusCode)
            {
                var datas = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(datas);
            }
            return default(T);
        }
        public async Task<T> PostDataAsync<T>(string url, object data,int id)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-access-key", Keyapi);
            if (UserItem != null)
            {
                client.DefaultRequestHeaders.Add("x-access-userId", UserItem.UserId.ToString());
            }
            if (id > 0)
            {
                client.DefaultRequestHeaders.Add("x-access-ItemID", id.ToString());
            }
            client.DefaultRequestHeaders.Add("x-access-isadmin", IsAdmin.ToString());

            if (data == null)
            {
                data = new object();
            }
            var json = JsonConvert.SerializeObject(data);
            var postdata = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, postdata);
            if (response.IsSuccessStatusCode)
            {
                var datas = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(datas);
            }
            return default(T);
        }
        public static async Task<T> PostDataAsync<T>(string url)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-access-key", Keyapi);

            if (UserItem != null)
            {
                client.DefaultRequestHeaders.Add("x-access-userId", UserItem.UserId.ToString());
            }


            var json = JsonConvert.SerializeObject(new object());
            var postdata = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, postdata);
            if (response.IsSuccessStatusCode)
            {
                var datas = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(datas);
            }

            return default(T);
        }
        protected static T GetObjJson<T>(string url, int agencyId) where T : new()
        {
            var data = new WebClient();
            data.Headers.Add("x-access-key", Keyapi);
            data.Headers.Add("x-access-agencyId", agencyId.ToString());
            try
            {
                data.Encoding = Encoding.UTF8;
                var datas = data.DownloadString(url);
                return JsonConvert.DeserializeObject<T>(datas);
            }
            catch (Exception ex)
            {
                return new T();
                Log2File.LogExceptionToFile(ex);
            }
        }

        protected T GetCache<T>(string key, string urlJson, int time) where T : new()
        {
            if (ConfigCache.EnableCache != 1)
                return GetObjJson<T>(urlJson);
            if (Cache.KeyExistsCache(key))
            {
                var data = (T)Cache.GetCache(key);
                if (data == null)
                {
                    Cache.DeleteCache(key);
                    data = GetObjJson<T>(urlJson);
                    Cache.Set(key, data, time);
                }
                return data;
            }
            var datas = GetObjJson<T>(urlJson);
            Cache.Set(key, datas, time);
            return datas;
        }

        protected T GetCacheNow<T>(string key, string urlJson, int time) where T : new()
        {
            if (Cache.KeyExistsCache(key))
            {
                var data = (T)Cache.GetCache(key);
                if (data != null) return data;
                Cache.DeleteCache(key);
                data = GetObjJson<T>(urlJson);
                Cache.Set(key, data, time);
                return data;
            }
            var datas = GetObjJson<T>(urlJson);
            Cache.Set(key, datas, time);
            return datas;
        }
        public List<int> GetPage(List<int> list, int page, int pageSize)
        {
            return list.Skip(page * pageSize).Take(pageSize).ToList();
        }
    }
}
