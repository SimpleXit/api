using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SimpleX.Data.Context;
using SimpleX.Domain.Tasks.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleX.Domain.Tasks.Helpers
{
    public class TaskRunner : IHostedService
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger _logger;

        public TaskRunner(IServiceProvider provider, ILogger<TaskRunner> logger) 
        {
            _provider = provider ??
                throw new ArgumentNullException(nameof(provider));
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace("TaskRunner.ExecuteAsync()");

            return RunTasks(cancellationToken);
        }

        private List<Task> tasksToRun = new List<Task>();
        private Task RunTasks(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogTrace("TaskRunner.RunTasks()");

                using (var scope = _provider.CreateScope())
                {
                    var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<SimpleContext>>();
                    using (var context = factory.CreateDbContext())
                    {
                        var enabledTasks = context.AppTask.Where(t => t.IsEnabled).ToList();

                        foreach (var enabledTask in enabledTasks)
                        {
                            SimpleTask task = GetTaskByName(scope, enabledTask.FullName);
                            tasksToRun.Add(Task.Run(() => task.StartAsync(cancellationToken)));
                        }
                    }
                    Task.WaitAll(tasksToRun.ToArray());
                    return Task.CompletedTask;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private SimpleTask GetTaskByName(IServiceScope scope, string fullName)
        {
            try
            {
                return (SimpleTask)scope.ServiceProvider.GetRequiredService(GetTaskTypeByName(fullName));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private Type GetTaskTypeByName(string fullName)
        {
            try
            {
                return Assembly.GetCallingAssembly().GetType(fullName, true, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
