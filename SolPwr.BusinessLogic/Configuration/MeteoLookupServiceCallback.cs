using OnionDlx.SolPwr.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.Configuration
{
    class MeteoLookupServiceCallback : IMeteoLookupServiceCallback
    {
        Func<IMeteoLookupService> _getService;

        public event EventHandler<IMeteoLookupService> ServicePushUpdate;

        public void InvokePush(IMeteoLookupService service)
        {
            if (ServicePushUpdate != null)
            {
                ServicePushUpdate(this, service);
            }
        }


        public void Teardown()
        {
            if (ServicePushUpdate != null)
            {
                foreach (var del in ServicePushUpdate.GetInvocationList())
                {
                    ServicePushUpdate -= (EventHandler<IMeteoLookupService>)del;
                }
            }
        }


        public void OnRequestEndpoint(Func<IMeteoLookupService> getService)
        {
            _getService = getService;
        }


        public IMeteoLookupService GetEndpoint()
        {
            if (_getService == null)
            {
                throw new InvalidOperationException("Service not set");
            }

            return _getService();
        }


        public MeteoLookupServiceCallback()
        {
        }
    }
}
