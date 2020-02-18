using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Trivial.DataModels;

namespace Trivial.Handlers
{

    public class HandleTrivialRequest : IHandleTrivialRequest
    {
        private readonly IDatabaseAccess _databaseAccess;
        private IHttpClientFactory _clientFactory;

        public HandleTrivialRequest(IDatabaseAccess databaseAccess, IHttpClientFactory clientFactory)
        {
            _databaseAccess = databaseAccess;
            _clientFactory = clientFactory;
        }

        public async Task<IEnumerable<ResponseModel>> Handle(RequestModel model)
        {

            var result = await GetQuestions(model);

            return result;
        }

        private async Task<IEnumerable<ResponseModel>> GetQuestions(RequestModel model)
        {
            try
            {
                var questions = await GetQuestionsOnline(model);
                if (questions == null)
                    return _databaseAccess.ReadQuestionsFromDatabase(model);

                return questions;
            }
            catch(Exception ex)
            {
                //Log the exception
                return new List<ResponseModel>();
            }
        }

        private async Task<List<ResponseModel>> GetQuestionsOnline(RequestModel model)
        {
            var client = _clientFactory.CreateClient();
            var amount = String.IsNullOrWhiteSpace(model.Amount) ? "amount=10" : $"amount={model.Amount}";
            var type = "" ;
            var difficulty = String.IsNullOrWhiteSpace(model.Difficulty) ? "" : $"&difficulty={model.Difficulty}";
            var category = String.IsNullOrWhiteSpace(model.Category) ? "" : $"&category={model.Category}";

            var url = $"https://opentdb.com/api.php?{amount}{type}{difficulty}{category}";
            var res = await client.GetAsync(url);
            if (!res.IsSuccessStatusCode)
                return null;

            var body = await res.Content.ReadAsStringAsync();
            var processedResponse = ProcessResponse(body);
            _databaseAccess.Persist(processedResponse);
            return processedResponse;
        }

        private List<ResponseModel> ProcessResponse(string body)
        {
            var extractedResponse = JsonConvert.DeserializeObject<RawModel>(body);
            foreach (var question in extractedResponse.Results)
            {
                question.question = HttpUtility.HtmlDecode(question.question).Replace(@"\", "");

            }
            return extractedResponse.Results;
        }
    }
}
