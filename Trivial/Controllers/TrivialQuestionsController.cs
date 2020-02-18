using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Trivial.DataModels;
using Trivial.Handlers;
using Trivial.Entities;
using System.Linq;

namespace Trivial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrivialQuestionsController : ControllerBase
    {
        private readonly IHandleTrivialRequest _handleTrivialRequest;


        public TrivialQuestionsController(IOptions<Config> conf,
            IHandleTrivialRequest handleTrivialRequest,
            IDatabaseAccess databaseAccess)
        {
            _handleTrivialRequest = handleTrivialRequest;
        }

        [HttpGet("amount/{amount}/category/{category}/difficulty/{difficulty}")]
        public async Task<ActionResult<IEnumerable<ResponseModel>>> GetQuestions(string amount,
            string category, string difficulty)
        {
            if (amount == "0" || String.IsNullOrEmpty(amount))
                amount = "";
            if (category == "0" || String.IsNullOrEmpty(category))
                category = "";
            if (difficulty == "0" || String.IsNullOrEmpty(difficulty))
                difficulty = "";
            var requetsModel = new RequestModel(amount, category, difficulty);

            var questionsResponse = await _handleTrivialRequest.Handle(requetsModel);
            if (!questionsResponse.Any())
                return BadRequest();
            return Ok(questionsResponse);
        }
    }
}