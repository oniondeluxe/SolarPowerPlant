using OnionDlx.SolPwr.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.Dto
{
    public class MeteoData : IDataTransferObject
    {
        public GeoCoordinate Location { get; set; }

        public DateTime UtcTime { get; set; }

        public double Visibility { get; set; }

        public int WeatherCode { get; set; }

        public MeteoData()
        {
        }
    }
}
