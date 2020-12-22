using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using FDI.Admin.Common;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Admin.Controllers
{
    public class DNCardController : BaseController
    {
        //
        // GET: /Admin/Customer/
        private readonly DNCardDA _da;
        private readonly SendCardDA _sendCardDa = new SendCardDA("#");

        public DNCardController()
        {
            _da = new DNCardDA("#");
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            var model = new ModelDNCardItem
            {
                ListItems = _da.GetListSimpleByRequest(Request),
                PageHtml = _da.GridHtmlPage
            };
            return View(model);
        }
        public ActionResult AjaxView()
        {
            var customerType = _da.GetById(ArrId.FirstOrDefault());
            var model = customerType;
            return View(model);
        }
        public ActionResult AjaxForm()
        {
            var model = new DN_Card();
            if (DoAction == ActionType.Edit)
            {
                model = _da.GetById(ArrId.FirstOrDefault());
            }
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View(model);
        }
        public ActionResult AjaxStatus()
        {
            return View();
        }
        public ActionResult AddSerial()
        {
            return View();
        }
        public ActionResult Auto()
        {
            var query = Request["query"];
            var ltsResults = _da.GetListAuto(query, 10);
            var resulValues = new AutoCompleteProduct
            {
                query = query,
                suggestions = ltsResults,
            };
            return Json(resulValues, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportExcell()
        {
            var model = _da.GetListExport(Request);
            var fileName = "Danh_sach_the.xlsx";
            var filePath = Path.Combine(Request.PhysicalApplicationPath, "File\\ExportImport", fileName);
            var folder = Request.PhysicalApplicationPath + "File\\ExportImport";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            var di = new DirectoryInfo(folder);
            foreach (var file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (var dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
            Excel.ExportListCard(filePath, model);
            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, "text/xls", fileName);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage(false,"Cập nhật dữ liệu thành công");
            var model = new DN_OrderCard();
            var itemCard = new DN_Card();
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        var number = int.Parse(Request["numberCard"]);
                        var date = DateTime.Now.ToString("yyMMdd");
                        model.Code = int.Parse(date);
                        var count = _da.Count(model.Code);
                        const int status = (int)Card.Create;
                        UpdateModel(model);
                        for (var i = 1; i <= number; i++)
                        {
                            var item = new DN_Card
                            {
                                Code = Guid.NewGuid().ToString("N").ToUpper(),
                                Serial = FDIUtils.RandomMaKh(date, count + i, 6),
                                PinCard = FDIUtils.RandomKey(9),
                                IsActive = false,
                                Status = status
                            };
                            model.DN_Card.Add(item);
                        }
                        model.DateCreate = ConvertDate.TotalSeconds(DateTime.Now);
                        _da.Add(model);
                        _da.Save();                        
                    }
                    catch (Exception ex)
                    {
                        msg.Erros = true;
                        msg.Message = "Dữ liệu chưa được cập nhật.";
                    }
                    break;
                case ActionType.Order:
                    try
                    {
                        var sendcarditem = new Send_Card();
                        UpdateModel(sendcarditem);
                        var customerid = Request["CustomerID"];
                        var lstSerial = Request["LstSerial"];
                        var lstInt = FDIUtils.StringToListInt(lstSerial);
                        foreach (var i in lstInt)
                        {
                            _sendCardDa.Add(new Send_Card
                            {
                                CustomerID = int.Parse(customerid),
                                //CardID = i,
                                DateCreate = ConvertDate.TotalSeconds(DateTime.Now)
                            });
                        }
                        _sendCardDa.Save();                        
                    }
                    catch (Exception ex)
                    {
                        msg.Erros = true;
                        msg.Message = "Dữ liệu chưa được cập nhật.";
                    }
                    break;
                case ActionType.Active:
                    try
                    {
                        var firstCard = Request["firstCard"];
                        var oldCard = Request["oldCard"];
                        var status = Request["Status"];
                        var lst = _da.UpdateCard(firstCard,oldCard);

                        if(lst!=null && !string.IsNullOrEmpty(status))
                        {
                            var t = int.Parse(status);
                            foreach(var item in lst){
                                item.Status = t;
                            }
                        }
                        _da.Save();
                    }
                    catch (Exception ex)
                    {
                        msg.Erros = true;
                        msg.Message = "Dữ liệu chưa được cập nhật.";
                    }
                    break;
               
                case ActionType.Show:
                    try
                    {
                        var lstCard = _da.GetById(ArrId);
                        foreach (var item in lstCard)
                        {
                            item.Status = (int)Card.Released;
                        }
                        _da.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = model.ID.ToString(),
                            Message = "Thẻ đã được mở thành công"
                        };
                    }
                    catch (Exception ex)
                    {
                        msg.Erros = true;
                        msg.Message = "Dữ liệu chưa được cập nhật.";
                    }
                    break;
                case ActionType.Hide:
                    try
                    {
                        var lstCard = _da.GetById(ArrId);
                        foreach (var item in lstCard)
                        {
                            item.Status = (int)Card.Lock;
                        }
                        _da.Save();                        
                    }
                    catch (Exception ex)
                    {
                        msg.Erros = true;
                        msg.Message = "Dữ liệu chưa được cập nhật.";
                    }
                    break;
            }
            if (string.IsNullOrEmpty(msg.Message))
            {
                msg.Message = "Không có hành động nào được thực hiện.";
                msg.Erros = true;
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
