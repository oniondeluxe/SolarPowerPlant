using Microsoft.Extensions.Logging;
using OnionDlx.SolPwr.BusinessObjects;
using OnionDlx.SolPwr.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.BusinessLogic
{
    public abstract class UnitOfWork
    {
        protected UnitOfWork()
        {
        }
    }


    public class UnitOfWork<T, D> : UnitOfWork
        where D : ITransactionalDto
        where T : IBusinessObjectRepository
    {
        readonly DtoFactory<D> _dtoFac;
        readonly UnitOfWorkTemplate<T> _creator;
        readonly IRepositoryFactory<T> _repoFac;
        readonly ILogger _logger;

        private IRepositoryFactory<T> Database => _repoFac;

        private DtoFactory<D> DtoFactory => _dtoFac;


        internal UnitOfWork(DtoFactory<D> factory, UnitOfWorkTemplate<T> creator, IRepositoryFactory<T> input, ILogger logger)
        {
            _dtoFac = factory;
            _creator = creator;
            _logger = logger;
            _repoFac = input;
        }


        public async Task<D> ExecuteCommandAsync(Func<T, CommandResultFactory<D>, CommandResult<D>> onExecute, Action<D> onPopulateResponse = null)
        {
            D response = default;
            try
            {
                using (var repo = Database.NewCommand())
                {
                    // The consumer class knows exactly what instance
                    response = DtoFactory.CreateInstance(null);
                    var commandResultFactory = new CommandResultFactory<D>(response);

                    // Here, the consumer will do its job (adding/deleting power plants, selecting etc)
                    var result = onExecute(repo, commandResultFactory);
                    if (result.PendingChanges)
                    {
                        // As we have pending changes, we will also get a transaction ID
                        var trx = await repo.SaveChangesAsync();
                        response.TransactionId = trx;

                        if (trx.HasValue)
                        {
                            // TODO: more elaborate logging would make sense
                            _logger.LogInformation(trx.Value.ToString());
                        }
                    }
                    if (onPopulateResponse != null)
                    {
                        onPopulateResponse(response);
                    }
                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = DtoFactory.CreateInstance(null);
                response.Message = ex.Message;
                response.Success = false;
            }

            return response;
        }


        public async Task<IEnumerable<P>> ExecuteQueryAsync<P>(Func<T, Task<IEnumerable<P>>> onExecute) where P : IDataTransferObject
        {
            try
            {
                using (var repo = Database.NewQuery())
                {
                    // No save, as this will be an immutable operation
                    return await onExecute(repo);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Array.Empty<P>();
            }
        }
    }
}
