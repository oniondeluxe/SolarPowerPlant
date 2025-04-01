using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.Dto
{ 
    public class PlantPowerData : IDataTransferObject
    {
        [JsonPropertyName("plant")]
        public Guid PlantId { get; set; }


        [JsonPropertyName("time")]
        public DateTime UtcTime { get; set; }


        [JsonPropertyName("power")]
        public double CurrentPower { get; set; }


        public PlantPowerData()
        {
        }
    }
}
