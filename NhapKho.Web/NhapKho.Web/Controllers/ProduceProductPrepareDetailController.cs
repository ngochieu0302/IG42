using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI.StorageWarehouse;
using FDI.Simple;
using FDI.Utils;
using NhapKho.Web.Common;

namespace NhapKho.Web.Controllers
{
    public class ProduceProductPrepareDetailController : Controller
    {
        private readonly ProduceAPI _produceApi = new ProduceAPI(new DNUserItem() { UserId = new Guid("E1DB6D69-865B-4F05-ACBA-B62498DB3B8F") });

        public Dictionary<int, List<string>> cache = new Dictionary<int, List<string>>();
        //
        // GET: /ProduceProductPrepareDetail/

        public ActionResult Index(int id)
        {
            return View(id);
        }

        public async Task<ActionResult> Detail(int id)
        {
            var item = await _produceApi.GetById(id);
            item.QuantityActive = CacheCustom.Instance.Get(id.ToString()).Count();
            return View(item);
        }


        public ActionResult ListItems(int id)
        {
            return View(CacheCustom.Instance.Get(id.ToString()));
        }

        public ActionResult Add(int id, string code)
        {
            if (CacheCustom.Instance.CheckExist(id.ToString(), code))
            {
                return Json(new JsonMessage() { Erros = true, Message = "Sản phẩm đã tồn tại" });
            }
            CacheCustom.Instance.GetOrAdd(id.ToString(), code);

            return Json(new JsonMessage());

        }

        [HttpPost]
        public async Task<ActionResult> Actions(int id)
        {
            var action = Request["do"];
            switch (action)
            {
                case "delete":
                    CacheCustom.Instance.Remove(id.ToString(), Request["itemId"]);
                    return Json(new JsonMessage() { Message = "Đã xóa" });
                case "insert":
                    var item = await _produceApi.GetById(id);
                    var obj = CacheCustom.Instance.Get(id);
                    if (obj.Count != item.Quantity)
                    {
                        //   return Json(new JsonMessage() { Erros = true, Message = "Chưa đủ số lượng" });
                    }

                    var result = await _produceApi.Insert(id, obj.ToArray());

                    if (!result.Erros)
                    {
                        CacheCustom.Instance.Remove(id);
                    }

                    return Json(result);
            }

            return Json(new JsonMessage() { Message = "Đã xóa" });
        }
    }
}
