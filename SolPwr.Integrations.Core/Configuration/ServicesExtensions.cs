using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OnionDlx.SolPwr.ComponentModel;
using OnionDlx.SolPwr.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.Configuration
{
    public static class ServicesIntegrationsExtensions
    {
        public static IServiceCollection AddIntegrationExtensions(this IServiceCollection coll, IConfigurationSection meteoServiceSection)
        {
            // From here on, and downwards, we will instantiate without dependency injection
            // because the endpoints will be dynamically loaded
            coll.AddSingleton<IIntegrationProxy>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<IIntegrationProxy>>();
                var callback = provider.GetRequiredService<IMeteoLookupServiceCallback>();
                return new IntegrationProxy(meteoServiceSection, logger, callback);
            });

            return coll;
        }
    }
}

