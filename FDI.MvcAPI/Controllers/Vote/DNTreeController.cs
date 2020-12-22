using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers
{
    public class DNTreeController : BaseApiController
    {
        //
        // GET: /CostType/
        readonly DNTreeDA _da = new DNTreeDA();
        public ActionResult GetListSimple(string key, int agencyId)
        {
            var obj = Request["key"] != Keyapi ? new List<DNTreeItem>() : _da.GetListSimple(agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListParent(string key, int agencyId)
        {
            var obj = Request["key"] != Keyapi ? new List<DNTreeItem>() : _da.GetListParent(agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetList(string key, int agencyId)
        {
            var obj = Request["key"] != Keyapi ? new List<DNTreeItem>() : _da.GetList(agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListTree(string key, int agencyId)
        {
            var obj = Request["key"] != Keyapi ? new List<TreeViewItem>() : _da.GetListTree(agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListTreeItem(Guid userid)
        {
            var obj = Request["key"] != Keyapi ? new List<DNTreeItem>() : _da.GetListTreeItem(userid);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListUser(string key, int treeId, int agencyId)
        {
            var obj = Request["key"] != Keyapi ? new List<TreeViewItem>() : _da.GetListTree(treeId, agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDNTreeItem(string key, int id)
        {
            var obj = Request["key"] != Keyapi ? new DNTreeItem() : _da.GetDNTreeItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key, string json)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            try
            {
                var model = new DN_Tree();
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                var item = _da.GetDNTreeItem(model.ParentID ?? 0);
                if (model.ParentID == 1) model.ListID = "1";
                else model.ListID = item.ListID + "," + model.ParentID;
                model.Level = item.Level + 1;
                model.IsDelete = false;
                model.Name = HttpUtility.UrlDecode(model.Name);
                if (model.UserInRoleID != null)
                {
                    _da.Add(model);
                    _da.Save();
                }
                else
                {
                    msg.Erros = true;
                    msg.Message = "Dữ liệu chưa được thêm mới";
                }                  
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được thêm mới";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update(string key, string json)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetById(ItemId);
                var parent = model.ParentID;
                UpdateModel(model);
                model.ParentID = parent;
                var item = _da.GetDNTreeItem(model.ParentID ?? 0);
                if (model.ParentID == 1) model.ListID = "1";
                else model.ListID = item.ListID + "," + model.ParentID;
                model.Level = item.Level + 1;
                model.Name = HttpUtility.UrlDecode(model.Name);
                if (model.UserInRoleID != null)
                {
                    _da.Save();
                }
                else
                {
                    msg.Erros = true;
                    msg.Message = "Dữ liệu chưa được cập nhật";
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được cập nhật";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(string key, string lstArrId)
        {
            var msg = new JsonMessage(false, "Xóa dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var lstInt = FDIUtils.StringToListInt(lstArrId);
                    var lst = _da.GetListArrId(lstInt);
                    foreach (var item in lst)
                    {
                        //item.UserInRoleID = null;
                        item.IsDelete = true;
                    }
                    _da.Save();
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được xóa";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
