namespace resume.Models
{
    public class EmailMessage
    {
        public int ID { get; set; }
        public int UserEmailId { get; set; }
        public int ResumeId { get; set; }
        public string? Sender { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public string? Attachment { get; set; }
        public DateTime ReceivedDate { get; set; }
        public UserEmail UserEmail { get; set; } // Add this navigation property
        public Resume Resume { get; set; } // Add this navigation property
    }
}
