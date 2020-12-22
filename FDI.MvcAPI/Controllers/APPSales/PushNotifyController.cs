using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA.DA.AppSales;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers.APPSales
{
    public class PushNotifyController : BaseApiAuthController
    {
        //
        // GET: /PushNotify/
        readonly TokenDeviceDA _da = new TokenDeviceDA("#");

        [HttpPost]
        public ActionResult AddTokenDevice(TokenDeiveItem model)
        { //
            try
            {
                var tokentmp = _da.GetToken(model.Token);

                if (tokentmp == null)
                {
                    _da.Add(new TokenDevice()
                    {
                        App = model.App,
                        Token = model.Token,
                        UserId = CustomerId != 0 ? CustomerId.ToString() : AgencyUserId.ToString()
                    });
                    _da.Save();
                    return Json(new JsonMessage(false, ""));
                }

                //update token with user
                var tem = CustomerId != 0 ? CustomerId.ToString() : AgencyUserId.ToString();

                if (tokentmp.UserId != (CustomerId != 0 ? CustomerId.ToString() : AgencyUserId.ToString()))
                {
                    tokentmp.UserId = CustomerId != 0 ? CustomerId.ToString() : AgencyUserId.ToString();
                    _da.Save();
                }
                return Json(new JsonMessage(false, ""));
            }
            catch (Exception ex)
            {
                return Json(new JsonMessage(true, ex.Message));
            }
        }
    }
}
