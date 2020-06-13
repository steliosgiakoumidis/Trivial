using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using Web.Interfaces;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private IQuestionHandler _questionHandler;

        public HomeController(IQuestionHandler questionHandler)
        {
            _questionHandler = questionHandler;
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
            var questions = await _questionHandler.GetQuestionsInternally(questionParameters);
            if (questions.Count == 0)
                return BadRequest();

            return View("~/Views/Questions.cshtml", new Transformations().ToViewModel(questions));
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
