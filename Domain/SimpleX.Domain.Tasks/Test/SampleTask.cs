using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleX.Data.Context;
using SimpleX.Domain.Tasks.Base;
using SimpleX.Domain.Tasks.Helpers;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleX.Domain.Tasks.Test
{
    [Task("SampleTask", "A sample of an automated task")]
    public class SampleTask : SimpleTask
    {       
        public SampleTask(
            ILogger<SampleTask> logger,
            IDbContextFactory<SimpleContext> contextFactory) : base(logger, contextFactory)
        {

        }

        protected override int IntervalInSeconds => 6;

        protected override async Task ExecuteAsync(CancellationToken token)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                _logger.LogDebug("ExecuteAsync started for {0}", this.GetType().Name);
                await Task.Delay(3000);
                _logger.LogDebug("ExecuteAsync stopped for {0}", this.GetType().Name);
            }
        }
    }
}
