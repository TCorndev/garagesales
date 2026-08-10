using garagesales.Models.dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace garagesales.Services
{
    public class LoginService
    {
        private readonly HttpClient _httpClient;

        public LoginService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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

        public async Task RegisterUser(UserDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/User/register", dto);
            response.EnsureSuccessStatusCode();
        }

    }
}
