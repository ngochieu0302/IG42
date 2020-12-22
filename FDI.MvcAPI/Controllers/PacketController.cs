using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers
{
    public class PacketController : BaseApiController
    {
        //
        // GET: /ContactOrder/
        private readonly PacketDA _da = new PacketDA();       

        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelPacketItem()
                : new ModelPacketItem { ListItem = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListNotInBedDesk(string key, string code)
        {
            var obj = key != Keyapi ? new List<PacketItem>() : _da.GetListNotInBedDesk(Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListTreeByTypeListId(string key, int type, string lstId)
        {
            var obj = key != Keyapi ? new List<TreeViewItem>() : _da.GetListTreeByTypeListId(type, lstId, Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListSimple(string key,int agencyId)
        {
            var obj = key != Keyapi ? new List<PacketItem>() : _da.GetListSimple(agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPacketItems(string key,int id)
        {
            var obj = key != Keyapi ? new PacketItem() : _da.GetPacketItems(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key)
        {
            var model = new DN_Packet();
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                model.Name = HttpUtility.UrlDecode(model.Name);
                //var lstP = Request["LstProductID"];
                var lstB = Request["BedDeskID"];
                //model.Shop_Product = _da.GetListProduct(FDIUtils.StringToListInt(lstP));
                model.DN_Bed_Desk = _da.GetListBeDesk(FDIUtils.StringToListInt(lstB));
                var packetproduct = new List<DN_Product_Packet>();
                var stt = ConvertUtil.ToInt32(Request["do_stt"]);
                for (var i = 1; i <= stt; i++)
                {
                    var a = Request["Default_" + i];
                    var b = Request["Price_" + i];
                    var c = Request["Obj_" + i];
                    if (!string.IsNullOrEmpty(c))
                    {
                        var obj = new DN_Product_Packet
                        {
                            ProductId = ConvertUtil.ToInt32(Request["Obj_" + i] ?? "0"),
                            IsDefault = !string.IsNullOrEmpty(a)
                            
                        };
                        packetproduct.Add(obj);
                        //total += obj.Price ?? 0;

                    }
                }
                model.DN_Product_Packet = packetproduct;
                _da.Add(model);
                _da.Save();
                return Json(model.ID, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Update(string key)
        {
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetById(ItemId);
                UpdateModel(model);
                var lstBd = Request["BedDeskID"];
                //var lstP = Request["LstProductID"];
                //model.Shop_Product.Clear();
                

                model.DN_Bed_Desk.Clear();
                model.Name = HttpUtility.UrlDecode(model.Name);
                model.DN_Bed_Desk = _da.GetListBeDesk(FDIUtils.StringToListInt(lstBd));
                //model.Shop_Product = _da.GetListProduct(FDIUtils.StringToListInt(lstP));
                var pack = _da.GetListByPacketID(ItemId);
                var dn = new List<DN_Product_Packet>();
                foreach (var item in pack)
                {
                    var name = Request["Obj_old" + item.ProductId];
                    if (string.IsNullOrEmpty(name))
                    {
                        dn.Add(item);
                    }
                    else
                    {
                        var a = Request["Default_old" + item.ProductId];
                        var c = Request["Obj_old" + item.ProductId];
                        item.ProductId = ConvertUtil.ToInt32(c);
                        item.IsDefault = !string.IsNullOrEmpty(a);

                    }

                }
                foreach (var item in dn)
                {
                    _da.DeleteProductPacket(item);
                }
                var stt = ConvertUtil.ToInt32(Request["do_stt"]);
                for (var i = 1; i <= stt; i++)
                {
                    var a = Request["Default_" + i];
                    var c = Request["Obj_" + i];
                    if (!string.IsNullOrEmpty(c))
                    {
                        var obj = new DN_Product_Packet
                        {
                            ProductId = ConvertUtil.ToInt32(c),
                            IsDefault = !string.IsNullOrEmpty(a),
                            PacketID = ItemId
                        };
                        _da.AddProduct(obj);
                        //total += obj.Price ?? 0;

                    }
                }
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
                var lst = _da.GetListByArrId(lstInt);
                foreach (var item in lst)
                {
                    _da.Delete(item);
                }
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}
