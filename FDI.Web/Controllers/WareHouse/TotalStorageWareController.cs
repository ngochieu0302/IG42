using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.Base;
using FDI.CORE;
using FDI.DA.DA.StorageWarehouse;
using FDI.GetAPI;
using FDI.GetAPI.StorageWarehouse;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers.WareHouse
{
    public class TotalStorageWareController : BaseController
    {
        //
        // GET: /TotalStorageWare/
        readonly TotalStorageWareAPI _api = new TotalStorageWareAPI();
        readonly TotalStorageWareDA _da = new TotalStorageWareDA("#");
        readonly SupplieAPI _supplieApi = new SupplieAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.ListItems(Request.Url.Query, UserItem.AreaID));
        }
        public ActionResult AjaxForm()
        {
            var model = new TotalStorageWareItem();
            if (DoAction == ActionType.Edit)
                model = _api.GetTotalStorageWareItem(ArrId.FirstOrDefault());
            ViewBag.UserID = UserItem.UserId;
            ViewBag.User = UserItem.UserName;
            ViewBag.Wallet = UserItem.AgencyWallet ?? 0;
            ViewBag.Deposit = UserItem.AgencyDeposit ?? 0;
            ViewBag.Action = DoAction;
            ViewBag.listncc = _supplieApi.GetList(UserItem.AgencyID);
            return View(model);
        }

        public ActionResult GetListSupplie()
        {
            //var model = new ModelSupplierItem();
            //var lst1 = new JavaScriptSerializer().Deserialize<List<Itemdisable>>(lst ?? "");
            //model.ListInts = new List<int>();
            //if (lst1 != null)
            //{
            //    model.ListInts = lst1.Select(c => c.value).ToList();
            //}
            //model.ListItem = _supplieApi.GetList(UserItem.AgencyID);
            var model = _supplieApi.GetList(UserItem.AgencyID);
            return View(model);
        }

        public ActionResult Actions()
        {
            var msg = new JsonMessage() { Erros = false, Message = "Cập nhập dữ liệu thành công.!" };
            //var url = Request.Form.ToString();
            //url = HttpUtility.UrlDecode(url);
            try
            {
                //msg = _api.AddStorage(url, UserItem.AgencyID, UserItem.UserId);
                var stora = _da.GetListStorabyId(ArrId.FirstOrDefault());
                var ncc = _da.GetbyId(ArrId.FirstOrDefault());
                var lst = new List<StorageProduct>();
                var totalquantity = 0;
                var stt = ConvertUtil.ToInt32(Request["do_stt"]);
                //sua
                foreach (var item in stora)
                {
                    var sup = Request["DNSupplie_old" + item.ID];
                    if (string.IsNullOrEmpty(sup))
                    {
                        lst.Add(item);
                    }
                    else
                    {
                        var quantity = Request["QuantityActive_old_" + item.ID];
                        var tmp = Math.Ceiling(decimal.Parse(quantity ?? "0"));
                        var hourI = Request["HourImport_old" + item.ID] ?? "0";
                        var dateI = ConvertUtil.ToDateTime(Request["today_old" + item.ID]).TotalSeconds();
                        item.SupID = int.Parse(sup);
                        item.Quantity = (int)tmp;
                        item.DateImport = dateI;
                        item.HoursImport = int.Parse(hourI);
                        totalquantity += (int)tmp;
                    }
                }
                //xoa
                foreach (var item in lst)
                {
                    _da.DeleteStora(item);
                }
                //them moi
                for (var i = 1; i <= stt; i++)
                {
                    var supId = ConvertUtil.ToInt32(Request["DNSupplie_add_" + i] ?? "0");
                    if (supId > 0)
                    {
                        var test = Request["QuantityActive_add_" + i];
                        var tmp = Math.Ceiling(decimal.Parse(test ?? "0"));
                        //var quantity = ConvertUtil.ToInt32(test ?? "0");
                        var hourI = Request["Hours_add_" + i] ?? "0";
                        var dateI = ConvertUtil.ToDateTime(Request["Date_add_" + i]).TotalSeconds();
                        totalquantity += (int)tmp;
                        var obj = new StorageProduct
                        {
                            UserID = UserItem.UserId,
                            AgencyId = UserItem.AgencyID,
                            IsDelete = false,
                            DateCreated = DateTime.Now.TotalSeconds(),
                            Code = DateTime.Now.ToString("yyMMddHHmm"),
                            Quantity = (int)tmp,
                            Price = ncc.Category.Price,
                            SupID = supId,
                            Today = ncc.Today,
                            Hour = ncc.Hour,
                            TotalPrice = tmp * (ncc.Category.Price ?? 0),
                            TotalID = ncc.ID,
                            CateID = ncc.CateID,
                            DateImport = dateI,
                            HoursImport = int.Parse(hourI),
                            AreaID = UserItem.AreaID
                        };
                        _da.AddStora(obj);
                    }
                    else
                    {
                        msg = new JsonMessage
                        {
                            Erros = true,
                            Message = "Bạn Chưa chọn nhà cung cấp.!"
                        };
                        return Json(msg, JsonRequestBehavior.AllowGet);
                    }
                }
                var dem = (double)(totalquantity - (ncc.Quantity));
                if (dem > 0.5)
                {
                    msg = new JsonMessage
                    {
                        Erros = true,
                        Message = "Bạn đã xuất quá số lượng thực tế.!"
                    };
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ncc.QuantityOut = totalquantity;
                    _da.Save();
                }


            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được cập nhật.";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}