namespace Trivial.DataModels
{
    public class RequestModel
    {
        public RequestModel(string amount, string category,
            string difficulty, string type)
        {
            Amount = amount;
            Category = category;
            Difficulty = difficulty;
            Type = type;
        }
        public string Amount { get; set; }
        public string Category { get; set; }
        public string Difficulty { get; set; }
        public string Type { get; set; }
    }
}
