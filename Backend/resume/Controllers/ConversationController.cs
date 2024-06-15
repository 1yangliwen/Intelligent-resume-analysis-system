using Microsoft.AspNetCore.Mvc;
using resume.ResultModels;
using resume.Models;
using resume.Services;
using resume.WebSentModel;

namespace resume.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : Controller
    {
        private readonly ConversationService _conversationService;

        public ConversationController(ConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        [HttpPost("createConversatoin")]
        public int CreateConversation(int resumeId)
        {
            return _conversationService.CreateConversationAsync(resumeId).Result;
        }

        [HttpPost("addMessage")]
        public int AddMessage(int conversationId, string question, string message)
        {
            return _conversationService.AddMessageAsync(conversationId, question, message).Result;
        }

        [HttpGet("getConversation")]
        public List<ConversationMessage> GetConversation(int conversationId)
        {
            return _conversationService.GetMessagesByConversationIdAsync(conversationId).Result;
        }
    }
}
