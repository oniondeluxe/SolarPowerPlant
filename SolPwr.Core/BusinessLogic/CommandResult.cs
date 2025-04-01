using OnionDlx.SolPwr.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.BusinessLogic
{
    public class CommandResultFactory<D> where D : ITransactionalDto
    {
        readonly D _payload;

        public CommandResult<D> AsSuccessful(bool commit = false)
        {
            return new CommandResult<D>(_payload, commit);
        }


        public CommandResult<D> AsSuccessful(string transactionMessage, bool commit = false)
        {
            if (_payload != null)
            {
                _payload.Message = transactionMessage;
            }
            return new CommandResult<D>(_payload, commit, transactionMessage);
        }


        public CommandResult<D> AsFaulted(string transactionErrorMessage)
        {
            // TODO
            return null; // $"Error";
        }


        internal CommandResultFactory(D payload)
        {
            _payload = payload;
        }
    }


    public class CommandResult<T> where T : ITransactionalDto
    {
        readonly bool _commit;
        readonly string _message;

        public T Payload { get; init; }

        public bool PendingChanges
        {
            get
            {
                return _commit;
            }
        }


        internal CommandResult(T payload, bool commit, string message = null) //)
        {
            Payload = payload;
            _commit = commit;
            _message = message;
        }
    }

}
