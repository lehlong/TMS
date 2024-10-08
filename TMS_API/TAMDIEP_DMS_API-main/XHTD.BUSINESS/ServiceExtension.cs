using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using XHTD.CORE.Entities;
using AutoMapper.Extensions.ExpressionMapping;
using XHTD.BUSINESS.Services;
using XHTD.BUSINESS.Common;

namespace XHTD.BUSINESS
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddDIXHTDServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(cfg => { cfg.AddExpressionMapping(); }, typeof(MappingProfile).Assembly);
            // Add Entity Framework

            services.AddDbContext<XHTDDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("XHTDConnection"),
                b=> b.UseCompatibilityLevel(120)));

            //Add all service
            var allProviderTypes = Assembly.GetAssembly(typeof(IStoreOrderOperatingService))
             .GetTypes().Where(t => t.Namespace != null).ToList();
            foreach (var intfc in allProviderTypes.Where(t => t.IsInterface))
            {
                if(intfc.Name == "IGatewayService")
                {

                }
                var impl = allProviderTypes.FirstOrDefault(c => c.IsClass && !c.IsAbstract && intfc.Name[1..] == c.Name);
                if (impl != null) services.AddScoped(intfc, impl);
            }

            return services;
        }
    }
}
