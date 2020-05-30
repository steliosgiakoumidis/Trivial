using System.Collections.Generic;

namespace Trivial.DataModels
{
    public class RawModel
    {
        public int Response_code { get; set; }
        public List<ResponseModel> Results { get; set; }

    }
}
