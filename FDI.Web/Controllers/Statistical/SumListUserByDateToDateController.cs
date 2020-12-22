using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.CORE;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers.Statistical
{
    public class SumListUserByDateToDateController : BaseController
    {
        readonly VoteAPI _voteApi = new VoteAPI();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            var dates = Request["fromDate"];
            var datee = Request["toDate"];
            var de = DateTime.Now;
            var ds = new DateTime(de.Year, de.Month, 1);
            if (!string.IsNullOrEmpty(dates) || !string.IsNullOrEmpty(dates))
            {
                de = ConvertUtil.ToDateTime(datee);
                ds = ConvertUtil.ToDateTime(dates);
            }
            var model = new ModelDNUserAddItem
            {
                ListItems = _voteApi.GetSumListUser(UserItem.AgencyID, ConvertDate.TotalSeconds(ds), ConvertDate.TotalSeconds(de)),
                Dates = ds.ToString("dd/MM/yyyy"),
                Datee = de.ToString("dd/MM/yyyy"),
            };
            return View(model);
        }
    }
}
