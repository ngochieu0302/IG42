using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FDI.CORE;
using FDI.Utils;

namespace FDI.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //ModelBinders.Binders.Add(typeof(decimal), new DecimalBinder());
            //ModelBinders.Binders.Add(typeof(decimal?), new NullableDecimalBinder());
        }
    }
    //public class DateTimeBinder : IModelBinder
    //{
    //    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    //    {
    //        var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
    //        bindingContext.ModelState.SetModelValue(bindingContext.ModelName, value);
    //        if (bindingContext.ModelMetadata.DataTypeName == DataType.DateTime.ToString())
    //        {
    //            var a = DateTime.Parse(value.AttemptedValue);
    //            return a.TotalSeconds();
    //        }


    //        var model = bindingContext.Model;
    //        PropertyInfo property = model.GetType().GetProperty(bindingContext.ModelName);


    //        return value.ConvertTo(typeof(decimal), CultureInfo.CurrentCulture);
    //    }

    //}
}