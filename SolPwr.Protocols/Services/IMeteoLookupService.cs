using OnionDlx.SolPwr.Data;
using OnionDlx.SolPwr.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.Services
{
    public interface IMeteoLookupService
    {
        Task<IEnumerable<MeteoData>> GetMeteoDataAsync(GeoCoordinate geoCoordinate, TimeResolution resol, TimeSpanCode code, int timeSpan);

        Task StartFeedAsync(Guid plantId, GeoCoordinate location);
    }


    public interface IMeteoLookupServiceCallback : IServiceCallback<IMeteoLookupService>
    {
    }
}
