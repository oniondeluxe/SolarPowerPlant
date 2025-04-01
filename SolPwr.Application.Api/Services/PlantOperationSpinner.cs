using OnionDlx.SolPwr.Services;

namespace OnionDlx.SolPwr.Application.Services
{
    public class PlantOperationSpinner : IHostedService
    {
        public static bool Enabled { get; set; }
        public static int IntervalSeconds { get; set; }

        private readonly ILogger<PlantOperationSpinner> _logger;
        private Timer _timer;
        private readonly IIntegrationProxy _integrationProxy;

        public PlantOperationSpinner(ILogger<PlantOperationSpinner> logger, IIntegrationProxy integrationProxy)
        {
            _logger = logger;
            _integrationProxy = integrationProxy;
        }


        static PlantOperationSpinner()
        {
            IntervalSeconds = 10;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _integrationProxy.Initialize(cancellationToken);
                if (!Enabled)
                {
                    return Task.CompletedTask;
                }

                // This should not be run this often, but every <IntervalSeconds> second is for illustration/debugging proposes
                _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(IntervalSeconds));
            }
            catch (Exception ex)
            {
                // The initialize went to sh*t, so no point trying further
                _logger.LogError(ex.Message);
                return Task.FromException(ex);
            }

            return Task.CompletedTask;
        }


        private void DoWork(object state)
        {
            _logger.LogInformation($"Background task is running on '{_integrationProxy.Title}'.");

            // We will ow push back into the Plant CRUD layer to run the show,
            // and with ourselves as arguments
            _integrationProxy.ExecuteWorker();
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            _integrationProxy?.Cleanup();
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}
