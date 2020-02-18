using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Enum;

namespace Web.Models
{
    public class QuestionParameters
    {
        public Difficulty Difficulty { get; set; }
        public int Amount { get; set; }
    }
}
