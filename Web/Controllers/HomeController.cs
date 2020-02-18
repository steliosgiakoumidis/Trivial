using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
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

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> GetQuestions()
        {
            var client = new HttpClient();
            var test = await client.GetAsync("https://localhost:44357/api/trivialquestions/3/0/0/0");
            var responseAsAsString = await test.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize(responseAsAsString, typeof(List<ResponseModel>)) as List<ResponseModel>;
            var viewModel = new Transformations().ToViewModel(responseObject);
            return View("~/Views/Questions.cshtml", viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
