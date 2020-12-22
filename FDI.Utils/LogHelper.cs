using log4net;
using System;
using System.Diagnostics;
using System.IO;

namespace FDI.Utils
{
    public sealed class LogHelper
    {
        private static volatile LogHelper _instance;
        private static readonly object SyncRoot = new Object();
        //declare log variable for log4net

        private LogHelper() { }

        public static LogHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new LogHelper();
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// Log system exception
        /// </summary>
        /// <param name="type">Type of target class that directly invokes this function</param>
        /// <param name="ex">Exception object</param>
        public void LogError(Type type, Exception ex)
        {
            var msg = "";
            var logger = LogManager.GetLogger(type);
            var st = new StackTrace(ex, true);
            st.GetFrame(0);
            if (ex.TargetSite.DeclaringType != null)
                msg = "Class: " + ex.TargetSite.DeclaringType.Name + " - ";
            msg += "Method: " + ex.TargetSite.Name + "\r\n" +
                  "Error: " + ex;
            logger.Error(msg);
        }

        public void WriteLog(string strlog)
        {
            try
            {
                const string foldername = @"D:\\Log\";
                if ((!Directory.Exists(foldername)))
                {
                    Directory.CreateDirectory(foldername);
                }
                var filename = foldername + DateTime.Now.ToShortDateString().Replace("/", "") + ".txt";
                var fi = new FileInfo(filename);
                var w = File.AppendText(filename);
                if (fi.Length < 1048576)
                {
                    w.WriteLine(DateTime.Now + ":");
                    w.WriteLine(strlog);
                    w.WriteLine();
                    w.Flush();
                }
                w.Close();
            }
            catch (Exception)
            {
            }
        }
    }
}
