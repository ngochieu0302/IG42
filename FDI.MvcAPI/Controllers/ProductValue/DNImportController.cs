using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers
{
	public class DNImportController : BaseApiController
	{
		//
		// GET: /DNImport/
		readonly DNImportDA _da = new DNImportDA();
		public ActionResult GetListSimple(string key, int agencyId)
		{
			var obj = key != Keyapi ? new List<StorageItem>() : _da.GetListSimple(agencyId);
			return Json(obj, JsonRequestBehavior.AllowGet);
		}

		public ActionResult ListItems()
		{
			var obj = Request["key"] != Keyapi
				? new ModelStorageItem()
                : new ModelStorageItem { ListItem = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
			return Json(obj, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetListValueByRequest(int agencyId)
		{
			decimal? total;
			decimal? quantity;
			var obj = Request["key"] != Keyapi
				? new ModuleShopProductValueItem()
				:  _da.GetListValueByRequest(Request, agencyId, out total, out quantity);
			return Json(obj, JsonRequestBehavior.AllowGet);
		}
        public ActionResult GetListValueView(int agencyId,int id)
		{
			var obj = Request["key"] != Keyapi
                ? new List<ShopProductValueItem>()
                : _da.GetListValueView(Request, agencyId, id);
			return Json(obj, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetListValueLater(int agencyId)
		{
			decimal? total;
			decimal? quantity;
			var obj = Request["key"] != Keyapi
				? new ModuleShopProductValueItem()
				: new ModuleShopProductValueItem { ListItems = _da.GetListValueLater(Request, agencyId, out total, out quantity), PageHtml = _da.GridHtmlPage, TotalPrice = total ?? 0, Quantity = quantity ?? 0 };
			return Json(obj, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetStorageItem(string key, int id)
		{
			var obj = key != Keyapi ? new StorageItem() : _da.GetStorageItem(id);
			return Json(obj, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetListAuto(string key, string keword, int showLimit, int agencyId, int type)
		{
			var obj = key != Keyapi ? new List<SuggestionsProduct>() : _da.GetListAuto(keword, showLimit, agencyId, type);
			return Json(obj, JsonRequestBehavior.AllowGet);
		}

		public ActionResult Add(string key, string codeLogin)
		{
			var model = new Storage();
			try
			{
				if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
				UpdateModel(model);
				model.Note = HttpUtility.UrlDecode(model.Note);
				var dateCreated = Request["DateCreated_"];
				var date = dateCreated.StringToDate();
				model.DN_Import = GetListImportItem(codeLogin, date);
				model.DateImport = ConvertDate.TotalSeconds(date);
				model.DateCreated = ConvertDate.TotalSeconds(DateTime.Now);
				model.IsDelete = false;
                model.AgencyId = Agencyid();
                //var lstInt = model.DN_Import.Select(c => c.ValueId ?? 0).ToList();
                //var lstValue = _da.GetListProductValue(lstInt);
                //foreach (var i in lstValue)
                //{
                //    var item = model.DN_Import.FirstOrDefault(c => c.ValueId == i.ID);
                //    if (item != null)
                //    {
                //        i.Quantity = i.Quantity + item.Quantity;
                //    }
                //}
                model.Code = DateTime.Now.ToString("yyMMddHHmm");
                _da.Add(model);
				_da.Save();
				return Json(model.ID, JsonRequestBehavior.AllowGet);
			}
			catch (Exception)
			{
				return Json(0, JsonRequestBehavior.AllowGet);
			}
		}
		public List<DN_Import> GetListImportItem(string codeLogin, DateTime date)
		{
			const string url = "Utility/GetListImportNewItem?key=";
			var urlJson = string.Format("{0}{1}", UrlG + url, codeLogin);
			var list = Utility.GetObjJson<List<DNImportNewItem>>(urlJson);
			return list.Select(item => new DN_Import
			{
				ValueId = item.ValueId,
				Quantity = item.Quantity,
				Price = item.Price,
				Date = DateTime.Now.TotalSeconds(),
				DateEnd = (date.AddDays(item.QuantityDay ?? 0)).TotalSeconds(),
				IsDelete = false,
                QuantityOut = 0
			}).ToList();

		}
		public ActionResult Update(string key, string codeLogin)
		{
			try
			{
				if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
				var model = _da.GetById(ItemId);
				if (model == null)
				{
					return Json(1, JsonRequestBehavior.AllowGet);
				}
				UpdateModel(model);
				var dateCreated = Request["DateCreated_"];
                var date = dateCreated.StringToDate();
                var lst = model.DN_Import.Where(c => c.IsDelete == false).ToList();
				var lstNew = GetListImportItem(codeLogin, date);
				//xóa
				var result1 = lst.Where(p => lstNew.All(p2 => p2.ValueId != p.ValueId));
				foreach (var i in result1)
				{
					i.Shop_Product_Value.Quantity = i.Shop_Product_Value.Quantity - i.Quantity;
					i.IsDelete = true;
				}
				foreach (var i in lst)
				{
					var j = lstNew.FirstOrDefault(c => c.ValueId == i.ValueId);
					if (j == null) continue;
					i.Shop_Product_Value.Quantity = i.Shop_Product_Value.Quantity - i.Quantity + j.Quantity;
					i.Quantity = j.Quantity;
					i.Price = j.Price;
					i.DateEnd = j.DateEnd;
				}
				//thêm mới
				var result2 = lstNew.Where(p => lst.All(p2 => p2.ValueId != p.ValueId)).ToList();
				model.DN_Import.AddRange(result2);
				model.DateImport = ConvertDate.TotalSeconds(date);
				model.Note = HttpUtility.UrlDecode(model.Note);
				_da.Save();
				return Json(1, JsonRequestBehavior.AllowGet);
			}
			catch (Exception)
			{
				return Json(0, JsonRequestBehavior.AllowGet);
			}
		}
		public ActionResult Delete(string key, string lstArrId)
		{
			if (key == Keyapi)
			{
				var lstInt = FDIUtils.StringToListInt(lstArrId);
				var lst = _da.GetListArrId(lstInt);
				foreach (var item in lst)
				{
					var lstImport = item.DN_Import.Where(c => c.IsDelete == false);
					foreach (var i in lstImport)
					{
						i.Shop_Product_Value.Quantity = i.Shop_Product_Value.Quantity - i.Quantity;
					}
					item.IsDelete = true;
				}
				_da.Save();
				return Json(1, JsonRequestBehavior.AllowGet);
			}
			return Json(0, JsonRequestBehavior.AllowGet);
		}

	}
}
