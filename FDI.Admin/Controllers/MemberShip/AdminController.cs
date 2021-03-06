﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FDI.DA;
using FDI.Simple;
using System.Web;
using System;

namespace FDI.Admin.Controllers
{
    public class AdminController : BaseController
    {
       
        private readonly AdminDA _adminDa;
        public AdminController()
        {
            _adminDa = new AdminDA("#");
        }

        public ActionResult Index()
        {
            AddCookies("ParentId", 0);
            AddCookies("ModuleId", 0);
            return PartialView();
        }

        void AddCookies(string name, int val)
        {
            var codeCookie = HttpContext.Request.Cookies[name];
            if (codeCookie == null)
            {
                codeCookie = new HttpCookie(name) { Value = val.ToString(), Expires = DateTime.Now.AddHours(6) };
                Response.Cookies.Add(codeCookie);
            }
            else
            {
                codeCookie.Value = val.ToString();
                codeCookie.Expires = DateTime.Now.AddHours(6);
                Response.Cookies.Add(codeCookie);
            }
        }
        public ActionResult MenuDefault(bool check)
        {
            var ltsSourceModule = SystemActionItem.IsAdmin ? _adminDa.getAllListSimple() : _adminDa.GetAllModuleByUserName(User.Identity.Name);
            var list = GetLeftMenu(ltsSourceModule);
            var module = new ModelModuleadminItem
            {
                check = check,
                ListItem = list.Where(m => m.PrarentID == 1),
            };
            return PartialView(module);
        }
        public ActionResult Procedure()
        {
            var id = Utils.ConvertUtil.ToInt32(ParentId());
            var list =  _adminDa.getListByParentId(id);
            var model = new ModelModuleadminItem
            {
                ListItem = list,
                Container = Module(),
                PrarentID = id
            };
            return View(model);
        }
        public ActionResult TotalNotifications()
        {
            return PartialView();
        }
        public ActionResult Menu()
        {
            var ltsSourceModule = SystemActionItem.IsAdmin ? _adminDa.getAllListSimple() : _adminDa.GetAllModuleByUserName(User.Identity.Name);
            var list = GetLeftMenu(ltsSourceModule);
            return View(list);
        }
        public List<ModuleadminItem> GetLeftMenu(List<ModuleadminItem> lst)
        {
            var list = lst.Where(m => m.PrarentID > 1);
            var listPrarent = lst.Where(m => m.PrarentID == 1).ToList();
            foreach (var obj in from item in list where listPrarent.All(m => m.ID != item.PrarentID) select new ModuleadminItem
            {
                ID = item.Module2.ID,
                NameModule = item.Module2.NameModule,
                Tag = item.Module2.Tag,
                ClassCss = item.Module2.ClassCss,
                PrarentID = 1,
                Module1 = list.Where(m=>m.PrarentID == item.PrarentID).Select(m=>new ModuleItem
                {
                    ID = m.ID,
                    NameModule = m.NameModule,
                    Tag = m.Tag,
                    ClassCss = m.ClassCss
                })
            })
            {
                listPrarent.Add(obj);
            }
            return listPrarent;
        }
    }
}
