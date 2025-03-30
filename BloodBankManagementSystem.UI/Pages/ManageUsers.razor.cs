using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BloodBankManagementSystem.Core.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using Microsoft.JSInterop;
namespace BloodBankManagementSystem.UI.Pages
{
    public partial class ManageUsers
    {
        private List<User> AllUsersList { get; set; } = new();
        private List<User> FilteredUsersList { get; set; } = new();

        private string Message {get; set;} = string.Empty;
        private bool IsCreateUpdatePopup {get; set;} = false;
        private User SelectedUser;
        private bool IsShowActiveOnly{get; set;} = true;
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
            AllUsersList = await response.Content.ReadFromJsonAsync<List<User>>();
            ApplyFilterUser();
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
        IsCreateUpdatePopup = true;
    }

    private void CloseAddUserModal()
    {
        IsCreateUpdatePopup = false;
        SelectedUser = null;
    }

    private async Task RefreshUserList()
    {
        await FetchAllUsersList();
        ApplyFilterUser();
        StateHasChanged();
    }


    private void EditUser(User user)
    {
        SelectedUser = user;
        IsCreateUpdatePopup = true;
    }

    private int userIdToDelete {get;set;} 
    private DotNetObjectReference<ManageUsers> objRef;
    protected override void OnInitialized()
    {
        objRef = DotNetObjectReference.Create(this);
    }
    private async Task DeleteUser(int userId)
    {
        userIdToDelete = userId;
        await JSRuntime.InvokeVoidAsync("ShowDeleteConfirmation", objRef, "User");
    }
    [JSInvokable]
    public async Task PerformDelete()
    {
        try
        {
            var response = await Http.DeleteAsync($"api/users/{userIdToDelete}");
            if (response.IsSuccessStatusCode)
            {
                AllUsersList.Remove(AllUsersList.FirstOrDefault(p => p.Id == userIdToDelete));
                await RefreshUserList();
            }
            else
            {
                var errorText = await response.Content.ReadAsStringAsync();
            }
        }
        catch (Exception ex)
        {
          throw ex;
        }
    }
    public void Dispose()
    {
        objRef?.Dispose();
    }

    public void ApplyFilterUser(){
        Console.WriteLine("Filter method");
        if(IsShowActiveOnly){
            FilteredUsersList = AllUsersList.Where(p => p.IsActive && p.IsAlive).ToList();
        }
        else{
            FilteredUsersList = AllUsersList.Where(p => p.IsActive).ToList();
        }
    }
}
}