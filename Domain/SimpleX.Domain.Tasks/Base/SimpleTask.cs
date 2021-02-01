using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleX.Data.Context;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleX.Domain.Tasks.Base
{
    public abstract class SimpleTask 
    {
        private Timer _timer;
        private bool _isExecuting;

        protected readonly ILogger _logger;
        protected readonly IDbContextFactory<SimpleContext> _contextFactory;

        public SimpleTask(ILogger logger, IDbContextFactory<SimpleContext> contextFactory)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            _contextFactory = contextFactory ??
                throw new ArgumentNullException(nameof(contextFactory));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (IsEnabled)
            {
                _timer = new Timer(ExecuteTask, cancellationToken, 10000, IntervalInSeconds * 1000);
                _logger.LogDebug("Timer started for task {0}", this.GetType().Name);
            }

            return Task.CompletedTask;
        }

        private async void ExecuteTask(object token)
        {
            try
            {
                if (_isExecuting)
                    return;

                if (IsEnabled && IsSheduled)
                {
                    _isExecuting = true;
                    _logger.LogDebug("Execution started for task {0}", this.GetType().Name);
                    
                    await CleanUpLog();
                    await ExecuteAsync((CancellationToken)token);
                    
                    _logger.LogDebug("Execution stopped for task {0}", this.GetType().Name);
                }
            }
            catch(Exception ex)
            {
                //TODO
                _logger.LogError(ex, ex.Message);
            }
            finally
            {
                _isExecuting = false;
            }   
        }

        protected virtual bool IsEnabled
        {
            get
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    var appTask = context.AppTask.FirstOrDefault(t => t.FullName == this.GetType().FullName);

                    if (appTask == null)
                        return false;
                    else
                        return appTask.IsEnabled;
                }
            }
        }

        protected virtual bool IsSheduled
        {
            get
            {
                //TODO:
                using (var context = _contextFactory.CreateDbContext())
                {
                    var task = context.AppTask.FirstOrDefault(t => t.FullName == this.GetType().FullName);

                    if (task == null)
                        return false;

                    switch (DateTime.Today.DayOfWeek)
                    {
                        case DayOfWeek.Sunday:
                            if (!task.RunOnSunday) return false;
                            break;
                        case DayOfWeek.Monday:
                            if (!task.RunOnMonday) return false;
                            break;
                        case DayOfWeek.Tuesday:
                            if (!task.RunOnTuesday) return false;
                            break;
                        case DayOfWeek.Wednesday:
                            if (!task.RunOnWednesday) return false;
                            break;
                        case DayOfWeek.Thursday:
                            if (!task.RunOnThursday) return false;
                            break;
                        case DayOfWeek.Friday:
                            if (!task.RunOnFriday) return false;
                            break;
                        case DayOfWeek.Saturday:
                            if (!task.RunOnSaturday) return false;
                            break;
                    }

                    return (bool?)task.GetType().GetProperty("RunOnHour" + DateTime.Now.Hour.ToString("00"))?.GetValue(task, null) ?? false;
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (_timer != null)
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
                _timer.Dispose();
            }

            return Task.CompletedTask;
        }

        protected abstract int IntervalInSeconds { get; }
        protected abstract Task ExecuteAsync(CancellationToken token);

        private DateTime? _lastCleanUp;
        private async Task CleanUpLog()
        {
            try
            {
                if (_lastCleanUp != null && _lastCleanUp.Value.Date.Equals(DateTime.Today))
                    return;

                using(var context = _contextFactory.CreateDbContext())
                {
                    DateTime removeUntil = DateTime.Today.AddMonths(-1);
                    var logs = context.AppTaskLog.Where(l => l.FullName == this.GetType().FullName && l.TimeStamp < removeUntil);
                    context.RemoveRange(logs);
                    await context.SaveChangesAsync();
                    
                    _lastCleanUp = DateTime.Now;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error during CleanUpLog for task {this.GetType().FullName}");
            }
        }
    }
}
