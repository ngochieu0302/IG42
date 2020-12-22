using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using FDI.Simple;
using FDI.Utils;

namespace FDI
{
    public class Config
    {
        static readonly Assembly Asm = Assembly.GetExecutingAssembly();

        /// <summary>
        /// Lấy RenderSection từ Layout
        /// </summary>
        /// <param name="layout"></param>
        /// <returns></returns>
        public static List<string> GetSectsion(string layout)
        {
            var fileContents = File.ReadAllText(Path.Combine(HttpContext.Current.Server.MapPath("~/Views/Shared/" + layout + ".cshtml")));
            var listControlId = Regex.Matches(fileContents, "RenderSection(.*?),", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return (from Match item in listControlId select item.Value.Replace(@"RenderSection(""", "").Replace(@""",", "")).ToList();
        }

        /// <summary>
        /// Lấy các Action hệ thống
        /// </summary>
        /// <param name="nameSpace"></param>
        /// <returns></returns>
        public static List<ModeItem> GetAction(string nameSpace)
        {
            var lstAction = Asm.GetTypes().Where(type => typeof(Controller).IsAssignableFrom(type) && type.Namespace.Contains(nameSpace)).SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
             .Where(m => m.ReturnType.Name == "PartialViewResult" && !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
             .Select(x => x.DeclaringType != null && x.DeclaringType.Namespace != null ? new ModeItem
             {
                 Controller = x.DeclaringType.Name.Replace("Controller", ""),
                 Action = x.Name,
                 ReturnType = x.ReturnType.Name
             } : null).ToList();
            return lstAction;
        }

        /// <summary>
        /// Lấy Layout từ File
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string> GetLayout(string path)
        {
            var foder = HttpContext.Current.Server.MapPath(path);
            var filePaths = Directory.GetFiles(foder, "_*.cshtml", SearchOption.AllDirectories);
            return filePaths.Select(filePath => filePath.Split('\\').LastOrDefault()).Select(str => str.Split('.')[0]).ToList();
        }

        public static void BeginRequest(string lang)
        {
            Utility.Setcookie(lang, "LanguageId", 1);
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(lang);
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture;
        }
    }
}