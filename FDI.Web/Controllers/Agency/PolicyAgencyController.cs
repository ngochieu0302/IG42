using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers.Agency
{
    public class PolicyAgencyController : BaseController
    {
        PolicyAgencyDA _policyAgencyDa = new PolicyAgencyDA();
        OrdersDA _ordersDa = new OrdersDA();
        CategoryDA _categoryDa = new CategoryDA();

        public ActionResult Index()
        {
            var categories = _categoryDa.GetCategoryForPolicy();
            return View(categories);
        }

        public ActionResult ListItems()
        {
            var request = new ParramRequest(Request);
            var policies = _policyAgencyDa.GetAll(request.CategoryID);
            return View(policies);
        }

        public ActionResult AjaxForm()
        {
            var model = new ModelPolicyAgenciesItem();
            if (DoAction == ActionType.Edit)
            {
                var tmp = _policyAgencyDa.GetItemById(ArrId.FirstOrDefault());
                model.Quantity = tmp.Quantity;
                model.Profit = tmp.Profit;
                model.LevelAgency = tmp.LevelAgency;
                model.ID = tmp.ID;
                model.PercentProfit = tmp.PercentProfit;
                model.CategoryId = tmp.CategoryId;
            }
            var categories = _categoryDa.GetCategoryForPolicy();
            model.Categories = categories;
          
            ViewBag.Action = DoAction;

            return View(model);
        }


        [HttpPost]
        public ActionResult Actions()
        {
            var msg = new JsonMessage {Erros = false, Message = "Cập nhật thành công"};

            switch (DoAction)
            {
                case ActionType.Add:
                    var model = new PolicyAgenciesItem();
                    UpdateModel(model);
                    var policy = new PolicyAgency()
                    {
                        CategoryId = model.CategoryId,
                        Profit = model.Profit,
                        Quantity = model.Quantity,
                        LevelAgency = model.LevelAgency,
                        PercentProfit = model.PercentProfit,
                    };
                    _policyAgencyDa.Add(policy);
                    _policyAgencyDa.Save();
                    break;
                case ActionType.Edit:
                    var modelUpdate = new PolicyAgenciesItem();
                    UpdateModel(modelUpdate);
                    var policyUpdate = _policyAgencyDa.GetById(ArrId.FirstOrDefault());
                    policyUpdate.Quantity = modelUpdate.Quantity;
                    policyUpdate.Profit = modelUpdate.Profit;
                    policyUpdate.LevelAgency = modelUpdate.LevelAgency;
                    policyUpdate.PercentProfit = modelUpdate.PercentProfit;
                    _policyAgencyDa.Save();
                    break;
                case ActionType.Delete:
                    var policyDelete = _policyAgencyDa.GetById(ArrId.FirstOrDefault());
                    policyDelete.Isdelete = true;
                    _policyAgencyDa.Save();
                    break;
            }

            return Json(msg);
        }
    }
}
