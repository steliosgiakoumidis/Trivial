using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Remotion.Linq.Clauses;
using Trivial.DataModels;
using Trivial.Models;


namespace Trivial.DatabaseAccessLayer
{
    public class DatabaseAccess: IDatabaseAccess
    {
        private readonly Config _config;
        private readonly string _connectionString;
        private Dictionary<int, Question> _listOfQuestions;
        private TrivialContext _context;
        public DatabaseAccess(IOptions<Config> config, TrivialContext context)
        {
            _config = config.Value;
            _connectionString = _config.ConnectionString;
            _context = context;
            _listOfQuestions = GetAllQuestions();


        }

        public Dictionary<int, Question> GetAllQuestions()
        {
            Dictionary<int, Question> listOfQuestions = new Dictionary<int, Question>();
            var entries = _context.Trivial.ToList();
            foreach (var entry in entries)
            {
                var qst = new Question(entry.Category,
                    entry.Type, entry.Difficulty,
                    entry.Question, entry.CorrectAnswer,
                    entry.IncorrectAnswers.Split(","));
                var idd = entry.Id;
                listOfQuestions.Add(idd, qst);
            }
            return listOfQuestions;
        }

        public void Persist(IEnumerable<Question> questions)
        {
            
            List<int> listOfIds = GetHashIds();               
            foreach (var question in questions)
            {
                var id = question.question.GetHashCode();
                if (!listOfIds.Contains(id))
                {
                    PersistQuestion(question, id);
                }                                                                          
            }                
            
        }

        private void PersistQuestion(Question question, int id)
        {
            var a = _context.Trivial.Add(new Models.Trivial()
            {
                Id = id,
                Category = question.category,
                CorrectAnswer = question.correct_answer,
                Difficulty = question.difficulty,
                IncorrectAnswers = question.incorrect_answers.ToString(),
                Question = question.question,
                Type = question.type
            });
            _context.SaveChanges();
        }

        public IEnumerable<Question> ReadFromMemory(RequestModel request)
        {
            var rand = new Random();
            var reponse = new List<Question>();
            reponse = _listOfQuestions.Values
                .Where(q => q.category == request.Category)
                .Where(q => q.difficulty == request.Difficulty)
                .Where(q => q.type == request.Type)
                .OrderBy(r => rand.Next()).Take(Convert.ToInt32(request.Amount)).ToList();
            if (reponse.Count() < Convert.ToInt32(request.Amount)) return null;
            //Random selection out of the list
            return reponse;
        }

        public List<int> GetHashIds()
        {       
            return _context.Trivial.Select(a => a.Id).ToList(); ;
        }
    }

    public interface IDatabaseAccess
    {
        Dictionary<int, Question> GetAllQuestions();
        void Persist(IEnumerable<Question> questions);
        IEnumerable<Question> ReadFromMemory(RequestModel request);
    }
}
