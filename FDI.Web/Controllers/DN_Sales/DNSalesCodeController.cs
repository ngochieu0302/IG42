using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.GetAPI.DN_Sales;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers.DN_Sales
{
    public class DNSalesCodeController : BaseController
    {
        //
        // GET: /DNSalesCode/
        readonly DNSalesAPI _api = new DNSalesAPI();
        public ActionResult Index()
        {
            var id = Request["id"];
            if (string.IsNullOrEmpty(id))
            {
                return Redirect("DNSales/Index");
            }
            ViewBag.id = int.Parse(id);
            return View();
        }
        public ActionResult ListItems(int id)
        {
            return View(_api.ListItemsCode(UserItem.AgencyID, Request.Url.Query, id));
        }
        public ActionResult AjaxForm(int id)
        {
            var model = _api.GetDNSalesItem(id);
            ViewBag.id = id;
            ViewBag.Action = DoAction;
            return View(model);
        }

        public ActionResult Actions()
        {
            try
            {
                var table = new DataTable();
                table.Columns.Add("code", typeof(string));
                table.Columns.Add("saleId", typeof(int));
                table.Columns.Add("isUse", typeof(bool));
                var preCode = Request["Pre_Code"];
                var lenght = Request["LenghtChar"];
                var quantity = Request["QuantityCode"];
                var saleId = ArrId.FirstOrDefault();
                for (int i = 1; i <= int.Parse(quantity); i++)
                {
                    var code = FDIUtils.RandomKey(int.Parse(lenght ?? "6"));
                    var model = new SaleCode
                    {
                        Code = preCode + code,
                        SaleID = saleId,
                        IsUse = false
                    };
                    table.Rows.Add(model.Code, model.SaleID, model.IsUse);
                }
                sp_InsertUpdate(WebConfig.ConnectString, "sp_InsertSalesCode", "@tbl", table);
                var msg = new JsonMessage
                {
                    Erros = false,
                    Message = "Tạo mã thành công.!"
                };
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                var msg = new JsonMessage
                {
                    Erros = true,
                    Message = "Có lỗi xảy ra.!"
                };
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }
        protected void sp_InsertUpdate(string str, string storename, string tbType, DataTable table)
        {
            using (var con = GetConnect(str))
            {
                using (var cmd = new SqlCommand(storename))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    cmd.CommandTimeout = 1000;
                    cmd.Parameters.AddWithValue(tbType, table);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        protected SqlConnection GetConnect(string str)
        {
            try
            {
                return new SqlConnection(str);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
