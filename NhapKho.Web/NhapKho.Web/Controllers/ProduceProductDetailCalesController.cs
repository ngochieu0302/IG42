using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Printing;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using FDI.CORE;
using FDI.DA;
using FDI.GetAPI;
using FDI.GetAPI.StorageWarehouse;
using FDI.Simple;
using FDI.Simple.Logistics;
using FDI.Simple.StorageWarehouse;
using log4net;
using Newtonsoft.Json.Linq;
using NhapKho.Web.Common;
using NhapKho.Web.Models;
using Quobject.SocketIoClientDotNet.Client;
using Serilog;
using JsonMessage = FDI.Utils.JsonMessage;

namespace NhapKho.Web.Controllers
{
    public class ProduceProductDetailCalesController : Controller
    {
        private readonly CateRecipeAPI _cateRecipeApi = new CateRecipeAPI();
        private readonly ProduceAPI _produceApi = new ProduceAPI(new DNUserItem() { UserId = new Guid("E1DB6D69-865B-4F05-ACBA-B62498DB3B8F") });
        private readonly ShopProductDetailAPI _shopProductDetailApi = new ShopProductDetailAPI();
        public static string Com = ConfigurationManager.AppSettings["Com"];
        private const string Prefix = "CalesProduct";

        public static int count = 0;
        public Socket socket;
        public Socket socket2;

        public static int value = 0;
        public static string code;
        public static int productId;
        private static bool readly = true;
        private static string ProductName = "";
        private static FDI.Simple.ShopProductDetailItem product;


        ILog log = log4net.LogManager.GetLogger(typeof(HomeController));

        List<FDI.Simple.ShopProductDetailItem> getProduct()
        {
            var lst = new List<FDI.Simple.ShopProductDetailItem>();
            lst.Add(new FDI.Simple.ShopProductDetailItem()
            {
                Name = "Xúc xích xay thô",
                Price = 270000,
                ID = 1,
            });
            lst.Add(new FDI.Simple.ShopProductDetailItem()
            {
                ID = 2,
                Name = "Thịt lợn xông khói",
                Price = 265000,
            });
            lst.Add(new FDI.Simple.ShopProductDetailItem()
            {
                ID = 3,
                Name = "Thịt chân giò xông khói",
                Price = 295000,
            });
            lst.Add(new FDI.Simple.ShopProductDetailItem()
            {
                ID = 4,
                Name = "Jăm bông",
                Price = 280000,
            });
            lst.Add(new FDI.Simple.ShopProductDetailItem()
            {
                ID = 5,
                Name = "Salami",
                Price = 690000,
            });
            lst.Add(new FDI.Simple.ShopProductDetailItem()
            {
                ID = 6,
                Name = "Giò nạc",
                Price = 250000,
            });
            return lst;
        }
        public ActionResult Index(string code = "")
        {
            ProduceProductDetailCalesController.code = code;

            if (socket == null)
            {
                socket = IO.Socket("http://localhost:3000");
                socket2 = IO.Socket("http://localhost:3001");
            }
            else
            {
                socket.Close();
                socket.Disconnect();
                socket = IO.Socket("http://localhost:3000");

                socket2.Close();
                socket2.Disconnect();
                socket2 = IO.Socket("http://localhost:3001");

            }
            var reader = new ArduinoSerialReaderNew(Com);
            reader.AddTriger(serialPort_DataReceived);
            return View((object)code);
        }
        public ActionResult BarCodeTmp()
        {
            DNImportAPI _da = new DNImportAPI();

            var lst =  _da.GetAll(1006).Take(12).ToList();
            return View(lst);
        }

        public async Task<ActionResult> Detail(string code)
        {
            var lst = getProduct().Select(m => new OrderDetailProductItem()
            {
                ProductId = m.ID,
                ProductName = m.Name
            });
            return View(lst.ToList());

            var response = await _produceApi.GetProductDetail(code);
            if ((response != null || response.Data.Count == 0) && !response.Erros)
            {
                return View(response.Data);
            }

            return View(new List<OrderDetailProductItem>());
        }
        public ActionResult ListItems(string code)
        {
            var lst = CacheCustomObject.Instance.Get($"{Prefix}{productId}{code}").Cast<ProductCales>();

            return View(lst.OrderByDescending(m => m.DateCreate).ToList());
        }

