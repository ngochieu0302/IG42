using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using FDI.Simple;

namespace FDI.Areas.Admin.Controllers
{
    public class ExecuteController : BaseController
    {

        public SqlConnection Getconnection { get; private set; }

        public ExecuteController()
        {
            DbConnect();
        }

        //public ActionResult GetIndex(int dropDownList = 0, string executeQuery = "")
        //{
        //    var executeItem = new ExecuteItem
        //    {
        //        DropDownList = dropDownList,
        //        ExecuteQuery = executeQuery
        //    };
        //    return View(executeItem);
        //}

        public ActionResult Index()
        {
            if (!SystemActionItem.IsAdmin) return View("Index");
            var executeItem = new ExecuteItem();
            UpdateModel(executeItem);
            var model = new ExecuteQueryItem { ExecuteItem = executeItem,Erros = false};
            try
            {
                if (executeItem.DropDownList == 1)
                {
                    model.DataTable = Gets(executeItem.ExecuteQuery);
                    model.Message = "Câu lệnh đã được thực thi ...";
                }
                if (executeItem.DropDownList == 2)
                {
                    ExecuteQuerys(executeItem.ExecuteQuery);
                    model.DataTable = new DataTable();
                    model.Message = "Câu lệnh đã được thực thi ...";
                }
            }
            catch (Exception ex)
            {
                model.Message = ex.Message;
                model.Erros = true;
            }
            return View(model);
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

        public void ExecuteQuerys(string query)
        {
            if (Getconnection.State == ConnectionState.Closed)
                Getconnection.Open();
            var sql = query;
            var cmd = new SqlCommand(sql, Getconnection);
            cmd.ExecuteNonQuery();
        }

        public DataTable Gets(string query)
        {
            if (Getconnection.State == ConnectionState.Closed)
                Getconnection.Open();
            var sql = query;
            var da = new SqlDataAdapter(sql, Getconnection);
            var ds = new DataTable();
            da.Fill(ds);
            Getconnection.Close();
            return ds;
        }

    }
}
