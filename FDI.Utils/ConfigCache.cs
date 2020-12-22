using System;
using System.Configuration;

namespace FDI.Utils
{
    public class ConfigCache
    {
        /// <summary>
        /// 1: enable cache
        /// 0: disable cache
        /// </summary>
        public static int EnableCache = Convert.ToInt32(ConfigurationManager.AppSettings["EnableCache"]);
        public static string ServerCache = Convert.ToString(ConfigurationManager.AppSettings["ServerCache"]);
        public static string ServerRedisCache = Convert.ToString(ConfigurationManager.AppSettings["ServerRedisCache"]);
        public static int PortRedisCache = Convert.ToInt32(ConfigurationManager.AppSettings["PortRedisCache"]);// time expire
        public static int TimeExpire = Convert.ToInt32(ConfigurationManager.AppSettings["TimeExpireCache"]);// tinh = phut
        public static int TimeExpire30 = Convert.ToInt32(ConfigurationManager.AppSettings["TimeExpireCache30"]);
        public static int TimeExpire60 = Convert.ToInt32(ConfigurationManager.AppSettings["TimeExpireCache60"]);
        public static int TimeExpire120 = Convert.ToInt32(ConfigurationManager.AppSettings["TimeExpireCache120"]);
        public static int TimeExpire360 = Convert.ToInt32(ConfigurationManager.AppSettings["TimeExpireCache360"]);
        public static string SrcSaveOrder = ConfigurationManager.AppSettings["SrcLocalSaveOrder"];
        public static string SrcMoveOrder = ConfigurationManager.AppSettings["SrcLocalMoveOrder"];
        public static string UrlContaintGvgs = ConfigurationManager.AppSettings["UrlContaintgvgs"];
        public static string SrcLocalSaveOrderSub = ConfigurationManager.AppSettings["SrcLocalSaveOrderSub"];
        public static string SrcMoveOrderSub = ConfigurationManager.AppSettings["SrcLocalMoveOrderSub"];
        public static int TimeFlushQueue = Convert.ToInt32(ConfigurationManager.AppSettings["TimeFlushQueue"]);
        public static int IsQueue = Convert.ToInt32(ConfigurationManager.AppSettings["IsQueue"]);
        public static string FolderIp = ConfigurationManager.AppSettings["FolderIP"];
        public static string FolderSource = ConfigurationManager.AppSettings["FolderSource"];
        public static int TimeExpireClient = Convert.ToInt32(ConfigurationManager.AppSettings["TimeExpireClient"]);
        public static int TimeExpireIp = Convert.ToInt32(ConfigurationManager.AppSettings["TimeExpireIp"]);
        public static string CurrentPriceFolder = ConfigurationManager.AppSettings["CurrentPriceFolder"];
        public static string CurrentSource = ConfigurationManager.AppSettings["CurrentSource"];
        public static string MailSalesGvgs = ConfigurationManager.AppSettings["MailSalesGVGS"];
        public static int TopGamer930 = Convert.ToInt32(ConfigurationManager.AppSettings["TopGamer930"]);
    }
}
