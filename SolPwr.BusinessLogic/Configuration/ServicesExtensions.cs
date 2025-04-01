using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OnionDlx.SolPwr.BusinessLogic;
using OnionDlx.SolPwr.BusinessObjects;
using OnionDlx.SolPwr.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.Configuration
{
    public static class ServicesDomainExtensions
    {
        /// <summary>
        /// Will add the needed boilerplate to IoC, without the need to introduce a dependency to EF in the main app
        /// </summary>
        /// <param name="connString"></param>
        /// <returns></returns>
        public static IServiceCollection AddBusinessLogic(this IServiceCollection coll)
        {
            // This is the connection point between the external communication layer and the domain layer
            var glue = new MeteoLookupServiceCallback();
            coll.AddSingleton<IMeteoLookupServiceCallback>(provider => glue);
            coll.AddScoped<IPlantManagementService>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<IPlantManagementService>>();
                var factory = provider.GetRequiredService<IUtilitiesRepositoryFactory>();
                return new PlantManagementService(factory, logger, glue);
            });

            return coll;
        }
    }
}
