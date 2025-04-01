using OnionDlx.SolPwr.Data;
using System.ComponentModel.DataAnnotations;

namespace OnionDlx.SolPwr.BusinessObjects
{
    public class PowerPlant : BusinessObject
    {
        [StringLength(32)]
        public string PlantName { get; set; }

        [Required]
        public DateTime UtcInstallDate { get; set; }

        [Required]
        public GeoCoordinate Location { get; set; }

        [Required]
        public double PowerCapacity { get; set; }

        public IList<PowerGenerationRecord> GenerationRecords { get; set; }

        public PowerPlant()
        {
        }
    }


}
