using Ecommerce.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace Ecommerce.Services
{
    public class CerebrasChatService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly IServiceScopeFactory _scopeFactory;

        public CerebrasChatService(HttpClient httpClient, IConfiguration config, IServiceScopeFactory scopeFactory)
        {
            _httpClient = httpClient;
            _config = config;
            _scopeFactory = scopeFactory;
        }

        public async Task<string> GetChatResponseAsync(string userMessage)
        {
            string apiKey = _config["Cerebras:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                return "AI Chat is currently unavailable. (Missing API Key)";
            }

            // Fetch products to give context to the AI
            string productContext = "";
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<EcommerceContext>();
                var products = await context.Products.Include(p => p.Category).ToListAsync();
                
                var sb = new StringBuilder();
                foreach (var p in products)
                {
                    sb.AppendLine($"- {p.Name} (Category: {p.Category?.Name ?? "General"}): ${p.Price}. Description: {p.Description}");
                }
                productContext = sb.ToString();
            }

            var systemPrompt = $@"You are a helpful and polite virtual shopping assistant for our E-commerce store. 
You ONLY recommend products from our store backend. If the user asks about something we don't sell, neatly decline.
Here are the products currently available in our system:
{productContext}";

            var requestBody = new
            {
                model = "llama3.1-8b", 
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = userMessage }
                },
                max_tokens = 250,
                temperature = 0.5
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.cerebras.ai/v1/chat/completions");
            request.Headers.Add("Authorization", $"Bearer {apiKey}");
            request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                
                using var jsonDoc = JsonDocument.Parse(responseContent);
                var answer = jsonDoc.RootElement
                                    .GetProperty("choices")[0]
                                    .GetProperty("message")
                                    .GetProperty("content")
                                    .GetString();
                return answer ?? "Sorry, I couldn't understand that.";
            }

            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine(error);
            return "I apologize, but I am having trouble connecting to my brain right now.";
        }
    }
}
