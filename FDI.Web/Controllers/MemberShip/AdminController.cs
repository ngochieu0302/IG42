using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.GetAPI.StorageWarehouse;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class AdminDNController : BaseController
    {
        readonly DNModuleAPI _dnModuleApi = new DNModuleAPI();
        readonly CustomerAPI _customerApi = new CustomerAPI();
        readonly StorageFreightWarehouseAPI _freightWarehouseApi = new StorageFreightWarehouseAPI();
        public ActionResult Index()
        {           
            return PartialView();
        }        
       
        public ActionResult Chart()
        {
            var list = _customerApi.GetAll();
            return View(list);
        }
        public ActionResult MenuDefault(bool check)
        {
            var ltsSourceModule = IsAdmin ? _dnModuleApi.GetAllListSimpleItems(UserItem.AgencyID) : _dnModuleApi.GetAllModuleByUserName(UserItem.UserId, UserItem.AgencyID);
            var list = SystemActionItem.IsAdmin ? ltsSourceModule : GetLeftMenu(ltsSourceModule);
            var module = new ModelModuleadminItem
            {
                check = check,
                ListItem = list.Where(m => m.PrarentID == 1),
            };
            return PartialView(module);
        }
        public ActionResult NotRoles()
        {
            return PartialView();
        }

        public ActionResult TotalNotifications()
        {
            var model = new List<StorageFreightWarehouseItemNew>();
            var ware = UserItem.listRole.Any(m => m.ToLower() == "duyệt kho");
            if (ware)
            {
                var temp = _freightWarehouseApi.ListItemsNotActive(false);
                model = temp;
            }
            return PartialView(model);
        }

        public ActionResult Agency()
        {
            var enterprisesApi = new DNEnterprisesAPI();
            if (Request.Url != null) Utility._d = Request.Url.Host;
            var model = enterprisesApi.GetContent(Utility._d);
            var user = UserItem;
            if (model != null)
                user.Url = model.PictureUrl;
            ViewBag.codelogin = CodeLogin();
            return PartialView(user);
        }
        public ActionResult Procedure()
        {
            var list = IsAdmin? _dnModuleApi.GetListByParentIdAdmin(UserItem.AgencyID, ParentId())
                :_dnModuleApi.GetListByParentID(UserItem.AgencyID, ParentId(), UserItem.UserId, string.Join(",", UserItem.listRoleID ?? new List<int>()));
            var model = new ModelModuleItem
            {
                ListItem = list.OrderBy(m=>m.Level),                
                Container = Module(),
                PrarentID = int.Parse(ParentId())
            };
            return View(model);
        }

        public List<ModuleadminItem> GetLeftMenu(List<ModuleadminItem> lst)
        {
            var list = lst.Where(m => m.PrarentID > 1).ToList();
            var listPrarent = lst.Where(m => m.PrarentID == 1).ToList();
            foreach (var obj in from item in list
                                where listPrarent.All(m => m.ID != item.PrarentID)
                                select new ModuleadminItem
                                {
                                    ID = item.Module2.ID,
                                    NameModule = item.Module2.NameModule,
                                    Tag = item.Module2.Tag,
                                    ClassCss = item.Module2.ClassCss,
                                    PrarentID = item.PrarentID,
                                    Module1 = list.Where(m => m.PrarentID == item.PrarentID).Select(m => new ModuleItem
                                    {
                                        ID = m.ID,
                                        NameModule = m.NameModule,
                                        PrarentID = m.PrarentID,
                                        Tag = m.Tag,
                                        ClassCss = m.ClassCss,
                                    })
                                })
            {
                listPrarent.Add(obj);
            }
            return listPrarent;
        }
    }
}