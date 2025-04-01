using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.Data
{
    public class GeoCoordinate
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public GeoCoordinate()
        {            
        }


        public GeoCoordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
