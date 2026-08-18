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

        // =====================================================
        // Games
        // =====================================================

        public async Task<List<Game>> GetGamesAsync()
        {
            var result =
                await _httpClient.GetFromJsonAsync<List<Game>>(
                    "api/GameApi");

            return result ?? new List<Game>();
        }

        public async Task<List<Game>> SearchGamesAsync(
            string text)
        {
            string encodedText =
                Uri.EscapeDataString(text.Trim());

            var result =
                await _httpClient.GetFromJsonAsync<List<Game>>(
                    $"api/GameApi/search?text={encodedText}");

            return result ?? new List<Game>();
        }

        public async Task<bool> AddGameAsync(Game game)
        {
            var response =
                await _httpClient.PostAsJsonAsync(
                    "api/GameApi",
                    game);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateGameAsync(Game game)
        {
            if (game.GameID == null)
                return false;

            var response =
                await _httpClient.PutAsJsonAsync(
                    $"api/GameApi/{game.GameID}",
                    game);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteGameAsync(int id)
        {
            var response =
                await _httpClient.DeleteAsync(
                    $"api/GameApi/{id}");

            return response.IsSuccessStatusCode;
        }


        // =====================================================
        // Amusement Parks
        // =====================================================

        public async Task<List<string>>
            GetAmusementParksAsync()
        {
            var result =
                await _httpClient.GetFromJsonAsync<List<string>>(
                    "api/GameApi/parks");

            return result ?? new List<string>();
        }

        public async Task<bool>
            RenameAmusementParkAsync(
                string oldName,
                string newName)
        {
            var response =
                await _httpClient.PutAsJsonAsync(
                    "api/GameApi/parks",
                    new
                    {
                        oldName,
                        newName
                    });

            return response.IsSuccessStatusCode;
        }

        public async Task<bool>
            DeleteAmusementParkAsync(
                string name)
        {
            string encodedName =
                Uri.EscapeDataString(name);

            var response =
                await _httpClient.DeleteAsync(
                    $"api/GameApi/parks?name={encodedName}");

            return response.IsSuccessStatusCode;
        }
    }
}