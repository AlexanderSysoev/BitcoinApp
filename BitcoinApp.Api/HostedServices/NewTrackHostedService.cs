using System;
using System.Threading;
using System.Threading.Tasks;
using BitcoinApp.Domain.InTransactionAggregate;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BitcoinApp.Api.HostedServices
{
    public class NewTrackHostedService : IHostedService
    {
        private readonly IServiceProvider _services;
        private Timer _timer;

        public NewTrackHostedService(IServiceProvider services)
        {
            _services = services;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            using (var scope = _services.CreateScope())
            {
                var newTrackService = scope.ServiceProvider.GetRequiredService<NewTrackService>();
                newTrackService.TrackNewAsync().Wait();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}
