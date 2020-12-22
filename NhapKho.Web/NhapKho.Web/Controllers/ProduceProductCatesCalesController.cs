using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FDI.CORE;
using FDI.GetAPI;
using FDI.GetAPI.StorageWarehouse;
using FDI.Simple;
using FDI.Simple.StorageWarehouse;
using log4net;
using Newtonsoft.Json.Linq;
using NhapKho.Web.Common;
using NhapKho.Web.Models;
using Quobject.SocketIoClientDotNet.Client;
using JsonMessage = FDI.Utils.JsonMessage;

namespace NhapKho.Web.Controllers
{

    public class ProduceProductCatesCalesController : Controller
    {
        private readonly CateRecipeAPI _cateRecipeApi = new CateRecipeAPI();
        private readonly ProduceAPI _produceApi = new ProduceAPI(new DNUserItem() { UserId = new Guid("E1DB6D69-865B-4F05-ACBA-B62498DB3B8F") });
        public static string Com = ConfigurationManager.AppSettings["Com"];

        private const string Prefix = "Cales";

        public static int count = 0;
        public static Socket socket;

        public static int value = 0;
        public static string code;
        public static int productId;
        private static bool readly = false;
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
                    if (i == 0)
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
                    var data = new ProductCales() { Weight = value, ProductId = productId, Code = StringExtensions.GetCode() };
                    var obj = JObject.FromObject(data);
                    socket.Emit("calesvalue", obj);
                    CacheCustomObject.Instance.GetOrAdd($"{Prefix}{productId}{code}", data);
                }

            }
            catch (Exception exception)
            {
              
            }
            
        }
        public ActionResult Index(string code = "")
        {
            Dispose();
            ProduceProductCatesCalesController.code = code;
            if (socket == null)
            {
                socket = IO.Socket("http://localhost:3000");

            }
            else
            {
                socket.Close();
                socket.Disconnect();
                socket = IO.Socket("http://localhost:3000");
            }
            var reader = new ArduinoSerialReaderNew(Com);
            reader.AddTriger(serialPort_DataReceived);
            return View((object)code);
        }

        public async Task<ActionResult> Detail(string code)
        {
            var productdetail = await _produceApi.GetProduceDetail(code);

            if (productdetail == null)
            {
                return View(new List<CategoryRecipeItem>());
            }
            var model = await _cateRecipeApi.GetProductCate(productdetail.ProductId);
            return View(model);
        }
        public ActionResult ListItems(string code)
        {
            var lst = CacheCustomObject.Instance.Get($"{Prefix}{productId}{code}").Cast<ProductCales>();

            return View(lst.ToList());
        }

        public ActionResult SetProduct(int id)
        {
            productId = id;
            return Json(new JsonMessage());
        }

        [HttpPost]
        public async Task<ActionResult> Actions()
        {
            var action = Request["do"];
            switch (action)
            {
                case "add":
                    var lst = CacheCustomObject.Instance.Get($"{Prefix}{productId}{code}").Cast<ProductCales>();
                    var model = new List<ProduceCatogoryItem>();
                    foreach (var productCalese in lst)
                    {
                        model.Add(new ProduceCatogoryItem()
                        {
                            ProductId = productCalese.ProductId,
                            ProductOriginalCode = code,
                            Weight = productCalese.Weight,
                            Code = productCalese.Code,
                            IdLog = Request["itemId"]
                        });
                    }

                    var result = await _produceApi.InsertProductCategory(model);
                    if (result != null && !result.Erros)
                    {
                        CacheCustomObject.Instance.Remove($"{Prefix}{productId}{code}");
                    }
                    return Json(result);

                    break;

                case "delete":
                    CacheCustomObject.Instance.Remove($"{Prefix}{productId}{code}", new ProductCales() { Code = Request["itemId"] });
                    return Json(new JsonMessage() { Message = "Đã xóa" });
            }

            return Json(new JsonMessage() { Message = "Đã xóa" });
        }

    }
    public class ArduinoSerialReaderNew : IDisposable
    {
        private static SerialPort _serialPort;
        Dictionary<string, string> lst = new Dictionary<string, string>();
        ILog log = log4net.LogManager.GetLogger(typeof(ArduinoSerialReaderNew));
        public ArduinoSerialReaderNew(string portName)
        {
            try
            {
                if (_serialPort == null)
                {
                    _serialPort = new SerialPort(portName);
                }
                else
                {
                    _serialPort.Close();
                    _serialPort.Dispose();
                    _serialPort = new SerialPort(portName);
                    _serialPort.Open();
                }

                //if (!_serialPort.IsOpen)
                //{
                //    _serialPort.Open();
                //}
                log.Info("ArduinoSerialReaderNew Open");
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        public void AddTriger(SerialDataReceivedEventHandler ev)
        {
            _serialPort.DataReceived += ev;
        }

        public void Dispose()
        {
            log.Info("ArduinoSerialReaderNew Dispose");
            if (_serialPort != null)
            {
                _serialPort.Close();
                _serialPort.Dispose();
            }
        }
    }
}
