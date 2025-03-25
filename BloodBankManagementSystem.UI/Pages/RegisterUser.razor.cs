using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BloodBankManagementSystem.Core.Models;
using Microsoft.AspNetCore.Components;
namespace BloodBankManagementSystem.UI.Pages{
     public partial class RegisterUser
     {
        private User NewUser = new();
        private string ConfirmPassword{get;set;} = string.Empty;
        private string Message{get;set;} = string.Empty;
        private string Address{get; set;} = string.Empty;
        [Parameter] public EventCallback OnClose { get; set; }
        [Parameter] public EventCallback OnUserAddedOrUpdated { get; set; }
        [Parameter] public User SelectedUserData { get; set; }
        [Parameter] public bool IsCreateUpdateUserPopup {get; set;}
        private string SaveButtonTitle {get; set;} = "Register";
        private string ClearResetTitle {get; set;} = "Clear";
        private string RegisterUpdateTitle {get; set;} = "Register User";
        private bool IsVisible {get; set;} = false;
        private bool IsReadOnly {get; set;} = false;
        private Dictionary<int, string> Roles = new()
        {
          {1, "Admin" },
          {2, "User" },
          {3, "Doctor"},
          {4, "Hospital"}
        };
     
        protected override void OnParametersSet()
        {
          if (SelectedUserData != null && SelectedUserData.Id > 0)
          {
               SaveButtonTitle = "Update";
               ClearResetTitle = "Reset";
               RegisterUpdateTitle = "Update User";
               IsVisible = true;
               AssingUserData();
          }
          else
          {
               NewUser = new User();
          }
       }

    private async Task HandleSubmit()
    {
        if(SelectedUserData!=null && SelectedUserData.Id>0){
          await UpdateUser();
        }
        else{
          await CreateUser();
        }
    }

    private void AssingUserData()
    {

     NewUser = new User
               {
                    Id = SelectedUserData.Id,
                    FirstName = SelectedUserData.FirstName,
                    LastName = SelectedUserData.LastName,
                    Email = SelectedUserData.Email,
                    Phone = SelectedUserData.Phone,
                    Role = SelectedUserData.Role,
                    Address = SelectedUserData.Address
               };

    }

     private async Task CreateUser(){
          Message = string.Empty;
        if (NewUser.PasswordHash != ConfirmPassword)
        {
            Message = "Passwords do not match.";
            return;
        }
        try
        {
            var response = await Http.PostAsJsonAsync("api/Users/register", NewUser);
            if (response.IsSuccessStatusCode)
            {
                Message = "User registered successfully!";
                NewUser = new User();
                ConfirmPassword = string.Empty;
                await OnUserAddedOrUpdated.InvokeAsync();
                await OnClose.InvokeAsync();
            }
            else
            {
                Message = "Registration failed.";
            }
        }
        catch (Exception ex)
        {
            Message = $"Error: {ex.Message}";
        }
     }
    private async Task UpdateUser()
    {
          if (SelectedUserData != null)
          {
               var response = await Http.PutAsJsonAsync($"/api/users/update/{NewUser.Id}", NewUser);
               if (response.IsSuccessStatusCode)
               {
                    await OnUserAddedOrUpdated.InvokeAsync();
                    await OnClose.InvokeAsync();
               }
               else
               {
                    Console.WriteLine("Error updating user");
               }
          }
     }
    private void HandleClearReset()
    {
        if(SelectedUserData!=null && SelectedUserData.Id>0){
          AssingUserData();
        }
        else{
          NewUser = new User();
          ConfirmPassword = string.Empty;
        }
        Message = string.Empty;
    }
    private async Task HandleCancelOrClose()
    {
        await OnClose.InvokeAsync();
    }
  }
}
