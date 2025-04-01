using OnionDlx.SolPwr.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.BusinessObjects
{
    internal static class ConverterExtensions
    {
        public static PowerPlantImmutable ToDto(this PowerPlant dbRecord)
        {
            return new PowerPlantImmutable
            {
                Id = dbRecord.Id,
                UtcInstallDate = dbRecord.UtcInstallDate,
                PlantName = dbRecord.PlantName,
                PowerCapacity = dbRecord.PowerCapacity,
                Location = dbRecord.Location
            };
        }
    }
}
