using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;
using System.Data.SqlClient;
using System.Web.Security;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml;

namespace FDI.Areas.Admin.Controllers
{
    public class CreateSitemapController : BaseController
    {
        const string url = "https://simso.vip/";
        const string file = "sitemapsim.txt";
        //
        // GET: /Admin/CustomerType/      
        readonly ProductAPI _api = new ProductAPI();
        private readonly CategoryDA _da = new CategoryDA();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AjaxForm()
        {
            return PartialView();
        }
        public ActionResult AjaxRemove()
        {
            return PartialView();
        }
        public ActionResult ListItems()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            switch (DoAction)
            {
                case ActionType.Show:
                    try
                    {
                        int total = _api.TotalSim();
                        const int t = 30000;
                        int du = total % t;
                        int p = total / t;
                        p = du > 0 ? p + 1 : p;
                        for (int i = 1; i <= p; i++)
                        {
                            var filesale = ConfigData.SitemapFolder + "sitemap-sim-so-" + i + ".txt";
                            System.IO.File.Copy(ConfigData.SitemapFileFolder + file, filesale);
                            var list = _api.PageProductSitemap(t, i, 2010, total);
                            var listener = new TextWriterTraceListener(filesale);
                            const string text = url;
                            listener.WriteLine(text);
                            foreach (var item in list)
                            {
                                listener.WriteLine(url + "dat-sim-sim-so-dep-0" + item);
                            }
                            listener.Flush();
                            listener.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        Log2File.LogExceptionToFile(ex);
                        msg.Message = ex.Message;
                        //msg.Message = "Dữ liệu chưa được backup.";
                        msg.Erros = true;
                    }
                    break;
                case ActionType.Active:
                    try
                    {
                        string filesale = ConfigData.Folder + "SitemapUpdate.txt";
                        System.IO.File.Copy(ConfigData.SitemapFileFolder + file, filesale);
                        System.IO.File.Copy(ConfigData.SitemapFileFolder + "SitemapUpdate.xml", ConfigData.Folder + "SitemapUpdate.xml");
                        var sSiteMapFilePath = Server.MapPath(@"~/SitemapUpdate.xml");
                        msg = new JsonMessage(false, "SitemapUpdate đã được cập nhật.");
                        var xmlDoc = new XmlDocument();
                        xmlDoc.Load(sSiteMapFilePath);
                        var listener = new TextWriterTraceListener(filesale);
                        var listsite = Sitemap.Sitemapsimso();
                        foreach (var item in listsite)
                        {
                            listener.WriteLine(url + item);
                            var nodeSite = xmlDoc.CreateElement("url");
                            var loc = xmlDoc.CreateElement("loc");
                            loc.InnerText = url + item;
                            //var lastmod = xmlDoc.CreateElement("lastmod");
                            //lastmod.InnerText = date;
                            var priority = xmlDoc.CreateElement("priority");
                            priority.InnerText = string.IsNullOrEmpty(item) ? "1.0" : "0.5";
                            var changefreq = xmlDoc.CreateElement("changefreq");
                            changefreq.InnerText = "daily";
                            nodeSite.AppendChild(loc);
                            //nodeSite.AppendChild(lastmod);
                            nodeSite.AppendChild(priority);
                            nodeSite.AppendChild(changefreq);
                            XmlNode node = nodeSite;
                            XmlNode childNode = xmlDoc.DocumentElement;
                            if (childNode != null) childNode.InsertAfter(node, childNode.LastChild);
                        }
                        //listener.Flush();
                        var list = _da.GetListHomeWork();
                        foreach (var item in list)
                        {
                            var nodeSite = xmlDoc.CreateElement("url");
                            var loc = xmlDoc.CreateElement("loc");
                            loc.InnerText = url + item.Slug;
                            listener.WriteLine(url + item.Slug);
                            //var lastmod = xmlDoc.CreateElement("lastmod");
                            //lastmod.InnerText = date;
                            var priority = xmlDoc.CreateElement("priority");
                            priority.InnerText = "0.5";
                            var changefreq = xmlDoc.CreateElement("changefreq");
                            changefreq.InnerText = "daily";
                            nodeSite.AppendChild(loc);
                            //nodeSite.AppendChild(lastmod);
                            nodeSite.AppendChild(priority);
                            nodeSite.AppendChild(changefreq);
                            XmlNode node = nodeSite;
                            XmlNode childNode = xmlDoc.DocumentElement;
                            if (childNode != null) childNode.InsertAfter(node, childNode.LastChild);
                        }
                        //listener.Flush();
                        var listcate = _da.GetTagSitemap();
                        foreach (var item in listcate)
                        {
                            var nodeSite = xmlDoc.CreateElement("url");
                            var loc = xmlDoc.CreateElement("loc");
                            loc.InnerText = url + item.Slug;
                            listener.WriteLine(url + item.Slug);
                            //var lastmod = xmlDoc.CreateElement("lastmod");
                            //lastmod.InnerText = date;
                            var priority = xmlDoc.CreateElement("priority");
                            priority.InnerText = "0.5";
                            var changefreq = xmlDoc.CreateElement("changefreq");
                            changefreq.InnerText = "daily";
                            nodeSite.AppendChild(loc);
                            //nodeSite.AppendChild(lastmod);
                            nodeSite.AppendChild(priority);
                            nodeSite.AppendChild(changefreq);
                            XmlNode node = nodeSite;
                            XmlNode childNode = xmlDoc.DocumentElement;
                            if (childNode != null) childNode.InsertAfter(node, childNode.LastChild);
                            //listener.Flush();
                            foreach (var itemnews in item.ListNewsItem)
                            {
                                var nodeSiten = xmlDoc.CreateElement("url");
                                var locn = xmlDoc.CreateElement("loc");
                                locn.InnerText = url + itemnews + ".html";
                                listener.WriteLine(url + itemnews + ".html");
                                //var lastmodn = xmlDoc.CreateElement("lastmod");
                                //lastmodn.InnerText = date;
                                var priorityn = xmlDoc.CreateElement("priority");
                                priorityn.InnerText = "0.5";
                                var changefreqn = xmlDoc.CreateElement("changefreq");
                                changefreqn.InnerText = "daily";
                                nodeSiten.AppendChild(locn);
                                //nodeSiten.AppendChild(lastmodn);
                                nodeSiten.AppendChild(priorityn);
                                nodeSiten.AppendChild(changefreqn);
                                XmlNode noden = nodeSiten;
                                XmlNode childNoden = xmlDoc.DocumentElement;
                                if (childNoden != null) childNoden.InsertAfter(noden, childNoden.LastChild);
                            }
                        }
                        var listTag = _da.GetListTag();
                        foreach (var item in listTag)
                        {
                            var nodeSite = xmlDoc.CreateElement("url");
                            var loc = xmlDoc.CreateElement("loc");
                            loc.InnerText = url +"tag/"+ item.Url;
                            listener.WriteLine(url + item.Url);
                            //var lastmod = xmlDoc.CreateElement("lastmod");
                            //lastmod.InnerText = date;
                            var priority = xmlDoc.CreateElement("priority");
                            priority.InnerText = "0.5";
                            var changefreq = xmlDoc.CreateElement("changefreq");
                            changefreq.InnerText = "daily";
                            nodeSite.AppendChild(loc);
                            //nodeSite.AppendChild(lastmod);
                            nodeSite.AppendChild(priority);
                            nodeSite.AppendChild(changefreq);
                            XmlNode node = nodeSite;
                            XmlNode childNode = xmlDoc.DocumentElement;
                            if (childNode != null) childNode.InsertAfter(node, childNode.LastChild);
                        }
                        listener.Flush();
                        listener.Close();
                        xmlDoc.Save(sSiteMapFilePath);
                    }
                    catch (Exception ex)
                    {
                        Log2File.LogExceptionToFile(ex);
                        msg.Message = ex.Message;
                        //msg.Message = "Dữ liệu chưa được backup.";
                        msg.Erros = true;
                    }
                    break;
                default:
                    msg.Message = "Không có hành động nào được thực hiện.";
                    msg.Erros = true;
                    break;

            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}