using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class DNTreeController : BaseController
    {
        private readonly DNTreeAPI _api = new DNTreeAPI();
        private readonly DNRoleAPI _roleApi = new DNRoleAPI();
        private readonly DepartmentAPI _departmentApi = new DepartmentAPI();
        private readonly VoteAPI _voteApi = new VoteAPI();
        private readonly ContentVoteAPI _contentVoteApi = new ContentVoteAPI();
        private readonly DNUsersInRoleAPI _usersInRoleApi = new DNUsersInRoleAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            var model = _api.GetListSimple(UserItem.AgencyID);
            var lst = _api.GetListTree(UserItem.AgencyID);
            ViewBag.tree = GetTree(lst, 1);
            return View(model);
        }
        public StringBuilder GetTree(List<TreeViewItem> lst, int id)
        {
            var tempMenu = lst.Where(m => m.ParentId == id);
            var mystring = new StringBuilder();
            var count = 0;
            foreach (var item in tempMenu)
            {
                count++;
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
            var txt = tree.Replace("CustomerUser", item.Name);
            txt = txt.Replace("CustomerName", item.FullName);
            txt = txt.Replace("UserName_", item.UserName);
            txt = txt.Replace("CustomerRoles", item.RolesName);
            txt = txt.Replace("UserId", item.GuiId.ToString());
            txt = txt.Replace("TreeId", item.ID.ToString());
            return txt;
        }
        public ActionResult AjaxForm()
        {
            var model = new DNTreeItem();
            if (DoAction == ActionType.Edit)            
                model = _api.GetDNTreeItem(ArrId.FirstOrDefault());
                           
            model.ParentID = model.ParentID ?? ArrId.FirstOrDefault();
            ViewBag.Roles = _roleApi.GetAll(UserItem.AgencyID);
            ViewBag.Tree = _api.GetListParent(UserItem.AgencyID);
            ViewBag.Department = _departmentApi.GetAll(UserItem.AgencyID);
            ViewBag.AgencyID = UserItem.AgencyID;
            ViewBag.Action = DoAction;
            return View(model);
        }

        public ActionResult AjaxView()
        {
            ViewBag.GuiId = GuiId.FirstOrDefault();
            ViewBag.ArrId = ArrId.FirstOrDefault();
            ViewBag.AgencyID = UserItem.AgencyID;
            var model = _voteApi.GetListSimple(UserItem.AgencyID);
            return View(model);
        }

        public ActionResult GetUserInRole(Guid rId, int dId = 0)
        {
            var model = _usersInRoleApi.GetListAddTree(rId, dId);
            return Json(model);
        }

        public ActionResult GetUserView(Guid rId, int id = 0, int dId = 0)
        {
            var model = _usersInRoleApi.GetListAddTree(rId, dId);
            ViewBag.ID = id;
            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var url = Request.Form.ToString();
            switch (DoAction)
            {
                case ActionType.Add:
                    msg = _api.Add(url);
                    break;
                case ActionType.Edit:
                    msg = _api.Update(url);
                    break;
                case ActionType.Delete:
                    var lst1 = string.Join(",", ArrId);
                    msg = _api.Delete(lst1);
                    break;
                default:
                    msg.Message = "Bạn không được phân quyền cho chức năng này.";
                    msg.Erros = true;
                    break;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
