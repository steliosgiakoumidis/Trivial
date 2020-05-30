using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Web.Interfaces;
using Web.Models;

namespace Web.Handlers
{
    public class QuestionHandler : IQuestionHandler
    {
        private IHttpClientFactory _clientFactory;

        public QuestionHandler(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<List<ResponseModel>> GetQuestionsInternally(QuestionParameters questionParameters)
        {

            try
            {
                using var client = _clientFactory.CreateClient();
                var url = $"https://localhost:44357/api/trivialquestions/amount/{questionParameters.Amount.ToString()}/category/0/difficulty/{questionParameters.Difficulty.ToString().ToLower()}";
                var request = await client.GetAsync(url);
                if (!request.IsSuccessStatusCode)
                    return new List<ResponseModel>();

                var responseAsAsString = await request.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<ResponseModel>>(responseAsAsString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception)
            {
                //Log.Error
                return new List<ResponseModel>();
            }
        }
    }
}
