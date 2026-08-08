using garagesales.Models;

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
    }
}
