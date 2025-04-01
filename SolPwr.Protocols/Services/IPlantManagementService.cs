using OnionDlx.SolPwr.Data;
using OnionDlx.SolPwr.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.Services
{
    public interface IPlantManagementService
    {
        Task<IEnumerable<PowerPlantImmutable>> GetAllPlantsAsync();        

        Task<PlantMgmtResponse> CreatePlantAsync(PowerPlant dtoRegister);

        Task<PlantMgmtResponse> UpdatePlantAsync(Guid identity, PowerPlant dtoRegister);

        Task<PlantMgmtResponse> DeletePlantAsync(Guid identity);

        Task<IEnumerable<PlantPowerData>> GetPowerDataAsync(Guid identity, PowerDataTypes type, TimeResolution resol, TimeSpanCode code, int timeSpan);

        Task<PlantMgmtResponse> SeedPlantsAsync(int daysBehind);
    }
}
