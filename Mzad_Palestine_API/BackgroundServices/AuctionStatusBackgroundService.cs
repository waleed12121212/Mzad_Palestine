using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mzad_Palestine_Infrastructure.Services;

namespace Mzad_Palestine_API.BackgroundServices
{
    public class AuctionStatusBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<AuctionStatusBackgroundService> _logger;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(1); // تحديث كل دقيقة

        public AuctionStatusBackgroundService(
            IServiceProvider services,
            ILogger<AuctionStatusBackgroundService> logger)
        {
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _services.CreateScope())
                    {
                        var auctionStatusService = scope.ServiceProvider.GetRequiredService<AuctionStatusService>();
                        await auctionStatusService.UpdateAuctionStatusesAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "حدث خطأ أثناء تحديث حالة المزادات");
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }
    }
} 