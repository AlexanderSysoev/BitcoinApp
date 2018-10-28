using System;
using System.Threading;
using System.Threading.Tasks;
using BitcoinApp.Domain.InTransactionAggregate;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BitcoinApp.Api.HostedServices
{
    public class ConfirmationTrackHostedService : IHostedService
    {
        private readonly IServiceProvider _services;
        private Timer _timer;

        public ConfirmationTrackHostedService(IServiceProvider services)
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
                var confirmationTrackService = scope.ServiceProvider.GetRequiredService<ConfirmationTrackService>();
                confirmationTrackService.TrackConfirmationsAsync().Wait();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}
