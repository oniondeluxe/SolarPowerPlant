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
    public class UnitOfWorkTemplate<T> where T : IBusinessObjectRepository
    {
        readonly IRepositoryFactory<T> _input;
        readonly ILogger _logger;

        /// <summary>
        /// Defines what type of Dto the client will use
        /// </summary>
        /// <typeparam name="D"></typeparam>
        /// <param name="factory"></param>
        /// <returns></returns>
        public UnitOfWork<T, D> For<D>(DtoFactory<D> factory) where D : ITransactionalDto
        {
            return new UnitOfWork<T, D>(factory, this, _input, _logger);
        }


        internal UnitOfWorkTemplate(IRepositoryFactory<T> input, ILogger logger)
        {
            _input = input;
            _logger = logger;
        }
    }   


    public static class UnitOfWorkExtensions
    {
        public static UnitOfWorkTemplate<T> CreateUoW<T>(this IRepositoryFactory<T> input, ILogger logger)
         where T : IBusinessObjectRepository
        {
            return new UnitOfWorkTemplate<T>(input, logger);
        }
    }
}
