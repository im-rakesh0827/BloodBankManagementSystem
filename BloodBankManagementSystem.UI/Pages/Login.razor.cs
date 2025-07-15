using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Blazored.LocalStorage;
using BloodBankManagementSystem.Core.Models;


namespace BloodBankManagementSystem.UI.Pages
{
    public partial class Login
    {
        private LoginModel loginModel = new LoginModel();
        private string ErrorMessage { get; set; } = string.Empty;
        private bool IsLoading { get; set; } = false;

        // [Inject] private HttpClient Http { get; set; } = default!;
        // [Inject] private ILocalStorageService LocalStorage { get; set; } = default!;
        // [Inject] private JwtAuthStateProvider AuthProvider { get; set; } = default!;
        // [Inject] private NavigationManager Navigation { get; set; } = default!;

        private async Task HandleLogin()
        {
            try
            {
                IsLoading = true;
                await Task.Delay(500);
                var response = await Http.PostAsJsonAsync("api/auth/login", loginModel);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<TokenResponse>();

                    if (result != null && !string.IsNullOrWhiteSpace(result.Token))
                    {
                        await LocalStorage.SetItemAsync("authToken", result.Token);
                        AuthProvider.NotifyUserAuthentication(result.Token);
                        Navigation.NavigateTo("/");
                    }
                    else
                    {
                        ErrorMessage = "Login failed: Invalid token received.";
                    }
                }
                else
                {
                    ErrorMessage = "Login failed: Invalid email or password.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Login exception: " + ex.Message);
                ErrorMessage = "An unexpected error occurred during login.";
            }
            IsLoading = false;
        }

        private void NavigateToRegister()
    {
        Navigation.NavigateTo("/register");
    }

    private void NavigateToForgotPassword()
    {
        Navigation.NavigateTo("/forgot-password");
    }
    }
}
