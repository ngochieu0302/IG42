using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using AuthenticationService.Managers;
using AuthenticationService.Models;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.DA.DA.StorageWarehouse;
using FDI.Simple;
using FDI.Simple.Order;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;


using FDI.Utils;

namespace GAP.TEst
{
    [Flags]
    public enum PhoneService : int
    {
        None = 0,       // 00000001
        LandLine = 1,   // 00000010
        Cell = 2,       // 00000100
        Fax = 4,        // 00001000
        Internet = 8,   // 00010000
        Other = 16,     // 00100000

        // LandlineAndCell = LandLine | Cell // Supports combinations in the enum too
    }

    [TestClass]
    public class UnitTest1
    {
        private JsonSerializerSettings jsonsetting = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        [TestMethod]
        public void TestMethod9()
        {

            var a = ConvertDate.TotalSeconds(DateTime.Today);
            ContactOrderDA da = new ContactOrderDA();
            //var lst = da.GetFastOrder(a);

        }

        [TestMethod]
        public void TestMethod8()
        {
            RequestWareDA _requestWareDa = new RequestWareDA();
            var lst = _requestWareDa.GetItemsBySupplierId(new int[] { 5 });
        }


        [TestMethod]
        public void Truncate_WithTags()
        {
            var src = "<p>one two three</p>\r\n<div>four five six seven eight nine ten</div>";

            var expected = "<p>one two three</p>\r\n<div>four&hellip;</div>";

            var result = src.TruncateHtml(20, "&hellip;");
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CustomerDA_GetAllByAgencyId()
        {
            CustomerDA _customerDa = new CustomerDA();
            var dateResult = DateTime.Now;


            DateTime.TryParseExact("dd/MM/yyyy", "dd/MM/yyyy",
                new CultureInfo("pt-BR"),
                DateTimeStyles.None, out dateResult);

            if (DateTime.TryParseExact("3/5/2015", "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateResult))
            {
                var b = 1;
            }

            //   var lst = _customerDa.GetAllByAgencyId(2070, 1);
        }


        [TestMethod]
        public void BoolExtensions_IsNotDelete()
        {
            bool? isdelete = null;
            var b = isdelete.IsNotDelete();
        }

        [TestMethod]
        public void GetProduct()
        {
            CateRecipeDA _cateRecipeDa = new CateRecipeDA();
            var categories = _cateRecipeDa.GetCategoryChild(3);
            var list = _cateRecipeDa.GetProduct(categories);

            var b = _cateRecipeDa.GetProductFinal(list);

            ProduceDa _produceDa = new ProduceDa();
            var lst = _produceDa.GetCategoryRecipe(3);
        }

        [TestMethod]
        public void TestMethod7()
        {

            var arr = File.ReadAllBytes("b.png");
            var d = arr.Select(m => (int)m).ToList();
            var b = JsonConvert.SerializeObject(d);

            var a = ConvertDate.TotalSeconds(DateTime.Now.Date.AddDays(1));
            //var lst = _orderDA.GetListByCustomer(2, 1,1, out var i);

        }

        [TestMethod]
        public void GetTotalSeconds()
        {
            var a = ConvertDate.TotalSeconds(DateTime.Today);

            var b2 = (decimal)32344729;
            var c2 = ConvertDate.DecimalToDate(b2);


            //IAuthContainerModel model = new JWTContainerModel()
            //{
            //    Claims = new Claim[]
            //    {
            //        new Claim(ClaimTypes.Name, "abc"),
            //        new Claim("ID", "93"), 
            //    }
            //};
            //IAuthService authService = new JWTService();
            //var token = authService.GenerateToken(model);
            //var b = authService.IsTokenValid("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjAyOTQyMzkzIiwiSUQiOiI5MyIsIm5iZiI6MTU4OTc3MDg5OSwiZXhwIjoxNTkwMzc1Njk5LCJpYXQiOjE1ODk3NzA4OTl9.uQ4mIkacmm2JAGnZWSUGtf8iI352q6dyagrxu8dOMZI");
            //var c = authService.GetTokenClaims(token);
            //var id = c.Where(m => m.Type == "ID").FirstOrDefault().Value;
        }

        [TestMethod]
        public void TestMethod5()
        {

            var da = new RequestWareDA();
            CacheCustomObject.Instance.GetOrAdd("a", new Test() { id = "1", Name = "N" });
            CacheCustomObject.Instance.GetOrAdd("a", new Test() { id = "2", Name = "N" });
            var b = CacheCustomObject.Instance.CheckExist("a", new Test() { id = "1" });
            var c = CacheCustomObject.Instance.Remove("a", new Test() { id = "1" });
            var lst = CacheCustomObject.Instance.Get("a");
            //   var a=   da.test(41817601);

        }
        [TestMethod]
        public void TestMethod4()
        {
            var da = new RequestWareDA();
            var lst = da.GetTotalProductConfirm(40780800);

        }

        [TestMethod]
        public void TestMethod2()
        {
            var household1 = PhoneService.None | PhoneService.LandLine | PhoneService.Cell | PhoneService.Internet;
            var household2 = PhoneService.None;
            var household3 = PhoneService.Cell | PhoneService.Internet;
            var household4 = PhoneService.LandLine | PhoneService.Internet | PhoneService.Cell | PhoneService.Fax | PhoneService.Other;

            //var a = DateTime.Now.AddDays(4).Date.TotalSeconds();
            var b = household1.HasFlag(PhoneService.LandLine);
            var status = StatusWarehouse.New | StatusWarehouse.Pending | StatusWarehouse.WattingConfirm;

            var current = (StatusWarehouse)1;

            if (!status.HasFlag(current))
            {
                var c = 1;
            }
        }

        [TestMethod]
        public void TestMethod3()
        {
            HttpRequest request = new HttpRequest("", "http://localhost", "a=1");
            var httpRequestBase = new HttpRequestWrapper(request);
            var a = new StorageWareHouseDA();
            var b = a.GetRequestWareSummary(httpRequestBase);
            //  var query = from c in a.StorageWarehousings;

            //  var c1 = a.StorageWarehousings.Where(m => m.ID == 1).FirstOrDefault();
            // var b = query.FirstOrDefault();

        }

        [TestMethod]
        public void TestMethod1()
        {
            //var a = new TokenDeiveItem()
            //{
            //    App = "SPIG4",
            //    UserId = "afd",
            //    Token = "sdf",
            //};
            //   var b1 = JsonConvert.SerializeObject(a);
            var a = new StorageWarehousingRequest()
            {

                TotalPrice = 123,
                ReceiveDate = 100,
                RequestWares = new List<RequestWare>()
                {
                    new RequestWare()
                    {
                        CateId = 3,
                        Price = 5000000,
                        Quantity = 1,
                        Details = new List<RequestWareDetail>()
                        {
                            new RequestWareDetail()
                            {
                                ProductId =1,
                                Quantity = 2
                            }
                        }
                    }
                }
            };

            var b = JsonConvert.SerializeObject(a, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

        }
    }

    public class Test
    {
        public string id { get; set; }
        public string Name { get; set; }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }

            var a = (Test)obj;
            return a.id == id;
        }

    }
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

        public bool CheckExist(string key, object value)
        {
            return cache.ContainsKey(key) && cache[key].Any(m => value.Equals(m));
        }

    }
}
