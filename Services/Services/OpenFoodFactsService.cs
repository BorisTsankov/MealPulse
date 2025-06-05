using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Models;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using DTOs;
using DTOs.DTOs;
using Services.Services.Interfaces;

namespace Services.Services
{
    public class OpenFoodFactsService : IOpenFoodFactsService
    {
        private readonly HttpClient _httpClient;

        public OpenFoodFactsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<FoodItemDto?> GetFoodItemByBarcodeAsync(string barcode)
        {
            var response = await _httpClient.GetAsync($"https://world.openfoodfacts.org/api/v0/product/{barcode}.json");

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);

            if (json["status"]?.ToString() != "1")
                return null;

            var product = json["product"]!;
            return new FoodItemDto
            {
                Name = product["product_name"]?.ToString() ?? "Unknown",
                Brand = product["brands"]?.ToString(),
                Description = product["generic_name"]?.ToString(),
                Unit = "g",
                Barcode = barcode,
                Source = "OpenFoodFacts",
                ImageUrl = product["image_front_url"]?.ToString(),
                Calories = (decimal)(product["nutriments"]?["energy-kcal_100g"] ?? 0),
                Protein = (decimal)(product["nutriments"]?["proteins_100g"] ?? 0),
                Fat = (decimal)(product["nutriments"]?["fat_100g"] ?? 0),
                Carbohydrates = (decimal)(product["nutriments"]?["carbohydrates_100g"] ?? 0),
                Sugars = (decimal)(product["nutriments"]?["sugars_100g"] ?? 0),
                Fiber = (decimal)(product["nutriments"]?["fiber_100g"] ?? 0),
                Sodium = (decimal?)(product["nutriments"]?["sodium_100g"] ?? null),
                Potassium = (decimal?)(product["nutriments"]?["potassium_100g"] ?? null),
                Iron = (decimal?)(product["nutriments"]?["iron_100g"] ?? null),
                Calcium = (decimal?)(product["nutriments"]?["calcium_100g"] ?? null),
                CreatedAt = DateTime.UtcNow
            };
        }

        public async Task<List<FoodItemDto>> SearchByNameAsync(string term)
        {
            var url = $"https://world.openfoodfacts.org/cgi/search.pl?search_terms={Uri.EscapeDataString(term)}&search_simple=1&action=process&json=1&page_size=5";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return new();

            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);
            var products = json["products"]?.ToArray();

            if (products == null)
                return new();

            var results = new List<FoodItemDto>();
            foreach (var p in products)
            {
                var nutriments = p["nutriments"];

                //  Skip if any required values are null or missing
                if (nutriments?["energy-kcal_100g"] == null ||
                    nutriments["proteins_100g"] == null ||
                    nutriments["fat_100g"] == null ||
                    nutriments["carbohydrates_100g"] == null ||
                    nutriments["sugars_100g"] == null ||
                    nutriments["fiber_100g"] == null)
                    continue;

                results.Add(new FoodItemDto
                {
                    Name = p["product_name"]?.ToString() ?? "Unknown",
                    Brand = p["brands"]?.ToString(),
                    Description = p["generic_name"]?.ToString(),
                    Unit = "g",
                    Barcode = p["code"]?.ToString(),
                    Source = "OpenFoodFacts",
                    ImageUrl = p["image_front_url"]?.ToString(),
                    Calories = (decimal)nutriments["energy-kcal_100g"],
                    Protein = (decimal)nutriments["proteins_100g"],
                    Fat = (decimal)nutriments["fat_100g"],
                    Carbohydrates = (decimal)nutriments["carbohydrates_100g"],
                    Sugars = (decimal)nutriments["sugars_100g"],
                    Fiber = (decimal)nutriments["fiber_100g"],
                    Sodium = (decimal?)nutriments["sodium_100g"],
                    Potassium = (decimal?)nutriments["potassium_100g"],
                    Iron = (decimal?)nutriments["iron_100g"],
                    Calcium = (decimal?)nutriments["calcium_100g"],
                    CreatedAt = DateTime.UtcNow
                });
            }


            return results;
        }

    }
}