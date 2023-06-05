using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RentACar.Service;
using RentACar.Repository;
using RentACar.Repository.Common;
using System.Reflection;
using RentACar.Service.Common;
using Autofac.Integration.WebApi;
using System.Web.Http;

namespace RentACar.WebApi.App_Start
{
    public static class ContainerConfig
    {
        public static void Configure()
        {
            // Create the Autofac container builder
            var builder = new ContainerBuilder();

            // Register your Web API controllers
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<CarRepository>().As<ICarRepository>();
            builder.RegisterType<CarService>().As<ICarService>();

            // Build the container
            IContainer container = builder.Build();

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

        }
    }
}