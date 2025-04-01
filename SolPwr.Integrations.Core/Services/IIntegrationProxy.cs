using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.Services
{
    public interface IIntegrationProxy
    {
        void Initialize(CancellationToken cancellationToken);

        void Cleanup();

        void ExecuteWorker();

        string Title { get; }
    }
}
