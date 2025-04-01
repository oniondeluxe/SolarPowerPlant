using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.BusinessObjects
{
    public interface IUtilitiesRepository : IBusinessObjectRepository
    {
        IBusinessObjectCollection<PowerPlant> PowerPlants { get; }

        IBusinessObjectCollection<PowerGenerationRecord> GenerationRecords { get; }
    }


    public interface IUtilitiesRepositoryFactory : IRepositoryFactory<IUtilitiesRepository>
    {
    }
}
