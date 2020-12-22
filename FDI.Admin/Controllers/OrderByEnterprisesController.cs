using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Admin.Controllers
{
    public class OrderByEnterprisesController : Controller
    {
        //
        // GET: /OrderByEnterprises/

        private readonly OrdersDA _da = new OrdersDA();
        private readonly BonusTypeDA _bonusTypeDa = new BonusTypeDA();
        private readonly EnterprisesDA _d = new EnterprisesDA();
        public ActionResult Index()
        {

            return View(_d.GetAll());
        }
        public ActionResult ListItems(int id, int year)
        {
            var model = new ModelMonthEItem
            {
                BonusTypeItem = _bonusTypeDa.GetItemTop(),
                ListItem = _da.OrderByEnterprisesRequest(id, year),
            };
            return View(model);
        }
    }
}
