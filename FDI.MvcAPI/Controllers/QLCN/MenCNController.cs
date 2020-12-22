using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using FDI.Base;
using FDI.DA.DA.QLCN;
using FDI.Simple;
using FDI.Simple.QLCN;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers.QLCN
{
    public class MenCNController : BaseApiController
    {
        //
        // GET: /MenCN/
        private readonly MenCNDA _da = new MenCNDA("#");
        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelMenCNItem()
                : new ModelMenCNItem { ListItems = _da.GetListSimpleByRequest(Request), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetList(string key, string code)
        {
            var obj = key != Keyapi ? new List<MenCNItem>() : _da.GetList();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetMenCNItem(string key, int id)
        {
            var obj = key != Keyapi ? new MenCNItem() : _da.GetMenCNItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key, string json,string code)
        {
            var model = new Men_CN();
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                var lstnguyenlieu = GetListMenNguyenlieuCNItem(code);
                model.IsDeleted = false;
                model.IsShow = true;
                var lst = model.Men_Nguyenlieu_CN.Where(c => c.IsDeleted == false);
                var result2 = lstnguyenlieu.Where(p => lst.All(p2 => p2.IdNguyenlieu != p.IdNguyenlieu)).ToList();
                model.Men_Nguyenlieu_CN.AddRange(result2);
                _da.Add(model);
                _da.Save();
                return Json(model.ID, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Update(string key, string json,string code)
        {
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetById(ItemId);
                UpdateModel(model);
                
                //cong thuc men
                var lstnguyenlieu = GetListMenNguyenlieuCNItem(code);
                var lst = model.Men_Nguyenlieu_CN.Where(c => c.IsDeleted == false).ToList();
                var result1 = lst.Where(p => lstnguyenlieu.All(p2 => p2.IdNguyenlieu != p.IdNguyenlieu));
                //xoa
                foreach (var i in result1)
                {
                    i.IsDeleted = true;
                }
                //sua
                foreach (var i in lst)
                {
                    var j = lstnguyenlieu.FirstOrDefault(c => c.IdNguyenlieu == i.IdNguyenlieu);
                    if (j != null)
                    {
                        i.Quantity = j.Quantity;
                    }
                }
                //thêm mới
                var result2 = lstnguyenlieu.Where(p => lst.All(p2 => p2.IdNguyenlieu != p.IdNguyenlieu)).ToList();
                model.Men_Nguyenlieu_CN.AddRange(result2);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Delete(string key)
        {
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetById(ItemId);
                _da.Delete(model);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public List<Men_Nguyenlieu_CN> GetListMenNguyenlieuCNItem(string code)
        {
            const string url = "Utility/GetListNguyenlieuCNItem?key=";
            var urlJson = string.Format("{0}{1}", UrlG + url, code);
            var list = Utility.GetObjJson<List<MenNguyenlieuCNItem>>(urlJson);
            return list.Select(item => new Men_Nguyenlieu_CN
            {
                Quantity = item.Quantity,
                IdNguyenlieu = item.IdNguyenlieu,
                IsDeleted = false
            }).ToList();
        }
    }
}
