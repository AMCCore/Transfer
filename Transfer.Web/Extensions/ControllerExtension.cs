using Microsoft.AspNetCore.Mvc;
using System;

namespace Transfer.Web.Extensions;

public static class ControllerExtension
{
    public static string ControllerName(this Type controllerType)
    {
        Type baseType = typeof(Controller);
        if (baseType.IsAssignableFrom(controllerType))
        {
            int lastControllerIndex = controllerType.Name.LastIndexOf("Controller");
            if (lastControllerIndex > 0)
            {
                return controllerType.Name.Substring(0, lastControllerIndex);
            }
        }

        return controllerType.Name;
    }
}
