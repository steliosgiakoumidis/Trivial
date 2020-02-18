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
            this.category = category;
            this.difficulty = difficulty;
            this.question = question;
            correct_answer = correctAnswer;
            incorrect_answers = incorrectAnswer;
            this.type = type;
        }
        public string category { get; set; }
        public string difficulty { get; set; }
        public string question { get; set; }
        public string correct_answer { get; set; }
        public string type { get; set; }
        public string[] incorrect_answers { get; set; }
    }
}
