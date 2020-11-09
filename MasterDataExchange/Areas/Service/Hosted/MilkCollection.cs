using DataExchange.Areas.Service.Utility;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataExchange.Areas.Service.Hosted
{
    public class MilkCollection : BackgroundService
    {
        ConsumeApi _api;
        private Timer _timer;
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _api = new ConsumeApi("milk_collection");
            return base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this._timer = new Timer(ExecuteTask, null, TimeSpan.Zero,TimeSpan.FromMinutes(30));           
            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            this._timer?.Change(Timeout.Infinite, 0);
          //  return Task.CompletedTask;

        }
        private async void ExecuteTask(object state)
        {
            _api.Call();
        }
    }
}
