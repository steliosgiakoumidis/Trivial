using System.Threading;
using System.Threading.Tasks;

namespace Trivial.StartUpTask
{
    public interface IStartUpTask
    {
        Task ExecuteAsync(CancellationToken cancellationToken = default);
    }
}
