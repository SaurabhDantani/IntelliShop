using Ecommerce.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly CerebrasChatService _chatService;

        public ChatController(CerebrasChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] ChatRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Message)) return BadRequest();

            var response = await _chatService.GetChatResponseAsync(request.Message);
            return Ok(new { reply = response });
        }
    }

    public class ChatRequest
    {
        public string Message { get; set; }
    }
}
