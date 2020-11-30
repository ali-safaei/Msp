using Application;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Webframework.CQRS
{
   public static class CqrsConfiguration
    {
        public static void AddMediatRConfiguration(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddMediatR(typeof(ApplicationLayer).GetTypeInfo().Assembly);
        }
    }
}
