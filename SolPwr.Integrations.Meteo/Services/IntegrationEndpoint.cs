using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OnionDlx.SolPwr.Data;
using OnionDlx.SolPwr.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.Services
{
    internal class IntegrationEndpoint : IIntegrationEndpoint, IMeteoLookupService
    {
        ILogger<IIntegrationProxy> Logger { get; set; }
        IConfigurationSection ConfigurationSection { get; set; }

        #region IDisposable

        private bool disposed = false;

        ~IntegrationEndpoint()
        {
            Dispose(false);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                // TODO
                disposed = true;
            }
        }

        #endregion

        public IntegrationEndpoint()
        {
            // Instantiated by Reflection
        }

        public string Title => "api.open-meteo.com";


        public IMeteoLookupService GetLookupService()
        {
            return this;
        }


        public void Initialize(ILogger<IIntegrationProxy> logger, IConfigurationSection configurationSection)
        {
            Logger = logger;
            ConfigurationSection = configurationSection;
        }


        private async Task<IEnumerable<MeteoData>> FetchDataAsync(GeoCoordinate geoCoordinate, TimeResolution resol, TimeSpanCode code, int dayLapse)
        {
            var result = new List<MeteoData>();
            var url = $"https://api.open-meteo.com/v1/forecast?latitude={geoCoordinate.Latitude}&longitude={geoCoordinate.Longitude}&hourly=weather_code,visibility&forecast_days={dayLapse}";

            // Go to the meteo service
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                var data = await response.Content.ReadAsStringAsync();
                var instances = JsonSerializer.Deserialize<List<ProviderMeteoData>>(data);

                // Re-packaging into the public format
                result.AddRange(instances.Convert(resol, geoCoordinate));
            }

            return result.AsEnumerable();
        }


        public Task StartFeedAsync(Guid plantId, GeoCoordinate location)
        {
            // TODO: make sure we create production data on a regular basis
            return Task.CompletedTask;
        }


        public async Task<IEnumerable<MeteoData>> GetMeteoDataAsync(GeoCoordinate geoCoordinate, TimeResolution resol, TimeSpanCode code, int timeSpan)
        {
            // The API will always deliver for minimum one day
            var numDays = 1;
            if (code == TimeSpanCode.Minutes)
            {
                // How many days will these minutes be?
                var numPerDay = 24 * 60;
                numDays = timeSpan / numPerDay;
                if (numDays < 1)
                {
                    numDays = 1;
                }
            }
            else if (code == TimeSpanCode.Hours)
            {
                // How many days will these hours be?
                var numPerDay = 24;
                numDays = timeSpan / numPerDay;
                if (numDays < 1)
                {
                    numDays = 1;
                }
            }
            else if (code == TimeSpanCode.Days)
            {
                if (timeSpan > 1)
                {
                    numDays = timeSpan;
                }
            }

            return await FetchDataAsync(geoCoordinate, resol, code, numDays);
        }
    }
}
