using System.Collections.Generic;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using Newtonsoft.Json;
using FDI.Utils;
using System;

namespace FDI.MvcAPI.Controllers
{
    public class DNTimeJobController : BaseApiController
    {
        //
        // GET: /DNTimeJob/

        private readonly DNTimeJobDA _da = new DNTimeJobDA();

        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelDNTimeJobItem()
                : new ModelDNTimeJobItem { ListItem = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAll(string key)
        {
            var obj = key != Keyapi ? new List<DNTimeJobItem>() : _da.GetAll();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllByMonth(string key, int month, int year, int agencyid)
        {
            var obj = key != Keyapi ? new List<DNUserTimeJobItem>() : _da.GetAllByMonth(month, year, agencyid);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllOnlineByToday(string key, int agencyid)
        {
            agencyid = agencyid > 0 ? agencyid : Agencyid();
            var obj = key != Keyapi ? new List<UserViewItem>() : _da.GetAllOnlineByToday(agencyid);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetById(string key, int id)
        {
            var obj = key != Keyapi ? new DN_Time_Job() : _da.GetById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new DNTimeJobItem() : _da.GetItemById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemByScheduleId(string key, int agencyId, int scheduleId)
        {
            var obj = key != Keyapi ? new DNTimeJobItem() : _da.GetItemByScheduleId(agencyId, scheduleId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListByArrId(string key, string lstId)
        {
            var obj = key != Keyapi ? new List<DNTimeJobItem>() : _da.GetListByArrId(lstId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add(string key, string code, string json)
        {
            var msg = new JsonMessage(true, "Thêm mới dữ liệu thành công.");
            try
            {

                if (key == Keyapi)
                {
                    var dnTimeJob = JsonConvert.DeserializeObject<DNTimeJobItem>(json);
                    if (dnTimeJob.UserId != null)
                    {
                        var objExits = _da.GetItemByScheduleEndId(dnTimeJob.UserId.Value);
                        if (objExits != null)
                        {
                            objExits.DateEnd = dnTimeJob.DateEnd;
                            objExits.ScheduleEndID = dnTimeJob.ScheduleEndID;
                            _da.Save();
                            var url = ":3000/checkinout/" + dnTimeJob.UserId + "/0";
                            Node(url);
                        }
                        else if (dnTimeJob.ScheduleID.HasValue)
                        {
                            objExits = new DN_Time_Job();
                            dnTimeJob.ScheduleEndID = null;
                            dnTimeJob.DateEnd = null;
                            UpdateBase(objExits, dnTimeJob);
                            _da.Add(objExits);
                            _da.Save();
                            var url = ":3000/checkinout/" + dnTimeJob.UserId + "/1";
                            Node(url);
                        }
                    }
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

        public ActionResult Update(string key, string json, int id)
        {
            if (key == Keyapi)
            {
                var dayOff = JsonConvert.DeserializeObject<DNTimeJobItem>(json);
                var obj = _da.GetById(id);
                dayOff.AgencyID = Agencyid();
                UpdateBase(obj, dayOff);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }

            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public DN_Time_Job UpdateBase(DN_Time_Job timeJob, DNTimeJobItem timeJobItem)
        {
            timeJob.DateCreated = timeJobItem.DateCreated;
            timeJob.DateEnd = timeJobItem.DateEnd;
            timeJob.UserId = timeJobItem.UserId;
            timeJob.AgencyID = timeJobItem.AgencyID;
            timeJob.ScheduleID = timeJobItem.ScheduleID;
            timeJob.ScheduleEndID = timeJobItem.ScheduleEndID;
            timeJob.MinutesLater = timeJobItem.MinutesLater;
            timeJob.MinutesEarly = timeJobItem.MinutesEarly;
            return timeJob;
        }
    }
}
