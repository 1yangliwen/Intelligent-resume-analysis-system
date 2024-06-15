namespace resume.Models
{
    public class Conversation
    {
        public int Id { get; set; }
        public int ResumeId { get; set; }
        public Resume Resume { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<ConversationMessage> Messages { get; set; }
    }

    public class ConversationMessage
    {
        public int Id { get; set; }
        public int ConversationId { get; set; }
        public string Question { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }

        public Conversation Conversation { get; set; }
    }
}
