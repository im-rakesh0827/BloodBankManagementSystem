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

namespace BloodBankManagementSystem.UI.Pages.Requests
{
     public partial class RequestBlood{
     private bool IsLoading{get; set;} = false;
    private BloodRequest requestModel = new();
    private bool hasError = false;
    private List<string> BloodGroupsList = new List<string>();
    private List<string> GenderList = new List<string>();
    private Notification NotificationModel = new Notification();
    [Parameter] 
    public EventCallback OnClose { get; set; }
    [Parameter]
    public BloodRequest? SelectedRequest{get; set;}
    [Parameter] 
     public bool IsCreateUpdateRequestPopup{get; set;}
    protected override void OnInitialized()
     {
          BloodGroupsList = BloodBankHelper.GetAllBloodGroups();
          GenderList = BloodBankHelper.GetAllGenders();
     }

    private async Task SubmitRequest()
    {
        try
        {
          IsLoading = true;
          await Task.Delay(500);
            var response = await Http.PostAsJsonAsync("api/bloodrequest/create", requestModel);
            if (response.IsSuccessStatusCode)
            {
               NotificationModel.Message = "Blood request sent successfully!";
               NotificationModel.Header = "Success";
               NotificationModel.Icon = "success";
               requestModel = new(); 
            }
            else
            {
               NotificationModel.Message = "An error occurred while submitting the request";
               NotificationModel.Header = "Information";
               NotificationModel.Icon = "info";
            }
          IsLoading = false;
          OnClose.InvokeAsync();
          await JSRuntime.InvokeVoidAsync("ShowToastAlert", NotificationModel.Message, NotificationModel.Header, NotificationModel.Icon);

        }
        catch(Exception ex)
        {
               NotificationModel.Message = ex.Message;
               NotificationModel.Header = "Error";
               NotificationModel.Icon = "error";
        }
    }

    private async Task HandleClearRestore()
    {
          requestModel = new BloodRequest();
    }

    private async Task HandleCancelOrClose()
     {
        OnClose.InvokeAsync();
     }
     }
}