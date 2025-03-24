using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using BloodBankManagementSystem.Core.Models;
using Microsoft.AspNetCore.Components;
namespace BloodBankManagementSystem.UI.Pages
{
    public partial class ManageUsers
    {
        private List<User> UsersList { get; set; } = new();
        private string Message {get; set;} = string.Empty;
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var response = await Http.GetAsync("api/Users/allUsers");
                if (!response.IsSuccessStatusCode)
                {
                    var errorText = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Error: {response.StatusCode}, Response: {errorText}");
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        Message = "No users found.";
                    }
                    else
                    {
                        Message = $"Error fetching users: {errorText}";
                    }
                    return;
                }
                var responseText = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Raw API Response: {responseText}"); // Logs raw response for debugging
                UsersList = await response.Content.ReadFromJsonAsync<List<User>>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching users: {ex.Message}");
                Message = "An error occurred while fetching users.";
            }
        }

    }
}
