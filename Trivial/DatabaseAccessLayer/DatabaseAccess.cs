using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Remotion.Linq.Clauses;
using Trivial.DataModels;

namespace Trivial.DatabaseAccessLayer
{
    public class DatabaseAccess: IDatabaseAccess
    {
        private readonly Config _config;
        private readonly string _connectionString;
        private Dictionary<int, Question> _listOfQuestions;
        public DatabaseAccess(IOptions<Config> config)
        {
            _config = config.Value;
            _connectionString = _config.ConnectionString;
            _listOfQuestions = GetAllQuestions();

        }

        public Dictionary<int, Question> GetAllQuestions()
        {
            Dictionary<int, Question> listOfQuestions = new Dictionary<int, Question>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmdText = "SELECT id, category, type, difficulty, " +
                              "question, correct_answer, " +
                              "incorrect_answers FROM Trivial ";
                using (var sqlcmd = new SqlCommand(cmdText, conn))
                {
                    using (var reader = sqlcmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var qstn = new Question(reader["category"].ToString(),
                                reader["type"].ToString(),
                                reader["difficulty"].ToString(),
                                reader["question"].ToString(), 
                                reader["correct_answer"].ToString(),
                                reader["incorrect_answers"].ToString().Split(','));
                            var id = Convert.ToInt32(reader["id"]);
                            listOfQuestions.Add(id, qstn);
                        }
                    }
                }
            }
            return listOfQuestions;
        }

        public void Persist(IEnumerable<Question> questions)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                List<int> listOfIds = GetHashId(conn);               
                foreach (var question in questions)
                {
                    var id = question.question.GetHashCode();
                    if (!listOfIds.Contains(id))
                    {
                        PersistQuestion(question, conn, id);
                    }                                                                          
                }                
            }
        }

        private void PersistQuestion(Question question, SqlConnection conn, int id)
        {
            var cmd = "INSERT INTO Trivial " +
                      "(id, category, type, difficulty, " +
                      "question, correct_answer, " +
                      "incorrect_answers) " +
                      "VALUES (@id, @category, @type, )" +
                      "@difficulty, @question, " +
                      "@correct_answer, @incorrrect_answers)";

            using (var sqlcmd = new SqlCommand(cmd, conn))
            {
                sqlcmd.Parameters.AddWithValue("id", id);
                sqlcmd.Parameters.AddWithValue("category", question.category);
                sqlcmd.Parameters.AddWithValue("type", question.type);
                sqlcmd.Parameters.AddWithValue("difficulty", question.difficulty);
                sqlcmd.Parameters.AddWithValue("question", question.question);
                sqlcmd.Parameters.AddWithValue("correct_answer", question.correct_answer);
                sqlcmd.Parameters.AddWithValue("incorrect_answers", question.incorrect_answers);
                sqlcmd.ExecuteNonQuery();
            }
        }

        public IEnumerable<Question> ReadFromMemory(RequestModel request)
        {
            var ravd = new Random();
            var reponse = new List<Question>();
            reponse = _listOfQuestions.Values
                .Where(q => q.category == request.Category)
                .Where(q => q.difficulty == request.Difficulty)
                .Where(q => q.type == request.Type)
                .OrderBy(r => ravd.Next()).Take(Convert.ToInt32(request.Amount)).ToList();
            if (reponse.Count() < Convert.ToInt32(request.Amount)) return null;
            //Random selection out of the list
            return reponse;
        }

        public List<int> GetHashId(SqlConnection conn)
        {
            List<int> listOfIds = new List<int>();
            var cmdReadId = "select id from Trivial";
            using (var readIdcmd = new SqlCommand(cmdReadId, conn))
            {
                using (var dataReader = readIdcmd.ExecuteReader())
                {
                    listOfIds.Add(Convert.ToInt32(dataReader["id"].ToString()));
                }
            }
            return listOfIds;
        }
    }

    public interface IDatabaseAccess
    {
        Dictionary<int, Question> GetAllQuestions();
        void Persist(IEnumerable<Question> questions);
        IEnumerable<Question> ReadFromMemory(RequestModel request);
    }
}
