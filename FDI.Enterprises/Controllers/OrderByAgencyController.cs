using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;

namespace FDI.Enterprises.Controllers
{
    public class OrderByAgencyController : BaseController
    {
        //
        // GET: /OrderByEnterprises/

        private readonly OrderAPI _da = new OrderAPI();
        private readonly DNAgencyAPI _agencyApi = new DNAgencyAPI();
        public ActionResult Index()
        {
            return View(_agencyApi.GetAll(EnterprisesItem.ID));
        }
        public ActionResult ListItems(int id, int year)
        {
            return View(_da.OrderByAgencyRequest(id, year));
            
        }

    }
}
