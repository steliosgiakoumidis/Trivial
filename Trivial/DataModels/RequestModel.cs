namespace Trivial.DataModels
{
    public class RequestModel
    {
        public RequestModel(string amount, string category,
            string difficulty)
        {
            Amount = amount;
            Category = category;
            Difficulty = difficulty;
        }
        public string Amount { get; set; }
        public string Category { get; set; }
        public string Difficulty { get; set; }
    }
}
