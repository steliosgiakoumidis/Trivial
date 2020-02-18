using System.Collections.Generic;
using Trivial.DataModels;

namespace Trivial
{
    public interface IDatabaseAccess
    {
        void Persist(IEnumerable<ResponseModel> questions);
        IEnumerable<ResponseModel> ReadQuestionsFromDatabase(RequestModel request);
    }
}
