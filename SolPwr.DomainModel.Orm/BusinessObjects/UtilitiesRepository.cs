using Microsoft.Extensions.Logging;
using OnionDlx.SolPwr.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.BusinessObjects
{
    internal class UtilitiesRepositoryFactory : IUtilitiesRepositoryFactory
    {
        #region Bolierplate

        readonly string _connectionString;
        readonly ILogger<IUtilitiesRepositoryFactory> _logger;

        public UtilitiesRepositoryFactory(string connectionString, ILogger<IUtilitiesRepositoryFactory> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        #endregion    

        public string ProviderInfo => "EntityFrameworkCore";

        public IUtilitiesRepository NewCommand()
        {
            var context = new UtilitiesContext(_connectionString);
            return new MutableUtilitiesRepository(this, context, _logger);
        }


        public IUtilitiesRepository NewQuery()
        {
            var context = new UtilitiesContext(_connectionString);
            return new ImmutableUtilitiesRepository(this, context, _logger);
        }
    }


    internal abstract class UtilitiesRepository : BusinessObjectRepository, IUtilitiesRepository
    {
        #region Bolierplate

        readonly UtilitiesRepositoryFactory _creator;
        readonly UtilitiesContext _dbContext;
        readonly ILogger<IUtilitiesRepositoryFactory> _logger;

        protected UtilitiesRepository(UtilitiesRepositoryFactory creator, UtilitiesContext dbContext, ILogger<IUtilitiesRepositoryFactory> logger)
        {
            _creator = creator;
            _dbContext = dbContext;
            _logger = logger;
        }


        protected override void WriteLogMessage(string message)
        {
            _logger.LogInformation(message);
        }


        protected override void ExecuteSaveChanges()
        {
            _dbContext.SaveChanges();
        }


        protected override async Task ExecuteSaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }


        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }

            disposed = true;
        }

        #endregion

        EntityCollection<PowerPlant> _powerPlants;
        public IBusinessObjectCollection<PowerPlant> PowerPlants
        {
            get
            {
                if (_powerPlants == null)
                {
                    _powerPlants = new EntityCollection<PowerPlant>(_dbContext.PowerPlants, this);
                }

                return _powerPlants;
            }
        }


        EntityCollection<PowerGenerationRecord> _generationRecords;
        public IBusinessObjectCollection<PowerGenerationRecord> GenerationRecords
        {
            get
            {
                if (_generationRecords == null)
                {
                    _generationRecords = new EntityCollection<PowerGenerationRecord>(_dbContext.GenerationHistory, this);
                }

                return _generationRecords;
            }
        }
    }


    internal class ImmutableUtilitiesRepository : UtilitiesRepository
    {
        protected override void ExecuteSaveChanges()
        {
            throw new NotSupportedException("Collection is read-only");
        }

        public override bool IsReadonly => true;


        protected override Task ExecuteSaveChangesAsync()
        {
            throw new NotSupportedException("Collection is read-only");
        }


        public ImmutableUtilitiesRepository(UtilitiesRepositoryFactory creator, UtilitiesContext dbContext, ILogger<IUtilitiesRepositoryFactory> logger)
            : base(creator, dbContext, logger)
        {
        }
    }


    internal class MutableUtilitiesRepository : UtilitiesRepository
    {
        public override bool IsReadonly => false;

        public MutableUtilitiesRepository(UtilitiesRepositoryFactory creator, UtilitiesContext dbContext, ILogger<IUtilitiesRepositoryFactory> logger)
            : base(creator, dbContext, logger)
        {
        }
    }

}
