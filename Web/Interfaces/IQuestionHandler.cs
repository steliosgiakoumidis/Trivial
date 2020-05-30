using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Interfaces
{
    public interface IQuestionHandler
    {
        Task<List<ResponseModel>> GetQuestionsInternally(QuestionParameters questionParameters);
    }
}
