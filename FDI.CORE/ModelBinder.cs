using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FDI.CORE
{
    public class DecimalBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, value);
            //var dateResult = DateTime.Now;
            //if (DateTime.TryParse(value.AttemptedValue, CultureInfo.CreateSpecificCulture("vi-VN"), DateTimeStyles.None, out dateResult))
            //{
            //    return dateResult.TotalSeconds();
            //}

            if (bindingContext.ModelMetadata.DataTypeName == DataType.DateTime.ToString())
            {
                var dateResult = DateTime.Now;
                if (DateTime.TryParse(value.AttemptedValue, CultureInfo.CreateSpecificCulture("vi-VN"), DateTimeStyles.None, out dateResult))
                {
                    return dateResult.TotalSeconds();
                }
            }


            return value.ConvertTo(typeof(decimal), CultureInfo.CurrentCulture);
        }

    }
    public class NullableDecimalBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var holderType = bindingContext.ModelMetadata.ContainerType;
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, value);

            if (value == null) return null;

            //var dateResult = DateTime.Now;
            //if (DateTime.TryParse(value.AttemptedValue, CultureInfo.CreateSpecificCulture("vi-VN"), DateTimeStyles.None, out dateResult))
            //{
            //    return dateResult.TotalSeconds();
            //}

            if (bindingContext.ModelMetadata.DataTypeName == DataType.DateTime.ToString())
            {
                var dateResult = DateTime.Now;
                if (DateTime.TryParse(value.AttemptedValue, CultureInfo.CreateSpecificCulture("vi-VN"), DateTimeStyles.None, out dateResult))
                {
                    return dateResult.TotalSeconds();
                }
            }
            return value.ConvertTo(typeof(decimal), CultureInfo.CurrentCulture);
        }

    }
    public class CustomModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (value == null) return null;

            var dateResult = DateTime.Now;
            var culture = CultureInfo.CreateSpecificCulture("vi-VN");
            
            if (DateTime.TryParse(value.AttemptedValue, culture, DateTimeStyles.None, out dateResult))
            {
                return dateResult.TotalSeconds();
            }
            return value.ConvertTo(typeof(decimal), CultureInfo.CurrentCulture);

        }
    }
}
