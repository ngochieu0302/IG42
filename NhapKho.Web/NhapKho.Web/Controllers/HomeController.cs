using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using FDI.Simple;
using FDI.Utils;
using Newtonsoft.Json;
using NhapKho.Web.Models;
using System.Configuration;
using FDI.CORE;

namespace NhapKho.Web.Controllers
{
    public class HomeController : Controller
    {
        protected string Keyapi = "Fdi@123";
        protected int CateID = 2;
        protected int UnitID = 4;
        protected int AgencyID = 1006;
        protected int AreaID = 1;
        protected string domainApi = "http://api.ig4.vn/";
        //protected string domainApi = "http://192.168.100.16:82/";
        //public static string Url = ConfigurationManager.AppSettings["Url"];
        public static string Com = ConfigurationManager.AppSettings["Com"];
        public static double Kl = 0;
        public string Message { get; set; }
        public string ID { get; set; }
        public bool Erros { get; set; }
        Dictionary<string, string> lst = new Dictionary<string, string>();
        //bool? _checkconnect;
        #region Cookie CodeC
        protected string CodeC()
        {
            var codeCookie = Request.Cookies["CodeC"];
            return codeCookie == null ? "" : codeCookie.Value;
        }

        void AddCookies(string name, long val)
        {
            var codeCookie = HttpContext.Request.Cookies[name];
            if (codeCookie == null)
            {
                codeCookie = new HttpCookie(name) { Value = val.ToString(), Expires = DateTime.Now.AddHours(6) };
                Response.Cookies.Add(codeCookie);
            }
            else
            {
                codeCookie.Value = val.ToString();
                codeCookie.Expires = DateTime.Now.AddHours(6);
                Response.Cookies.Set(codeCookie);
            }
        }

        protected string SetCodeC()
        {
            var date = DateTime.Now;
            var code = FDIUtils.RandomCode(5);
            var expires = date.AddHours(3);
            var codeCookie = HttpContext.Request.Cookies["CodeC"];
            if (codeCookie == null)
            {
                codeCookie = new HttpCookie("CodeC") { Value = code, Expires = expires };
                Response.Cookies.Add(codeCookie);
            }
            else
            {
                if (string.IsNullOrEmpty(codeCookie.Value))
                {
                    codeCookie.Value = code;
                    codeCookie.Expires = expires;
                    Response.Cookies.Add(codeCookie);
                }
            }
            return code;
        }
        protected void DeleteCodeC()
        {
            var codeCookie = HttpContext.Request.Cookies["CodeC"];
            if (codeCookie != null)
            {
                codeCookie.Value = "";
                codeCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(codeCookie);
            }
        }
    
