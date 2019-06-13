using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using WebActivatorEx;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using System.Globalization;
using BKIC.SellingPoint.WebAPI.Framework;
using BLR = BKIC.SellingPoint.DL.BL.Repositories;
using BLI = BKIC.SellingPoint.DL.BL;

namespace BKIC.SellingPoint.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;

            // get decimal places for decimal input in api
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Culture = new CultureInfo(string.Empty)
            {
                NumberFormat = new NumberFormatInfo
                {
                    CurrencyDecimalDigits = 3
                }
            };

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new DecimalConverter());



            // Create the container as usual.
            var container = new SimpleInjector.Container();
            container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();

            InitializeRepositories(container);

            // This is an extension method from the integration package.
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);

        }

        public Container InitializeRepositories(Container container)
        {
            container.Register<BLR.IUser, BLI.User>(Lifestyle.Scoped);
            container.Register<BLR.IAdmin, BLI.Implementation.Admin>(Lifestyle.Scoped);
            container.Register<BLR.IDropDowns, BLI.Implementation.DropDowns>(Lifestyle.Scoped);
            container.Register<BLR.ITravelInsurance, BLI.Implematon.TravelInsurance>(Lifestyle.Scoped);
            container.Register<BLR.IMotorInsurance, BLI.Implematon.MotorInsurance>(Lifestyle.Scoped);
            container.Register<BLR.IDomesticHelp, BLI.Implematon.DomesticHelp>(Lifestyle.Scoped);
            container.Register<BLR.IHomeInsurance, BLI.Implematon.HomeInsurance>(Lifestyle.Scoped);
            container.Register<BLR.IInsurancePortal, BLI.Implematon.InsurancePortal>(Lifestyle.Scoped);
            container.Register<BLR.ISchedule, BLI.Implematon.Schedule>(Lifestyle.Scoped);
            container.Register<BLR.IMotorEndorsement, BLI.Implematon.MotorEndorsement>(Lifestyle.Scoped);
            container.Register<BLR.IReport, BLI.Implematon.Report>(Lifestyle.Scoped);
            container.Register<BLR.IHomeEndorsement, BLI.Implematon.HomeEndorsement>(Lifestyle.Scoped);
            container.Register<BLR.ITravelEndorsement, BLI.Implematon.TravelEndorsement>(Lifestyle.Scoped);
            return container;
        }

    }
}
