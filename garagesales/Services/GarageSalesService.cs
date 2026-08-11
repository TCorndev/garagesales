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

        //http client's base address is set in the constructor
        public GarageSalesService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            var request = httpContextAccessor.HttpContext!.Request;
            _httpClient.BaseAddress = new Uri($"{request.Scheme}://{request.Host}/");
        }
        //Creates the query based on the parameters in the dto then calls the api
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
        //Gets filters, returning an empty model if none are found
        public async Task<FilterModel> GetFilters()
        {
            return await _httpClient.GetFromJsonAsync<FilterModel>("api/GarageSales/filters") ?? new FilterModel();
        }
        //Gets a garage sale, retrning an empty garage sale if none are found
        public async Task<GarageSale> GetGarageSale(int id)
        {
            return await _httpClient.GetFromJsonAsync<GarageSale>($"api/GarageSales/{id}") ?? new GarageSale();
        }
        //Gets sales related to the user, used for deleting sales when a user is deleted
        public async Task<List<GarageSale>> GetUserGarageSales(string id)
        {
            return await _httpClient.GetFromJsonAsync<List<GarageSale>>($"api/GarageSales/User/{id}") ?? new List<GarageSale>();
        }
        //Creates a new garage sale and returns the Id for redirecting to that sale's page.
        public async Task<int> CreateGarageSale(GarageSaleDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/GarageSales", dto);
            response.EnsureSuccessStatusCode();
            var sale = await response.Content.ReadFromJsonAsync<GarageSale>();
            return sale.Id;
        }
        //Deletes the garage sale
        public async Task DeleteGarageSale(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/GarageSales/{id}");
            response.EnsureSuccessStatusCode();
        }
        //Creates a garage sale item
        public async Task CreateGarageSaleItem(GarageSaleItemDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/GarageSaleItems", dto);
            response.EnsureSuccessStatusCode();
        }
        //Deletes a garage sale item
        public async Task DeleteGarageSaleItem(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/GarageSaleItems/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
