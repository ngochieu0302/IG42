using System.Collections.Generic;
using FDI.Simple;

namespace FDI.GetAPI
{
    public class ModulePageAPI : BaseAPI
    {

        public List<SysPageItem> GetChildByParentId(int parentId = 1, int moduleType = 0, int root = 1)
        {
            var urlJson = string.Format("{0}ModulePage/GetListChildByParentId?key={1}&parentId={2}&moduleType={3}&root={4}", Domain, Keyapi, parentId, moduleType, root);
            return GetObjJson<List<SysPageItem>>(urlJson);
        }

        public List<TreeViewItem> GetListTree()
        {
            var urlJson = string.Format("{0}ModulePage/GetListTree?key={1}", Domain, Keyapi);
            return GetObjJson<List<TreeViewItem>>(urlJson);
        }

        public List<SysPageItem> GetAllListSimpleByParentId(int parentId)
        {
            var urlJson = string.Format("{0}ModulePage/GetAllListSimpleByParentId?key={1}&parentId={2}", Domain, Keyapi, parentId);
            return GetObjJson<List<SysPageItem>>(urlJson);
        }

        public List<SysPageItem> GetListSimpleByAutoComplete(string keyword, int showLimit, bool isShow)
        {
            var urlJson = string.Format("{0}ModulePage/GetListSimpleByAutoComplete?key={1}&keyword={2}&showLimit={3}&isShow={4}", Domain, Keyapi, keyword, showLimit, isShow);
            return GetObjJson<List<SysPageItem>>(urlJson);
        }

        public bool CheckTitleAsciiExits(string keyword, int id)
        {
            var urlJson = string.Format("{0}ModulePage/CheckTitleAsciiExits?key={1}&keyword={2}&id={3}", Domain, Keyapi, keyword, id);
            return GetObjJson<bool>(urlJson);
        }

        public SysPageItem GetBykey(string keyword, int agencyId)
        {
            var urlJson = string.Format("{0}ModulePage/GetBykey?key={1}&keyword={2}&agencyId={3}", Domain, Keyapi, keyword, agencyId);
            return GetObjJson<SysPageItem>(urlJson);
        }

        public SysPageItem GetById(int id)
        {
            var urlJson = string.Format("{0}ModulePage/GetById?key={1}&id={2}", Domain, Keyapi, id);
            return GetObjJson<SysPageItem>(urlJson);
        }

    }
}
