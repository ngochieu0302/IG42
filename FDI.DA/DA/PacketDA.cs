using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class PacketDA : BaseDA
    {
        public PacketDA()
        {
        }
        public PacketDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public PacketDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        public List<PacketItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyid)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.DN_Packet
                        where c.AgencyID == agencyid
                        orderby c.ID descending
                        select new PacketItem
                        {
                            ID = c.ID,
                            Name = c.Name.Trim(),
                            Sort = c.Sort,
                            Time = c.Time,
                            Price = c.Price,
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<PacketItem> GetListSimple(int agencyId)
        {
            var query = from o in FDIDB.DN_Packet
                        where o.AgencyID == agencyId
                        orderby o.ID descending
                        select new PacketItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            Sort = o.Sort
                        };
            return query.ToList();
        }
        public List<TreeViewItem> GetListTreeByTypeListId(int type, string lstId, int agencyId = 0)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from c in FDIDB.DN_Bed_Desk
                        where c.ID > 0 && c.IsDeleted == false  && c.IsShow == true  && c.AgencyId == agencyId
                        orderby c.ID
                        select new TreeViewItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            IsShow = ltsArrId.Any(m => m == c.ID),
                        };
            return query.ToList();
        }
        public List<PacketItem> GetListNotInBedDesk(int agencyId)
        {
            var query = from o in FDIDB.DN_Packet
                        where o.AgencyID == agencyId
                        orderby o.Sort descending
                        select new PacketItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            Sort = o.Sort,
                            Time = o.Time,
                            Price = o.Price,
                            TimeEarly = o.TimeEarly,
                            TimeWait = o.TimeWait,
                            IsEarly = o.IsEarly.HasValue && o.IsEarly.Value,
                            IsDefault = o.IsDefault,
                            LstProduct = o.DN_Product_Packet.Select(m=>new ProductItem
                            {
                                ID = m.Shop_Product.ID,
                                //Value = m.Shop_Product.Value,
                                PriceNew = (m.Shop_Product.Shop_Product_Detail.Price* (decimal)m.Shop_Product.Product_Size.Value/1000) ?? 0
                            })
                        };
            return query.ToList();
        }
        public PacketItem GetPacketItems(int id)
        {
            var query = from o in FDIDB.DN_Packet
                        where o.ID == id
                        orderby o.ID descending
                        select new PacketItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            IsEarly = o.IsEarly.HasValue && o.IsEarly.Value,
                            Sort = o.Sort,
                            Time = o.Time,
                            Price = o.Price,
                            TimeEarly = o.TimeEarly,
                            TimeWait = o.TimeWait,
                            IsDefault = o.IsDefault,
                            LstProduct = from v in o.DN_Product_Packet
                                         select new ProductItem
                                         {
                                             ID = v.Shop_Product.ID,
                                             //Name = v.Name
                                         },
                            LstBedDesk = from v in o.DN_Bed_Desk
                                         where v.IsDeleted == false && v.IsShow == true
                                         select new BedDeskItem
                                         {
                                             ID = v.ID,
                                             Name = v.Name
                                         },
                                         ListProductPacketItems = o.DN_Product_Packet.Select(z=>new DNProductPacketItem
                                         {
                                             ProductId = z.ProductId,
                                             Price = z.Shop_Product.Shop_Product_Detail.Price* (decimal)z.Shop_Product.Product_Size.Value/1000,
                                             NameProduct = z.Shop_Product.Shop_Product_Detail.Name,
                                             IsDefault = z.IsDefault,
                                            
                                         })
                        };
            return query.FirstOrDefault();
        }
        public List<DN_Bed_Desk> GetListBeDesk(List<int> ltsArrId)
        {
            var query = from c in FDIDB.DN_Bed_Desk where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }
        public List<Shop_Product> GetListProduct(List<int> ltsArrId)
        {
            var query = from c in FDIDB.Shop_Product where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }
        public List<DN_Packet> GetListByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.DN_Packet where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }
        public List<DN_Product_Packet> GetListByPacketID(int Id)
        {
            var query = from c in FDIDB.DN_Product_Packet where c.PacketID == Id select c;
            return query.ToList();
        }
        public DN_Packet GetById(int id)
        {
            var query = from c in FDIDB.DN_Packet where c.ID == id select c;
            return query.FirstOrDefault();
        }
        public void Add(DN_Packet item)
        {
            FDIDB.DN_Packet.Add(item);
        }
        public void AddProduct(DN_Product_Packet item)
        {
            FDIDB.DN_Product_Packet.Add(item);
        }
        public void Delete(DN_Packet item)
        {
            FDIDB.DN_Packet.Remove(item);
        }
        public void DeleteProductPacket(DN_Product_Packet item)
        {
            FDIDB.DN_Product_Packet.Remove(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
