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
        private Config _conf;
        private readonly IHandleTrivialRequest _handleTrivialRequest;
        private readonly HttpClient _httpClient;
        private readonly IDatabaseAccess _databaseAccess;


        public TrivialQuestionsController(IOptions<Config> conf,
            IHandleTrivialRequest handleTrivialRequest,
            IDatabaseAccess databaseAccess)
        {
            _handleTrivialRequest = handleTrivialRequest;
            _conf = conf.Value;
            _httpClient = new HttpClient();
            _databaseAccess = databaseAccess;
        }

        [HttpGet("{amount}/{category}/{difficulty}/{type}")]
        public async Task<ActionResult<IEnumerable<ResponseModel>>> GetQuestions(string amount,
            string category, string difficulty, string type)
        {
            if (amount == "0" || String.IsNullOrEmpty(amount))
                amount = "";
            if (category == "0" || String.IsNullOrEmpty(category))
                category = "";
            if (difficulty == "0" || String.IsNullOrEmpty(difficulty))
                difficulty = "";
            if (type == "0" || String.IsNullOrEmpty(type))
                type = "";
            var requetsModel = new RequestModel(amount, category, difficulty, type);

            var questionsResponse = await _handleTrivialRequest.Handle(requetsModel, _httpClient, _databaseAccess);
            if (!questionsResponse.Any())
                return BadRequest();
            return Ok(questionsResponse);
        }
    }
}