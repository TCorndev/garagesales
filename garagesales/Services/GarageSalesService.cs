using garagesales.Models;
using garagesales.Models.dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace garagesales.Services
{
    public class GarageSalesService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GarageSalesService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            var request = httpContextAccessor.HttpContext!.Request;
            _httpClient.BaseAddress = new Uri($"{request.Scheme}://{request.Host}/");
        }
        public async Task<List<GarageSale>> GetGarageSales(SaleFilterDto dto)
        {
            var query = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(dto.City))
            {
                query["City"] = dto.City;
            }
            if (!string.IsNullOrWhiteSpace(dto.State))
            {
                query["State"] = dto.State;
            }
            if (dto.StartDate.HasValue)
            {
                query["StartDate"] = dto.StartDate.Value.ToString("yyyy-MM-dd");
            }
            var url = QueryHelpers.AddQueryString("api/GarageSales", query);
            return await _httpClient.GetFromJsonAsync<List<GarageSale>>(url) ?? new List<GarageSale>();
        }
        public async Task<FilterModel> GetFilters()
        {
            return await _httpClient.GetFromJsonAsync<FilterModel>("api/GarageSales/filters") ?? new FilterModel();
        }
        public async Task<GarageSale> GetGarageSale(int id)
        {
            return await _httpClient.GetFromJsonAsync<GarageSale>($"api/GarageSales/{id}") ?? new GarageSale();
        }
        public async Task<List<GarageSale>> GetUserGarageSales(string id)
        {
            return await _httpClient.GetFromJsonAsync<List<GarageSale>>($"api/GarageSales/User/{id}") ?? new List<GarageSale>();
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
