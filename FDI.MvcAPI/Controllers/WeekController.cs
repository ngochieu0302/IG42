using System.Web.Mvc;

namespace FDI.MvcAPI.Controllers
{
    public class WeekController : Controller
    {
        //
        // GET: /Week/

        public ActionResult Index()
        {
            //var obj = Request["key"] != Keyapi ? new List<CustomerItem>() : _dl.GetList();
            //var datenow = DateTime.Now;
            //var datestart = new DateTime(datenow.Year, 1, 1);
            //var listWeekOfYear = FDIUtils.GetWeekNumbers(datestart, datestart.AddYears(1));
            //var numberWeek = FDIUtils.GetWeekNumber(datenow);
            //var date = FDIUtils.WeekDays(datenow.Year, itemWeek);
            return Json(1, JsonRequestBehavior.AllowGet);
        }

    }
}
