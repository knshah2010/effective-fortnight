using AndroidAmcu.Areas.Configuration.BAL;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace AndroidAmcu.Areas.Configuration.Hosted
{
    public class InboxParsing : BackgroundService
    {
        InboxParserBal _inbox;
        private Timer _timer;
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _inbox = new InboxParserBal();
            return base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this._timer = new Timer(ExecuteTask, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            this._timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;

        }
        private void ExecuteTask(object state)
        {
            _inbox.ParseInbox();
        }
    }
}
