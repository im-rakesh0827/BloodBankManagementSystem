using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BloodBankManagementSystem.Core.Models;
using Microsoft.AspNetCore.Components;
using BloodBankManagementSystem.Core.Helpers;
using BloodBankManagementSystem.Core.Enums;
using System.Collections.Generic;
using System.Linq;
using Microsoft.JSInterop;
namespace BloodBankManagementSystem.UI.Pages{
     public partial class RegisterUser
     {
        private User UserModel = new();
        private string ConfirmPassword{get;set;} = string.Empty;
        private string Message{get;set;} = string.Empty;
        private string Address{get; set;} = string.Empty;
        [Parameter] public EventCallback OnClose { get; set; }
        [Parameter] public EventCallback OnUserAddedOrUpdated { get; set; }
        [Parameter] public User SelectedUserData { get; set; }
        [Parameter] public bool IsCreateUpdateUserPopup {get; set;}
        private string SaveButtonTitle {get; set;} = "Save";
        private string ClearResetTitle {get; set;} = "Clear";
        private string RegisterUpdateTitle {get; set;} = "Register User";
        private bool IsVisible {get; set;} = false;
        private bool IsReadOnly {get; set;} = false;
        private bool IsLoading{get; set;} = false;
        private List<string> RolesList = new List<string>();
        private Notification NotificationModel = new Notification();

        protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSRuntime.InvokeVoidAsync("initializeJQueryToasts");
        }
    }
         protected override void OnInitialized()
        {
          RolesList = BloodBankHelper.GetRoles();
        }
     
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
               UserModel = new User();
          }
       }

    private async Task HandleSubmit()
    {
        NotificationModel = new Notification();
        Message = string.Empty;
        IsLoading = true;
        if(SelectedUserData!=null && SelectedUserData.Id>0){
          await Task.Delay(1500);
          await UpdateUser();
        }
        else{
          await CreateUser();
        }
        IsLoading = false;
        await JSRuntime.InvokeVoidAsync("ShowToastAlert", NotificationModel.Message, NotificationModel.Header, NotificationModel.Icon);

    }

    private void AssingUserData()
    {

     UserModel = new User
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
        try
        {
          if(UserModel!=null)
          {
               if (UserModel.PasswordHash != ConfirmPassword)
          {
               Message = "Passwords do not match.";
               return;
          }
            var response = await Http.PostAsJsonAsync("api/Users/register", UserModel);
            if (response.IsSuccessStatusCode)
            {
               NotificationModel.Message = "User registered successfully!";
               NotificationModel.Header = "Success";
               NotificationModel.Icon = "success";
                UserModel = new User();
                ConfirmPassword = string.Empty;
                await OnUserAddedOrUpdated.InvokeAsync();
                await OnClose.InvokeAsync();
            }
            else
            {
               NotificationModel.Message = "Registration failed.";
               NotificationModel.Header = "Information";
               NotificationModel.Icon = "info";
          }
          }
        }
        catch (Exception ex)
        {
            NotificationModel.Message = $"Error: {ex.Message}";;
            NotificationModel.Header = "Error";
            NotificationModel.Icon = "error";
        }
     }
    private async Task UpdateUser()
    {
          if (SelectedUserData != null)
          {
               var response = await Http.PutAsJsonAsync($"/api/users/update/{UserModel.Id}", UserModel);
               if (response.IsSuccessStatusCode)
               {
                    NotificationModel.Message = "User information updated successfully!";
                    NotificationModel.Header = "Success";
                    NotificationModel.Icon = "success";
                    await OnUserAddedOrUpdated.InvokeAsync();
                    await OnClose.InvokeAsync();
               }
               else
               {
                    NotificationModel.Message = "Error occured while updating user informations";
                    NotificationModel.Header = "Information";
                    NotificationModel.Icon = "info";
               }
          }
     }
    private void HandleClearReset()
    {
        if(SelectedUserData!=null && SelectedUserData.Id>0){
          AssingUserData();
        }
        else{
          UserModel = new User();
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
