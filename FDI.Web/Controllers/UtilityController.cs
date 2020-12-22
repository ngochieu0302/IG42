using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Simple.QLCN;
using FDI.Utils;
using Newtonsoft.Json;
using NPOI.HSSF.Record.Chart;
using NPOI.SS.Formula.Functions;

namespace FDI.Web.Controllers
{
    public class UtilityController : Controller
    {
        //
        // GET: /Utility/
        #region ListOrderDetailItems
        protected static List<OrderDetailNewItem> ListOrderDetailItems = new List<OrderDetailNewItem>();
        public string Add(string json)
        {
            var list = JsonConvert.DeserializeObject<OrderDetailNewItem>(json);
            ListOrderDetailItems.Add(list);
            return "";
        }
        public string AddList(string json)
        {
            var list = JsonConvert.DeserializeObject<List<OrderDetailNewItem>>(json);
            ListOrderDetailItems.AddRange(list);
            return "";
        }
        public ActionResult Delete(string key)
        {
            if (ListOrderDetailItems.Any())
                ListOrderDetailItems.RemoveAll(m => m.Key == key);
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListOrderDetailNewItem(string key)
        {
            return Json(ListOrderDetailItems.Where(m => m.Key == key), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region add list user
        protected static List<ListUsersItem> LstUsers = new List<ListUsersItem>();
        protected static List<ListRolesItem> LstRoles = new List<ListRolesItem>();
        public string AddUser(string lstUser)
        {
            if (!string.IsNullOrEmpty(lstUser))
            {
                var lst1 = lstUser.Split(',');
                foreach (var item in lst1.Select(s => new ListUsersItem
                {
                    Key = CodeLogin(),
                    UserId = Guid.Parse(s)
                }))
                {
                    LstUsers.Add(item);
                }
            }
            return "";
        }
        public string AddRole(string lstRole)
        {
            if (!string.IsNullOrEmpty(lstRole) && lstRole != "null")
            {
                var lst2 = lstRole.Split(',');
                foreach (var item in lst2.Select(s => new ListRolesItem
                {
                    Key = CodeLogin(),
                    RoleId = Guid.Parse(s)
                }))
                {
                    LstRoles.Add(item);
                }
            }
            return "";
        }

        public ActionResult DeleteUser(string key)
        {
            if (LstUsers.Any())
                LstUsers.RemoveAll(m => m.Key == key);
            if (LstRoles.Any())
                LstRoles.RemoveAll(m => m.Key == key);
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListUser(string key)
        {
            return Json(LstUsers.Where(m => m.Key == key), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListRole(string key)
        {
            return Json(LstRoles.Where(m => m.Key == key), JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region ListShowDeskItem
        protected static List<ShowDeskItem> ListShowDeskItem = new List<ShowDeskItem>();
        public string AddDesk(string json)
        {
            var list = JsonConvert.DeserializeObject<ShowDeskItem>(json);
            ListShowDeskItem.Add(list);
            return "";
        }
        public string AddDeskList(string json)
        {
            var list = JsonConvert.DeserializeObject<List<ShowDeskItem>>(json);
            ListShowDeskItem.AddRange(list);
            return "";
        }
        public ActionResult DeleteDesk(string key)
        {
            if (ListShowDeskItem.Any())
                ListShowDeskItem.RemoveAll(m => m.Key == key);
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListShowDeskItem(string key)
        {
            return Json(ListShowDeskItem.Where(m => m.Key == key), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region ListImportItems
        protected static List<DNImportNewItem> ListImportItems = new List<DNImportNewItem>();
        public string AddImport(string json)
        {
            var obj = JsonConvert.DeserializeObject<DNImportNewItem>(json);
            ListImportItems.Add(obj);
            return "";
        }
        public ActionResult GetListImportNewItem(string key)
        {
            return Json(ListImportItems.Where(m => m.Key == key), JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteImport(string key)
        {
            if (ListImportItems.Any(m => m.Key == key))
                ListImportItems.RemoveAll(m => m.Key == key);
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ListExportValueItems
        protected static List<ExportNewItem> ListExportNewItem = new List<ExportNewItem>();
        public string AddExportValue(string json)
        {
            var obj = JsonConvert.DeserializeObject<ExportNewItem>(json);
            if (obj != null)
            {
                ListExportNewItem.Add(obj);
            }
            return "";
        }
        public ActionResult GetListExportValue(string key)
        {
            return Json(ListExportNewItem.Where(m => m.Key == key), JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteExportValue(string key)
        {
            if (ListExportNewItem.Any(m => m.Key == key))
                ListExportNewItem.RemoveAll(m => m.Key == key);
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Cost_Product_User
        protected static List<CostProductUserItem> LstCostProductUser = new List<CostProductUserItem>();
        public string AddCostProductUser(string json)
        {
            var obj = JsonConvert.DeserializeObject<CostProductUserItem>(json);
            LstCostProductUser.Add(obj);
            return "";
        }
        public ActionResult GetListCostProductUser(string key)
        {
            return Json(LstCostProductUser.Where(m => m.Key == key), JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteCostProductUser(string key)
        {
            if (LstCostProductUser.Any())
                LstCostProductUser.RemoveAll(m => m.Key == key);
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region ListExportProductItems
        protected static List<ExportProductNewItem> ListExportProductNewItem = new List<ExportProductNewItem>();
        public string AddExportProduct(string json)
        {
            var obj = JsonConvert.DeserializeObject<ExportProductNewItem>(json);
            ListExportProductNewItem.Add(obj);
            return "";
        }
        public ActionResult GetListExportProduct(string key)
        {
            return Json(ListExportProductNewItem.Where(m => m.Key == key), JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteExportProduct(string key)
        {
            if (ListExportProductNewItem.Any())
                ListExportProductNewItem.RemoveAll(m => m.Key == key);
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ListStorageProductItems
        protected static List<ImportProductNewItem> ListImportPs = new List<ImportProductNewItem>();
        public string AddImportPs(string json)
        {
            var obj = JsonConvert.DeserializeObject<ImportProductNewItem>(json);
            ListImportPs.Add(obj);
            return "";
        }
       
        public ActionResult GetListImportPs(string key)
        {
            return Json(ListImportPs.Where(m => m.Key == key), JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteImportPs(string key)
        {
            if (ListImportPs.Any(m => m.Key == key))
                ListImportPs.RemoveAll(m => m.Key == key);
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region add listproduct Promotion
        protected static List<DNImportProductPromotionItem> ListImportPp = new List<DNImportProductPromotionItem>();
        public string AddImportPp(string json)
        {
            var obj = JsonConvert.DeserializeObject<DNImportProductPromotionItem>(json);
            ListImportPp.Add(obj);
            return "";
        }
        public ActionResult DeleteImportPp(string key)
        {
            if (ListImportPp.Any(m => m.Key == key))
                ListImportPp.RemoveAll(m => m.Key == key);
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListImportPp(string key)
        {
            return Json(ListImportPp.Where(m => m.Key == key), JsonRequestBehavior.AllowGet);
        }
        protected static List<DNPromotionProductItem> ListImportPm = new List<DNPromotionProductItem>();
        public string AddImportPm(string json)
        {
            var obj = JsonConvert.DeserializeObject<DNPromotionProductItem>(json);
            ListImportPm.Add(obj);
            return "";
        }
        public ActionResult DeleteImportPm(string key)
        {
            if (ListImportPm.Any(m => m.Key == key))
                ListImportPm.RemoveAll(m => m.Key == key);
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListImportPm(string key)
        {
            return Json(ListImportPm.Where(m => m.Key == key), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region add list StorageWarehouse
        protected static List<DNRequestWareHouseNewItem> ListImportware = new List<DNRequestWareHouseNewItem>();
        public ActionResult DeleteImportware(string key)
        {
            if (ListImportware.Any(m => m.Key == key))
                ListImportware.RemoveAll(m => m.Key == key);
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        public string AddImportware(string json)
        {
            var obj = JsonConvert.DeserializeObject<DNRequestWareHouseNewItem>(json);
            ListImportware.Add(obj);
            return "";
        }

        public ActionResult GetListImportware(string key)
        {
            return Json(ListImportware.Where(m => m.Key == key), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region add list StorageWarehouse
        protected static List<DNRequestWareHouseActiveNewItem> ListImportwareActive = new List<DNRequestWareHouseActiveNewItem>();
        public ActionResult DeleteImportwareActive(string key)
        {
            if (ListImportwareActive.Any(m => m.Key == key))
                ListImportwareActive.RemoveAll(m => m.Key == key);
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        public string AddImportwareActive(string json)
        {
            var obj = JsonConvert.DeserializeObject<DNRequestWareHouseActiveNewItem>(json);
            ListImportwareActive.Add(obj);
            return "";
        }

        public ActionResult GetListImportwareActive(string key)
        {
            return Json(ListImportwareActive.Where(m => m.Key == key), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region add list StorageFreihouse
        protected static List<FreightWarehouseNewItem> ListImportFrei = new List<FreightWarehouseNewItem>();
        public ActionResult DeleteImportFrei(string key)
        {
            if (ListImportFrei.Any(m => m.Key == key))
                ListImportFrei.RemoveAll(m => m.Key == key);
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        public string AddImportFrei(string json)
        {
            var obj = JsonConvert.DeserializeObject<FreightWarehouseNewItem>(json);
            ListImportFrei.Add(obj);
            return "";
        }

        public ActionResult GetListImportFrei(string key)
        {
            return Json(ListImportFrei.Where(m => m.Key == key), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region add list StorageFreihouse
        protected static List<FreightWarehouseActiveNewItem> ListImportFreiActive = new List<FreightWarehouseActiveNewItem>();
        public ActionResult DeleteImportFreiActive(string key)
        {
            if (ListImportFreiActive.Any(m => m.Key == key))
                ListImportFreiActive.RemoveAll(m => m.Key == key);
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        public string AddImportFreiActive(string json)
        {
            var obj = JsonConvert.DeserializeObject<FreightWarehouseActiveNewItem>(json);
            ListImportFreiActive.Add(obj);
            return "";
        }

        public ActionResult GetListImportFreiActive(string key)
        {
            return Json(ListImportFreiActive.Where(m => m.Key == key), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region ListProductRecipe
        protected static List<RecipeItem> ListRecipeItem = new List<RecipeItem>();
        public string AddRecipeItem(string json)
        {
            var obj = JsonConvert.DeserializeObject<RecipeItem>(json);
            ListRecipeItem.Add(obj);
            return "";
        }
        public ActionResult GetListRecipeItem(string key)
        {
            return Json(ListRecipeItem.Where(m => m.Key == key), JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteRecipeItem(string key)
        {
            if (ListRecipeItem.Any())
                ListRecipeItem.RemoveAll(m => m.Key == key);
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region ListProductDetail Recipe
        protected static List<ProductRecipeItem> ListDetailRecipeItem = new List<ProductRecipeItem>();
        public string AddDetailRecipeItem(string json)
        {
            var obj = JsonConvert.DeserializeObject<ProductRecipeItem>(json);
            ListDetailRecipeItem.Add(obj);
            return "";
        }
        public ActionResult GetListDetailRecipeItem(string key)
        {
            return Json(ListDetailRecipeItem.Where(m => m.Key == key), JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteDetailRecipeItem(string key)
        {
            if (ListDetailRecipeItem.Any())
                ListDetailRecipeItem.RemoveAll(m => m.Key == key);
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region category Recipe
        protected static List<RecipeCateItem> ListCategoryRecipeItem = new List<RecipeCateItem>();
        public string AddCategoryRecipeItem(string json)
        {
            var obj = JsonConvert.DeserializeObject<RecipeCateItem>(json);
            ListCategoryRecipeItem.Add(obj);
            return "";
        }
        public ActionResult GetListCategoryRecipeItem(string key)
        {
            return Json(ListCategoryRecipeItem.Where(m => m.Key == key), JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteCategoryRecipeItem(string key)
        {
            if (ListCategoryRecipeItem.Any())
                ListCategoryRecipeItem.RemoveAll(m => m.Key == key);
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region ListNguyenlieu
        protected static List<MenNguyenlieuCNItem> ListMenNguyenlieu = new List<MenNguyenlieuCNItem>();
        public string AddNguyenlieuCNItem(string json)
        {
            var obj = JsonConvert.DeserializeObject<MenNguyenlieuCNItem>(json);
            ListMenNguyenlieu.Add(obj);
            return "";
        }
        public ActionResult GetListNguyenlieuCNItem(string key)
        {
            return Json(ListMenNguyenlieu.Where(m => m.Key == key), JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteNguyenlieuCNItem(string key)
        {
            if (ListMenNguyenlieu.Any())
                ListMenNguyenlieu.RemoveAll(m => m.Key == key);
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region ListNguyenlieuCamCam
        protected static List<CamNguyenlieuCNItem> ListMenNguyenlieuCam = new List<CamNguyenlieuCNItem>();
        public string AddNguyenlieuCamCNItem(string json)
        {
            var obj = JsonConvert.DeserializeObject<CamNguyenlieuCNItem>(json);
            ListMenNguyenlieuCam.Add(obj);
            return "";
        }
        public ActionResult GetListNguyenlieuCamCNItem(string key)
        {
            return Json(ListMenNguyenlieuCam.Where(m => m.Key == key), JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteNguyenlieuCamCNItem(string key)
        {
            if (ListMenNguyenlieuCam.Any())
                ListMenNguyenlieuCam.RemoveAll(m => m.Key == key);
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Stack
        readonly StackDA _stackDa = new StackDA();
        static List<StackValueItem> listStack = new List<StackValueItem>();
        static List<DNUserItem> listUserItem = new List<DNUserItem>();
        private static int CountStack = 0;
        public ActionResult GetAllStackByToday(string key, int agencyid)
        {
            if (key == "checkinfdi")
            {
                if (!listStack.Any())
                {
                    var date = DateTime.Today.TotalSeconds();
                    var jsonStack = _stackDa.GetJsonByDate(date, agencyid);
                    listStack = jsonStack!=null ? JsonConvert.DeserializeObject<List<StackValueItem>>(jsonStack) : new List<StackValueItem>();
                    //var json = new JavaScriptSerializer().Serialize(objitem);
                    CountStack = listStack.OrderByDescending(m => m.I).Select(m => m.I).FirstOrDefault();
                }
                listUserItem = _dnUser.GetListAllSevice(agencyid, string.Join(",", listStack.Select(m => m.U)));
                foreach (var item in listUserItem)
                {
                    var ci = ConvertUtil.ToInt32(item.CodeCheckIn);
                    var obj = listStack.FirstOrDefault(m => m.U == ci);
                    if (obj != null)
                    {
                        item.Sort = obj.I;
                    }
                }
                return View(listUserItem.OrderBy(m=>m.Sort));
            }
            return View(new List<DNUserItem>());
        }
        public ActionResult GetJsonAllStackByToday(string key, int agencyid)
        {
            if (key == "checkinfdi")
            {
                if (!listStack.Any())
                {
                    var date = DateTime.Today.TotalSeconds();
                    var jsonStack = _stackDa.GetJsonByDate(date, agencyid);
                    listStack = jsonStack != null ? JsonConvert.DeserializeObject<List<StackValueItem>>(jsonStack) : new List<StackValueItem>();
                    //var json = new JavaScriptSerializer().Serialize(objitem);
                    CountStack = listStack.OrderByDescending(m => m.I).Select(m => m.I).FirstOrDefault();
                }
                listUserItem = _dnUser.GetListAllSevice(agencyid, string.Join(",", listStack.Select(m => m.U)));
                foreach (var item in listUserItem)
                {
                    var ci = ConvertUtil.ToInt32(item.CodeCheckIn);
                    var obj = listStack.FirstOrDefault(m => m.U == ci);
                    if (obj != null)
                    {
                        item.Sort = obj.I;
                    }
                }
                return Json(listUserItem.OrderBy(m => m.Sort), JsonRequestBehavior.AllowGet);
            }
            return Json(new List<DNUserItem>(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateUserStack(string key, int agencyid, string jsonUser)
        {
            if (key == "checkinfdi")
            {
                if (listStack.Any())
                {
                    var datetoday = DateTime.Today.TotalSeconds();
                    var stack = _stackDa.GetByDate(datetoday, agencyid);
                    var obj = JsonConvert.DeserializeObject<List<ListBebDesk>>(jsonUser);
                    foreach (var listBebDesk in obj)
                    {
                        CountStack++;
                        var objstack = listStack.FirstOrDefault(m => m.U == listBebDesk.c);
                        if (objstack != null) objstack.I = CountStack;
                        else
                        {
                            objstack = new StackValueItem { I = CountStack, U = listBebDesk.c };
                            listStack.Add(objstack);
                        }
                    }
                    var jsons = new JavaScriptSerializer().Serialize(listStack);
                    if (stack != null && stack.Date.HasValue) stack.Json = jsons;
                    else
                    {
                        stack = new DN_Stack
                        {
                            Date = datetoday,
                            AgencyID = agencyid,
                            Json = jsons
                        };
                        _stackDa.Add(stack);
                    }
                    _stackDa.Save();

                }
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        private void UpdateStack(string codeon, int agencyid, bool? check)
        {
            if (check.HasValue && check.Value)
            {
                var mck = ConvertUtil.ToInt32(codeon);
                var objstack = listStack.FirstOrDefault(m => m.U == mck);
                if (objstack != null) listStack.Remove(objstack);
                else
                {
                    CountStack++;
                    objstack = new StackValueItem { I = CountStack, U = mck };
                    listStack.Add(objstack);
                }
                var datetoday = DateTime.Today.TotalSeconds();
                var stack = _stackDa.GetByDate(datetoday, agencyid);
                var jsons = new JavaScriptSerializer().Serialize(listStack);
                if (stack != null && stack.Date.HasValue) stack.Json = jsons;
                else
                {
                    stack = new DN_Stack
                    {
                        Date = datetoday,
                        AgencyID = agencyid,
                        Json = jsons
                    };
                    _stackDa.Add(stack);
                }
                _stackDa.Save();
            }
        }
        #endregion
        #region Chấm công
        readonly DNTimeJobAPI _dnTimeJobApi = new DNTimeJobAPI();
        readonly DNAgencyAPI _dnAgencyApi = new DNAgencyAPI();
        readonly DNCalendarAPI _calendarApi = new DNCalendarAPI();
        readonly DNUserAPI _dnUser = new DNUserAPI();
        public ActionResult GetAllOnlineByToday(string key, int agencyid)
        {
            if (key == "checkinfdi")
            {
                var listItem = _dnTimeJobApi.GetAllOnlineByToday(agencyid);
                return Json(listItem, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAgencyItemById(string key, int agencyid)
        {
            if (key == "checkinfdi")
            {
                var listItem = _dnAgencyApi.GetItemById(agencyid);
                return Json(listItem, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult checkall(int agencyid)
        {
            _dnUser.GetListAllAgency(agencyid);
                var listItem = _dnAgencyApi.GetItemById(agencyid);
                return Json(listItem, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckIn(string key, string codeon, int agencyid)
        {
            if (key == "checkinfdi")
            {
                var userid = _dnUser.GetUserIdByCodeCheckIn(codeon, agencyid);
                if (userid != null && userid.UserId != new Guid())
                {
                    UpdateStack(codeon, agencyid, userid.IsService);

                    var date = DateTime.Now;
                    var totalS = date.Hour * 60 + date.Minute;
                    var dates = date.TotalSeconds();
                    var dnTimeJob = new DNTimeJobItem
                    {
                        UserId = userid.UserId,
                        AgencyID = agencyid,
                        DateCreated = dates,
                        DateEnd = dates,
                        ScheduleEndID = null,
                        MinutesEarly = 0,
                        ScheduleID = null,
                        MinutesLater = 0,
                    };
                    var list = _calendarApi.GetItemByUserIdDate(userid.UserId, agencyid, dates).OrderByDescending(m => m.Hms).ToList();
                    if (list.Any())
                    {
                        //// Get Ca check in chấm công
                        dnTimeJob = JobTimeCheckIn(list, dnTimeJob, totalS);
                        //// Get Ca check out chấm công
                        dnTimeJob = JobTimeCheckOut(list, dnTimeJob, totalS);
                       
                        
                    }
                    var json = new JavaScriptSerializer().Serialize(dnTimeJob);
                    var obj = _dnTimeJobApi.Add("", json, agencyid);
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public DNTimeJobItem JobTimeCheckIn(List<CheckInItem> list, DNTimeJobItem objItem, decimal totalS)
        {
            var obj = list.FirstOrDefault(m => (m.Hms < totalS && m.Hme > totalS) || m.Hms - 32 <= totalS);
            if (obj != null)
            {
                var b = totalS - obj.Hms;
                objItem.ScheduleID = obj.ID;
                objItem.MinutesLater = b > 0 ? (int)b : 0;
            }
            return objItem;
        }

        public DNTimeJobItem JobTimeCheckOut(List<CheckInItem> list, DNTimeJobItem objItem, decimal totalS)
        {
            var obj = list.FirstOrDefault(m => (m.Hms >= totalS && m.Hme > totalS) || m.Hme + 32 > totalS);
            if (obj != null)
            {
                var b = obj.Hme - totalS;
                objItem.ScheduleEndID = obj.ID;
                objItem.MinutesEarly = b > 0 ? (int)b : 0;
            }
            return objItem;
        }
        #endregion
        readonly DNMailSSCAPI _dnMailSscapi = new DNMailSSCAPI();
        public ActionResult GetContentEmail(string key)
        {
            var obj = _dnMailSscapi.GetCacheMail(key);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public string CodeLogin()
        {
            var codeCookie = HttpContext.Request.Cookies["CodeLogin"];
            return codeCookie == null ? null : codeCookie.Value;
        }


        

    }
}