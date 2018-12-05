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

        [HttpGet("que")]
        public async Task<ActionResult<List<Question>>> GetQuestions()
        {
            var a = new RequestModel() {Amount = "10", Category = "10", Difficulty = "10", Type = "10"};
            var b = await _handleTrivialRequest.Handle(a, _httpClient, _databaseAccess);
            return b.results;
        }
    }
}