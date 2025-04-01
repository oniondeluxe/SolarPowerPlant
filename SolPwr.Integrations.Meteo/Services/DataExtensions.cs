using OnionDlx.SolPwr.Data;
using OnionDlx.SolPwr.Dto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.Services
{
    internal static class DataExtensions
    {
        public static IEnumerable<MeteoData> Convert(this IEnumerable<ProviderMeteoData> input, TimeResolution resol, GeoCoordinate location)
        {
            var increment = new TimeSpan(0, 15, 0);
            if (resol == TimeResolution.SixtyMinutes)
            {
                increment = new TimeSpan(1, 0, 0);
            }

            foreach (var item in input)
            {
                if (item.Elevation == 0.0)
                {
                    if (!item.HourlyValues.Time.Any())
                    {
                        yield break;
                    }
                    DateTime start;
                    if (!DateTime.TryParse(item.HourlyValues.Time.First(), out start))
                    {
                        throw new ArgumentException("Invalid time format");
                    }

                    if ((item.HourlyValues.Time.Count != item.HourlyValues.WeatherCode.Count)
                     && (item.HourlyValues.WeatherCode.Count != item.HourlyValues.Visibility.Count))
                    {
                        throw new ArgumentException("Array lengths don't match");
                    }

                    var lapsedDataPoints = 0;

                    // The API authors seem to like FORTRAN ?!
                    var last = (item.HourlyValues.Time.First(),
                                item.HourlyValues.Visibility.First(),
                                item.HourlyValues.WeatherCode.First());
                    for (var i = 0; i < item.HourlyValues.Time.Count; i++)
                    {
                        var cur = (item.HourlyValues.Time[i],
                                item.HourlyValues.Visibility[i],
                                item.HourlyValues.WeatherCode[i]);

                        // TODO: Some interpolation would be needed (using last)
                        DateTime current;
                        if (DateTime.TryParse(cur.Item1, out current))
                        {
                            var next = start + increment * lapsedDataPoints;
                            if (next <= current)
                            {
                                lapsedDataPoints++;

                                // Add record
                                var record = new MeteoData
                                {
                                    Location = location,
                                    UtcTime = current,
                                    Visibility = cur.Item2,
                                    WeatherCode = cur.Item3
                                };

                                yield return record;
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Invalid time format");
                        }
                    }
                }
            }
        }
    }
}
