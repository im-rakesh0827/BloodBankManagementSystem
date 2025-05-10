using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BloodBankManagementSystem.Core.Models;
using BloodBankManagementSystem.UI.Pages;
using BloodBankManagementSystem.UI.Helpers;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using Microsoft.JSInterop;
namespace BloodBankManagementSystem.UI.Pages.Users
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
            var url = $"{ServerConstants.APICallNames.GetAllUsers.GetStringValue()}";
            var response = await Http.GetAsync(url);
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
            var url = $"{ServerConstants.APICallNames.DeleteUser.GetStringValue()}{userIdToDelete}";
            var response = await Http.DeleteAsync(url);
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
        if(IsShowActiveOnly){
            FilteredUsersList = AllUsersList.Where(p => p.IsActive && p.IsAlive).ToList();
        }
        else{
            FilteredUsersList = AllUsersList.Where(p => p.IsActive).ToList();
        }
    }




    private bool ShowHistoryPopup = false;
    private List<UserHistory> UserHistoryList = new();

    private async Task ShowUsertHistory(User user)
    {
        SelectedUser = user;
        UserHistoryList = await GetUserHistory(user.Id); 
        ShowHistoryPopup = true;
    }

    private void CloseHistoryPopup()
    {
        ShowHistoryPopup = false;
    }

private async Task<List<UserHistory>> GetUserHistory(int userId)
{
    try
    {
        var url = $"{ServerConstants.APICallNames.GetUserHistoryById.GetStringValue()}{userId}";
        var response = await Http.GetFromJsonAsync<List<UserHistory>>(url);
        return response ?? new List<UserHistory>();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error fetching patient history: {ex.Message}");
        return new List<UserHistory>();
    }
}
}
}