        #endregion
        public ActionResult Index()
        {
            SetCodeC();
            Dispose();
            AddCookies("test", DateTime.Now.Ticks);
            //var reader = new ArduinoSerialReader(Com);
            return View();
        }
        public ActionResult ListItems()
        {
            var model = GetListSimple(CodeC());
            return View(model);
        }
        public ActionResult Barcode(int n)
        {
            var code = FDIUtils.RandomCode(n);
            return Json(code, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Inmavach()
        {
            var h = DateTime.Now.Hour;
            var time = 18;
            if (h < 4) time = 4;
            else if (h < 8) time = 8;
            else if (h < 15) time = 15;
            return View(time);
        }
        public ActionResult ActionsAdd(string codep, int pi)
        {
            var code = FDIUtils.RandomCode(8);
            var obj = Add(code, codep, pi);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ActionsIn(string codep, string codei, int pi)
        {
            Add(codei, codep, pi, 2);
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ActionsDelete(string codep, string codei)
        {
            Add(codei, codep, 0, 3);
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ActionsDeleteAll()
        {
            DeleteAll();
            DeleteCodeC();
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ActionsSave()
        {
            var i = Save(CodeC(), 2, 4);
            if (i == 1)
            {
                DeleteCodeC();
                SetCodeC();
                return Json(GetListSimple(CodeC()), JsonRequestBehavior.AllowGet);
            }
            return Json(i, JsonRequestBehavior.AllowGet);
        }

        public int Save(string codec, int cateid, int unitid)
        {
            var urlJson = string.Format("{0}ImportWarehouse/Save?key={1}&codec={2}&agencyid={3}&cateid={4}&unit={5}&areaid={6}", domainApi, Keyapi, CodeC(), AgencyID, cateid, unitid, AreaID);
            var model = GetObjJson<int>(urlJson);
            return model;
        }

        public ImportProductItem Add(string codei, string codep, int pi, int type = 1)
        {
            var urlJson = string.Format("{0}ImportWarehouse/Actions?key={1}&codec={2}&codep={3}&codei={4}&value={5}&type={6}&pi={7}", domainApi, Keyapi, CodeC(), codep, codei, Kl, type, pi);
            var model = GetObjJson<ImportProductItem>(urlJson);
            return model;
        }
        public ImportProductItem DeleteAll()
        {
            var urlJson = string.Format("{0}ImportWarehouse/ActionsDeleteAll?key={1}&codec={2}", domainApi, Keyapi, CodeC());
            var model = GetObjJson<ImportProductItem>(urlJson);
            return model;
        }
        public CateValueAddItem GetListSimple(string codec)
        {
            const string url = "?";
            var urlJson = string.Format("{0}ImportWarehouse/GetListSimpleProductDetail{1}&key={2}&codec={3}", domainApi, url, Keyapi, codec);
            var model = GetObjJson<CateValueAddItem>(urlJson);
            return model;
        }

        public ProductItem GetProductItem(int id)
        {
            var urlJson = string.Format("{0}Product/GetProductItem?key={1}&id={2}", domainApi, Keyapi, id);
            return GetObjJson<ProductItem>(urlJson);
        }

        public ActionResult SelectProduct(int id)
        {
            var model = GetProductItem(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult KLKG()
        {
            return Json(Kl, JsonRequestBehavior.AllowGet);
        }
        protected static T GetObjJson<T>(string url) where T : new()
        {
            var data = new WebClient();
            try
            {
                data.Encoding = Encoding.UTF8;
                var datas = data.DownloadString(url);
                return JsonConvert.DeserializeObject<T>(datas);
            }
            catch (Exception ex)
            {
                return new T();
            }
        }
    }

    public class ArduinoSerialReader : IDisposable
    {
        private readonly SerialPort _serialPort;
        Dictionary<string,string> lst= new Dictionary<string, string>();

        public ArduinoSerialReader(string portName)
        {
            try
            {
                _serialPort = new SerialPort(portName);
                _serialPort.Open();
                _serialPort.DataReceived += serialPort_DataReceived;
            }
            catch (Exception)
            {

            }
        }

        void serialPort_DataReceived(object s, SerialDataReceivedEventArgs e)
        {
            try
            {
                var text = _serialPort.ReadLine();

                var leng = text.Length;
                var bien = text.Substring(8, leng - 11);

                double kla;
                double.TryParse(bien.Trim(), out kla);
                //post http:
                // value final =>in
                HomeController.Kl = kla;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            if (_serialPort != null)
            {
                _serialPort.Close();
                _serialPort.Dispose();
            }
        }
    }
}

public class ProductItem
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string CodeSku { get; set; }
    public decimal? PriceNew { get; set; }
    public string Note { get; set; }
    public decimal? TotalPrice { get; set; }
    public decimal? Quantity { get; set; }
    public decimal? DateImport { get; set; }
    public decimal? Expriry { get; set; }
    public int? Type { get; set; }
}
public class ProductImportItem
{
    public string Name { get; set; }
    public decimal? Price { get; set; }
    public decimal? Quantity { get; set; }
    public int? QuantityDay { get; set; }
    public string CodeSku { get; set; }
}
public class ShopProductDetailItem
{
    public decimal? QuantityDay { get; set; }
    public int? ID { get; set; }
    public decimal? StartDate { get; set; }
    public int? Minutes { get; set; }
    public decimal? Price { get; set; }
    public string Name { get; set; }
    public string NamePicture { get; set; }
    public string NameAscii { get; set; }
    public string Description { get; set; }
    public string Details { get; set; }
    public int? UnitID { get; set; }
    public bool? IsHot { get; set; }
    public bool? IsShow { get; set; }
    public int? PictureID { get; set; }
    public string UrlPicture { get; set; }
    public string Code { get; set; }
    public string ListCateId { get; set; }
    public string UnitName { get; set; }
    public virtual IEnumerable<ProductItem> ListProductItem { get; set; }

}
public class ModelProductItem
{
    public IEnumerable<ProductItem> ListItem { get; set; }
    public IEnumerable<ShopProductDetailItem> ListItemDetail { get; set; }
    public ProductItem ProductItem { get; set; }
    public decimal? TotalOld { get; set; }
    public int Quantity { get; set; }
}