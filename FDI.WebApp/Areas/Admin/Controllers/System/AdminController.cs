using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FDI.DA;
using FDI.Simple;

namespace FDI.Areas.Admin.Controllers
{
    public class AdminController : BaseController
    {
        //
        // GET: /Admin/Admin/   

        private readonly AdminDA _adminDa;
        public AdminController()
        {
            _adminDa = new AdminDA("#");
        }

        public ActionResult Index()
        {
            return View();
        }
       
        public ActionResult TotalNotifications()
        {
            return PartialView();
        }       

        public ActionResult Menu()
        {
            var ltsSourceModule = SystemActionItem.IsAdmin ? _adminDa.getAllListSimpleWeb() : _adminDa.GetAllModuleByUserNameWeb(User.Identity.Name);
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
