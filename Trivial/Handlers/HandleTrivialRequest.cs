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
            try
            {
                var questionsAndAnswers = await GetDataOnline(model);
                if (questionsAndAnswers == null)
                    return _databaseAccess.ReadQuestionsFromDatabase(model);

                if (questionsAndAnswers.Count > 0)
                    await _databaseAccess.Persist(questionsAndAnswers);

                return questionsAndAnswers;
            }
            catch (Exception ex)
            {
                //Log.Error();
                return new List<ResponseModel>();
            }
        }

        private async Task<List<ResponseModel>> GetDataOnline(RequestModel model)
        {
            try
            {
                using var client = _clientFactory.CreateClient();
                var amount = String.IsNullOrWhiteSpace(model.Amount) ? "amount=10" : $"amount={model.Amount}";
                var type = String.Empty;
                var difficulty = String.IsNullOrWhiteSpace(model.Difficulty) ? "" : $"&difficulty={model.Difficulty}";
                var category = String.IsNullOrWhiteSpace(model.Category) ? "" : $"&category={model.Category}";

                var url = $"https://opentdb.com/api.php?{amount}{type}{difficulty}{category}";
                var res = await client.GetAsync(url);
                if (!res.IsSuccessStatusCode)
                    return null;

                return await ProcessResponse(res);
            }
            catch
            {
                //Log.Error();
                return null;
            }

        }

        private async Task<List<ResponseModel>> ProcessResponse(HttpResponseMessage responseMessage)
        {
            try
            {
                var body = await responseMessage.Content.ReadAsStringAsync();
                var extractedResponse = JsonConvert.DeserializeObject<RawModel>(body);
                foreach (var question in extractedResponse.Results)
                {
                    question.Question = HttpUtility.HtmlDecode(question.Question).Replace(@"\", "");

                }

                return extractedResponse.Results;
            }
            catch (Exception ex)
            {
                //Log.Error();
                return null;
            }

        }
    }
}
