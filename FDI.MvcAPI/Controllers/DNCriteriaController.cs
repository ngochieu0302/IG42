using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using Newtonsoft.Json;
using FDI.Utils;
using System.Text;

namespace FDI.MvcAPI.Controllers
{
    public class DNCriteriaController : BaseApiController
    {
        //
        // GET: /DNCriteria/

        private readonly DNCriteriaDA _da = new DNCriteriaDA("#");

        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelDNCriteriaItem()
                : new ModelDNCriteriaItem { ListItem = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAll(string key, string code)
        {
            var obj = key != Keyapi ? new List<DNCriteriaItem>() : _da.GetAll(Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTypeAll(string key)
        {
            var obj = key != Keyapi ? new List<TypeCriteriaItem>() : _da.GetTypeAll();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetById(string key, int id)
        {
            var obj = key != Keyapi ? new DN_Criteria() : _da.GetById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new DNCriteriaItem() : _da.GetItemById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListByArrId(string key, string lstId)
        {
            var obj = key != Keyapi ? new List<DNCriteriaItem>() : _da.GetListByArrId(lstId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add(string key, string json)
        {
            if (key == Keyapi)
            {
                var dNCriteria = JsonConvert.DeserializeObject<DNCriteriaItem>(json);
                var obj = new DN_Criteria();
                dNCriteria.AgencyID = Agencyid();
               obj = UpdateBase(obj, dNCriteria);
                _da.Add(obj);

                // add user
                var lstUser = _da.GetUserArrId(dNCriteria.LstUserIds);
                foreach (var item in lstUser)
                {
                    obj.DN_Users.Add(item);
                }

                // add role
                var lstRoles = _da.GetRolesArrId(dNCriteria.LstRoleIds);
                foreach (var item in lstRoles)
                {
                    obj.DN_Roles.Add(item);
                }

                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(string key, string json, int id)
        {
            if (key == Keyapi)
            {
                var dNCriteria = JsonConvert.DeserializeObject<DNCriteriaItem>(json);
                var obj = _da.GetById(id);
                dNCriteria.AgencyID = Agencyid();
                obj = UpdateBase(obj, dNCriteria);

                // add user
                obj.DN_Users.Clear();
                var lstUser = _da.GetUserArrId(dNCriteria.LstUserIds);
                foreach (var item in lstUser)
                {
                    obj.DN_Users.Add(item);
                }

                // add role
                obj.DN_Roles.Clear();
                var lstRoles = _da.GetRolesArrId(dNCriteria.LstRoleIds);
                foreach (var item in lstRoles)
                {
                    obj.DN_Roles.Add(item);
                }

                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }

            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(string key, string lstArrId)
        {
            var msg = new JsonMessage
            {
                Erros = true,
                Message = "Không có hành động nào được thực hiện."
            };
            if (key == Keyapi)
            {
                var list = _da.ListByArrId(lstArrId);
                var stbMessage = new StringBuilder();
                foreach (var item in list)
                {
                    if (!item.DN_Salary.Any())
                    {
                        _da.Delete(item);
                        stbMessage.AppendFormat("Đã xóa  <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                }
                _da.Save();
                msg.Erros = false;
                msg.ID = lstArrId;
                msg.Message = stbMessage.ToString();
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public DN_Criteria UpdateBase(DN_Criteria dnCriteria, DNCriteriaItem dnCriteriaItem)
        {
            dnCriteria.AgencyID = dnCriteriaItem.AgencyID;
            dnCriteria.Name = dnCriteriaItem.Name;
            dnCriteria.Value = dnCriteriaItem.Value;
            dnCriteria.TypeID = dnCriteriaItem.TypeID;
            dnCriteria.Price = dnCriteriaItem.Price;
            dnCriteria.IsSchedule = dnCriteriaItem.IsSchedule;
            dnCriteria.IsAll = dnCriteriaItem.IsAll;
            return dnCriteria;
        }
    }
}
