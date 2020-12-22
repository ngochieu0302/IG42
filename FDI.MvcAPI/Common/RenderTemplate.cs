using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FDI.MvcAPI.Common
{
    public sealed class RenderTemplate
    {
        private static RenderTemplate instance = null;
        private IList<string> keys = new List<string>();
        private RenderTemplate()
        {
        }

        public static RenderTemplate Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RenderTemplate();
                }

                return instance;
            }
        }

        public void Remove(string key)
        {
            keys.Remove(key);
        }

        public string GetString<T>(string template, T model, string key)
        {
            CheckExistKey(template, key, typeof(T));
            return RazorEngine.Razor.Run(key, model);
        }

        private void CheckExistKey(string template, string key, Type typeModel)
        {
            if (keys.Contains(key)) return;
            keys.Add(key);
            RazorEngine.Razor.Compile(template, typeModel, key);
        }
    }
}