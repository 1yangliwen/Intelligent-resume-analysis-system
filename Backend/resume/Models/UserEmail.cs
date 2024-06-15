namespace resume.Models
{
    public class UserEmail
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        public string? EmailAddress { get; set; }
        public string? EmailPassword { get; set; } // Note: It's recommended to store passwords securely (e.g., hashing).
        public bool IsSyncEnabled { get; set; } = true; // Default to true

        public User User { get; set; }
        public ICollection<EmailMessage> EmailMessages { get; set; }
    }
}
