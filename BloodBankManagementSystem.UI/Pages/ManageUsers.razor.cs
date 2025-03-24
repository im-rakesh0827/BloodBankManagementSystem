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
        private bool ShowAddUserModal = false;
        private User SelectedUser;
        protected override async Task OnInitializedAsync()
        {
            await FetchAllUsersList();
        }
    
    private async Task FetchAllUsersList()
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
                //var responseText = await response.Content.ReadAsStringAsync();
                //Console.WriteLine($"Raw API Response: {responseText}"); // Logs raw response for debugging
                UsersList = await response.Content.ReadFromJsonAsync<List<User>>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching users: {ex.Message}");
                Message = "An error occurred while fetching users.";
            }
    }


    private void OpenAddUserModal()
    {
        SelectedUser = new User();
        ShowAddUserModal = true;
    }

    private void CloseAddUserModal()
    {
        ShowAddUserModal = false;
        SelectedUser = null;
    }

    private async Task RefreshUserList()
    {
        await FetchAllUsersList();
        StateHasChanged(); // Force UI update
    }


    private void EditUser(User user)
    {
        SelectedUser = user;
        ShowAddUserModal = true;
    }


    private async Task DeleteUser(int userId)
        {
          //    bool confirmDelete = await JS.InvokeAsync<bool>("confirm", "Are you sure you want to delete this patient?");
          bool confirmDelete = true;
          if (confirmDelete)
          {
               var response = await Http.DeleteAsync($"api/users/{userId}");
               if (response.IsSuccessStatusCode)
               {
                    UsersList.Remove(UsersList.FirstOrDefault(p => p.Id == userId));
                    StateHasChanged();
               }
               else
               {
                    var errorText = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {errorText}");
               }
          }
       }
}

    }