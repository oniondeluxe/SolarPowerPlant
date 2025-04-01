using OnionDlx.SolPwr.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.Dto
{
    [Serializable]
    public class UnitDescriptor
    {
        [JsonPropertyName("time")]
        public string Time { get; set; }

        [JsonPropertyName("weather_code")]
        public string WeatherCode { get; set; }

        [JsonPropertyName("visibility")]
        public string Visibility { get; set; }

        public UnitDescriptor()
        {
        }
    }


    [Serializable]
    public class ValueDescriptor
    {
        [JsonPropertyName("time")]
        public List<string> Time { get; set; }

        [JsonPropertyName("weather_code")]
        public List<int> WeatherCode { get; set; }

        [JsonPropertyName("visibility")]
        public List<double> Visibility { get; set; }

        public ValueDescriptor()
        {
        }
    }


    [Serializable]
    public class ProviderMeteoData : IDataTransferObject
    {
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("generationtime_ms")]
        public double GenerationTime { get; set; }

        [JsonPropertyName("utc_offset_seconds")]
        public int UtcOffset { get; set; }

        [JsonPropertyName("timezone")]
        public string TimeZone { get; set; }

        [JsonPropertyName("timezone_abbreviation")]
        public string TimeZoneAbbrev { get; set; }

        [JsonPropertyName("elevation")]
        public double Elevation { get; set; }

        [JsonPropertyName("hourly_units")]
        public UnitDescriptor HourlyUnits { get; set; }

        [JsonPropertyName("hourly")]
        public ValueDescriptor HourlyValues { get; set; }


        public ProviderMeteoData()
        {
        }
    }
}

