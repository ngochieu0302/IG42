using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.Simple;

namespace FDI.MvcAPI.Controllers
{
    public class DNExchangeController : BaseApiController
    {
        //
        // GET: /DNExchange/
        private readonly DNExchangeDA _dlDa = new DNExchangeDA();

        public ActionResult GetListByNow(string key, int agencyid)
        {
            var obj = key != Keyapi ? new List<DNExchangeItem>() : _dlDa.GetListByNow(agencyid);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key, string code, int bedid, string name, decimal end)
        {
            if (key == Keyapi)
            {
                var bed = _dlDa.GetBedIDByName(name, Agencyid());
                if (bed > 0)
                {
                    var obj = new DN_Exchange
                    {
                        BedDeskID = bedid,
                        BedDeskExID = bed,
                        StartDate = ConvertDate.TotalSeconds(DateTime.Now),
                        EndDate = end,
                        AgencyID = Agencyid()
                    };
                    _dlDa.Add(obj);
                    _dlDa.Save();
                    return Json(bed, JsonRequestBehavior.AllowGet);
                }

            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

    }
}
