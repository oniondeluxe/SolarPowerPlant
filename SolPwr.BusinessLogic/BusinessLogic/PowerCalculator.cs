using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.Services
{
    /// <summary>
    /// A very blunt calculation implementation of solar power based on weather code and visibility
    /// </summary>
    internal class PowerCalculator
    {
        public const double VISIBILITY_THRESHOLD = 50000.0;

        readonly double _nominalPowerCapacity;
        readonly double _latitude;

        public double GetCurrentPower(int weatherCode, double visibility)
        {
            // Fake science is applied here. Elon Musk will know for sure
            if (weatherCode > 3)
            {
                // Cloudy - need to switch on the nuclear power instead
                return 0.0;
            }

            var currentVisibility = VISIBILITY_THRESHOLD;
            if (visibility < currentVisibility)
            {
                currentVisibility = visibility;
            }

            // Full sun blast at the Equator, zero at the North Pole
            return _nominalPowerCapacity * (currentVisibility / VISIBILITY_THRESHOLD) * ((90.0 - _latitude) / 90.0);
        }


        public PowerCalculator(double nominalPowerCapacity, double latitude)
        {
            _nominalPowerCapacity = nominalPowerCapacity;
            _latitude = latitude;
        }
    }
}
