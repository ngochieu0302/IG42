using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using FDI.DA.DA;
using FDI.DA.DA.Logistics;
using FDI.MvcAPI.Common;
using FDI.Simple.StorageWarehouse;

namespace FDI.MvcAPI.Controllers
{
    public class TestModel
    {
        public List<Test> Items { get; set; }
    }
    public static class ListFDI
    {
        public static List<string> List()
        {
            var list = new List<string>
            {
                {"Đông"},
                {"Tuấn"},
                {"Liêm"},
                {"Phước"},
                {"Thắm"},
                {"Dịu"},
                {"Chiến"},
                {"Tungtt"},
                {"Quyên"},
                {"Tungcv"},
                {"Huy"},
                {"Đạt"},
            };
            return list;
        }
    }
    public class Test
    {
        public string Name { get; set; }
        public int ID { get; set; }
    }
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var list  = new List<string>
            {
                {"Đông"},
                {"Tuấn"},
                {"Liêm"},
                {"Phước"},
                {"Thắm"},
                {"Dịu"},
                {"Chiến"},
                {"Tungtt"},
                {"Quyên"},
                //{"Tungcv"},
                //{"Huy"},
                {"Đạt"},
            };
            var random = new System.Random();
            
            var listr = from item in list
                                 orderby random.Next()
                                 select item;
            return Json(listr, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Template()
        {
            //   string template = "Hello @Model.Name!";
            var _da = new OrderCarProductDetailDA();
            var products = _da.GetItemByOrderCarId(29);

            var model = new PurchaseOrderModel();
            model.ListItems = products;


            // var result = RazorEngine.Razor.Run("key1", new Test() { Name = "abc" });
            //var model = new TestModel();
            // model.Items = Enumerable.Range(1, 102).Select(m => new Test() { Name = $"Test {m}" }).ToList();
            return View(model);
        }

        public ActionResult TemplateTest()
        {
            var _da = new OrderCarProductDetailDA();
            var products = _da.GetItemByOrderCarId(29);

            var model = new PurchaseOrderModel();
            model.ListItems = products;


            //var model = new TestModel();
            //model.Items = Enumerable.Range(1, 100).Select(m => new Test() { Name = $"Test {m}" }).ToList();
            var _datem = new TemplateDocumentDA("#");
            var template = _datem.GetTemplateDocItem(2);
            var a = RenderTemplate.Instance;
            var result = a.GetString<PurchaseOrderModel>(template.Content, model, "TemplateDocumentTest" + template.ID);
            ViewBag.result = result;
            return View();
        }
    }
}
