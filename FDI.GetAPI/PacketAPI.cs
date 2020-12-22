using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class PacketAPI : BaseAPI
    {
        public List<PacketItem> GetListSimple(int agencyId)
        {
            var urlJson = string.Format("{0}Packet/GetListSimple?key={1}&agencyId={2}", Domain, Keyapi, agencyId);
            return GetObjJson<List<PacketItem>>(urlJson);
        }

        public List<PacketItem> GetListNotInBedDesk(int agencyid)
        {
            var urlJson = string.Format("{0}Packet/GetListNotInBedDesk?key={1}&agencyId={2}", Domain, Keyapi,agencyid);
            return GetObjJson<List<PacketItem>>(urlJson);
        }

        public ModelPacketItem ListItems(int agencyid, string url)
        {
            url = string.IsNullOrEmpty(url) ? "?" : url;
            var urlJson = string.Format("{0}Packet/ListItems{1}&key={2}&agencyId={3}", Domain, url, Keyapi,agencyid);
            return GetObjJson<ModelPacketItem>(urlJson);
        }

        public PacketItem GetPacketItems(int id)
        {
            var urlJson = string.Format("{0}Packet/GetPacketItems?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<PacketItem>(urlJson);
        }
        public List<TreeViewItem> GetListTreeByTypeListId(int type, string lstId, int agencyId)
        {
            var urlJson = string.Format("{0}Packet/GetListTreeByTypeListId?key={1}&type={2}&lstId={3}&agencyId={4}", Domain, Keyapi, type, lstId, agencyId);
            return GetObjJson<List<TreeViewItem>>(urlJson);
        }

        public int Add(string json)
        {
            var urlJson = string.Format("{0}Packet/Add?key={1}&{2}", Domain, Keyapi,json);
            return GetObjJson<int>(urlJson);
        }
        public int Update(string json)
        {
            var urlJson = string.Format("{0}Packet/Update?key={1}&{2}", Domain, Keyapi,json);
            return GetObjJson<int>(urlJson);
        }
        public int Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}Packet/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<int>(urlJson);
        }
    }
}
