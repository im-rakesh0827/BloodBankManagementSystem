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
        private string ConfirmPassword = string.Empty;
        private string Message = string.Empty;
        private string Address{get; set;} = string.Empty;
        [Parameter] public EventCallback OnClose { get; set; }
        [Parameter] public EventCallback OnUserAddedOrUpdated { get; set; }
        [Parameter] public User SelectedUserData { get; set; }
        private string SaveButtonTitle {get; set;} = "Register";
        private string ClearResetTitle {get; set;} = "Clear";
        private bool HideInputField{get;set;} = false;
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
               HideInputField = true;
               AssingUserData();
          }
          else
          {
               NewUser = new User();
          }
       }

    private async Task HandleSubmit()
    {
        if(SelectedUserData.Id==0){
          await CreateUser();
        }
        else{
          await UpdateUser();
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
                    // await OnClose.InvokeAsync();
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
        if(SelectedUserData.Id==0){
          NewUser = new User();
          ConfirmPassword = string.Empty;
        }
        else{
          AssingUserData();
        }
        Message = string.Empty;
    }
    private async Task HandleCancel()
    {
        await OnClose.InvokeAsync();
    }
  }
}
