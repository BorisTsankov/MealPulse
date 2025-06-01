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

            var body = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
            new { role = "system", content = @"You are MealPulseBot, ..." },
            new { role = "user", content = prompt }
        },
                max_tokens = 300,
                temperature = 0.7
            };

            request.AddJsonBody(body);

            var response = await _client.ExecuteAsync(request); // ✅ USE THE MOCKABLE CLIENT

            if (!response.IsSuccessful)
            {
                return $"API request failed: {response.StatusCode} - {response.ErrorMessage ?? "Unknown error"}";
            }

            try
            {
                dynamic result = JsonConvert.DeserializeObject(response.Content);
                return result?.choices[0]?.message?.content ?? " No answer returned.";
            }
            catch (Exception ex)
            {
                return $"Error parsing response: {ex.Message}";
            }
        }

    }
}
