using garagesales.Models;
using garagesales.Models.dto;
using Microsoft.AspNetCore.Mvc;

namespace garagesales.Services
{
    public class GarageSalesService
    {
        private readonly HttpClient _httpClient;

        public GarageSalesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<GarageSale>> GetGarageSales()
        {
            return await _httpClient.GetFromJsonAsync<List<GarageSale>>("api/GarageSales") ?? new List<GarageSale>();
        }
        public async Task<GarageSale> GetGarageSale(int id)
        {
            return await _httpClient.GetFromJsonAsync<GarageSale>($"api/GarageSales/{id}") ?? new GarageSale();
        }
        public async Task<int> CreateGarageSale(GarageSaleDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/GarageSales", dto);
            response.EnsureSuccessStatusCode();
            var sale = await response.Content.ReadFromJsonAsync<GarageSale>();
            return sale.Id;
        }

        public async Task DeleteGarageSale(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/GarageSales/{id}");
            response.EnsureSuccessStatusCode();
        }

        //Might not be used, delete?
        public async Task<List<GarageSaleItem>> GetGarageSaleItems(int id)
        {
            return await _httpClient.GetFromJsonAsync<List<GarageSaleItem>>($"api/GarageSaleItems/{id}") ?? new List<GarageSaleItem>();
        }
        public async Task CreateGarageSaleItem(GarageSaleItemDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/GarageSaleItems", dto);
            response.EnsureSuccessStatusCode();
        }
        public async Task DeleteGarageSaleItem(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/GarageSaleItems/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
