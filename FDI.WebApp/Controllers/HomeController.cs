using System.Web.Mvc;
using FDI.DA;
using System.Linq;
using FDI.Utils;
using FDI.Web.Common;
using FDI.Simple;
using System;
using System.Web.UI;

namespace FDI.Web.Controllers
{
    public class HomeController : Controller
    {
        readonly ModulePageBL _pageDa = new ModulePageBL();
        readonly SeoBL _selBL = new SeoBL();

        //readonly SEOCommon _seo = new SEOCommon();
        [MinifyHtmlFilter] // nén nội dung html
        public ActionResult Index(int? pageId, int cateId = 0, string slug = "home", string tag = "")
        {
            //var domain = Request.Url.Authority;
            //if (!string.IsNullOrEmpty(domain) && "localhost:2163" != domain)
            //{
            //    var lw = domain.Split('.');
            //    var key = lw[0];
            //    var address8 = new Uri(Request.Url.AbsoluteUri);
            //    if (key.ToLower() == "www")
            //    {
            //        var u = "https://" + domain.Substring(4, domain.Length - 4) + Request.Url.AbsolutePath;
            //        return Redirect(u);
            //    }
            //    if (Uri.UriSchemeHttps != address8.Scheme)
            //    {
            //        var u = "https://" + domain + Request.Url.AbsolutePath;
            //        return Redirect(u);
            //    }
            //}
            var seo = new SEOItem();
            if (tag.Equals("tag"))
            {
                seo = _selBL.GetSeoTag(slug);
                pageId = int.Parse(WebConfig.NewsId);
            }
            if (slug == "home")
            {
                seo = _selBL.GetSeoPage(2);
            }
            
            var model = pageId != null ? _pageDa.GetById((int)pageId) : _pageDa.GetBykey(slug);
            model.SeoItem = seo;
            //if (model == null || !WebConfig.LstLang.Contains(lang))
            //{
            //    return Redirect("/");
            //}
            //Config.BeginRequest(lang);
            return PartialView(model);
        }

        public ActionResult News(string slug)
        {
            var domain = Request.Url.Authority;
            if (!string.IsNullOrEmpty(domain) && "localhost:2163" != domain)
            {
                var lw = domain.Split('.');
                var key = lw[0];
                var address8 = new Uri(Request.Url.AbsoluteUri);
                if (key.ToLower() == "www")
                {
                    var u = "https://" + domain.Substring(4, domain.Length - 4) + Request.Url.AbsolutePath;
                    return Redirect(u);
                }
                if (Uri.UriSchemeHttps != address8.Scheme)
                {
                    var u = "https://" + domain + Request.Url.AbsolutePath;
                    return Redirect(u);
                }
            }
            var pageId = int.Parse(WebConfig.NewsId);
            var model = _pageDa.GetById(pageId);
            if (model == null)
            {
                return Redirect("/");
            }
            var seo = _selBL.GetSeoNews(slug);
            model.SeoItem = seo ?? new SEOItem();
            return PartialView("~/Views/Home/Index.cshtml", model);
        }
        public ActionResult Detail(string slug)
        {
            var domain = Request.Url.Authority;
            if (!string.IsNullOrEmpty(domain) && "localhost:2163" != domain)
            {
                var lw = domain.Split('.');
                var key = lw[0];
                var address8 = new Uri(Request.Url.AbsoluteUri);
                if (key.ToLower() == "www")
                {
                    var u = "https://" + domain.Substring(4, domain.Length - 4) + Request.Url.AbsolutePath;
                    return Redirect(u);
                }
                if (Uri.UriSchemeHttps != address8.Scheme)
                {
                    var u = "https://" + domain + Request.Url.AbsolutePath;
                    return Redirect(u);
                }
            }
            var pageId = int.Parse(WebConfig.NewDetail);
            var model = _pageDa.GetById(pageId);
            if (model == null)
            {
                return Redirect("/");
            }
            var seo = _selBL.GetSeoNews(slug);
            model.SeoItem = seo ?? new SEOItem();
            model.Key = "chi-tiet";
            return PartialView("~/Views/Home/Index.cshtml", model);
        }

       

        

        

        public ActionResult Google()
        {
            return PartialView();
        }
        
    }
}