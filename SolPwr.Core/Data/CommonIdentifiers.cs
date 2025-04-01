using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.Data
{
    [Flags]
    public enum PowerDataTypes : uint
    {
        None = 0,
        History = 1,
        Forecast = 2
    }


    public enum TimeResolution
    {
        None,
        FifteenMinutes,
        SixtyMinutes
    }


    public enum TimeSpanCode
    {
        None,
        Minutes,
        Hours,
        Days
    }

}
