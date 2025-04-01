using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.BusinessObjects
{
    public class PowerGenerationRecord : BusinessObject
    {
        public PowerPlant PowerPlant { get; set; }

        public DateTime UtcTimestamp { get; set; }

        public double PowerGenerated { get; set; }

        public PowerGenerationRecord()
        {            
        }
    }
}
