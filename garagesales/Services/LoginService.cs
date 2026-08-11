using garagesales.Models.dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace garagesales.Services
{
    public class LoginService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        //http client's base address is set in the constructor
        public LoginService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            var request = httpContextAccessor.HttpContext!.Request;
            _httpClient.BaseAddress = new Uri($"{request.Scheme}://{request.Host}/");
        }

        //Attempts to login using the parameters in the dto
        public async Task<LoginResponseDto> Login(LoginDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/User", dto);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            return await response.Content.ReadFromJsonAsync<LoginResponseDto>();

        }

        //Attempts to create a new user using the parameters in the dto
        public async Task RegisterUser(CreateUserDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/User/register", dto);
            response.EnsureSuccessStatusCode();
        }

        //Gets a list of user(for Admins)
        public async Task<List<UserDto>> GetUsers()
        {
            AddCookie();
            return await _httpClient.GetFromJsonAsync<List<UserDto>>("api/User") ?? new List<UserDto>();
        }

        //Updates a user's role(for Admins)
        public async Task UpdateRole(UserDto dto)
        {
            AddCookie();
            var response = await _httpClient.PutAsJsonAsync("api/User", dto);
            response.EnsureSuccessStatusCode();
        }
        //Deletes a user(for Admin)
        public async Task DeleteUser(string id)
        {
            AddCookie();
            var response = await _httpClient.DeleteAsync($"api/User/{id}");
            response.EnsureSuccessStatusCode();
        }
        //This forwards the authentication cookie to the API, it's necessary when using the Admin functions when editing users
        public void AddCookie()
        {
            _httpClient.DefaultRequestHeaders.Remove("Cookie");
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Cookie", _httpContextAccessor.HttpContext?.Request.Headers.Cookie.ToString());
        }
    }
}
