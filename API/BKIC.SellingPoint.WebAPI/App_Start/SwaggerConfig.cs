using System.Web.Http;
using BKIC.SellingPoint.WebAPI;
using WebActivatorEx;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace BKIC.SellingPoint.WebAPI
{
    public class SwaggerConfig
    {
        public static void Register()
        {           

            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
           .EnableSwagger(c =>
           {
               c.SingleApiVersion("v1", "BKIC SellingPoint API Services");
          
           })
          .EnableSwaggerUi(c =>
          {
          });

        }

        protected static string GetXmlCommentsPath()
        {
            return System.String.Format(@"{0}\bin\WebApiSwagger.XML", System.AppDomain.CurrentDomain.BaseDirectory);
        }

    }
}