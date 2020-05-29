using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Trivial.DataModels;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IHttpClientFactory _clientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult QuestionsParameters()
        {
            return View("~/Views/QuestionsParameters.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> GetQuestions(QuestionParameters questionParameters)
        {
            var questions = await GetQuestionsInternally(questionParameters);
            if (questions == null)
                return BadRequest();
            
            var viewModel = new Transformations().ToViewModel(responseObject.ToList());
            return View("~/Views/Questions.cshtml", viewModel);
        }

        private async Task<List<ResponseModel>> GetQuestionsInternally(QuestionParameters questionParameters)
        {
            try
            {
                using var client = _clientFactory.CreateClient();
                var url = $"https://localhost:44357/api/trivialquestions/amount/{questionParameters.Amount.ToString()}/category/0/difficulty/{questionParameters.Difficulty.ToString().ToLower()}";
                var request = await client.GetAsync(url);
                if (!request.IsSuccessStatusCode)
                    return null;

                var responseAsAsString = await request.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<ResponseModel>>(responseAsAsString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception)
            {
                //Log.Error
                return null;
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
