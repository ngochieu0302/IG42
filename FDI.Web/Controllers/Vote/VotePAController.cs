using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers.Vote
{
    public class VotePAController : BaseController
    {
        private readonly VoteAPI _api = new VoteAPI();
        private readonly ContentVoteAPI _contentVoteApi = new ContentVoteAPI();
        private readonly DNTreeAPI _dnTreeApi = new DNTreeAPI();
        private readonly LevelVoteAPI _levelVoteApi = new LevelVoteAPI();
        //private readonly DNUserDA _daUser = new DNUserDA("#");
        public ActionResult Index()
        {
            var list = _dnTreeApi.GetListSimple(UserItem.AgencyID);
            var obj = list.Where(m => m.UserName == UserItem.UserName).ToList();
            var model = new ModelDNTreeItem
            {
                UserID = UserItem.UserId,
                ListItem = new List<DNTreeItem>()
            };
            foreach (var lst in obj.Select(item => list.Where(c => (2 - c.Level) < 3 && c.ParentID != 1 && (item.ListID.Contains("," + c.ID + ",") || c.ID == item.ID) || c.ListID.Contains("," + item.ID + ",")).ToList()).Where(lst => lst != null && lst.Count > 0))
            {
                model.ListItem.AddRange(lst);
            }

            return View(model);
        }
        public ActionResult ListItems(string date, int treeid = 0)
        {
            var model = new ModelVoteItem
            {
                ListItems = _api.GetList(treeid, UserItem.AgencyID, UserItem.AgencyID, date, UserItem.UserId),
                LevelVoteItems = _levelVoteApi.GetList(UserItem.AgencyID)
            };
            return View(model);
        }

        public ActionResult SumWMY()
        {
            var model = _contentVoteApi.SumContentVoteItem(UserItem.UserId);
            return View(model);
        }

        public ActionResult ListVote(string date, Guid? userid)
        {
            var model = _contentVoteApi.ListItemByUserId(UserItem.AgencyID, date, userid);
            return View(model);
        }

        public ActionResult AddUpdate()
        {
            var url = Request.QueryString.ToString();
            var msg = _contentVoteApi.AddUpdate(url, UserItem.AgencyID, UserItem.UserId);
            return Json(msg, JsonRequestBehavior.AllowGet);
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
                default:
                    msg.Message = "Không có hành động nào được thực hiện.";
                    msg.Erros = true;
                    break;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
