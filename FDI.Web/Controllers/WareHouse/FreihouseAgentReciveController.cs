using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.CORE;
using FDI.DA;
using FDI.DA.DA.DN_Sales;
using FDI.DA.DA.StorageWarehouse;
using FDI.GetAPI;
using FDI.GetAPI.StorageWarehouse;
using FDI.Simple;
using FDI.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace FDI.Web.Controllers.WareHouse
{
    public class FreihouseAgentReciveController : BaseController
    {
        //
        // GET: /FreihouseAgentRecive/
        readonly StorageFreightWarehouseDA _da = new StorageFreightWarehouseDA("#");
        private readonly StorageFreightWarehouseAPI _api = new StorageFreightWarehouseAPI();
        readonly DNSalesDA _dnSalesDa = new DNSalesDA("#");
        readonly DNPromotionDA _dnPromotionDa = new DNPromotionDA("#");
        readonly AgencyDA _dnAgencyDa = new AgencyDA();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.ListItemsRecive(UserItem.AgencyID, Request.Url.Query));
        }
        public ActionResult AjaxForm()
        {
            var model = new StorageFreightWarehouseItem();
            if (DoAction == ActionType.Edit)
                model = _api.GetStorageFreightWarehousesItem(ArrId.FirstOrDefault());
            ViewBag.UserID = UserItem.UserId;
            ViewBag.User = UserItem.UserName;
            ViewBag.Action = DoAction;
            return View(model);
        }
        public ActionResult AjaxView()
        {
            var model = _api.GetStorageFreightWarehousesItem(ArrId.FirstOrDefault());
            return View(model);
        }
        public ActionResult Actions()
        {
            var msg = new JsonMessage { Erros = false, Message = "Cập nhật dữ liệu thành công" };
            var url = Request.Form.ToString();
            switch (DoAction)
            {
                case ActionType.Edit:
                    if (_api.UpdateActive(url, CodeLogin(), UserItem.UserId) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra.";
                    }
                    break;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Auto()
        {
            var query = Request["query"];
            var type = Request["type"] ?? "0";
            var ltsResults = _api.GetListAutoOne(query, 10, UserItem.AgencyID, int.Parse(type));
            var resulValues = new AutoCompleteProduct
            {
                query = query,
                suggestions = ltsResults
            };
            return Json(resulValues, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CreateOder(int id, Guid keyGuid)
        {
            var lstOrder = (List<ModelWholeSaleItem>)Session["WholeSale"] ?? new List<ModelWholeSaleItem>();
            if (lstOrder.Any())
            {
                var temp = _da.GetById(id);
                if (keyGuid == temp.keyreq)
                {
                    var models = lstOrder.Where(c => c.Key == keyGuid);
                    if (!models.Any())
                    {
                        var lst = temp.FreightWareHouse_Active;
                        var model = new ModelWholeSaleItem { Key = keyGuid };
                        var lsttemp = new List<WholeSaleItem>();
                        foreach (var item in lst)
                        {
                            var saleProducts = _dnSalesDa.GetSaleProduct(item.ProductID ?? 0, UserItem.AgencyID);
                            var pricesale = saleProducts.Sale.Any() ? saleProducts.Sale.Sum(c => c.Price) : 0;
                            var percent = saleProducts.Sale.Any() ? saleProducts.Sale.Sum(c => c.PercentSale) : 0;
                            var discount = pricesale + (item.Price * percent / 100);
                            // var checkpromotion = _dnPromotionDa.GetPromotionProduct(item.ProductID ?? 0, UserItem.AgencyID, item.QuantityActive ?? 1);
                            var urlimg = "/Content/Admin/images/auto-default.jpg";
                            if (item.Shop_Product.Shop_Product_Detail.Gallery_Picture != null)
                            {
                                urlimg = "/Uploads/" + item.Shop_Product.Shop_Product_Detail.Gallery_Picture.Folder +
                                         item.Shop_Product.Shop_Product_Detail.Gallery_Picture.Url;
                            }

                            var a = new WholeSaleItem
                            {
                                Key = CodeLogin(),
                                PriceSale = pricesale,
                                PercentSale = percent,
                                Value = item.ValueWeight,
                                Discount = discount,
                                //TotalPrice = (item.Shop_Product.Shop_Product_Detail.Price * item.Shop_Product.Product_Size.Value / 1000 * item.Quantity * item.ValueWeight) - discount,
                                Quantity = item.Quantity,
                                Barcode = item.BarCode,
                                //Price = item.Shop_Product.Shop_Product_Detail.Price * item.Shop_Product.Product_Size.Value / 1000,
                                Code = item.Shop_Product.CodeSku,
                                ProductID = item.ProductID ?? 0,
                                Idimport = item.ImportProductGID,
                                ProductdetailID = item.Shop_Product.Shop_Product_Detail.ID,
                                Title = item.Shop_Product.Shop_Product_Detail.Name,
                                UrlImg = urlimg
                            };
                            lsttemp.Add(a);
                        }
                        model.WholeSaleItems = lsttemp;
                        var total = model.WholeSaleItems.Sum(c => c.TotalPrice * c.Quantity);
                        var sale = _dnSalesDa.GetSaleByTotalOrder((decimal)total, UserItem.AgencyID);
                        model.SaleOrder = sale;
                        var checkdiscount = _dnAgencyDa.GetById(temp.AgencyId ?? 0);
                        var agency = new AgentSaleItem
                        {
                            Address = checkdiscount.Address,
                            Name = checkdiscount.Name,
                            Phone = checkdiscount.Phone,
                            //Bonus = checkdiscount.
                            ID = checkdiscount.ID,
                            AgentGroup = checkdiscount.DN_GroupAgency.Name
                        };
                        model.AgentSaleItem = agency;
                        var discsl = ((total * sale.Sum(p => p.PercentSale) / 100) + sale.Sum(p => p.Price));
                        var disc = (total * (checkdiscount.DN_GroupAgency.Discount ?? 0) / 100);
                        model.DiscountSale = discsl;
                        model.Discount = disc;
                        model.SalePercent = total * sale.Sum(p => p.PercentSale) / 100;
                        model.SalePrice = sale.Sum(p => p.Price);
                        model.Total = total;
                        model.TotalPrice = total - discsl - disc;
                        lstOrder.Add(model);
                        Session["WholeSale"] = lstOrder;
                    }
                }

            }
            else
            {
                var temp = _da.GetById(id);
                if (keyGuid == temp.keyreq)
                {
                    var lst = temp.FreightWareHouse_Active;
                    var model = new ModelWholeSaleItem { Key = keyGuid };
                    var lsttemp = new List<WholeSaleItem>();
                    foreach (var item in lst)
                    {
                        var saleProducts = _dnSalesDa.GetSaleProduct(item.ProductID ?? 0, UserItem.AgencyID);
                        var pricesale = saleProducts.Sale.Any() ? saleProducts.Sale.Sum(c => c.Price) : 0;
                        var percent = saleProducts.Sale.Any() ? saleProducts.Sale.Sum(c => c.PercentSale) : 0;
                        var discount = pricesale + (item.Price * percent / 100);
                        // var checkpromotion = _dnPromotionDa.GetPromotionProduct(item.ProductID ?? 0, UserItem.AgencyID, item.QuantityActive ?? 1);
                        var urlimg = "/Content/Admin/images/auto-default.jpg";
                        if (item.Shop_Product.Shop_Product_Detail.Gallery_Picture != null)
                        {
                            urlimg = "/Uploads/" + item.Shop_Product.Shop_Product_Detail.Gallery_Picture.Folder +
                                     item.Shop_Product.Shop_Product_Detail.Gallery_Picture.Url;
                        }

                        var a = new WholeSaleItem
                        {
                            Key = CodeLogin(),
                            PriceSale = pricesale,
                            PercentSale = percent,
                            Value = item.ValueWeight,
                            Idimport = item.ImportProductGID,
                            Discount = discount,
                            //TotalPrice = (item.Shop_Product.Shop_Product_Detail.Price * item.Shop_Product.Product_Size.Value / 1000 * item.Quantity * item.ValueWeight) - discount,
                            Quantity = item.Quantity,
                            //Price = item.Shop_Product.Shop_Product_Detail.Price * item.Shop_Product.Product_Size.Value / 1000,
                            Code = item.Shop_Product.CodeSku,
                            Barcode = item.BarCode,
                            ProductID = item.ProductID ?? 0,
                            ProductdetailID = item.Shop_Product.Shop_Product_Detail.ID,
                            Title = item.Shop_Product.Shop_Product_Detail.Name,
                            UrlImg = urlimg
                        };
                        lsttemp.Add(a);
                    }
                    model.WholeSaleItems = lsttemp;
                    var total = model.WholeSaleItems.Sum(c => c.TotalPrice);
                    var sale = _dnSalesDa.GetSaleByTotalOrder((decimal)total, UserItem.AgencyID);
                    model.SaleOrder = sale;
                    var checkdiscount = _dnAgencyDa.GetById(temp.AgencyId ?? 0);
                    var agency = new AgentSaleItem
                    {
                        Address = checkdiscount.Address,
                        Name = checkdiscount.Name,
                        Phone = checkdiscount.Phone,
                        //Bonus = checkdiscount.
                        ID = checkdiscount.ID,
                        AgentGroup = checkdiscount.DN_GroupAgency.Name
                    };
                    model.AgentSaleItem = agency;
                    var discsl = ((total * sale.Sum(p => p.PercentSale) / 100) + sale.Sum(p => p.Price));
                    var disc = (total * (checkdiscount.DN_GroupAgency.Discount ?? 0) / 100);
                    model.DiscountSale = discsl;
                    model.Discount = disc;
                    model.Total = total;
                    model.TotalPrice = total - discsl - disc;
                    lstOrder.Add(model);
                    Session["WholeSale"] = lstOrder;
                }

            }

            return RedirectToAction("Index", "WholeSale");
        }
        #region xuất ra file excel
        public ActionResult ProcessExportFile()
        {
            var lst = _api.GetListExcel(UserItem.AgencyID, Request.Url.Query);
            var fileName = string.Format("nhap-kho_{0}.xlsx", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            var filePath = Path.Combine(Request.PhysicalApplicationPath, "File\\ExportImport", fileName);
            var folder = Request.PhysicalApplicationPath + "File\\ExportImport";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            ExportProductsExcel(filePath, lst);
            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, "text/xls", fileName);
        }

        public virtual void ExportProductsExcel(string filePath, List<StorageFreightWarehouseItem> lstStorageProduct)
        {
            var newFile = new FileInfo(filePath);

            // ok, we can run the real code of the sample now
            int dem = 0;
            using (var xlPackage = new ExcelPackage(newFile))
            {
                // uncomment this line if you want the XML written out to the outputDir
                //xlPackage.DebugMode = true; 
                // get handle to the existing worksheet
                var worksheet = xlPackage.Workbook.Worksheets.Add("Phiếu nhập kho");
                xlPackage.Workbook.CalcMode = ExcelCalcMode.Manual;
                //Create Headers and format them
                var properties = new string[]
                    {
                        "STT",
                        "Mã phiếu",
                        "Ngày nhập",
                        "Tổng tiền",
                        "Ghi chú"
                    };
                for (var i = 0; i < properties.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = properties[i];
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                }

                var row = 2;
                foreach (var item in lstStorageProduct)
                {
                    dem++;
                    int col = 1;
                    worksheet.Cells[row, col].Value = dem;
                    col++;

                    worksheet.Cells[row, col].Value = item.Code;
                    col++;
                    // email
                    worksheet.Cells[row, col].Value = ConvertDate.DecimalToDate(item.DateCreated).ToString("dd/MM/yyyy");
                    col++;
                    // điện thoại
                    worksheet.Cells[row, col].Value = item.TotalPrice.Money();
                    col++;
                    // địa chỉ
                    worksheet.Cells[row, col].Value = item.Note;
                    row++;
                }

                // we had better add some document properties to the spreadsheet 
                // set some core property values
                var nameexcel = "Danh sách nhập kho" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
                xlPackage.Workbook.Properties.Title = string.Format("{0} reports", nameexcel);
                xlPackage.Workbook.Properties.Author = "FDI-IT";
                xlPackage.Workbook.Properties.Subject = string.Format("{0} reports", "");
                //xlPackage.Workbook.Properties.Keywords = string.Format("{0} orders", _storeInformationSettings.StoreName);
                xlPackage.Workbook.Properties.Category = "Report";
                //xlPackage.Workbook.Properties.Comments = string.Format("{0} orders", _storeInformationSettings.StoreName);
                // set some extended property values
                xlPackage.Workbook.Properties.Company = "FDI ";
                //xlPackage.Workbook.Properties.HyperlinkBase = new Uri(_storeInformationSettings.StoreUrl);
                // save the new spreadsheet
                xlPackage.Save();
            }
        }

        #endregion

    }
}
