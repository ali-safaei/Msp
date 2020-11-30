using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Webframework
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddCustomizedServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            return services;
        }
    }
}
