using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace DroneWebApp.App_Start
{
    class WebApiConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
            //// Attribute routing.
            //configuration.MapHttpAttributeRoutes();

            // Convention-based routing.
            configuration.Routes.MapHttpRoute("API Default", "WebAPI/api/{controller}/{id}/{imageid}",
                new { id = RouteParameter.Optional, imageid = RouteParameter.Optional });
        }
    }
}