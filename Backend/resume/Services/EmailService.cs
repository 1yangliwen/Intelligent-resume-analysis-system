using Microsoft.EntityFrameworkCore;
using resume.Models;
using resume.WebSentModel;

namespace resume.Services
{
    public class EmailService
    {
        private readonly MyDbContext _dbContext;

        public EmailService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddUserEmailAsync(int userId, string emailAddress, string emailPassword)
        {
            var user = _dbContext.Users.Find(userId);
            var userEmail = new UserEmail
            {
                UserId = userId,
                EmailAddress = emailAddress,
                EmailPassword = emailPassword,
                User = user
            };

            _dbContext.UserEmails.Add(userEmail);
            await _dbContext.SaveChangesAsync();

            return userEmail.ID;
        }

        public async Task<IEnumerable<UserEmail>> GetUserEmailsAsync(int userId)
        {
            return await _dbContext.UserEmails.Where(u => u.UserId == userId).ToListAsync();
        }

        public async Task<bool> UpdateIsSyncEnabledAsync(int userEmailId, bool isSyncEnabled)
        {
            var userEmail = await _dbContext.UserEmails.FindAsync(userEmailId);
            if (userEmail == null) return false;

            userEmail.IsSyncEnabled = isSyncEnabled;
            _dbContext.UserEmails.Update(userEmail);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<int> AddEmailMessageAsync(int userEmailId, string? sender, string? subject, string? body, string? attachment, DateTime receivedDate)
        {
            var userEmail = await _dbContext.UserEmails.FindAsync(userEmailId);
            var emailMessage = new EmailMessage
            {
                UserEmailId = userEmailId,
                Sender = sender,
                Subject = subject,
                Body = body,
                Attachment = attachment,
                ReceivedDate = receivedDate,
                UserEmail = userEmail
            };

            _dbContext.EmailMessages.Add(emailMessage);
            userEmail.EmailMessages.Add(emailMessage);
            await _dbContext.SaveChangesAsync();

            return emailMessage.ID;
        }

        public async Task<IEnumerable<EmailMessage>> GetEmailMessagesAsync(int userEmailId)
        {
            return await _dbContext.EmailMessages.Where(e => e.UserEmailId == userEmailId).ToListAsync();
        }

        public async Task<EmailMessage?> GetEmailMessageAsync(int emailMessageId)
        {
            return await _dbContext.EmailMessages.FirstOrDefaultAsync(e => e.ID == emailMessageId);
        }

        public async Task DeleteEmailMessageAsync(int emailMessageId)
        {
            var emailMessage = await _dbContext.EmailMessages.FirstOrDefaultAsync(e => e.ID == emailMessageId);
            if (emailMessage != null)
            {
                _dbContext.EmailMessages.Remove(emailMessage);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteUserEmailAsync(int userEmailId)
        {
            var userEmail = await _dbContext.UserEmails.FirstOrDefaultAsync(u => u.ID == userEmailId);
            if (userEmail != null)
            {
                _dbContext.UserEmails.Remove(userEmail);
                await _dbContext.SaveChangesAsync();
            }
        }

        // 分析完简历后更新邮件信息的简历ID
        public async Task UpdateEmailMessageAsync(int emailMessageId, int ResumeId)
        {
            var emailMessage = await _dbContext.EmailMessages.FirstOrDefaultAsync(e => e.ID == emailMessageId);
            var resume = await _dbContext.Resumes.FirstOrDefaultAsync(r => r.ID == ResumeId);
            if (emailMessage != null)
            {
                emailMessage.ResumeId = ResumeId;
                emailMessage.Resume = resume;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<Dictionary<DateTime, int>> GetEmailCountForLastWeekAsync(int userId)
        {
            var endDate = DateTime.UtcNow;
            var startDate = endDate.AddDays(-7);

            var emailCounts = await _dbContext.UserEmails
            .Where(ue => ue.UserId == userId)
            .Join(
                _dbContext.EmailMessages,
                ue => ue.ID,
                em => em.UserEmailId,
                (ue, em) => em
            )
            .Where(em => em.ReceivedDate >= startDate && em.ReceivedDate <= endDate)
            .GroupBy(em => em.ReceivedDate.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .ToListAsync();

            var result = new Dictionary<DateTime, int>();

            for (int i = 0; i < 7; i++)
            {
                var date = startDate.Date.AddDays(i);
                var count = emailCounts.FirstOrDefault(x => x.Date == date)?.Count ?? 0;
                result[date] = count;
            }


            return result;
        }

        public async Task<List<EmailSummary>> GetUserEmailSummariesAsync(int userId)
        {
            var emailSummaries = await _dbContext.UserEmails
                .Where(ue => ue.UserId == userId)
                .Join(
                    _dbContext.EmailMessages,
                    ue => ue.ID,
                    em => em.UserEmailId,
                    (ue, em) => em
                ).Include(ue => ue.Resume)
                .ThenInclude(r => r.Applicant)
                .Select(em => new EmailSummary
                {
                    ReceivingEmail = em.UserEmail.EmailAddress,
                    SendingEmail = em.Sender,
                    Subject = em.Subject,   
                    ApplicantName = em.Resume.Applicant.Name,
                    DesiredPosition = em.Resume.Applicant.JobIntention,
                    ReceivedDate = em.ReceivedDate
                })
                .ToListAsync();

            return emailSummaries;
        }
    }
}
