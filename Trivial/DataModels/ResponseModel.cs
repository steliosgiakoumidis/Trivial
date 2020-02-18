using System.Collections.Generic;
namespace Trivial.DataModels
{
    public class RawModel
    {
        public int response_code { get; set; }
        public List<ResponseModel> Results { get; set; }

    }

    public class ResponseModel
    {
        public ResponseModel()
        {
        }

        public ResponseModel(string category, string type, string difficulty,
            string question, string correctAnswer, string[] incorrectAnswer)
        {
            Category = category;
            Type = type;
            Difficulty = difficulty;
            Question = question;
            Correct_answer = correctAnswer;
            Incorrect_answers = incorrectAnswer;
        }
        public string Category { get; set; }
        public string Type { get; set; }
        public string Difficulty { get; set; }
        public string Question { get; set; }
        public string Correct_answer { get; set; }
        public string[] Incorrect_answers { get; set; }
    }
}
