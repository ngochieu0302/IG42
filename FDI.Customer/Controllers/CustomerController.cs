using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Customer.Controllers
{
    public class CustomerController : BaseController
    {
        //
        // GET: /Admin/Customer/
        private readonly CustomerAPI _api = new CustomerAPI();
        public ActionResult Index()
        {
            //var model = _api.GetListTree(UserItem.ID, UserItem.Level ?? 0, UserItem.AgencyId ?? 0);
            //ViewBag.Tree = GetTree(model, UserItem.ParentID ?? 0);
            var model = _api.GetListByParent(UserItem.ID);
            return View(model);
        }
        public StringBuilder GetTreeUser(int id, int lv, int pr)
        {
            var model = _api.GetListTree(id, lv, 0);
            return GetTree(model, pr);
        }
        public StringBuilder GetTree(List<TreeViewItem> lst, int id)
        {
            var tempMenu = lst.Where(m => m.ParentId == id);
            var mystring = new StringBuilder();
            foreach (var item in tempMenu)
            {
                var child = lst.Where(m => m.ParentId == item.ID);
                var totalChild = child.Count();
                if (totalChild > 0)
                {
                    mystring.Append("<li>");
                    mystring.Append(GetTreeItem(item));
                    mystring.Append("<ul>");
                    mystring.Append(GetTree(lst, item.ID));
                    mystring.Append("</ul></li>");
                }
                else
                {
                    mystring.Append("<li>" + GetTreeItem(item) + "</li>");
                }
            }
            return mystring;
        }
        public string GetTreeItem(TreeViewItem item)
        {
            var tree = Resources.Resource.TreeView;
            var txt = tree.Replace("CustomerUser", item.UserName);
            txt = txt.Replace("CustomerName", item.Name);
            txt = txt.Replace("TreeId", item.ID.ToString());
            txt = txt.Replace("level", item.Level.ToString());
            txt = txt.Replace("parent", item.ParentId.ToString());
            return txt;
        }
        public ActionResult Actions()
        {
            var url = Request.Form.ToString();
            var msg = _api.Update(url);

            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
