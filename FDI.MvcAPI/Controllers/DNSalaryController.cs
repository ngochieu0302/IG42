using System.Collections.Generic;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using Newtonsoft.Json;


namespace FDI.MvcAPI.Controllers
{
    public class DNSalaryController : BaseApiController
    {
        //
        // GET: /DNSalary/

        private readonly DNSalaryDA _da = new DNSalaryDA();

        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelDNSalaryItem()
                : new ModelDNSalaryItem { ListItem = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAll(string key, string code)
        {
            var obj = key != Keyapi ? new List<DNSalaryItem>() : _da.GetAll(Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetById(string key, int id)
        {
            var obj = key != Keyapi ? new DN_Salary() : _da.GetById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new DNSalaryItem() : _da.GetItemById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListByArrId(string key, string lstId)
        {
            var obj = key != Keyapi ? new List<DNSalaryItem>() : _da.GetListByArrId(lstId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add(string key, string json)
        {
            if (key == Keyapi)
            {
                var dNCriteria = JsonConvert.DeserializeObject<DNSalaryItem>(json);
                var obj = new DN_Salary();
                dNCriteria.AgencyID = Agencyid();
                UpdateBase(obj, dNCriteria);
                _da.Add(obj);

                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(string key, string json, int id)
        {
            if (key == Keyapi)
            {
                var dNCriteria = JsonConvert.DeserializeObject<DNSalaryItem>(json);
                var obj = _da.GetById(id);
                dNCriteria.AgencyID = Agencyid();
                UpdateBase(obj, dNCriteria);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }

            return Json(0, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult Delete(string key, List<int> listint)
        //{
        //    if (key == Keyapi)
        //    {
        //        var list = _dl.GetListByArrId(listint);
        //        foreach (var item in list)
        //        {
        //            _dl.Delete(item);
        //        }
        //        _dl.Save();
        //        return Json(1, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(0, JsonRequestBehavior.AllowGet);
        //}

        public DN_Salary UpdateBase(DN_Salary dnSalary, DNSalaryItem dnSalaryItem)
        {
            dnSalary.AgencyID = dnSalaryItem.AgencyID;
            dnSalary.UserID = dnSalaryItem.UserId;
            dnSalary.Salary = dnSalaryItem.Salary;
            return dnSalary;
        }

        public ActionResult InsertSalary(string key, string json)
        {
            if (key == Keyapi)
            {

                var dNCriteria = JsonConvert.DeserializeObject<DNSalaryItem>(json);
                var obj = new DN_Salary();
                dNCriteria.AgencyID = Agencyid();
                UpdateBase(obj, dNCriteria);
                _da.Add(obj);

                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }


    }
}
