using System;
using System.Collections.Generic;

namespace Trivial.Models
{
    public class Trivial
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Question { get; set; }
        public string Type { get; set; }
        public string Difficulty { get; set; }
        public string CorrectAnswer { get; set; }
        public string IncorrectAnswers { get; set; }
    }
}
