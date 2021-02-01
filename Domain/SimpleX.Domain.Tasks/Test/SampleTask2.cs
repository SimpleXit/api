using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleX.Data.Context;
using SimpleX.Domain.Tasks.Base;
using SimpleX.Domain.Tasks.Helpers;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleX.Domain.Tasks.Test
{
    [Task("SampleTask2", "A sample of an automated task")]
    public class SampleTask2 : SimpleTask
    {       
        public SampleTask2(
            ILogger<SampleTask2> logger,
            IDbContextFactory<SimpleContext> contextFactory) : base(logger, contextFactory)
        {

        }

        protected override int IntervalInSeconds => 10;

        protected override Task ExecuteAsync(CancellationToken token)
        {
            _logger.LogDebug("ExecuteAsync started for {0}", this.GetType().Name);
            //await Task.Delay(2000);
            _logger.LogDebug("ExecuteAsync stopped for {0}", this.GetType().Name);
            return Task.CompletedTask;
        }
    }
}
