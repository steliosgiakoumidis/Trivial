using DbUp;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Trivial.StartUpTask
{
    public class EnsureDatabase : IStartUpTask
    {
        public IServiceProvider _provider;
        public Config _config;

        public EnsureDatabase(IServiceProvider provider, IOptions<Config> config)
        {
            _config = config.Value;
            _provider = provider;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var upgrader =
                    DeployChanges.To
                        .SqlDatabase(_config.ConnectionString)
                        .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                        .LogToConsole()
                        .Build();

                if (upgrader.IsUpgradeRequired())
                    upgrader.PerformUpgrade();
            }
            catch
            {
                //Log error
                throw;
            }
        }
    }
}
