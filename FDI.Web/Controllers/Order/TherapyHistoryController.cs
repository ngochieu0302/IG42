using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FDI.Web.Controllers.Order
{
    public class TherapyHistoryController : BaseController
    {
        //
        // GET: /TherapyHistory/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            var model = new ModelDiscountItem
            {
                //DiscountPItem = _discountAPI.GetDiscountItem(0),
                ListItem = _discountAPI.GetDiscountItem(1, UserItem.AgencyID)
            };
            return View(model);
        }

    }
}
