using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using resume.Models;
using resume.Others;
using resume.ResultModels;

namespace resume.Services
{
    public class ConversationService
    {
        private readonly MyDbContext _dbContext;
        public ConversationService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateConversationAsync(int resumeId)
        {
            var resume = await _dbContext.Resumes.FindAsync(resumeId);
            if (resume == null)
            {
                throw new Exception("Resume not found");
            }
            var conversation = new Conversation
            {
                ResumeId = resumeId,
                Resume = resume,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Conversations.Add(conversation);
            await _dbContext.SaveChangesAsync();

            return conversation.Id;
        }

        public async Task<int> AddMessageAsync(int conversationId, string question, string message)
        {
            var conversationMessage = new ConversationMessage
            {
                ConversationId = conversationId,
                Question = question,
                Message = message,
                Timestamp = DateTime.UtcNow
            };

            _dbContext.ConversationMessages.Add(conversationMessage);
            var conversation = await _dbContext.Conversations.FindAsync(conversationId);
            conversation.Messages.Add(conversationMessage);
            await _dbContext.SaveChangesAsync();

            return conversationMessage.Id;
        }

        public async Task<List<ConversationMessage>> GetMessagesByConversationIdAsync(int conversationId)
        {
            return await _dbContext.ConversationMessages
                .Where(cm => cm.ConversationId == conversationId)
                .ToListAsync();
        }
    }
}
