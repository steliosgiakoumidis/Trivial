using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trivial.DataModels;
using Trivial.Entities;


namespace Trivial.DatabaseAccessLayer
{
    public class DatabaseAccess : IDatabaseAccess
    {
        private TrivialContext _context;
        public DatabaseAccess(TrivialContext context)
        {
            _context = context;
        }

        public async Task Persist(IEnumerable<ResponseModel> questions)
        {
            try
            {
                List<int> listOfIds = await GetHashIds();
                foreach (var question in questions)
                {
                    var id = question.Question.GetHashCode();
                    if (!listOfIds.Contains(id))
                    {
                        PersistQuestion(question, id);
                    }
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                //Log.Error()
            }
        }

        private void PersistQuestion(ResponseModel question, int id)
        {
            _context.Trivial.Add(new Entities.Trivial()
            {
                Id = id,
                Category = question.Category,
                Correct_Answer = question.Correct_Answer,
                Difficulty = question.Difficulty,
                Incorrect_Answers = String.Join(".!.", question.Incorrect_Answers),
                Question = question.Question,
                Type = question.Type
            });
        }

        public IEnumerable<ResponseModel> ReadQuestionsFromDatabase(RequestModel request)
        {
            var rand = new Random();
            var response = _context.Trivial
                .Where(q => q.Category == request.Category)
                .Where(q => q.Difficulty == request.Difficulty)
                .OrderBy(r => rand.Next()).Take(Convert.ToInt32(request.Amount)).ToList();
            if (response.Count() < Convert.ToInt32(request.Amount))
                return null;

            return EntityToResponseModel(response);
        }

        private IEnumerable<ResponseModel> EntityToResponseModel(List<Entities.Trivial> response)
        {
            return response.Select(trivial => new ResponseModel(trivial.Category,
                trivial.Difficulty, trivial.Question, trivial.Correct_Answer,
                trivial.Incorrect_Answers.Split(".!."), trivial.Type));
        }

        public async Task<List<int>> GetHashIds()
        {
            return await _context.Trivial.Select(a => a.Id).ToListAsync(); ;
        }
    }

}
