using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace EMPower.QnA.WebApi.StandAlone
{
    public static class WebApiConfig
    {
        public static string ControllerAction = "ApiControllerAction";

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            

            config.Routes.MapHttpRoute(
              name: ControllerAction,
              routeTemplate: "api/{controller}/{action}"
           );

            //config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
        }
    }
}
