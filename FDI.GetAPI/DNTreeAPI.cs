using System;
using System.Collections.Generic;
using FDI.Simple;
using FDI.Utils;

namespace FDI.GetAPI
{
    public class DNTreeAPI : BaseAPI
    {
        public List<DNTreeItem> GetListSimple(int agencyId)
        {
            var urlJson = string.Format("{0}DNTree/GetListSimple?key={1}&agencyId={2}", Domain, Keyapi, agencyId);
            return GetObjJson<List<DNTreeItem>>(urlJson);
        }
        public List<DNTreeItem> GetList(int agencyId)
        {
            var urlJson = string.Format("{0}DNTree/GetList?key={1}&agencyId={2}", Domain, Keyapi, agencyId);
            return GetObjJson<List<DNTreeItem>>(urlJson);
        }
        public List<DNTreeItem> GetListParent(int agencyId)
        {
            var urlJson = string.Format("{0}DNTree/GetListParent?key={1}&agencyId={2}", Domain, Keyapi, agencyId);
            return GetObjJson<List<DNTreeItem>>(urlJson);
        }
        public List<DNTreeItem> GetListTreeItem(Guid userid)
        {
            var urlJson = string.Format("{0}DNTree/GetListTreeItem?key={1}&userid={2}", Domain, Keyapi, userid);
            return GetObjJson<List<DNTreeItem>>(urlJson);
        }
        public List<TreeViewItem> GetListTree(int agencyId)
        {
            var urlJson = string.Format("{0}DNTree/GetListTree?key={1}&agencyId={2}", Domain, Keyapi, agencyId);
            return GetObjJson<List<TreeViewItem>>(urlJson);
        }
        public List<TreeViewItem> GetListUser(int treeId, int agencyId)
        {
            var urlJson = string.Format("{0}DNTree/GetListUser?key={1}&treeId={2}&agencyId={3}", Domain, Keyapi, treeId, agencyId);
            return GetObjJson<List<TreeViewItem>>(urlJson);
        }
        public DNTreeItem GetDNTreeItem(int id)
        {
            var urlJson = string.Format("{0}DNTree/GetDNTreeItem?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<DNTreeItem>(urlJson);
        }
        public JsonMessage Add(string json)
        {
            var urlJson = string.Format("{0}DNTree/Add?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Update(string json)
        {
            var urlJson = string.Format("{0}DNTree/Update?key={1}&{2}", Domain, Keyapi, json);
            return GetObjJson<JsonMessage>(urlJson);
        }
        public JsonMessage Delete(string lstArrId)
        {
            var urlJson = string.Format("{0}DNTree/Delete?key={1}&lstArrId={2}", Domain, Keyapi, lstArrId);
            return GetObjJson<JsonMessage>(urlJson);
        }
    }
}
