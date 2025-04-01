using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OnionDlx.SolPwr.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.Services
{
    internal class IntegrationProxy : IIntegrationProxy
    {
        IIntegrationEndpoint _endpoint;
        readonly ILogger<IIntegrationProxy> _logger;
        readonly IConfigurationSection _configurationSection;
        readonly IMeteoLookupServiceCallback _callback;

        public void Initialize(CancellationToken cancellationToken)
        {
            if (_endpoint == null)
            {
                // Find the proper plugin/extension and load it
                var chosenProvider = _configurationSection["Provider"];

                var loader = new PluginLoader(_logger, _configurationSection);
                string message;
                if (!loader.TryLoadProvider(chosenProvider, _logger, out _endpoint, out message))
                {
                    _logger.LogError(message);
                }

                // When we explicitly need to call the endpoint from inside the CRUD layer, 
                // like when calculating forecast values
                _callback.OnRequestEndpoint(() =>
                {
                    return _endpoint.GetLookupService();
                });
            }
        }

        public string Title => _endpoint?.Title;


        public void Cleanup()
        {
            _callback?.Teardown();
            _endpoint?.Dispose();
        }


        public void ExecuteWorker()
        {
            // Place the call back to the PlantManagementService here
            _callback.InvokePush(_endpoint.GetLookupService());
        }


        public IntegrationProxy(IConfigurationSection configurationSection, ILogger<IIntegrationProxy> logger, IMeteoLookupServiceCallback factory)
        {
            _configurationSection = configurationSection;
            _logger = logger;
            _callback = factory;
        }
    }
}
