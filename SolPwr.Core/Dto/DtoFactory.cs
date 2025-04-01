using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.Dto
{
    public class DtoFactory<D> where D : ITransactionalDto
    {
        readonly Func<Guid?, D> _onCreateInstance;

        internal D CreateInstance(Guid? transactionId)
        {
            return _onCreateInstance(transactionId);
        }

        public DtoFactory(Func<Guid?, D> onCreateInstance)
        {
            if (onCreateInstance == null)
            {
                throw new ArgumentNullException();
            }

            _onCreateInstance = onCreateInstance;
        }
    }
}
