using System.Collections.Generic;
namespace Trivial.DataModels
{
    public class RawModel: ResponseModel
    {
        public int response_code { get; set; }
    }

    public class Question
    {
        public string category { get; set; }
        public string type { get; set; }
        public string difficulty { get; set; }
        public string question { get; set; }
        public string correct_answer { get; set; }
        public string[] incorrect_answers { get; set; }
    }

    public class ResponseModel
    {
        public List<Question> results { get; set; }
    }
}
