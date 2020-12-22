using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web.Mvc;
using System.Xml.Linq;

namespace FDI.Web.Controllers
{
    public class SitemapController : Controller
    {
        //[Route("sitemap.xml")]
        public ActionResult Index()
        {
            var a = Server.MapPath(@"sitemap.xml");

            List<SitemapNode> nodes = new List<SitemapNode>();
            nodes.Add(new SitemapNode(){Url = "http://simso.local/Sitemap", Frequency = SitemapFrequency.Weekly,Priority = 1});
            nodes.Add(new SitemapNode(){Url = Url.Action("About", "Home", null, Request.Url.Scheme),Frequency = SitemapFrequency.Daily,Priority = 0.9});
            nodes.Add(new SitemapNode(){Url = Url.Action("Contact", "Home", null, Request.Url.Scheme),Frequency = SitemapFrequency.Always,Priority = 0.9});

            XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            XElement root = new XElement(xmlns + "urlset");
            foreach (var item in nodes)
            {
                XElement urlElement = new XElement(
                    xmlns + "url",
                    new XElement(xmlns + "loc", Uri.EscapeUriString(item.Url)),
                    item.LastModified == null ? null : new XElement(xmlns + "lastmod", item.LastModified.Value.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:sszzz")),
                    item.Frequency == null ? null : new XElement(xmlns + "changefreq", item.Frequency.Value.ToString().ToLowerInvariant()),
                    item.Priority == null ? null : new XElement(xmlns + "priority", item.Priority.Value.ToString("F1", CultureInfo.InvariantCulture))
                    );
                root.Add(urlElement);
            }
            //root.Save(a);

            XDocument document = new XDocument(root);
            document.Save(a);

            //Console.WriteLine(System.IO.File.ReadAllText("sitemap.xml"));

            return this.Content(document.ToString(), "text/xml", Encoding.UTF8);
        }
        public class SitemapNode
        {
            public SitemapFrequency? Frequency { get; set; }
            public DateTime? LastModified { get; set; }
            public double? Priority { get; set; }
            public string Url { get; set; }
        }
        public enum SitemapFrequency
        {
            Never,
            Yearly,
            Monthly,
            Weekly,
            Daily,
            Hourly,
            Always
        }        
    }
}
