using System.Collections.Generic;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Utils;
using FDI.Simple;
using Newtonsoft.Json;
using System;
using FDI.CORE;


namespace FDI.MvcAPI.Controllers
{
    public class DNDepartmentController : BaseApiController
    {
        //
        // GET: /DNActive/

        private readonly DepartmentDA _da = new DepartmentDA();

        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelDepartmentItem()
                : new ModelDepartmentItem { ListItem = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAll(string key, int agencyid)
        {
            var obj = key != Keyapi ? new List<DepartmentItem>() : _da.GetAll(agencyid);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetById(string key, int id)
        {
            var obj = key != Keyapi ? new DN_Department() : _da.GetById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDepartmentItemById(string key, int id)
        {
            var obj = key != Keyapi ? new DepartmentItem() : _da.GetDepartmentItemById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListByArrId(string key, string lstId)
        {
            var obj = key != Keyapi ? new List<DepartmentItem>() : _da.GetListByArrId(lstId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add(string key, string code, string json)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var departmentItem = JsonConvert.DeserializeObject<DepartmentItem>(json);
                    var obj = new DN_Department();
                    departmentItem.AgencyID = Agencyid();
                    UpdateBase(obj, departmentItem);
                    obj.DateCreate = DateTime.Now.TotalSeconds();
                    _da.Add(obj);
                    _da.Save();
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

        public ActionResult Update(string key, string code, string json, int id)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var departmentItem = JsonConvert.DeserializeObject<DepartmentItem>(json);
                    var obj = _da.GetById(id);
                    departmentItem.AgencyID = Agencyid();
                    UpdateBase(obj, departmentItem);
                    _da.Save();
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

        public DN_Department UpdateBase(DN_Department department, DepartmentItem departmentItem)
        {
            department.AgencyID = departmentItem.AgencyID;
            department.Name = departmentItem.Name;
            department.Description = departmentItem.Description;
            department.IsShow = departmentItem.IsShow;
            department.Sort = departmentItem.Sort;
            department.IsDelete = departmentItem.IsDelete;
            return department;
        }
    }
}
