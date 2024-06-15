using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using resume.ResultModels;
using resume.Models;
using resume.WebSentModel;
using resume.Services;

namespace resume.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController
    {
        private readonly EmailService _emailService;
        public EmailController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("addUserEmail")]
        public async Task<int> AddUserEmailAsync(WebSentUserEmail webSentUserEmail)
        {
            return await _emailService.AddUserEmailAsync(webSentUserEmail.UserId, webSentUserEmail.EmailAddress, webSentUserEmail.EmailPassword);
        }

        [HttpPost("getUserEmail")]
        public async Task<IEnumerable<UserEmail>> GetUserEmailAsync(WebSentUserId webSentUserId)
        {
            return await _emailService.GetUserEmailsAsync(webSentUserId.Id);
        }
        [HttpPatch("updateEmailStatus")]
        public async Task<bool> UpdateIsSyncEnabledAsync(WebSentUserEmail webSentUserEmail)
        {
            return await _emailService.UpdateIsSyncEnabledAsync(webSentUserEmail.UserId, webSentUserEmail.IsSyncEnabled);
        }
        [HttpDelete("deleteUserEmail")]
        public async Task DeleteUserEmailAsync(int userEmailId)
        {
            await _emailService.DeleteUserEmailAsync(userEmailId);
        }

        [HttpGet("CountLastWeek")]
        public async Task<Dictionary<DateTime, int>> GetEmailCountForLastWeekAsync(int userId)
        {
            return await _emailService.GetEmailCountForLastWeekAsync(userId);
        }


        [HttpPost("getEmailMessages")]
        // 查看用户该邮箱的所有邮件
        public async Task<IEnumerable<EmailMessage>> GetEmailMessagesAsync(int userEmailId)
        {
            return await _emailService.GetEmailMessagesAsync(userEmailId);
        }

        [HttpPost("getEmailMessage")]
        // 查看某一特定邮件内容
        public async Task<EmailMessage?> GetEmailMessageAsync(int emailMessageId)
        {
            return await _emailService.GetEmailMessageAsync(emailMessageId);
        }

        [HttpPost("getEmailSummaries")]
        public async Task<List<EmailSummary>> GetUserEmailSummariesAsync(int userId)
        {
            return await _emailService.GetUserEmailSummariesAsync(userId);
        }
    }
}
