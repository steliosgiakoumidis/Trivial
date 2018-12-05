using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Trivial.DatabaseAccessLayer;
using Trivial.DataModels;

namespace Trivial.Handlers
{
    public class HandleTrivialRequest:IHandleTrivialRequest
    {
        public async Task<IEnumerable<Question>> Handle(RequestModel model,HttpClient client,
            IDatabaseAccess databaseAccess)
        {
            var result = await GetQuestions(client, databaseAccess, model);
        
            return result;
        }

        private async Task<IEnumerable<Question>> GetQuestions(HttpClient client,
            IDatabaseAccess databaseAccess, RequestModel model)
        {
            try
            {
                return await GetQuestionsOnline(client, databaseAccess, model);
            }
            catch
            {
                try
                {                
                    return databaseAccess.ReadFromMemory(model);
                }
                catch (Exception e)
                {
                    //log exception 
                    return new List<Question>();
                }
            }
        }

        private async Task<List<Question>> GetQuestionsOnline(HttpClient client, IDatabaseAccess databasesAccess, RequestModel model)
        {
            var amount = String.IsNullOrWhiteSpace(model.Amount) ? "amount=10" : $"amount={model.Amount}";
            var type = String.IsNullOrWhiteSpace(model.Type) ? "" : $"&type={model.Type}";
            var difficulty = String.IsNullOrWhiteSpace(model.Difficulty) ? "" : $"&difficulty={model.Difficulty}";
            var category = String.IsNullOrWhiteSpace(model.Category) ? "" : $"&category={model.Category}";

            var url = $"https://opentdb.com/api.php?{amount}{type}{difficulty}{category}";
            var res = await client.GetAsync(url);
            var body = await res.Content.ReadAsStringAsync();

            var processedResponse = ProcessResponse(body);
            databasesAccess.Persist(processedResponse);
            return processedResponse;
        }

        private List<Question> ProcessResponse(string body)
        {
            var extractedResponse = JsonConvert.DeserializeObject<RawModel>(body);
            foreach (var question in extractedResponse.results)
            {
                question.question = HttpUtility.HtmlDecode(question.question);
            }
            return extractedResponse.results;
        }
    }
}
