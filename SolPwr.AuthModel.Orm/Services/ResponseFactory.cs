using Microsoft.Extensions.Logging;
using OnionDlx.SolPwr.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.Services
{
    internal class ResponseFactory<TCategoryName>
    {
        protected readonly ILogger<TCategoryName> _logger;

        public UserAuthResponse CreateSuccess(string message)
        {            
            _logger.LogInformation(message);
            return UserAuthResponse.CreateSuccess(message);
        }


        public UserAuthResponse CreateFaulted(string message)
        {
            // TODO: Log
            return UserAuthResponse.CreateFaulted(message);
        }


        public ResponseFactory(ILogger<TCategoryName> logger)
        {
            _logger = logger;
        }
    }


    internal class ResponseFactory<TCategoryName, TResponseRecord> : ResponseFactory<TCategoryName>
        where TResponseRecord : IDataTransferObject
    {
        readonly Func<string, TResponseRecord> _createSuccess;
        readonly Func<string, TResponseRecord> _createFaulted;

        public new TResponseRecord CreateSuccess(string message)
        {
            _logger.LogInformation(message);
            return _createSuccess(message);
        }


        public new TResponseRecord CreateFaulted(string message)
        {
            // TODO: Log
            return _createFaulted(message);
        }


        public ResponseFactory(ILogger<TCategoryName> logger, Func<string, TResponseRecord> success, Func<string, TResponseRecord> error) : base(logger)
        {
            _createSuccess = success;
            _createFaulted = error;
        }
    }
}
