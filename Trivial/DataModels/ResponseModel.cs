using System.Collections.Generic;
namespace Trivial.DataModels
{
    public class RawModel
    {
        public int response_code { get; set; }
        public List<Question> results { get; set; }

    }

    public class Question
    {
        public Question(string ctg, string tp, string diff, string qstn, string ca, string[] ia)
        {
            category = ctg;
            type = tp;
            difficulty = diff;
            question = qstn;
            correct_answer = ca;
            incorrect_answers = ia;
        }
        public string category { get; set; }
        public string type { get; set; }
        public string difficulty { get; set; }
        public string question { get; set; }
        public string correct_answer { get; set; }
        public string[] incorrect_answers { get; set; }
    }
}
