using Autofac;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebAppNoSql.Repo.Repositories;

namespace WebAppNoSql.Web.App_Start
{
    [ExcludeFromCodeCoverage]
    public class ContainerConfig
    {
        public static void RegisterContainer()
        {
            var builder = new ContainerBuilder();
            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;
            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config);
            builder.RegisterType<CustomerRepository>().As<ICustomerRepository>().WithParameter(new TypedParameter(typeof(string), "http://localhost:8081/ravendbserver/databases/northwind"));
            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

    }
}