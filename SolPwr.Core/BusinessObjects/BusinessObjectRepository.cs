using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.BusinessObjects
{
    /// <summary>
    /// Abstract base class for implementation of the repo, representing a transparent wrapper around an Orm context
    /// This abstract class has no dependencies to any specific Orm, like EntityFramework
    /// </summary>
    public abstract class BusinessObjectRepository : IBusinessObjectRepository
    {
        Guid? _pendingTransactionId;
        readonly List<string> _pendingLogMessages;

        #region IDisposable

        protected bool disposed = false;

        ~BusinessObjectRepository()
        {
            Dispose(false);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                // TODO
                disposed = true;
            }
        }

        #endregion

        protected BusinessObjectRepository()
        {
            _pendingLogMessages = new List<string>();
        }


        public void SetDirty(object invoker)
        {
            if (!_pendingTransactionId.HasValue)
            {
                _pendingTransactionId = Guid.NewGuid();
            }
        }

        public abstract bool IsReadonly { get; }


        protected Guid? GetTransactionID()
        {
            return _pendingTransactionId;
        }


        public void AddPendingLogMessage(string message)
        {
            // We don't want to emit logs for CRUD operations until the save operation is completed
            if (!string.IsNullOrEmpty(message))
            {
                _pendingLogMessages.Add(message);
            }
        }


        protected virtual void WriteLogMessage(string message)
        {
        }


        private void FlushLogMessages()
        {
            try
            {
                foreach (var message in _pendingLogMessages)
                {
                    WriteLogMessage(message);
                }
            }
            finally
            {
                // Clearing regardless
                _pendingLogMessages.Clear();
            }
        }

        protected abstract void ExecuteSaveChanges();

        protected abstract Task ExecuteSaveChangesAsync();


        public Guid? SaveChanges()
        {
            ExecuteSaveChanges();
            FlushLogMessages();

            // Save away and then clear
            var trx = GetTransactionID();
            _pendingTransactionId = null;

            return trx;
        }


        public async Task<Guid?> SaveChangesAsync()
        {
            await ExecuteSaveChangesAsync();
            FlushLogMessages();

            // Save away and then clear
            var trx = GetTransactionID();
            _pendingTransactionId = null;

            return trx;
        }
    }
}
