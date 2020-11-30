using Application;
using Application.Abstractions.Identity;
using Application.Abstractions.Mapping;
using Application.Behaviors;
using Application.Products.Commands.Create;
using Application.Products.Mapping;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Domain;
using Domain.Abstractions;
using Domain.Common.Dependencies;
using Domain.Products;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Context;
using Infrastructure.Services.Users;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Webframework.IoC
{
    public static class IoCConfigurations
    {
        private static void AddServices(this ContainerBuilder container)
        {
            container.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            container.RegisterGeneric(typeof(RequestValidationBehavior<,>)).As(typeof(IPipelineBehavior<,>)).InstancePerLifetimeScope();
            #region auto implement
            var applicationAssembly = typeof(ApplicationLayer).Assembly;
            var domainAssembly = typeof(DomainLayer).Assembly;
            var infraAssembly = typeof(InfrastructureLayer).Assembly;

            container.RegisterAssemblyTypes(applicationAssembly).AsClosedTypesOf(typeof(ICustomMapper<,>)).InstancePerLifetimeScope();
            container.RegisterAssemblyTypes(applicationAssembly, domainAssembly, infraAssembly).AssignableTo<ISingleton>().AsImplementedInterfaces().SingleInstance();
            container.RegisterAssemblyTypes(applicationAssembly, domainAssembly, infraAssembly).AssignableTo<IScoped>().AsImplementedInterfaces().InstancePerLifetimeScope();
            container.RegisterAssemblyTypes(domainAssembly, applicationAssembly, infraAssembly).AssignableTo<ITransient>().AsImplementedInterfaces().InstancePerDependency();
            #endregion
        }

        public static IServiceProvider AutofacServiceProvider(this IServiceCollection services)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);
            containerBuilder.AddServices();

            var container = containerBuilder.Build();
            return new AutofacServiceProvider(container);
        }
    }
}
