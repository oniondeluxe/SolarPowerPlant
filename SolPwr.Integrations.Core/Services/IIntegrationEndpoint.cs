using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.Services
{
    public interface IIntegrationEndpoint : IDisposable
    {
        void Initialize(ILogger<IIntegrationProxy> logger, IConfigurationSection configurationSection);

        IMeteoLookupService GetLookupService();

        string Title { get; }
    }
}
