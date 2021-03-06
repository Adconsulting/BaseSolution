﻿using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using BaselineSolution.Framework.Logging;
using BaselineSolution.IOC;

namespace BaselineSolution.WebApp
{
    public class AutoFacConfig
    {
        public static void ConfigureContainer(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterModule<BackendModule>();
            builder.RegisterModule(new LogModule(NlogConfig.GetConfig("Baseline.WebApp"), "Baseline.WebApp"));
            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

    }

}