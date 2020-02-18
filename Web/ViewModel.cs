using System.Collections.Generic;

namespace Web
{
    public class ViewModel
    {
        public string Question { get; set; }
        public List<string> Answers { get; set; }
        public string QuestionType { get; set; }
        public int CorrectAnswer { get; set; }
    }
}