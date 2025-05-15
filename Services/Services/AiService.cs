using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace Services.Services
{
    public class AiService
    {
        private readonly string _apiKey;
        private readonly string _apiUrl = "https://api.openai.com/v1/chat/completions";

        public AiService(IConfiguration configuration)
        {
            _apiKey = configuration["OpenAI:ApiKey"];

            if (string.IsNullOrWhiteSpace(_apiKey))
            {
                Console.WriteLine("OpenAI API key not loaded from configuration.");
            }
            else
            {
                Console.WriteLine(" OpenAI API key loaded successfully.");
            }
        }

        public async Task<string> AskAsync(string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
                return "Please ask a non-empty question.";

            var client = new RestClient();
            var request = new RestRequest(_apiUrl, Method.Post);
            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            request.AddHeader("Content-Type", "application/json");

            var body = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
            new { role = "system", content = "You are an assistant for MealPulse. Answer questions about calories and the MealPulse website features." },
            new { role = "user", content = prompt }
        },
                max_tokens = 300,
                temperature = 0.7
            };

            request.AddJsonBody(body);

            var response = await client.ExecuteAsync(request);

            Console.WriteLine("Response Status: " + response.StatusCode);
            Console.WriteLine("Response Content: " + response.Content);
            Console.WriteLine("Error Message: " + response.ErrorMessage);

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
