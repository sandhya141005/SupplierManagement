namespace SupplierManagement.Api.AI
{
    public class GroqRequest
    {
        public string model { get; set; }
        public List<GroqMessage> messages { get; set; }
        public int max_tokens { get; set; } = 500;
    }
    public class GroqMessage
    {
        public string role { get; set; }
        public string content { get; set; }
    }
    public class GroqResponse
    {
        public List<GroqChoice> choices { get; set; }
    }
    public class GroqChoice
    {
        public GroqMessage message { get; set; }
    }
}
