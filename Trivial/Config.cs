using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trivial
{
    public class Config
    {
        public TrivialInfo TrivialInfo { get; set; }
        public string ConnectionString { get; set; }
    }

    public class TrivialInfo
    {
        public string Topic { get; set; }
        public string QuestionsNumber { get; set; }
    }
}
