using Ceb.MerlinTool.WebAPI.Interfaces;
using Ceb.MerlinTool.WebAPI.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Ceb.MerlinTool.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var container = new UnityContainer();
            container.RegisterType<IAdminConfigurations, AdminConfigurationsService>();
            container.RegisterType<IOverAllScore, OverallScoreService>();
            container.RegisterType<IUser, UserService>();
            container.RegisterType<ICategoryAnalysis, CategoryAnalysisService>();
            container.RegisterType<IMerlinLookup, LookupService>();
            container.RegisterType<ISupplierAnalysis, SupplierAnalysisService>();
            container.RegisterType<ITrendAnalysis, TrendAnalysisService>();
            container.RegisterType<IPerceptionGaps, PerceptionGapsService>();
            container.RegisterType<ILogService, LogService>();
            config.DependencyResolver = new UnityResolver(container);
        }
    }
}
