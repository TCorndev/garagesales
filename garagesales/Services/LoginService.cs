using garagesales.Models.dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace garagesales.Services
{
    public class LoginService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAcessor;

        public LoginService(HttpClient httpClient, IHttpContextAccessor httpContextAcessor)
        {
            _httpClient = httpClient;
            _httpContextAcessor = httpContextAcessor;
        }

        public async Task<LoginResponseDto> Login(LoginDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/User", dto);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            return await response.Content.ReadFromJsonAsync<LoginResponseDto>();

        }

        public async Task RegisterUser(CreateUserDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/User/register", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<UserDto>> GetUsers()
        {
            AddCookie();
            return await _httpClient.GetFromJsonAsync<List<UserDto>>("api/User") ?? new List<UserDto>();
        }

        public async Task UpdateRole(UserDto dto)
        {
            AddCookie();
            var response = await _httpClient.PutAsJsonAsync("api/User", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteUser(string id)
        {
            AddCookie();
            var response = await _httpClient.DeleteAsync($"api/User/{id}");
            response.EnsureSuccessStatusCode();
        }

        public void AddCookie()
        {
            _httpClient.DefaultRequestHeaders.Remove("Cookie");
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Cookie", _httpContextAcessor.HttpContext?.Request.Headers.Cookie.ToString());
        }
    }
}
