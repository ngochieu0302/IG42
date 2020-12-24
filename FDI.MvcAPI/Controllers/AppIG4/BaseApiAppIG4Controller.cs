using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;
using Newtonsoft.Json;

namespace FDI.MvcAPI.Controllers
{
    public class BaseApiAppIG4Controller : Controller
    {
        private readonly CustomerDA _customerDa = new CustomerDA("#");
        
        public static string UrlG = ConfigurationManager.AppSettings["Url"];
        public static string UrlCustomer = ConfigurationManager.AppSettings["UrlCustomer"];
        public static string UrlNode = ConfigurationManager.AppSettings["UrlNote"];
        public string ConnectString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        protected string Keyapi = "Fdi@123";
        public SqlConnection Getconnection { get; private set; }
        public int EnterprisesId()
        {
            //codee
            var id = Request["enterprisesId"];
            return id != null ? int.Parse(id) : 0;
        }
        public static T GetObjJson<T>(string url) where T : new()
        {
            var data = new WebClient();
            try
            {
                data.Encoding = Encoding.UTF8;
                var datas = data.DownloadString(url);
                return JsonConvert.DeserializeObject<T>(datas);
            }
            catch (Exception)
            {
                return new T();
            }
        }
        public int Agencyid()
        {
            var id = Request["agencyId"];
            return id != null ? int.Parse(id) : 0;
        }
        public Guid? UserId()
        {
            var guid = Request["UserId"];
            return guid != null ? Guid.Parse(guid) : new Guid();
        }
        
        protected int ItemId
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["ItemID"])) return Convert.ToInt32(Request["ItemID"]);
                return -1;
            }
        }
        
        public void DbConnect()
        {
            try
            {
                var strCon = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                Getconnection = new SqlConnection(strCon);
                Getconnection.Open();
            }
            catch (Exception)
            {
                throw new Exception("Lỗi kết nối DataBase !");
            }
        }
        public DataTable Gets(string query)
        {
            var sql = query;
            var da = new SqlDataAdapter(sql, Getconnection);
            var ds = new DataTable();
            da.Fill(ds);
            Getconnection.Close();
            return ds;
        }
    }
}