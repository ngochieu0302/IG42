using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.SYS;
using FDI.Utils;

namespace FDI.Areas.Admin.Controllers
{
    public class SystemTagController : BaseController
    {
        readonly System_TagDA _tagDa = new System_TagDA("#");
        
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult ListItems()
        {
            var model = new ModelTagItem
            {
                ListItem = _tagDa.GetListSimpleByRequest(Request),
                PageHtml = _tagDa.GridHtmlPage
            };
            ViewData.Model = model;
            return View();
        }

        public ActionResult AjaxForm()
        {
            var tagModel = new System_Tag();
            if (DoAction == ActionType.Edit)
                tagModel = _tagDa.GetById(ArrId.FirstOrDefault());

            ViewData.Model = tagModel;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }

        public ActionResult AutoComplete()
        {
            if (DoAction == ActionType.Add) //Nếu thêm từ khóa
            {
                JsonMessage msg;
                var tagValue = Request["Values"];
                
                if (string.IsNullOrEmpty(tagValue))
                {
                    msg = new JsonMessage
                    {
                        Erros = true,
                        Message = "Bạn phải nhập tên từ khóa"
                    };
                }
                else
                {
                    var tag = _tagDa.AddOrGet(tagValue);
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = tag.ID.ToString(),
                        Message = tag.Name
                    };
                }
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            var query = Request["query"];
            var ltsResults = _tagDa.GetListSimpleByAutoComplete(query, 10);
            var resulValues = new AutoCompleteItem
            {
                query = query,
                data = ltsResults.Select(o => o.ID.ToString()).ToList(),
                suggestions = ltsResults.Select(o => o.Name).ToList()
            };
            return Json(resulValues, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutoSystemTag()
        {
            var query = Request["query"];
            var ltsResults = _tagDa.GetListSimpleByAutoComplete(query, 10);
            var productsItems = ltsResults as List<TagItem> ?? ltsResults.ToList();
            var suggItem = (from c in productsItems
                            select new SuggestionsItem
                            {
                                value = c.Name,
                                data = c.ID.ToString(),
                            }).ToList();

            var resulValues = new AutoCompleteCommonItem
            {
                query = query,
                suggestions = suggItem,
            };
            return Json(resulValues, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var tag = new System_Tag();
            List<System_Tag> ltsTagItems;
            StringBuilder stbMessage;
            switch (DoAction)
            {
                case ActionType.Add:
                    UpdateModel(tag);
                    tag.LanguageId = Fdisystem.LanguageId;
                    tag.NameAscii = FomatString.Slug(tag.Name);
                    tag.IsDelete = false;
                    tag.IsShow = true;
                    _tagDa.Add(tag);
                    _tagDa.Save();
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = tag.ID.ToString(),
                        Message = string.Format("Đã thêm mới từ khóa: <b>{0}</b>", Server.HtmlEncode(tag.Name))
                    };
                    break;
                case ActionType.Edit:
                    tag = _tagDa.GetById(ArrId.FirstOrDefault());
                    UpdateModel(tag);
                    tag.NameAscii = FomatString.Slug(tag.Name);
                    _tagDa.Save();
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = tag.ID.ToString(),
                        Message = string.Format("Đã cập nhật từ khóa: <b>{0}</b>", Server.HtmlEncode(tag.Name))
                    };
                    break;
                case ActionType.Delete:
                    ltsTagItems = _tagDa.GetListByArrId(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsTagItems)
                    {
                        item.IsDelete = true;
                        stbMessage.AppendFormat("Đã xóa từ khóa <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    msg.ID = string.Join(",", ArrId);
                    _tagDa.Save();
                    msg.Message = stbMessage.ToString();
                    break;
                case ActionType.Show:
                    ltsTagItems = _tagDa.GetListByArrId(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsTagItems)
                    {
                        item.IsShow = true;
                        stbMessage.AppendFormat("Đã hiển thị từ khóa <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    _tagDa.Save();
                    msg.ID = string.Join(",", ltsTagItems.Select(o => o.ID));
                    msg.Message = stbMessage.ToString();
                    break;
                case ActionType.Hide:
                    ltsTagItems = _tagDa.GetListByArrId(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsTagItems)
                    {
                        item.IsShow = false;
                        stbMessage.AppendFormat("Đã ẩn từ khóa <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    _tagDa.Save();
                    msg.ID = string.Join(",", ltsTagItems.Select(o => o.ID));
                    msg.Message = stbMessage.ToString();
                    break;
            }
            if (string.IsNullOrEmpty(msg.Message))
            {
                msg.Message = "Không có hành động nào được thực hiện.";
                msg.Erros = true;
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AutoCompleteTag(string name)
        {
            var list = new List<TagItem>();
            if (!string.IsNullOrEmpty(name))
                list = _tagDa.GetListSimpleByAutoComplete(name, 10);
            return Json(new { list = list }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddTagToProduct(int tagId, int productId)
        {
            _tagDa.AddTagToProduct(tagId, productId);
            return Json(true);
        }

        [HttpPost]
        public ActionResult RemoveTagFromProduct(int tagId, int productId)
        {
            _tagDa.RemoveTagFrompProduct(tagId, productId);
            return Json(true);
        }
    }
}
