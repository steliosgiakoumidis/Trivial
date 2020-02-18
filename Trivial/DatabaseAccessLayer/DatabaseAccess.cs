using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using Trivial.DataModels;
using Trivial.Entities;


namespace Trivial.DatabaseAccessLayer
{
    public class DatabaseAccess : IDatabaseAccess
    {
        private readonly Config _config;
        private readonly string _connectionString;
        private TrivialContext _context;
        public DatabaseAccess(IOptions<Config> config, TrivialContext context)
        {
            _config = config.Value;
            _connectionString = _config.ConnectionString;
            _context = context;


        }

        public void Persist(IEnumerable<ResponseModel> questions)
        {

            List<int> listOfIds = GetHashIds();
            foreach (var question in questions)
            {
                var id = question.Question.GetHashCode();
                if (!listOfIds.Contains(id))
                {
                    PersistQuestion(question, id);
                }
            }

        }

        private void PersistQuestion(ResponseModel question, int id)
        {
            _context.Trivial.Add(new Entities.Trivial()
            {
                Id = id,
                Category = question.Category,
                CorrectAnswer = question.Correct_answer,
                Difficulty = question.Difficulty,
                IncorrectAnswers = String.Join(".!.", question.Incorrect_answers),
                Question = question.Question,
                Type = question.Type
            });
            _context.SaveChanges();
        }

        public IEnumerable<ResponseModel> ReadQuestionsFromDatabase(RequestModel request)
        {
            var rand = new Random();
            var response = _context.Trivial
                .Where(q => q.Category == request.Category)
                .Where(q => q.Difficulty == request.Difficulty)
                .Where(q => q.Type == request.Type)
                .OrderBy(r => rand.Next()).Take(Convert.ToInt32(request.Amount)).ToList();
            if (response.Count() < Convert.ToInt32(request.Amount)) 
                return null;         

            return EntityToResponseModel(response);
        }

        private IEnumerable<ResponseModel> EntityToResponseModel(List<Entities.Trivial> response)
        {
            return response.Select(trivial => new ResponseModel(trivial.Category, trivial.Type,
                trivial.Difficulty, trivial.Question, trivial.CorrectAnswer,
                trivial.IncorrectAnswers.Split(".!.")));
        }

        public List<int> GetHashIds()
        {
            return _context.Trivial.Select(a => a.Id).ToList(); ;
        }
    }

}
