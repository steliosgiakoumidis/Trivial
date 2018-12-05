using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trivial.DataModels
{
    public class RequestModel
    {
        public string Amount { get; set; }
        public string Category { get; set; }
        public string Difficulty { get; set; }
        public string Type { get; set; }
    }
}
