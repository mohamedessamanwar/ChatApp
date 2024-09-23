namespace Chating.Models
{
    public class OfflineMessage
    {
       
         public int Id { get; set; }
         public string UserId { get; set; }
         public string Sender { get; set; }
         public string Message { get; set; }
         public DateTime Timestamp { get; set; }
        
    }
}
