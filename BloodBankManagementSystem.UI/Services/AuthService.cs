using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BloodBankManagementSystem.Core.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;
using Blazored.LocalStorage;
using BloodBankManagementSystem.UI.Services;




// namespace BloodBankManagementSystem.UI.Services
// {
//     public class AuthService
//     {
//         private readonly HttpClient _httpClient;
//         private readonly ILocalStorageService _localStorage;
//         private readonly AuthenticationStateProvider _authStateProvider;
//         private readonly NavigationManager _navigation;

//         public AuthService(HttpClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider, NavigationManager navigation)
//         {
//             _httpClient = httpClient;
//             _localStorage = localStorage;
//             _authStateProvider = authStateProvider;
//             _navigation = navigation;
//         }

//         public async Task<bool> Login(LoginRequest loginRequest)
//         {
//             Console.WriteLine($"🔹 AuthService: Attempting Login for {loginRequest.Email}");

//             var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginRequest);

//             if (!response.IsSuccessStatusCode)
//             {
//                 return false;
//             }

//             var result = await response.Content.ReadFromJsonAsync<TokenResponse>();

//             if (result == null || string.IsNullOrEmpty(result.Token))
//             {
//                 return false;
//             }

//             await _localStorage.SetItemAsync("authToken", result.Token);
//             _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Token);
//             ((JwtAuthenticationStateProvider)_authStateProvider).MarkUserAsAuthenticated(result.Token);

//             _navigation.NavigateTo("/", forceLoad: true);
//             return true;
//         }

//         public async Task Logout()
//         {
//             await _localStorage.RemoveItemAsync("authToken");
//             _httpClient.DefaultRequestHeaders.Authorization = null;
//             ((JwtAuthenticationStateProvider)_authStateProvider).MarkUserAsLoggedOut();
//             _navigation.NavigateTo("/login", forceLoad: true);
//         }
//     }
// }




namespace BloodBankManagementSystem.UI.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly JwtAuthenticationStateProvider _authStateProvider;
        private readonly NavigationManager _navigation;

        public AuthService(HttpClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider, NavigationManager navigation)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authStateProvider = (JwtAuthenticationStateProvider)authStateProvider;
            _navigation = navigation;
        }

        public async Task<bool> Login(LoginRequest loginRequest)
        {
            Console.WriteLine($"🔹 AuthService: Attempting login for {loginRequest.Email}");

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginRequest);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"❌ Login failed. API Response: {response.StatusCode}");
                    return false;
                }

                var result = await response.Content.ReadFromJsonAsync<TokenResponse>();

                if (result == null || string.IsNullOrEmpty(result.Token))
                {
                    Console.WriteLine("❌ Token is null or empty.");
                    return false;
                }

                Console.WriteLine($"✅ Received Token: {result.Token}");

                // Store token and update authentication state
                await _localStorage.SetItemAsync("authToken", result.Token);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Token);
                _authStateProvider.MarkUserAsAuthenticated(result.Token);

                Console.WriteLine("✅ User authenticated successfully. Redirecting...");
                _navigation.NavigateTo("/", forceLoad: true);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exception in AuthService Login: {ex.Message}");
                return false;
            }
        }

        public async Task Logout()
        {
            try
            {
                Console.WriteLine("🔹 Logging out user...");

                await _localStorage.RemoveItemAsync("authToken");
                _httpClient.DefaultRequestHeaders.Authorization = null;
                _authStateProvider.MarkUserAsLoggedOut();

                Console.WriteLine("✅ User logged out successfully. Redirecting to login...");
                _navigation.NavigateTo("/login", forceLoad: true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exception in Logout: {ex.Message}");
            }
        }
    }
}
