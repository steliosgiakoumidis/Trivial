using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Trivial.StartUpTask
{
    public static class StartUpWebHostExtension
    {
        public static async Task RunWithTaskAsync(this IHost webhost,
        CancellationToken token = default)
        {
            var startupTasks = webhost.Services.GetServices<IStartUpTask>();
            foreach (var task in startupTasks)
            {
                await task.ExecuteAsync(token);
            }
            await webhost.RunAsync(token);
        }
    }
}
