using Newtonsoft.Json;
using System.Collections.Generic;

namespace Trivial.DataModels
{
    public class RawModel
    {
        public int Response_code { get; set; }
        public List<ResponseModel> Results { get; set; }

    }

    public class ResponseModel
    {
        public ResponseModel()
        {
        }

        public ResponseModel(string category, string difficulty,
            string question, string correctAnswer, string[] incorrectAnswer, string type)
        {
            Category = category;
            Difficulty = difficulty;
            Question = question;
            Correct_Answer = correctAnswer;
            Incorrect_Answers = incorrectAnswer;
            Type = type;
        }
        public string Category { get; set; }
        public string Difficulty { get; set; }
        public string Question { get; set; }
        public string Correct_Answer { get; set; }
        public string Type { get; set; }
        public string[] Incorrect_Answers { get; set; }
    }
}
