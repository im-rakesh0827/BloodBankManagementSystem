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
using BloodBankManagementSystem.UI.Helpers;
using System.Security.Claims;




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
     [Parameter] 
     public EventCallback OnRequestAddedOrUpdated { get; set; }

     private bool IsReadOnly{get; set;} = false;
     private string SaveUpdateButton{get; set;} = "Save";
     private string ClearResetButton{get; set;} = "Clear";
     private string Title{get; set;} = "Create Blood Request";
    protected override void OnInitialized()
     {
          BloodGroupsList = BloodBankHelper.GetAllBloodGroups();
          GenderList = BloodBankHelper.GetAllGenders();
     }

     protected override void OnParametersSet(){

          if(SelectedRequest!=null && SelectedRequest.Id>0){
               IsReadOnly = "Approved".Equals(SelectedRequest.Status, StringComparison.OrdinalIgnoreCase);
                AssignRequestInfo();
                SaveUpdateButton = "Update";
                ClearResetButton = "Reset";
                Title = "Update Blood Request";
          }
          else{
               requestModel = new BloodRequest();
          }
     }

     private void AssignRequestInfo(){
          requestModel = SelectedRequest;
     }



     private async Task HandleSubmit(){

          if(SelectedRequest!=null && SelectedRequest.Id>0){
               await UpateRequest();
          }
          else{
               await CreateRequest();
          }

     }

     private async Task UpateRequest(){
          IsLoading = true;
          await Task.Delay(500);
          var url = $"{ServerConstants.APICallNames.UpdateBloodRequest.GetStringValue()}";
          var response = await Http.PutAsJsonAsync(url, requestModel);

            if (response.IsSuccessStatusCode)
            {
               NotificationModel.Message = "Blood request updated successfully!";
               NotificationModel.Header = "Success";
               NotificationModel.Icon = "success";
               await OnRequestAddedOrUpdated.InvokeAsync();
               OnClose.InvokeAsync();
            }
            else
            {
               NotificationModel.Message = "An error occurred while updating the request";
               NotificationModel.Header = "Information";
               NotificationModel.Icon = "info";
            }
          IsLoading = false;
          await JSRuntime.InvokeVoidAsync("ShowToastAlert", NotificationModel.Message, NotificationModel.Header, NotificationModel.Icon);
          
     }

    private async Task CreateRequest()
    {
        try
        {
          IsLoading = true;
          await Task.Delay(500);
          // var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
          // var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

          requestModel.CreatedBy = "Rakesh";
            var url = $"{ServerConstants.APICallNames.CreateBloodRequest.GetStringValue()}";
            var response = await Http.PostAsJsonAsync(url, requestModel);
            if (response.IsSuccessStatusCode)
            {
               NotificationModel.Message = "Blood request sent successfully!";
               NotificationModel.Header = "Success";
               NotificationModel.Icon = "success";
               requestModel = new(); 
               await OnRequestAddedOrUpdated.InvokeAsync();
               OnClose.InvokeAsync();
            }
            else
            {
               NotificationModel.Message = "An error occurred while submitting the request";
               NotificationModel.Header = "Information";
               NotificationModel.Icon = "info";
            }
          IsLoading = false;
          await JSRuntime.InvokeVoidAsync("ShowToastAlert", NotificationModel.Message, NotificationModel.Header, NotificationModel.Icon);
        }
        catch(Exception ex)
        {
               NotificationModel.Message = ex.Message;
               NotificationModel.Header = "Error";
               NotificationModel.Icon = "error";
              await JSRuntime.InvokeVoidAsync("ShowToastAlert", NotificationModel.Message, NotificationModel.Header, NotificationModel.Icon);
        }
    }

    private async Task HandleClearRestore()
    {
          if(SelectedRequest!=null && SelectedRequest.Id>0){
               AssignRequestDataTobeUpdated();
          }
          else{
               requestModel = new BloodRequest();
          }
    }


    private void AssignRequestDataTobeUpdated(){
     requestModel = new BloodRequest(){
          Id = SelectedRequest.Id,
          RequesterId = SelectedRequest.RequesterId,
          PatientName = SelectedRequest.PatientName,
          UnitsRequired = SelectedRequest.UnitsRequired,
          Description = SelectedRequest.Description,
          BloodGroup = SelectedRequest.BloodGroup,
          Location = SelectedRequest.Location,
          Gender  = SelectedRequest.Gender,
          ContactNumber = SelectedRequest.ContactNumber
     };
    }

    private async Task HandleCancelOrClose()
     {
        OnClose.InvokeAsync();
     }
     }
}