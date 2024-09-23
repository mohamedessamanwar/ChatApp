namespace Chating.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public string recever { get; set; }
        public string Recipient { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
