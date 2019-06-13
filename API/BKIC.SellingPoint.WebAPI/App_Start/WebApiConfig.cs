using BKIC.SellingPoint.WebAPI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace BKIC.SellingPoint.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            //wrapping all api responses
            config.MessageHandlers.Add(new ApiResponseWrapperHandler());
            
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
