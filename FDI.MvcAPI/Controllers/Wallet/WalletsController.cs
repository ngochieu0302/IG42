using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.DA.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers
{
    public class WalletsController : BaseApiController
    {
        //
        // GET: /Wallets/

        readonly WalletsDA _da = new WalletsDA("#");
        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelWalletItem()
                : new ModelWalletItem { ListItems = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListCustomerByRequest()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelWalletItem()
                : new ModelWalletItem { ListItems = _da.GetListCustomerByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListWalletCusByCustomer(int page, int id, int agencyId)
        {
            var obj = Request["key"] != Keyapi
               ? new List<WalletCustomerItem>()
               : _da.GetListWalletCusByCustomer(page, id, agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListWalletCashByCustomer(int page, int id, int agencyId)
        {
            var obj = Request["key"] != Keyapi
               ? new List<CashOutWalletItem>()
               : _da.GetListWalletCashByCustomer(page, id, agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListWalletOrderByCustomer(int page, int id, int agencyId)
        {
            var obj = Request["key"] != Keyapi
               ? new List<WalletOrderHistoryItem>()
               : _da.GetListWalletOrderByCustomer(page, id, agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }
}
