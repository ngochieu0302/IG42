using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using System.IO;
using FDI.Utils;
namespace FDI.Web.Controllers
{
    public class ContentVoteController : BaseController
    {
        private readonly ContentVoteAPI _api = new ContentVoteAPI();
        private readonly DNUserAPI _userapi = new DNUserAPI();
        private readonly VoteAPI _voteapi = new VoteAPI();

        public ActionResult Index()
        {
            var model = new ModelContentVoteItem
            {
                UserItems = _userapi.GetListSimple(UserItem.AgencyID),
                VoteItems = _voteapi.GetListSimple(UserItem.AgencyID)
            };
            return View(model);
        }

        public ActionResult ListItems()
        {
            
            return View(_api.ListItems(UserItem.AgencyID, Request.Url.Query));
        }
        public ActionResult ExportExcel()
        {
            var model = _api.GetListExport(UserItem.AgencyID, Request.Url.Query);
            var fileName = "Danh_sach_danh_gia_nv.xlsx";
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
            Excels.ExportListCard(filePath, model);
            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, "text/xls", fileName);
        }
    }
}
