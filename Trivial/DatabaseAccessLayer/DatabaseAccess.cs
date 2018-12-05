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
                var cmdText = "SELECT id, category, type, difficulty, " +
                              "question, correct_answer, " +
                              "incorrect_answers FROM Trivial ";
                using (var sqlcmd = new SqlCommand(cmdText, conn))
                {
                    using (var reader = sqlcmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var qstn = new Question()
                            {
                                category = reader["category"].ToString(),
                                correct_answer = reader["correct_answer"].ToString(),
                                difficulty = reader["difficulty"].ToString(),
                                incorrect_answers = reader["incorrect_answers"].ToString().Split(','),
                                question = reader["question"].ToString(),
                                type = reader["type"].ToString()
                            };
                            var id = Convert.ToInt32(reader["id"]);
                            listOfQuestions.Add(id, qstn);
                        }
                    }
                }
            }
            return listOfQuestions;
        }

        public void Persist(ResponseModel questions)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                List<int> listOfIds = GetHashId(conn);               
                foreach (var question in questions.results)
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

        public ResponseModel ReadFromMemory(RequestModel request)
        {
            var ravd = new Random();
            var reponseModel = new ResponseModel();
            reponseModel.results = _listOfQuestions.Values
                .Where(q => q.category == request.Category)
                .Where(q => q.difficulty == request.Difficulty)
                .Where(q => q.type == request.Type)
                .OrderBy(r => ravd.Next()).Take(Convert.ToInt32(request.Amount)).ToList();
            if (reponseModel.results.Count() < Convert.ToInt32(request.Amount)) return null;
            //Random selection out of the list
            return reponseModel;
        }

        public List<int> GetHashId(SqlConnection conn)
        {
            List<int> listOfIds = new List<int>();
            var cmdReadId = "select id from Trivial";
            using (var readIdcmd = new SqlCommand(cmdReadId, conn))
            {
                using (var dataReader = readIdcmd.ExecuteReader())
                {
                    listOfIds.Add(Convert.ToInt32(dataReader.GetString(0)));
                }
            }
            return listOfIds;
        }
    }

    public interface IDatabaseAccess
    {
        Dictionary<int, Question> GetAllQuestions();
        void Persist(ResponseModel questions);
        ResponseModel ReadFromMemory(RequestModel request);
    }
}
