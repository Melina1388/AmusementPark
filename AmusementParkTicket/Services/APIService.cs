using System.Net.Http.Json;
using AmusementPark.Domain.Entities;

namespace AmusementPark.Web.Services
{
    public class APIService
    {
        private readonly HttpClient _httpClient;

        public APIService(HttpClient httpClient)
        {
            _httpClient = httpClient;


        }

        public async Task<List<Game>> GetGamesAsync()
        {

            var result =
                await _httpClient.GetFromJsonAsync<List<Game>>("api/GameApi");

            return result ?? new List<Game>();
        }
        public async Task<List<Game>> SearchGamesAsync(string text)
        {
            string encodedText = Uri.EscapeDataString(text.Trim());

            var result =
                await _httpClient.GetFromJsonAsync<List<Game>>(
                    $"api/GameApi/search?text={encodedText}");

            return result ?? new List<Game>();
        }
    }
}