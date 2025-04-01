using OnionDlx.SolPwr.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.Dto
{
    public class PowerPlant : IDataTransferObject
    {
        [StringLength(32)]
        public string PlantName { get; set; }

        [Required]
        public DateTime UtcInstallDate { get; set; }

        [Required]
        public GeoCoordinate Location { get; set; }

        [Required]
        public double PowerCapacity { get; set; }
    }


    public class PowerPlantImmutable : PowerPlant
    {
        [Required]
        public Guid? Id { get; set; }        
    }
}