        public ActionResult SetProduct(int id)
        {
            productId = id;

            var lst = getProduct();

            var a = lst.Where(m => m.ID == id).FirstOrDefault();
            product = a;

            //product = _shopProductDetailApi.GetItemById(0, id);
            return Json(product == null ? new JsonMessage(true, "Sản phẩm không tồn tại") : new JsonMessage());
        }

        [HttpPost]
        public async Task<ActionResult> Actions()
        {
            var action = Request["do"];
            switch (action)
            {
                case "add":
                    var lst = CacheCustomObject.Instance.Get($"{Prefix}{productId}{code}").Cast<ProductCales>();
                    var model = new List<ImportProductItem>();

                    foreach (var productCalese in lst)
                    {
                        model.Add(new ImportProductItem()
                        {
                            BarCode = productCalese.Code,
                            Quantity = 1,
                            Value = productCalese.Weight,
                            Date = productCalese.DateCreate,
                            DateEnd = productCalese.DateExpire,
                            Price = productCalese.PriceUnit,
                            PriceNew = productCalese.Price,
                            Code = code
                        });
                    }

                    var result = await _produceApi.InsertProductDetail(model);
                    if (result != null && !result.Erros)
                    {
                        CacheCustomObject.Instance.Remove($"{Prefix}{productId}{code}");
                    }
                    return Json(result);

                case "delete":
                    CacheCustomObject.Instance.Remove($"{Prefix}{productId}{code}", new ProductCales() { Code = Request["itemId"] });
                    return Json(new JsonMessage() { Message = "Đã xóa" });
            }

            return Json(new JsonMessage() { Message = "Đã xóa" });
        }
        void serialPort_DataReceived(object s, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort sp = (SerialPort)s;
                var text = sp.ReadLine();
                var final = text.Split(',')[0];
                text = text.Replace("\0", "");
                var start = text.IndexOf("+", StringComparison.Ordinal);
                var end = text.IndexOf("\r", StringComparison.Ordinal);
                var bien = text.Substring(start + 1, end - start - 1).Replace("g", "").Trim();

                socket.Emit("pingScalesCate", bien);

                if (final == "ST" && int.TryParse(bien, out int i))
                {
                    if (i <= 0)
                    {
                        readly = true;
                    }
                    if (i == value)
                    {
                        count += 1;
                    }
                    else
                    {
                        count = 0;
                        value = i;
                    }
                }
                else
                {
                    count = 0;
                }

                if (count == 2 && value > 0 && productId > 0 && readly)
                {
                    readly = false;
                    if (product == null)
                    {
                        return;
                    }
                    var h = DateTime.Now.Hour;
                    var time = 18;
                    if (h < 4) time = 4;
                    else if (h < 8) time = 8;
                    else if (h < 15) time = 15;

                    var data = new ProductCales()
                    {
                        Weight = value,
                        ProductId = productId,
                        Code = StringExtensions.GetCode(),
                        DateCreate = DateTime.Now.TotalSeconds(),
                        DateStr = DateTime.Today.AddHours(time).ToString("dd/MM/yyyy"),
                        DateExpire = DateTime.Today.AddHours(time + 6).TotalSeconds(),
                        DateExpireStr = DateTime.Today.AddDays(90).ToString("dd/MM/yyyy"),
                        Name = product.Name,
                        PriceUnit = product.Price ?? 0
                    };
                    var obj = JObject.FromObject(data);
                    socket.Emit("calesDetailvalue", obj);
                    CacheCustomObject.Instance.GetOrAdd($"{Prefix}{productId}{code}", data);
                }
            }
            catch (Exception exception)
            {
                log.Error(exception);
            }
        }
    }
}
