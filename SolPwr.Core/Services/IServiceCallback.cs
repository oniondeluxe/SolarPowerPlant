using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.Services
{
    public interface IServiceCallback<TService>
    {
        event EventHandler<TService> ServicePushUpdate;

        void OnRequestEndpoint(Func<TService> getService);

        TService GetEndpoint();

        void InvokePush(TService service);

        void Teardown();
    }
}
