using System.Collections.Generic;
using System.Threading.Tasks;
using Trivial.DataModels;

namespace Trivial.Handlers
{
    public interface IHandleTrivialRequest
    {
        Task<IEnumerable<ResponseModel>> Handle(RequestModel model);
    }
}
