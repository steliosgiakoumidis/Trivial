using System.Collections.Generic;
using System.Threading.Tasks;
using Trivial.DataModels;

namespace Trivial
{
    public interface IDatabaseAccess
    {
        Task Persist(IEnumerable<ResponseModel> questions);
        IEnumerable<ResponseModel> ReadQuestionsFromDatabase(RequestModel request);
    }
}
