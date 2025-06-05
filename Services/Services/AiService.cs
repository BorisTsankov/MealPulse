using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using Services.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Services.Services
{
    public class AiService : IAiService
    {
        private readonly string _apiKey;
        private readonly string _apiUrl = "https://api.openai.com/v1/chat/completions";
        private readonly IAiHttpClient _client;

        public AiService(IConfiguration config, IAiHttpClient client)
        {
            _apiKey = config["OpenAI:ApiKey"];
            _client = client;
        }

        public async Task<string> AskAsync(string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
                return "Please ask a non-empty question.";

            var request = new RestRequest(_apiUrl, Method.Post);
            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            request.AddHeader("Content-Type", "application/json");

            var systemMessage = @"
You are MealPulseBot, a helpful assistant for a nutrition tracking app.

Your job is to extract food log entries from user input.

📌 If the user says they ate something (e.g. 'I had two eggs for breakfast'), you MUST reply ONLY with a JSON object in the exact format:

{
  ""foodName"": ""banana"",
  ""quantity"": 100,
  ""unit"": ""g"",
  ""mealType"": ""breakfast"",
  ""calories"": 89,
  ""protein"": 1.1,
  ""fat"": 0.3,
  ""carbohydrates"": 22.8,
  ""sugars"": 12.2,
  ""fiber"": 2.6,
  ""sodium"": 1,
  ""potassium"": 358,
  ""iron"": 0.3,
  ""calcium"": 5
}

❗ ABSOLUTELY NO TEXT OUTSIDE THE JSON. NO EXPLANATIONS. JUST THE JSON BLOCK.

📎 If you need clarification (e.g. vague food), ask a short follow-up question.

💬 For general questions not related to food logging, reply as a normal assistant.";

            var body = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "system", content = systemMessage },
                    new { role = "user", content = prompt }
                },
                max_tokens = 300,
                temperature = 0.3
            };

            request.AddJsonBody(body);

            var response = await _client.ExecuteAsync(request); // USE THE MOCKABLE CLIENT

            if (!response.IsSuccessful)
            {
                return $"API request failed: {response.StatusCode} - {response.ErrorMessage ?? "Unknown error"}";
            }

            try
            {
                dynamic result = JsonConvert.DeserializeObject(response.Content);
                return result?.choices[0]?.message?.content ?? "No answer returned.";
            }
            catch (Exception ex)
            {
                return $"Error parsing response: {ex.Message}";
            }
        }
    }
}
