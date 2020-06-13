using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trivial.DataModels;
using Trivial.Handlers;

namespace Trivial.Controllers
{
    [Route("api/TrivialQuestions")]
    [ApiController]
    public class TrivialQuestionsController : ControllerBase
    {
        private readonly IHandleTrivialRequest _handleTrivialRequest;


        public TrivialQuestionsController(IHandleTrivialRequest handleTrivialRequest)
        {
            _handleTrivialRequest = handleTrivialRequest;
        }

        [HttpGet("amount/{amount}/category/{category}/difficulty/{difficulty}")]
        public async Task<ActionResult<IEnumerable<ResponseModel>>> GetQuestions(string amount,
            string category, string difficulty)
        {
            try
            {
                if (String.IsNullOrEmpty(amount) || amount == "0")
                    amount = "";
                if (String.IsNullOrEmpty(category) || category == "0")
                    category = "";
                if (String.IsNullOrEmpty(difficulty) || difficulty == "0")
                    difficulty = "";
                var requetsModel = new RequestModel(amount, category, difficulty);

                var questionsResponse = await _handleTrivialRequest.Handle(requetsModel);
                if (!questionsResponse.Any())
                    return BadRequest();

                return Ok(questionsResponse);
            }
            catch(Exception ex)
            {
                Log.Error($"Exception: {ex}");

                return BadRequest();
            }

        }
    }
}