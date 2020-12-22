using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;

namespace FDI.Enterprises.Controllers
{
    public class HomeController : BaseController
    {
        readonly DNEnterprisesAPI _dnLoginApi = new DNEnterprisesAPI();
        public ActionResult Index()
        {
            return View(_dnLoginApi.GetItemByCodeLogin(CodeLogin()));
        }

        public ActionResult Menu()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult StaticOrder()
        {
            var model = _dnLoginApi.GetStaticEnterprise(EnterprisesItem.ID);
            return View(model);
        }
    }
}
