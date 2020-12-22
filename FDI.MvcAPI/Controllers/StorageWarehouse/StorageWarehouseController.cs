using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using DotNetOpenAuth.Messaging;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.DA.DA.StorageWarehouse;
using FDI.Simple;
using FDI.Simple.Order;
using FDI.Utils;
using Newtonsoft.Json;

namespace FDI.MvcAPI.Controllers
{
    public class StorageWarehouseController : BaseApiAuthController
    {
        //
        // GET: /StorageWarehouse/
        readonly StorageWareHouseDA _da = new StorageWareHouseDA();
        private readonly CustomerDA _customerDa = new CustomerDA();
        private readonly StorageWareHouseLogDA _storageWareHouseLogDa = new StorageWareHouseLogDA();
        private readonly CategoryDA _categoryDa = new CategoryDA();

        public ActionResult GetListSimple(string key, int agencyId)
        {
            var obj = key != Keyapi ? new List<StorageWarehousingItem>() : _da.GetListSimple(agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelStorageWarehousingItem()
                : new ModelStorageWarehousingItem { ListItems = _da.GetListSimpleByRequest(Request), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListItemsByOrderId(int orderId)
        {
            var obj = new ModelDNRequestWareHouseItem { ListItems = _da.GetRequestWareByOrderId(orderId) };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConfirmAmount(int orderId)
        {
            var order = _da.GetById(orderId);

            if (order.Status != (int)StatusWarehouse.Pending)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }

            order.Status = (int)StatusWarehouse.WattingConfirm;
            _da.Save();
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MoveProduce(int orderId)
        {

            var order = _da.GetById(orderId);

            const StatusWarehouse status = StatusWarehouse.New | StatusWarehouse.Pending | StatusWarehouse.WattingConfirm;
            var current = (StatusWarehouse)order.Status;
            if (!status.HasFlag(current))
            {

                return Json(new JsonMessage() { Erros = true }, JsonRequestBehavior.AllowGet);
            }

            order.Status = (int)StatusWarehouse.AgencyConfirmed;

            _da.Save();
            return Json(new JsonMessage { Erros = false }, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> UpdateOrder(StorageWarehousingRequest request)
        {
            var customer = _customerDa.GetByQrCode(request.CustomerCode);
            if (customer == null)
            {
                return Json(new JsonMessage() { Erros = true, Message = "Customer is not exits" }, JsonRequestBehavior.AllowGet);
            }

            const StatusWarehouse status = StatusWarehouse.New | StatusWarehouse.Pending | StatusWarehouse.WattingConfirm;
            var order = _da.GetById(request.Code, customer.ID);
            if (!status.HasFlag((StatusWarehouse)order.Status))
            {
                return Json(new JsonMessage() { Erros = true, Message = "Đơn hàng đã được xác nhận" }, JsonRequestBehavior.AllowGet);
            }

            //get remove item
            var removeitems = order.DN_RequestWare.Where(m => request.RequestWares.All(n => n.CateId != m.CateID) && m.IsDelete == false).ToList();
            foreach (var dnRequestWare in removeitems)
            {
                dnRequestWare.IsDelete = true;
            }

            //update item
            var updateitems = order.DN_RequestWare.Where(m => request.RequestWares.Any(n => m.CateID == n.CateId && m.IsDelete == false)).ToList();
            foreach (var dnRequestWare in updateitems)
            {
                var requestitem = request.RequestWares.FirstOrDefault(m => m.CateId == dnRequestWare.CateID);
                dnRequestWare.Quantity = requestitem.Quantity;

                dnRequestWare.DN_RequestWareDetail.Clear();
                foreach (var detail in requestitem.Details)
                {
                    dnRequestWare.DN_RequestWareDetail.Add(new DN_RequestWareDetail()
                    {
                        RequestWareId = Guid.NewGuid(),
                        ProductId = detail.ProductId,
                        Quantity = detail.Quantity,
                    });
                }
            }

            //get add item
            var additems = request.RequestWares.Where(m => !order.DN_RequestWare.Any(n => n.CateID == m.CateId && n.IsDelete == false)).ToList();

            foreach (var item in additems)
            {
                var requestWare = new DN_RequestWare()
                {
                    GID = Guid.NewGuid(),
                    CateID = item.CateId,
                    Quantity = item.Quantity,
                    QuantityActive = item.Quantity,
                    Price = item.Price,
                    TotalPrice = item.TotalPrice,
                    Today = request.ReceiveDate.DecimalToDate().Date.TotalSeconds(),
                    IsDelete = false
                };

                foreach (var detail in item.Details)
                {
                    requestWare.DN_RequestWareDetail.Add(new DN_RequestWareDetail()
                    {
                        RequestWareId = Guid.NewGuid(),
                        ProductId = detail.ProductId,
                        Quantity = detail.Quantity,

                    });
                }
                order.DN_RequestWare.Add(requestWare);
            }
            _da.Save();
            return Json(new JsonMessage { Erros = false });
        }

        public ActionResult AgencyConfirmAmount(int orderId, bool status)
        {
            var order = _da.GetById(orderId);
            if (order.AgencyId != AgencyId)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            if (order.Status != (int)StatusWarehouse.WattingConfirm)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }

            order.Status = (int)StatusWarehouse.AgencyConfirmed;
            _da.Save();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListItemsAll(int area)
        {
            var obj = Request["key"] != Keyapi
                ? new ModelDNRequestWareHouseItem()
                : new ModelDNRequestWareHouseItem { ListItems = _da.GetListSimpleAllByRequest(Request, area), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListItemsStatic(int area)
        {
            var obj = Request["key"] != Keyapi
                ? new ModelDNRequestWareHouseItem()
                : new ModelDNRequestWareHouseItem { ListItems = _da.GetListSimpleStaticByRequest(Request, area), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListExcel(string key)
        {
            var obj = key != Keyapi ? new List<StorageWarehousingItem>() : _da.GetListExcel(Request, Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetStorageWarehousingItem(string key, int id)
        {
            var obj = key != Keyapi ? new StorageWarehousingItem() : _da.GetStorageWarehousingItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListAuto(string key, string keword, int showLimit, int agencyId, int type)
        {
            var obj = key != Keyapi ? new List<SuggestionsProduct>() : _da.GetListAuto(keword, showLimit, agencyId, type);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListAutoProductValue(string key, string keword, int showLimit, int agencyId, int type)
        {
            var obj = key != Keyapi ? new List<SuggestionsProduct>() : _da.GetListAutoProductValue(keword, showLimit, agencyId, type);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(StorageWarehousingRequest request)
        {
            //get customerid
            //var customer = _customerDa.GetByQrCode(request.CustomerCode);
            //if (customer == null)
            //{
            //    return Json(new JsonMessage() { Erros = true, Message = "Customer is not exits" }, JsonRequestBehavior.AllowGet);
            //}

            //if (_da.CheckExistOrder(request.Code, customer.ID))
            //{
            //    return Json(new JsonMessage() { Erros = true, Message = "Mã đơn hàng đã tồn tại" }, JsonRequestBehavior.AllowGet);
            //}

            var model = new StorageWarehousing
            {
                Code = request.Code,
                Status = (int)StatusWarehouse.New,
                // CustomerId = customer.ID,
                DateCreated = DateTime.Now.TotalSeconds(),
                IsDelete = false,
                DateRecive = request.ReceiveDate,
                DN_RequestWare = new List<DN_RequestWare>(),
                UrlConfirm = request.UrlConfirm,
                AgencyId = AgencyId
            };

            foreach (var item in request.RequestWares)
            {

                //get priceunit and costprice
                var category = _categoryDa.GetItemById(item.CateId);
                if (category == null || category.CostPrice == null)
                {
                    return Json(new JsonMessage(true, "Sản phẩm chưa có giá nhập"));
                }

                //get policyagency

                var policyDa = new PolicyAgencyDA();
                var requestWareDA = new RequestWareDA();

                // lay so luong da giao
                var quantityFinish =
                    requestWareDA.GetQuantityFinish(AgencyId, item.CateId, request.ReceiveDate.DecimalToDate().Month);

                //so luong tinh hoa hong
                var quantityCaculate = quantityFinish + item.Quantity;

                decimal sale = 0;
                // tim chinh sach dai ly
                var policies = policyDa.GetAll(item.CateId).OrderBy(m => m.Quantity).ToList();
                var policyActive = policies.Where(m => quantityCaculate <= m.Quantity)
                    .OrderBy(m => m.Quantity).FirstOrDefault();

                if (policyActive != null)
                {
                    sale = item.Quantity * policyActive.Profit;
                }
                else if (policies.Count > 0)
                {
                    sale = item.Quantity * policies[policies.Count - 1].Profit;
                }

                var requestWare = new DN_RequestWare()
                {
                    GID = Guid.NewGuid(),
                    CateID = item.CateId,
                    Quantity = item.Quantity,
                    QuantityActive = item.Quantity,
                    Price = category.Price,
                    CostPrice = category.CostPrice,
                    Sale = sale,
                    //TotalPrice = item.TotalPrice,
                    Today = request.ReceiveDate.DecimalToDate().Date.TotalSeconds(),
                    IsDelete = false
                };

                if (item.Details != null)
                    foreach (var detail in item.Details)
                    {
                        requestWare.DN_RequestWareDetail.Add(new DN_RequestWareDetail()
                        {
                            RequestWareId = Guid.NewGuid(),
                            ProductId = detail.ProductId,
                            Quantity = detail.Quantity,

                        });
                    }
                model.DN_RequestWare.Add(requestWare);
            }
            _da.Add(model);
            _da.Save();

            //add log
            _storageWareHouseLogDa.AddLog(new StorageWarehousingLog()
            {
                StorageWarehousingId = model.ID,
                // NewValue = JsonConvert.SerializeObject(model, new BinaryConverter())
            });

            return Json(model.ID, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AssignUser(UserStorageWarehousingRequest data)
        {
            var order = _da.GetById(data.orderId);
            var users = _da.GetAssignUser(data.orderId);

            if (order == null)
            {
                return Json(new JsonMessage() { Erros = true, Message = "Order not found" });
            }

            var userAdd = data.userIds.Where(m => order.StorageWarehousingUsers.All(n => n.UserId != m)).ToList();
            var userRemove = order.StorageWarehousingUsers.Where(m => data.userIds.All(n => n != m.UserId)).ToList();//get 

            _da.AddAssignUser(data.orderId, userAdd);
            _da.RemoveAssignUser(userRemove);

            _da.Save();
            return Json(new JsonMessage() { Erros = false }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key, string codeLogin)
        {
            var model = new StorageWarehousing();
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                var date = Request["DateCreated_"];

                model.DateRecive = ConvertUtil.ToDateTime(date).TotalSeconds();
                model.DN_RequestWare = GetListImportItem(codeLogin, DateTime.Now, model.DateRecive, Agencyid());
                //model.DateImport = date.TotalSeconds();
                model.Status = (int)StatusWarehouse.Pending;
                model.DateCreated = DateTime.Now.TotalSeconds();
                model.Code = DateTime.Now.ToString("yyMMddHHmm");
                model.AgencyId = Agencyid();
                model.IsDelete = false;
                _da.Add(model);
                _da.Save();
                return Json(model.ID, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Imported(string key, string codeLogin)
        {
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);

                var storage = _da.GetById(ItemId);
                storage.Status = (int)StatusWarehouse.Imported;
                var lstInt = GetListImportedItem(codeLogin, DateTime.Now, Agencyid());
                var lsttemp = _da.GetListProductValueListArr(lstInt);
                var temp = _da.GetListArrIdImport(lsttemp);
                foreach (var dnImportProduct in temp)
                {
                    dnImportProduct.AgencyId = Agencyid();
                }
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public List<DN_RequestWare> GetListImportItem(string code, DateTime date, decimal? dater, int agencyId)
        {
            const string url = "Utility/GetListImportware?key=";
            var urlJson = string.Format("{0}{1}", UrlG + url, code);
            var list = Utility.GetObjJson<List<DNRequestWareHouseNewItem>>(urlJson);
            return list.Select(item => new DN_RequestWare
            {
                GID = Guid.NewGuid(),
                Quantity = item.Quantity,
                Price = item.Price,
                CateID = item.ProductID,
                IsDelete = false,
                Date = DateTime.Now.TotalSeconds(),
                TotalPrice = item.Quantity * item.Price,
                QuantityActive = item.Quantity,
                Today = dater,
                Hour = item.Hours,
                MarketID = item.MarketId,
                AreaID = item.AreaId,
                AgencyID = agencyId,
            }).ToList();
        }
        public List<DN_RequestWarehousing> GetListImportActiveItem(string code, DateTime date)
        {
            const string url = "Utility/GetListImportwareActive?key=";
            var urlJson = string.Format("{0}{1}", UrlG + url, code);
            var list = Utility.GetObjJson<List<DNRequestWareHouseActiveNewItem>>(urlJson);
            return list.Select(item => new DN_RequestWarehousing()
            {
                Quantity = item.Quantity,
                Price = item.Price,
                CateValueID = item.CatevalueId,
                IsDelete = false,
                BarCode = item.BarCode,
            }).ToList();
        }
        public List<int> GetListImportedItem(string code, DateTime date, int agencyId)
        {
            const string url = "Utility/GetListImportwareActive?key=";
            var urlJson = string.Format("{0}{1}", UrlG + url, code);
            var list = Utility.GetObjJson<List<DNRequestWareHouseActiveNewItem>>(urlJson);
            var lstArr = string.Join(",", list.Select(c => c.CatevalueId));
            var lstInt = FDIUtils.StringToListInt(lstArr);
            return lstInt;
        }
        public ActionResult Update(string key, string codeLogin)
        {
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetById(ItemId);
                if (model == null)
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
                UpdateModel(model);
                model.Status = (int)StatusWarehouse.Pending;
                var dateCreated = Request["DateCreated_"];
                model.DateRecive = ConvertUtil.ToDateTime(dateCreated).TotalSeconds();
                var lst = model.DN_RequestWare.Where(c => c.IsDelete == false).ToList();

                var lstNew = GetListImportItem(codeLogin, DateTime.Now, model.DateRecive, Agencyid());
                //xóa
                var result1 = lst.Where(p => lstNew.All(p2 => p2.CateID != p.CateID)).ToList();
                foreach (var i in result1)
                {
                    i.IsDelete = true;
                }
                //sửa
                foreach (var i in lst)
                {
                    var j = lstNew.FirstOrDefault(c => c.CateID == i.CateID);
                    if (j == null) continue;
                    i.Quantity = j.Quantity;
                    i.Price = j.Price;
                    i.Date = j.Date;
                    i.TotalPrice = j.TotalPrice;
                    i.Today = j.Today;
                    i.Hour = j.Hour;
                }
                //thêm mới
                var result2 = lstNew.Where(p => lst.All(p2 => p2.CateID != p.CateID)).ToList();
                model.DN_RequestWare.AddRange(result2);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult UpdateActive(string key, string codeLogin)
        {
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetById(ItemId);
                if (model == null)
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
                UpdateModel(model);
                model.Status = (int)StatusWarehouse.Complete;
                var dateCreated = Request["DateCreated_"];
                var date = ConvertUtil.ToDateTime(dateCreated);
                //var lst = model.DN_RequestWarehousing.Where(c => c.IsDelete == false).ToList();
                var lstNew = GetListImportActiveItem(codeLogin, date);
                //xóa
                //var result1 = lst.Where(p => lstNew.All(p2 => p2.CateValueID != p.CateValueID)).ToList();
                //foreach (var i in result1)
                //{
                //    i.IsDelete = true;
                //}
                ////sửa
                ////foreach (var i in lst)
                ////{
                ////    var j = lstNew.FirstOrDefault(c => c.ProductID == i.ProductValueID);
                ////    if (j == null) continue;
                ////    i.Price = j.Price;
                ////    i.QuantityActive = j.QuantityActive;
                ////    i.DateEnd = j.DateEnd;
                ////    i.Date = j.Date;
                ////    i.BarCode = j.BarCode;
                ////}
                ////thêm mới
                //var result2 = lstNew.Where(p => lst.All(p2 => p2.CateValueID != p.CateValueID)).ToList();
                model.DN_RequestWarehousing.AddRange(lstNew);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Delete(string key, string lstArrId)
        {
            if (key == Keyapi)
            {
                var lstInt = FDIUtils.StringToListInt(lstArrId);
                var lst = _da.GetListArrId(lstInt);
                foreach (var item in lst)
                {
                    item.IsDelete = true;
                }
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Active(string key, Guid userId, string lstArrId)
        {
            var msg = new JsonMessage { Erros = false, Message = "Duyệt đơn đặt hàng thành công." };
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var lstInt = FDIUtils.StringToListInt(lstArrId);
                var model = _da.GetListArrId(lstInt);
                foreach (var item in model.Where(c => c.IsDelete == false))
                {
                    item.Status = (int)StatusWarehouse.Waitting;
                }
                _da.Save();
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                msg = new JsonMessage
                {
                    Erros = true,
                    Message = "Có lỗi xảy ra."
                };
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddSupplier(RequestWareSupplierRequest[] requests)
        {
            //var requestWare = _da.GetRequestWareById(request.RequestWareId);
            //if (requestWare == null)
            //{
            //    return Json(new JsonMessage() { Erros = true, Message = "Request not exits" }, JsonRequestBehavior.AllowGet);
            //}

            //var requestWareSupplier = _da.GetRequestWareBySupplier(request.RequestWareId, request.SupplierId);
            //if (requestWareSupplier != null)
            //{
            //    return Json(new JsonMessage() { Erros = true, Message = "Nhà cung cấp đã được chọn" }, JsonRequestBehavior.AllowGet);
            //}

            ////kiem tra so luong

            //var all = _da.GetAllRequestWareByRequestWareId(request.RequestWareId);
            //if (all.Sum(m => m.Quantity) + request.Quantity > requestWare.Quantity)
            //{
            //    return Json(new JsonMessage() { Erros = true, Message = "Tổng số lượng lớn hơn thực duyệt" }, JsonRequestBehavior.AllowGet);
            //}

            foreach (var request in requests)
            {
                var requestWare = _da.GetRequestWareById(request.RequestWareId);
                if (requestWare == null)
                {
                    return Json(new JsonMessage() { Erros = true, Message = "Request not exits" }, JsonRequestBehavior.AllowGet);
                }

                var requestWareSupplier = _da.GetRequestWareBySupplier(request.RequestWareId, request.SupplierId);
                if (requestWareSupplier != null)
                {
                    return Json(new JsonMessage() { Erros = true, Message = requestWareSupplier.DN_Supplier.Name + " đã được chọn" }, JsonRequestBehavior.AllowGet);
                }

                var item = new DN_RequestWareSupplier()
                {
                    RequestWareId = request.RequestWareId,
                    SupplierId = request.SupplierId,
                    Quantity = request.Quantity,
                    IsDelete = false
                };

                _da.AddRequestWareSupplier(item);
            }

            _da.Save();

            return Json(new JsonMessage() { Erros = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteSupplier(int id)
        {
            var supplier = _da.GetRequestWareBySupplierById(id);
            supplier.IsDelete = true;
            _da.Save();
            return Json(new JsonMessage() { Erros = false });
        }

        public ActionResult ChangeStatus(int orderId, int status)
        {
            var order = _da.GetById(orderId);
            if (order == null)
            {
                return Json(new JsonMessage() { Erros = true, Message = "Order không tồn tại trong hệ thống" });
            }

            order.Status = status;
            _da.Save();
            return Json(new JsonMessage() { Erros = false });

        }

        public ActionResult GetRequestWareSummary()
        {
            var lst = _da.GetRequestWareSummary(Request);
            var obj = new ModelDNRequestWareHouseItem { ListItems = _da.GetRequestWareSummary(Request), PageHtml = _da.GridHtmlPage };

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRequestWareById(Guid id)
        {
            var item = _da.GetRequestWareById(id);
            return Json(new DNRequestWareHouseItem { GID = item.GID, CateID = item.CateID });
        }

        [HttpPost]
        public ActionResult GetRequestWareSummaryByProduct(decimal today, int productId)
        {
            var item = _da.GetRequestWareSummaryByProduct(today, productId);
            return Json(item);
        }
        public ActionResult GetSummaryDetailConfirmed(decimal today)
        {
            return Json(_da.GetSummaryDetailConfirmed(today).ToList(), JsonRequestBehavior.AllowGet);
        }
    }

}
