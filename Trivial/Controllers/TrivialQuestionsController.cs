using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Trivial.DatabaseAccessLayer;
using Trivial.DataModels;
using Trivial.Handlers;

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
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestions(string amount,
            string category, string difficulty, string type)
        {
            if (amount == "0") amount = "";
            if (category == "0") category = "";
            if (difficulty == "0") difficulty = "";
            if (type == "0") type = "";

            var a = new RequestModel(amount, category, difficulty, type);
            var b = await _handleTrivialRequest.Handle(a, _httpClient, _databaseAccess);
            return Ok(b);
        }
    }
}